using EfUnitOfWorkDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EfUnitOfWorkDemo.Domain.BookStores
{
    public class BookAndStoreAssociated : Entity
    {
        public long BookStoreId { get; set; }

        public long BookId { get; set; }

    }
}
