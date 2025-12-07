using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.Interfaces
{
    public interface IRepository<TEntity>
        {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> FindByIdAsync(string id);
        Task<bool> InsertAsync(TEntity dto);
        Task<bool> UpdateAsync(string id, TEntity dto);
        Task<bool> DeleteAsync(string id);
    }
}
