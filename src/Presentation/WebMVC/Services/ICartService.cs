using System.Collections.Generic;
using System.Threading.Tasks;
using WebMVC.Models;
using WebMVC.Models.CartModels;

namespace WebMVC.Services
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(ApplicationUser user);
        Task AddItemToCartAsync(ApplicationUser user, CartItem product);
        Task<Cart> UpdateCartAsync(Cart cart);
        Task<Cart> SetQuantitiesAsync(ApplicationUser user, Dictionary<string, int> quantities);
        Task ClearCartAsync(ApplicationUser user);
    }
}