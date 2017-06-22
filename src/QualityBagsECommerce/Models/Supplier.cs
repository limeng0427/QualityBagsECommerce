using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBagsECommerce.Models
{
    public class Supplier
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(20, MinimumLength = 6)]
        [Display(Name = "Home Number")]
        public string HomeNumber { get; set; }

        [StringLength(20, MinimumLength = 6)]
        [Display(Name = "Work Number")]
        public string WorkNumber { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [StringLength(30, MinimumLength = 6)]
        public string Email { get; set; }

        public ICollection<Bag> Bags { get; set; }
    }
}
