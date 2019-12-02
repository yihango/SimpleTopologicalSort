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

        public virtual bool IsTransient()
        {
            if (EqualityComparer<TKey>.Default.Equals(Id, default(TKey)))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TKey) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(TKey) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }
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
