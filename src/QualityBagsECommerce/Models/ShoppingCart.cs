using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QualityBagsECommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBagsECommerce.Models
{
    public class ShoppingCart
    {
        public String ShoppingCartID { get; set; }
        public const string CartSessionKey = "cartId";
        public static ShoppingCart GetCart(HttpContext context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartID = cart.GetShoppingCartID(context);
            return cart;
        }

        public void AddToCart(Bag bag, BagContext bagDB)
        {
            var cartItem = bagDB.CartItems.SingleOrDefault(c => c.ShoppingCartID == ShoppingCartID && c.Bag.ID == bag.ID);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Bag = bag,
                    ShoppingCartID = ShoppingCartID,
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };
                bagDB.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }
            bagDB.SaveChanges();
        }

        public int RemoveFromCart(int id, BagContext bagDB)
        {
            var cartItem = bagDB.CartItems.SingleOrDefault(cart => cart.ShoppingCartID == ShoppingCartID && cart.Bag.ID == id);
            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    itemCount = cartItem.Quantity;
                }
                else
                {
                    bagDB.CartItems.Remove(cartItem);
                }
                bagDB.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart(BagContext bagDB)
        {
            var cartItems = bagDB.CartItems.Where(cart => cart.ShoppingCartID == ShoppingCartID);
            foreach (var cartItem in cartItems)
            {
                bagDB.CartItems.Remove(cartItem);
            }
            bagDB.SaveChanges();
        }

        public List<CartItem> GetCartItems(BagContext bagDB)
        {
            List<CartItem> cartItems = bagDB.CartItems.Include(i => i.Bag).Where(cartItem => cartItem.ShoppingCartID == ShoppingCartID).ToList();

            return cartItems;

        }

        public int GetCount(BagContext BagDB)
        {
            int? count =
                (from cartItems in BagDB.CartItems where cartItems.ShoppingCartID == ShoppingCartID select (int?)cartItems.Quantity).Sum();
            return count ?? 0;
        }

        public decimal GetTotal(BagContext bagDB)
        {
            decimal? total = (from cartItems in bagDB.CartItems
                              where cartItems.ShoppingCartID == ShoppingCartID
                              select (int?)cartItems.Quantity * cartItems.Bag.Price).Sum();
            return total ?? decimal.Zero;
        }

        public int GetTotalItem(BagContext bagDB)
        {
            int? totalItem = (from cartItems in bagDB.CartItems
                              where cartItems.ShoppingCartID == ShoppingCartID
                              select (int?)cartItems.Quantity).Sum();
            return totalItem ?? 0;
        }

        public string GetShoppingCartID(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) == null)
            {
                Guid tempCartId = Guid.NewGuid();
                context.Session.SetString(CartSessionKey, tempCartId.ToString());
            }
            return context.Session.GetString(CartSessionKey).ToString();
        }

    }
}
