using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EfUnitOfWorkDemo.Uow
{
    public static class DbContextOptionsFactory
    {
        const string connectionStringName = "Default";
        static string _connectionString = "";

        public static DbContextOptions<TDbContext> Create<TDbContext>(Action<DbContextOptionsBuilder<TDbContext>, string, IDbConnection> action)
            where TDbContext : DbContext
        {

            var dbContextCreationContext = DbContextCreationContext.Create(connectionStringName, _connectionString);
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<TDbContext>();

            action?.Invoke(dbContextOptionsBuilder, _connectionString, null);

            return dbContextOptionsBuilder.Options;
        }

        public static DbContextOptions Create(Action<DbContextOptionsBuilder, string, IDbConnection> action)
        {
            var dbContextCreationContext = DbContextCreationContext.Create(connectionStringName, _connectionString);
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();

            action?.Invoke(dbContextOptionsBuilder, _connectionString, null);

            return dbContextOptionsBuilder.Options;
        }
    }
}
