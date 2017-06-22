using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBagsECommerce.Models
{
    public class Bag
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        public int SupplierID { get; set; }

        public int CategoryID { get; set; }


        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? RawPrice { get; set; }
        public int? Stock { get; set; }

        [StringLength(500, ErrorMessage = "Description  cannot be longer than 500 characters.")]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public string PathOfFile { get; set; }

        public Category Category { get; set; }
        public Supplier Supplier { get; set; }

        //public ICollection<CartItem> CartItems { get; set; }

    }
}
