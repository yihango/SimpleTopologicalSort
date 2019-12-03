using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyUnitOfWork.Database
{
    public interface IDatabaseResolver
    {
        object GetService([NotNull]object key);

        TValue GetService<TValue>([NotNull]object key)
          where TValue : class, new();

        TValue GetService<TKey, TValue>([NotNull]TKey key)
            where TValue : class, new();
    }
}
