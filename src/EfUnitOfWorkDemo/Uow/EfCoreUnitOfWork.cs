using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace EfUnitOfWorkDemo.Uow
{
    /// <summary>
    /// 瞬时的
    /// </summary>
    /// <summary>
    /// Implements Unit of work for Entity Framework.
    /// </summary>
    public class EfCoreUnitOfWork : UnitOfWorkBase
    {
        protected IDictionary<string, DbContext> ActiveDbContexts { get; }
        protected IServiceProvider IocResolver { get; }

        protected readonly IDbContextResolver _dbContextResolver;
        protected readonly IEfCoreTransactionStrategy _transactionStrategy;

        protected readonly IDbContextTypeMatcher _dbContextTypeMatcher;

        protected readonly IConnectionStringResolver _connectionStringResolver;

        /// <summary>
        /// Creates a new <see cref="EfCoreUnitOfWork"/>.
        /// </summary>
        public EfCoreUnitOfWork(
            IServiceProvider iocResolver,
            IDbContextTypeMatcher dbContextTypeMatcher,
            IDbContextResolver dbContextResolver,
            IUnitOfWorkDefaultOptions defaultOptions,
            IEfCoreTransactionStrategy transactionStrategy,
            IConnectionStringResolver connectionStringResolver
            )
            : base(
                  defaultOptions
                  )
        {
            IocResolver = iocResolver;
            _dbContextTypeMatcher = dbContextTypeMatcher;
            _dbContextResolver = dbContextResolver;
            _transactionStrategy = transactionStrategy;

            ActiveDbContexts = new Dictionary<string, DbContext>();

            _connectionStringResolver = connectionStringResolver;
        }

        protected override void BeginUow()
        {
            if (Options.IsTransactional == true)
            {
                _transactionStrategy.InitOptions(Options);
            }
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

        public IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return ActiveDbContexts.Values.ToImmutableList();
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

        protected virtual void Release(DbContext dbContext)
        {
            dbContext.Dispose();
            //IocResolver.Release(dbContext);
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>(string name = null)
        where TDbContext : DbContext
        {

            var concreteDbContextType = _dbContextTypeMatcher.GetConcreteType(typeof(TDbContext));


            var connectionString = ResolveConnectionString(concreteDbContextType.FullName);

            var dbContextKey = concreteDbContextType.FullName + "#" + connectionString;
            if (name != null)
            {
                dbContextKey += "#" + name;
            }

            DbContext dbContext;
            if (!ActiveDbContexts.TryGetValue(dbContextKey, out dbContext))
            {
                if (Options.IsTransactional == true)
                {
                    dbContext = _transactionStrategy.CreateDbContext<TDbContext>(connectionString, _dbContextResolver);
                }
                else
                {
                    dbContext = _dbContextResolver.Resolve<TDbContext>(connectionString, null);
                }

                if (Options.Timeout.HasValue &&
                    dbContext.Database.IsRelational() &&
                    !dbContext.Database.GetCommandTimeout().HasValue)
                {
                    dbContext.Database.SetCommandTimeout(Options.Timeout.Value.TotalSeconds.To<int>());
                }

                //TODO: Object materialize event
                //TODO: Apply current filters to this dbcontext

                ActiveDbContexts[dbContextKey] = dbContext;
            }

            return (TDbContext)dbContext;
        }


        protected virtual string ResolveConnectionString(string concreteDbContextType)
        {
            return _connectionStringResolver.GetNameOrConnectionString(concreteDbContextType);
        }

    }
}