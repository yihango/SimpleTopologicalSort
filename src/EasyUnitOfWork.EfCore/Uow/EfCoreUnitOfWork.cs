using EasyUnitOfWork.Uow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using EasyUnitOfWork.Extensions;

namespace EasyUnitOfWork.Uow
{
    public class EfCoreUnitOfWork : UnitOfWorkBase
    {
        protected IDictionary<string, DbContext> ActiveDbContexts { get; }

        protected IServiceProvider ServiceProvider { get; }

        private readonly IEfCoreTransactionStrategy _transactionStrategy;

        private readonly IDbContextResolver _dbContextResolver;

        public EfCoreUnitOfWork(
            IServiceProvider serviceProvider,
            IEfCoreTransactionStrategy transactionStrategy,
            IDbContextResolver dbContextResolver
            )
        {
            this.ServiceProvider = serviceProvider;
            this._transactionStrategy = transactionStrategy;
            this.ActiveDbContexts = new Dictionary<string, DbContext>();
            this._dbContextResolver = dbContextResolver;
        }

        /// <summary>
        /// 获取或者创建数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
        /// <param name="name">名称</param>
        /// <returns>数据库上下文对象</returns>
        public virtual TDbContext GetOrCreateDbContext<TDbContext>(string name = null)
           where TDbContext : DbContext
        {
            var dbContextType = typeof(TDbContext);

            // TODO: 这里获取连接字符串,有待开发
            var nameOrConnectionString = "Default";

            var dbContextKey = $"{dbContextType.FullName}#{nameOrConnectionString}";
            if (string.IsNullOrWhiteSpace(name))
            {
                dbContextKey = $"{dbContextKey}#{name}";
            }

            DbContext dbContext;
            if (!ActiveDbContexts.TryGetValue(dbContextKey, out dbContext))
            {
                if (Options.IsTransactional == true)
                {
                    dbContext = _transactionStrategy.CreateDbContext<TDbContext>(nameOrConnectionString, _dbContextResolver);
                }
                else
                {
                    dbContext = _dbContextResolver.Resolve<TDbContext>(nameOrConnectionString, null);
                }

                if (Options.Timeout.HasValue &&
                    dbContext.Database.IsRelational() &&
                    !dbContext.Database.GetCommandTimeout().HasValue)
                {
                    var commandTimeout = Convert.ToInt32(Options.Timeout.Value.TotalSeconds);

                    //dbContext.Database.SetCommandTimeout(Options.Timeout.Value.TotalSeconds.To<int>());
                    dbContext.Database.SetCommandTimeout(commandTimeout);
                }

                //TODO: Object materialize event
                //TODO: Apply current filters to this dbcontext

                ActiveDbContexts[dbContextKey] = dbContext;
            }

            return (TDbContext)dbContext;
        }


        public override void SaveChanges()
        {
            foreach (var dbContext in GetAllActiveDbContexts())
            {
                SaveChangesInDbContext(dbContext);
            }
        }

        public override async Task SaveChangesAsync()
        {
            foreach (var dbContext in GetAllActiveDbContexts())
            {
                await SaveChangesInDbContextAsync(dbContext);
            }
        }
        protected override void CompleteUow()
        {
            SaveChanges();
            CommitTransaction();
        }

        protected override async Task CompleteUowAsync()
        {
            await SaveChangesAsync();
            CommitTransaction();
        }

        private void CommitTransaction()
        {
            if (Options.IsTransactional == true)
            {
                _transactionStrategy.Commit();
            }
        }



        protected override void DisposeUow()
        {
            if (Options.IsTransactional == true)
            {
                _transactionStrategy.Dispose();
            }
            else
            {
                foreach (var context in GetAllActiveDbContexts())
                {
                    Release(context);
                }
            }

            ActiveDbContexts.Clear();
        }


        protected virtual void SaveChangesInDbContext(DbContext dbContext)
        {
            dbContext.SaveChanges();
        }

        protected virtual async Task SaveChangesInDbContextAsync(DbContext dbContext)
        {
            await dbContext.SaveChangesAsync();
        }

        public IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return ActiveDbContexts.Values.ToImmutableList();
        }

        protected virtual void Release(DbContext dbContext)
        {
            dbContext.Dispose();
        }

    }
}
