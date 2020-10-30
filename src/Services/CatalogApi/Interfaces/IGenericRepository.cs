using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogApi.Interfaces
{
    public interface IGenericRepository<T> where T :class
    {
         Task<T> GetAsync(string id);
         Task<IEnumerable<T>> GetAllAsync();
         Task<T> UpdateAsync(T entity);

         Task<bool> DeleteAsync(string id);
    }
}