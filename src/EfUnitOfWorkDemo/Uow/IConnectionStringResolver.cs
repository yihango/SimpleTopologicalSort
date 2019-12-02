using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace EfUnitOfWorkDemo.Uow
{

    public interface IConnectionStringRegister
    {
        void Register([NotNull]string key, [NotNull]string value);
        void Register([NotNull]string key, [NotNull]Func<string, string> getConnectionStringFunc);
    }

    public interface IConnectionStringResolver
    {
        /// <summary>
        /// Gets a connection string name (in config file) or a valid connection string.
        /// </summary>
        /// <param name="args">Arguments that can be used while resolving connection string.</param>
        string GetNameOrConnectionString([NotNull]string key);
    }

    public class DefaultConnectionStringResolver : IConnectionStringResolver, IConnectionStringRegister
    {
        static Dictionary<string, object> _connectionStringDict;

        public DefaultConnectionStringResolver()
        {
            _connectionStringDict = new Dictionary<string, object>();
        }

        public string GetNameOrConnectionString([NotNull]string key)
        {
            if (!_connectionStringDict.TryGetValue(key, out object value) || value == null)
            {
                throw new Exception($"not'found connection string with key:  {key}");
            }

            if (value is string)
            {
                return value.ToString();
            }

            return (value as Func<string, string>).Invoke(key);
        }

        public void Register([NotNull]string key, [NotNull]string value)
        {
            _connectionStringDict[key] = value;
        }

        public void Register([NotNull]string key, [NotNull]Func<string, string> getConnectionStringFunc)
        {
            _connectionStringDict[key] = getConnectionStringFunc;
        }
    }
}
