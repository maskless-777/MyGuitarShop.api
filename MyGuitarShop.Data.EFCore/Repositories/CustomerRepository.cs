using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyGuitarShop.Data.EFCore.Context;
using MyGuitarShop.Data.EFCore.Entities;
using MyGuitarShop.Data.EFCore.Abstract;

namespace MyGuitarShop.Data.EFCore.Repositories
{
    public class CustomerRepository(MyGuitarShopContext dbContext) : RepositoryBase<Customer>(dbContext) { }
}
