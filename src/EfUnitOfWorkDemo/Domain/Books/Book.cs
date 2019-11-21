using EfUnitOfWorkDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfUnitOfWorkDemo.Domain.Books
{
    public class Book : Entity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
    }
}
