using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBagsECommerce.Models
{
    public class Category
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Bag> Bags { get; set; }
    }
}
