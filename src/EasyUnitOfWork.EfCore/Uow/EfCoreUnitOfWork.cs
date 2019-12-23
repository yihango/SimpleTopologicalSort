using EasyUnitOfWork.Uow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace EasyUnitOfWork.Uow
{
    public class EfCoreUnitOfWork : UnitOfWorkBase
    {
        protected IDictionary<string, DbContext> ActiveDbContexts { get; }

        protected IServiceProvider ServiceProvider { get; }

        private readonly IEfCoreTransactionStrategy _transactionStrategy;

        public EfCoreUnitOfWork(
            IServiceProvider serviceProvider,
            IEfCoreTransactionStrategy transactionStrategy
            )
        {
            this.ServiceProvider = serviceProvider;
            this._transactionStrategy = transactionStrategy;
            this.ActiveDbContexts = new Dictionary<string, DbContext>();
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
