using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBagsECommerce.Models.ShoppingCartViewModels
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public int ItemTotal { get; set; }
    }
}
