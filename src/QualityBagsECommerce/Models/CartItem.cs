using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBagsECommerce.Models
{
    public class CartItem
    {
        [Key]
        public int ID { get; set; }
        public String ShoppingCartID { get; set; }
        public int Quantity { get; set; }
        //public int OrderID { get; set; }
        public DateTime DateCreated { get; set; }  
        public Bag Bag { get; set; }

        //public Order Order { get; set; }
    }
}
