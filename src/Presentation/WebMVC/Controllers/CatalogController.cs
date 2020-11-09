using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebMVC.Models;
using WebMVC.Models.CartModels;
using WebMVC.Services;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    public class CatalogController : Controller
    {
        private ICatalogService _catalogService;
        private ILogger _logger;
        private readonly ICartService _cartService;
        private readonly IAuthService<ApplicationUser> _userManager;
        private readonly int itemsPage = 6;
        public CatalogController(ICatalogService catalogService,
        ICartService cartService,
        ILogger<CatalogController> logger,
        IAuthService<ApplicationUser> userManager)
        {
            _catalogService = catalogService;
            _logger = logger;
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? typesFilterApplied, int? page)
        {
            var catalog = await _catalogService.GetCatalogItems(page ?? 0, itemsPage, typesFilterApplied);
            var vm = new CatalogViewModel();
            vm.CatalogItems = catalog.Data;
            vm.Types = await _catalogService.GetTypes();
            vm.TypesFilterApplied = typesFilterApplied ?? 0;
            vm.PaginationInfo = new PaginationInfo();
            vm.PaginationInfo.ActualPage = page ?? 0;
            vm.PaginationInfo.ItemsPerPage = Math.Min(catalog.Count, itemsPage);
            vm.PaginationInfo.TotalItems = catalog.Count;
            vm.PaginationInfo.TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsPage);

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";
            ViewBag.TypesFilterApplied = typesFilterApplied;

            return View(vm);
        }

        public async Task<IActionResult> AddToCart(CatalogItem catalog)
        {
            var user = _userManager.Get(User);
            var product = new CartItem();
            product.Id = catalog.Id.ToString();
            product.ProductName = catalog.Name;
            product.PictureUrl = catalog.PictureUrl;
            product.UnitPrice = catalog.Price;



            await _cartService.AddItemToCartAsync(user, product);
            return View();
        }


    }
}