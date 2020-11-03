using System.Collections.Generic;
using System.Threading.Tasks;
using CartApi.Domain;

namespace CartApi.Interfaces
{
    public interface ICartRepository:IGenericRepository<Cart>
    {
        // Task<Cart> GetAsync(string cartId);
        // Task<Cart> UpdateAsync(Cart basket);
        // Task<bool> DeleteAsync(string id);
        IEnumerable<string> GetUsers();
    }
}