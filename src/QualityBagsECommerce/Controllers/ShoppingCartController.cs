using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QualityBagsECommerce.Data;
using QualityBagsECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using QualityBagsECommerce.Models.ShoppingCartViewModels;

namespace QualityBagsECommerce.Controllers
{
    //[Produces("application/json")]
    //[Route("api/ShoppingCart")]
    [AllowAnonymous]
    [Authorize(Roles = "Member")]
    public class ShoppingCartController : Controller
    {
        BagContext _context;

        public ShoppingCartController(BagContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(_context),
                CartTotal = cart.GetTotal(_context),
                ItemTotal = cart.GetTotalItem(_context),
            };
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5

        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedBag = _context.Bags
                .Single(bag => bag.ID == id);
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedBag, _context);
            // Go back to the main store page for more shopping
            return RedirectToAction("Index", "MemberBags");
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int itemCount = cart.RemoveFromCart(id, _context);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public ActionResult EmptyCart(int i)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            //int itemCount = cart.RemoveFromCart(id, _context);
            //return Redirect(Request.Headers["Referer"].ToString());
            cart.EmptyCart(_context);
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}