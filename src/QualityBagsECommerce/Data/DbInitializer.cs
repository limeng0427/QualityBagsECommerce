using Microsoft.EntityFrameworkCore;
using QualityBagsECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBagsECommerce.Data
{
    public class DbInitializer
    {
        public static void Initialize(BagContext context)
        {
            context.Database.EnsureCreated();
            //context.Database.Migrate();

            if (context.Bags.Any())
            {
                return; // DB has been seeded
            }

            var categories = new Category[]
            {
                new Category {Name = "Men’s Bags"},
                new Category {Name = "Women’s Bags"},
                new Category {Name = "Children’s Bags"},
            };
            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var suppliers = new Supplier[]
            {
                new Supplier {Name = "Supplier 1",
                MobileNumber="020-123456",
                Email = "Admin@Supplier1.com"
                },
                new Supplier {Name = "Supplier 2",
                MobileNumber="021-123456",
                Email = "Admin@Supplier2.com"
                },
                new Supplier {Name = "Supplier 3",
                MobileNumber="022-123456",
                Email = "Admin@Supplier3.com"
                },
            };
            foreach (Supplier s in suppliers)
            {
                context.Suppliers.Add(s);
            }
            context.SaveChanges();



            var bags = new Bag[]
            {
                new Bag {Name = "Bag 1", 
                SupplierID = suppliers.Single( s => s.Name == "Supplier 1").ID,
                CategoryID = categories.Single( c => c.Name == "Men’s Bags").ID,
                Price = 100,
                Description = "This is Bag 1"
                },
                new Bag {Name = "Bag 2",
                SupplierID = suppliers.Single( s => s.Name == "Supplier 2").ID,
                CategoryID = categories.Single( c => c.Name == "Women’s Bags").ID,
                Price = 100,
                Description = "This is Bag 2"
                },
                new Bag {Name = "Bag 3",
                SupplierID = suppliers.Single( s => s.Name == "Supplier 3").ID,
                CategoryID = categories.Single( c => c.Name == "Children’s Bags").ID,
                Price = 100,
                Description = "This is Bag 3"
                },
            };
            foreach (Bag b in bags)
            {
                context.Bags.Add(b);
            }
            context.SaveChanges();

        }
    }
}
