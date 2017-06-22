using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QualityBagsECommerce.Models.BagCategoryModels;
using QualityBagsECommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace QualityBagsECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly BagContext _context;

        public HomeController(BagContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var bagContext2 = _context.Bags.AsNoTracking().ToList();
            var cates2 = _context.Categories.AsNoTracking().ToList();
            return View(new BagCategory { Bag = bagContext2, Category = cates2 });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact us.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
