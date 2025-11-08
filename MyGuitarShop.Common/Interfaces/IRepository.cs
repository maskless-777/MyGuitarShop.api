using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.Interfaces
{
    public interface IRepository<TDTO>
        {
        Task<IEnumerable<TDTO>> GetAllAsync();
        Task<TDTO?> FindByIdAsync(int id);
        Task<int> InsertAsync(TDTO dto);
        Task<int> UpdateAsync(int id, TDTO dto);
        Task<int> DeleteAsync(int id);
    }
}
