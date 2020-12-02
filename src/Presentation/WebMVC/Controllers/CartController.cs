using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class CartController : Controller
    {

        private ILogger _logger;
        private readonly ICartService _cartService;
        private readonly IAuthService<ApplicationUser> _userManager;

        public CartController(
            ICartService cartService,
            ILogger<CartController> logger,
            IAuthService<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> AddToCart(CatalogItem catalog)
        {
            var user = _userManager.Get(User);
            var product = new CartItem();
            product.Id = catalog.Id.ToString();
            product.ProductId = catalog.Id.ToString();
            product.ProductName = catalog.Name;
            product.PictureUrl = catalog.PictureUrl;
            product.UnitPrice = catalog.Price;
            product.Quantity = 1;

            await _cartService.AddItemToCartAsync(user, product);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = _userManager.Get(User);
                var vm = await _cartService.GetCartAsync(user);
                return View(vm);
            }
            catch (Exception ex)
            {
                // TODO
            }
            return View();
        }
        //todo add update cart action
        [HttpPost]
        public async Task<IActionResult> Index(Dictionary<string, int> quantities, string action)
        {
            try
            {
                var user = _userManager.Get(User);
                await _cartService.SetQuantitiesAsync(user, quantities);

            }
            catch (Exception ex)
            {
                // TODO
            }
            return View();

        }


    }
}