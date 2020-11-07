using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Services;
using WebMVC.ViewModels;

namespace WebMVC.ViewComponents
{
    public class Cart : ViewComponent
    {
        private readonly ICartService _service;
        public Cart(ICartService service)
        {
            _service = service;
        }
        public async Task<IViewComponentResult> InvokeAsync(ApplicationUser user)
        {
            var vm = new CartComponentViewModel();
            try
            {
                var cart = await _service.GetCartAsync(user);
                vm.ItemsInCart = cart.Items.Count;
                vm.TotalCost = cart.Total();
                return View<CartComponentViewModel>(vm);
            }
            catch (Exception)
            {
                ViewBag.IsCartInoperative = true;
            }

            return View(vm);
        }

    }
}