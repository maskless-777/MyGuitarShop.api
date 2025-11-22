using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.Interfaces
{
    public interface IRepository<TEntity, in TKey>
        {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> FindByIdAsync(TKey id);
        Task<bool> InsertAsync(TEntity entity);
        Task<bool> UpdateAsync(TKey id, TEntity entity);
        Task<bool> DeleteAsync(TKey id);
    }
}
