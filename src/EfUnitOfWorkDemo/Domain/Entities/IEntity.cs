using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfUnitOfWorkDemo.Domain.Entities
{
    public interface IEntity<TKey>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public TKey Id { get; set; }
    }

    public interface IEntity : IEntity<long>
    {

    }

    public abstract class Entity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public TKey Id { get; set; }
    }

    public abstract class Entity : Entity<long>
    {

    }
}
