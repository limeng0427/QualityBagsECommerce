using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBagsECommerce.Data;
using QualityBagsECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace QualityBagsECommerce.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class OrdersController : Controller
    {
        private readonly BagContext _context;
        private UserManager<ApplicationUser> _userManager;

        public OrdersController(BagContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Orders.ToListAsync());
            return View(await _context.Orders.Include(i => i.User).AsNoTracking().ToListAsync());
        }

        // GET: Orders/Create
        [Authorize(Roles = "Member")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Create([Bind("City,Country,FirstName,LastName,Phone,PostalCode,State")] Order order)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {

                ShoppingCart cart = ShoppingCart.GetCart(this.HttpContext);
                List<CartItem> items = cart.GetCartItems(_context);
                List<OrderDetail> details = new List<OrderDetail>();
                foreach (CartItem item in items)
                {

                    OrderDetail detail = CreateOrderDetailForThisItem(item);
                    detail.Order = order;
                    details.Add(detail);
                    _context.Add(detail);

                }

                order.User = user;
                //
                _context.Entry(user).State = EntityState.Unchanged;

                order.OrderDate = DateTime.Today;
                order.Status = "Shipped";
                order.Total = ShoppingCart.GetCart(this.HttpContext).GetTotal(_context);
                order.OrderDetails = details;
                _context.SaveChanges();


                return RedirectToAction("Purchased", new RouteValueDictionary(
                new { action = "Purchased", id = order.ID }));
            }

            return View(order);
        }

        private OrderDetail CreateOrderDetailForThisItem(CartItem item)
        {

            OrderDetail detail = new OrderDetail();


            detail.Quantity = item.Quantity;
            detail.Bag = item.Bag;
            detail.Price = item.Bag.Price;

            return detail;

        }
        public async Task<IActionResult> Purchased(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            var details = _context.OrderDetails.Where(detail => detail.Order.ID == order.ID).Include(detail => detail.Bag).ToList();

            order.OrderDetails = details;
            ShoppingCart.GetCart(this.HttpContext).EmptyCart(_context);
            return View(order);
        }


        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == id);
            var order = await _context.Orders.Include(i => i.User).AsNoTracking().SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            var details = _context.OrderDetails.Where(detail => detail.Order.ID == order.ID).Include(detail => detail.Bag).ToList();

            order.OrderDetails = details;


            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //private bool OrderExists(int id)
        //{
        //    return _context.Orders.Any(e => e.ID == id);
        //}
    }
}
