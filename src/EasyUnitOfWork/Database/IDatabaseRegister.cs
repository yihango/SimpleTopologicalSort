using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyUnitOfWork.Database
{
    /// <summary>
    /// 数据库注册
    /// </summary>
    public interface IDatabaseRegister
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="createFunc"></param>
        void Register([NotNull]object key, [NotNull]Func<object, object> createFunc);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="createFunc"></param>
        void Register<TKey,TValue>([NotNull]TKey key, [NotNull]Func<TKey, TValue> createFunc)
            where TValue : class, new();

    }
}
