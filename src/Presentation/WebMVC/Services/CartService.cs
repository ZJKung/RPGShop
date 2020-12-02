using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Models;
using WebMVC.Models.CartModels;

namespace WebMVC.Services
{
    public class CartService : ICartService
    {
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly IHttpClient _apiClient;
        private readonly string _remoteServiceBaseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartService> _logger;
        public CartService(
            IOptionsSnapshot<AppSettings> settings,
            IHttpClient httpClient,
            ILogger<CartService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _settings = settings;
            _remoteServiceBaseUrl = $"{_settings.Value.CartUrl}/api/cart";
            _httpContextAccessor = httpContextAccessor;
            _apiClient = httpClient;
            _logger = logger;
        }
        public async Task AddItemToCartAsync(ApplicationUser user, CartItem product)
        {
            Cart cart = await GetCartAsync(user);
            _logger.LogDebug("user name" + user.Id);
            //if nothing in cart Create Cart
            if (cart == null)
            {
                cart = new Models.CartModels.Cart()
                {
                    BuyerId = user.Id,
                    Items = new List<CartItem>()
                };
            }

            var cartItem = cart.Items
                .Where(p => p.ProductId == product.ProductId)
                .FirstOrDefault();

            if (cartItem == null)
            {
                cart.Items.Add(product);
            }
            else
            {
                cartItem.Quantity += 1;
            }

            await UpdateCartAsync(cart);
        }

        public async Task ClearCartAsync(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            var cleanCartUri = ApiPaths.Cart.CleanCart(_remoteServiceBaseUrl, user.Id);
            _logger.LogDebug("Clean cart uri : " + cleanCartUri);
            var response = await _apiClient.DeleteAsync(cleanCartUri);
            _logger.LogDebug("cart cleaned");
        }

        public async Task<Cart> GetCartAsync(ApplicationUser user)
        {
            var token = await GetUserTokenAsync();
            _logger.LogInformation("we are in get cart and user id " + user.Id);
            _logger.LogInformation(_remoteServiceBaseUrl);

            var getCartUri = ApiPaths.Cart.GetCart(_remoteServiceBaseUrl, user.Id);
            _logger.LogInformation(getCartUri);
            var dataString = await _apiClient.GetStringAsync(getCartUri, token);
            _logger.LogInformation(dataString);
            var response = JsonConvert.DeserializeObject<Cart>(dataString.ToString()) ??
                new Cart()
                {
                    BuyerId = user.Id
                };
            return response;
        }

        public async Task<Cart> SetQuantitiesAsync(ApplicationUser user, Dictionary<string, int> quantities)
        {
            var cart = await GetCartAsync(user);
            cart.Items.ForEach(x =>
            {
                if (quantities.TryGetValue(x.Id, out var quantity))
                {
                    x.Quantity = quantity;
                }
            });
            return await UpdateCartAsync(cart);
        }

        public async Task<Cart> UpdateCartAsync(Cart cart)
        {
            var token = await GetUserTokenAsync();
            _logger.LogDebug("Service url: " + _remoteServiceBaseUrl);
            var updateCartUri = ApiPaths.Cart.UpdateCart(_remoteServiceBaseUrl);
            _logger.LogDebug("Update cart url: " + updateCartUri);
            var response = await _apiClient.PostAsync(updateCartUri, cart, token);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Cart>(jsonResponse);
        }

        private async Task<string> GetUserTokenAsync()
        {
            var context = _httpContextAccessor.HttpContext;
            return await context.GetTokenAsync("access_token");
        }
    }
}