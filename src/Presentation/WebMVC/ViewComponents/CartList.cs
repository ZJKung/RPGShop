using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Services;

namespace WebMVC.ViewComponents
{
    public class CartList : ViewComponent
    {
        private readonly ICartService _service;
        public CartList(ICartService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync(ApplicationUser user)
        {
            var vm = new Models.CartModels.Cart();
            try
            {
                vm = await _service.GetCartAsync(user);
                return View(vm);
            }
            catch (Exception)
            {
                ViewBag.IsCartInoperative = true;
                TempData["CartInoperativeMsg"] = "Cart Service is inoperative, please retry later.";
            }

            return View(vm);
        }

    }
}