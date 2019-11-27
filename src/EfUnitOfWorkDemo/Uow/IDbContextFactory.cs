using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EfUnitOfWorkDemo.Uow
{
    public interface IDbContextFactory
    {
        TDbContext Create<TDbContext>(string connectionString, IDbConnection dbConnection)
            where TDbContext : DbContext;

        void Register<TDbContext>(Func<string, IDbConnection, TDbContext> dbContextOptionsBuilderAction)
            where TDbContext : DbContext;
    }


    public class DbContextFactory : IDbContextFactory
    {
        static Dictionary<Type, object> _dict;

        public DbContextFactory()
        {
            if (_dict == null)
            {
                _dict = new Dictionary<Type, object>();
            }
        }

        public TDbContext Create<TDbContext>(string connectionString, IDbConnection dbConnection) where TDbContext : DbContext
        {
            var dbContext = (_dict[typeof(TDbContext)] as Func<string, IDbConnection, TDbContext>).Invoke(connectionString, dbConnection);

            return dbContext;
        }




        public void Register<TDbContext>([NotNull]Func<string, IDbConnection, TDbContext> dbContextCreateFunc)
            where TDbContext : DbContext
        {
            _dict[typeof(TDbContext)] = dbContextCreateFunc;
        }

    }
}
