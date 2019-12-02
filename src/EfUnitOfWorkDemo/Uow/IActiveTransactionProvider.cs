using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EfUnitOfWorkDemo.Uow
{
    public interface IActiveTransactionProvider
    {
        /// <summary>
        ///     Gets the active transaction or null if current UOW is not transactional.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args);

        /// <summary>
        ///     Gets the active database connection.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args);
    }

    public class ActiveTransactionProviderArgs : Dictionary<string, object>
    {
        public static ActiveTransactionProviderArgs Empty { get; } = new ActiveTransactionProviderArgs();
    }

    public class EfCoreActiveTransactionProvider : IActiveTransactionProvider
    {
        private readonly IServiceProvider _iocResolver;

        public EfCoreActiveTransactionProvider(IServiceProvider iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public IDbTransaction GetActiveTransaction(ActiveTransactionProviderArgs args)
        {
            return GetDbContext(args).Database.CurrentTransaction?.GetDbTransaction();
        }

        public IDbConnection GetActiveConnection(ActiveTransactionProviderArgs args)
        {
            return GetDbContext(args).Database.GetDbConnection();
        }

        private DbContext GetDbContext(ActiveTransactionProviderArgs args)
        {
            Type dbContextProviderType = typeof(IDbContextProvider<>).MakeGenericType((Type)args["ContextType"]);

            var dbContextProvider = _iocResolver.GetService(dbContextProviderType);

            var method = dbContextProvider.GetType().GetMethod(nameof(IDbContextProvider<DbContext>.GetDbContext), new[]
            {
                (Type)args["ContextType"]
            });

            return (DbContext)method.Invoke(dbContextProvider, new object[] {

            });
        }
    }
}
