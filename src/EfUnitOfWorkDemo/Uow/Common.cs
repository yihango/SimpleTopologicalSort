using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace EfUnitOfWorkDemo.Uow
{
    public static class Common
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue obj;
            return dictionary.TryGetValue(key, out obj) ? obj : default(TValue);
        }
    }

    public static class IsolationLevelExtensions
    {
        /// <summary>
        /// Converts <see cref="IsolationLevel"/> to <see cref="IsolationLevel"/>.
        /// </summary>
        public static IsolationLevel ToSystemDataIsolationLevel(this IsolationLevel isolationLevel)
        {
            switch (isolationLevel)
            {
                case IsolationLevel.Chaos:
                    return IsolationLevel.Chaos;
                case IsolationLevel.ReadCommitted:
                    return IsolationLevel.ReadCommitted;
                case IsolationLevel.ReadUncommitted:
                    return IsolationLevel.ReadUncommitted;
                case IsolationLevel.RepeatableRead:
                    return IsolationLevel.RepeatableRead;
                case IsolationLevel.Serializable:
                    return IsolationLevel.Serializable;
                case IsolationLevel.Snapshot:
                    return IsolationLevel.Snapshot;
                case IsolationLevel.Unspecified:
                    return IsolationLevel.Unspecified;
                default:
                    throw new Exception("Unknown isolation level: " + isolationLevel);
            }
        }
    }

    public static class DbContextExtensions
    {
        public static bool HasRelationalTransactionManager(this DbContext dbContext)
        {
            return dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
        }
    }
}
