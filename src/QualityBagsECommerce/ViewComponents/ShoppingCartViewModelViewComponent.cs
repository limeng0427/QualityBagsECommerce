using Microsoft.AspNetCore.Mvc;
using QualityBagsECommerce.Data;
using QualityBagsECommerce.Models;
using QualityBagsECommerce.Models.ShoppingCartViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBagsECommerce.ViewComponents
{
    public class ShoppingCartViewModelViewComponent: ViewComponent
    {
        private readonly BagContext _context;
        public ShoppingCartViewModelViewComponent(BagContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(ReturnCurrentCartViewModel());
        }

        public ShoppingCartViewModel ReturnCurrentCartViewModel()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(_context),
                CartTotal = cart.GetTotal(_context),
                ItemTotal = cart.GetTotalItem(_context),
            };
            return viewModel;
        }

    }
}
