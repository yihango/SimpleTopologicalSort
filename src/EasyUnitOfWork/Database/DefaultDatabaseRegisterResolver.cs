using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace EasyUnitOfWork.Database
{
    public class DefaultDatabaseRegisterResolver : IDatabaseRegister, IDatabaseResolver
    {
        static Dictionary<object, object> _dict;

        public DefaultDatabaseRegisterResolver()
        {
            _dict = new Dictionary<object, object>();
        }

        public object GetService([NotNull] object key)
        {
            return (_dict[key] as Func<object, object>).Invoke(key);
        }

        public TValue GetService<TValue>([NotNull] object key)
            where TValue : class, new()
        {
            return this.GetService(key) as TValue;
        }

        public TValue GetService<TKey, TValue>([NotNull] TKey key)
            where TValue : class, new()
        {
            return (_dict[key] as Func<object, object>).Invoke(key) as TValue;
        }

        public void Register([NotNull] object key, [NotNull] Func<object, object> createFunc)
        {
            _dict[key] = createFunc;
        }

        public void Register<TKey, TValue>([NotNull] TKey key, [NotNull] Func<TKey, TValue> createFunc)
            where TValue : class, new()
        {
            _dict[key] = createFunc;
        }
    }
}
