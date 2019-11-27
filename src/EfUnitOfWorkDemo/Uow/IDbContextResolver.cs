using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EfUnitOfWorkDemo.Uow
{
    public interface IDbContextResolver
    {
        TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection)
           where TDbContext : DbContext;
    }

    public class DbContextResolver : IDbContextResolver
    {
        protected readonly IDbContextFactory _dbContextFactory;

        public DbContextResolver(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public virtual TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection)
            where TDbContext : DbContext
        {
            return this._dbContextFactory.Create<TDbContext>(connectionString, existingConnection);
        }
    }
}
