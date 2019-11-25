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
    public class EfCoreUnitOfWork : UnitOfWorkBase
    {
        protected IDictionary<string, DbContext> ActiveDbContexts { get; }


        private readonly IDbContextResolver _dbContextResolver;
        private readonly IEfCoreTransactionStrategy _transactionStrategy;

        public EfCoreUnitOfWork(IUnitOfWorkDefaultOptions defaultOptions)
            : base(defaultOptions)
        {
            ActiveDbContexts = new Dictionary<string, DbContext>();
        }

        public IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return ActiveDbContexts.Values.ToImmutableList();
        }


        protected override void CompleteUow()
        {
            throw new NotImplementedException();
        }

        protected override Task CompleteUowAsync()
        {
            throw new NotImplementedException();
        }

        protected override void DisposeUow()
        {
            throw new NotImplementedException();
        }
    }
}
