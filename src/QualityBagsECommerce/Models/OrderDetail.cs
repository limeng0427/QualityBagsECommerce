using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBagsECommerce.Models
{
    public class OrderDetail
    {
        public int ID { get; set; }
        public  int Quantity { get; set; }
        public  decimal Price { get; set; }

        public Bag Bag { get; set; }
        public Order Order { get; set; }
    }
}
