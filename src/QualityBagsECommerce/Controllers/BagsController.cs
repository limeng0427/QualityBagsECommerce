using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBagsECommerce.Data;
using QualityBagsECommerce.Models;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace QualityBagsECommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BagsController : Controller
    {
        private readonly BagContext _context;
        private readonly IHostingEnvironment _hostingEnv;

        
        public BagsController(BagContext context, IHostingEnvironment hEnv)
        {
            _context = context;
            _hostingEnv = hEnv;
        }

        // GET: Bags
        public async Task<IActionResult> Index(int? CategoryID,
                                               string sortOrder,
                                               string currentFilter,
                                               string searchString,
                                               int? page)
        {
            if(CategoryID != null)
            {
               return View(_context.Bags.Include(b => b.Category).Where(c => c.Category.ID == CategoryID).AsNoTracking());
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var bags = from b in _context.Bags
                           select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                bags = bags.Where(b => b.Name.Contains(searchString)
                                       || b.Description.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    bags = bags.OrderByDescending(b => b.Name);
                    break;
                case "supplier":
                    bags = bags.OrderBy(b => b.SupplierID);
                    break;
                case "supplier_desc":
                    bags = bags.OrderByDescending(b => b.SupplierID);
                    break;
                case "category":
                    bags = bags.OrderBy(b => b.CategoryID);
                    break;
                case "category_desc":
                    bags = bags.OrderByDescending(b => b.CategoryID);
                    break;
                default:
                    bags = bags.OrderBy(b => b.SupplierID);
                    break;
            }

            int pageSize = 100;
            return View(await PaginatedList<Bag>.CreateAsync(bags.Include(b => b.Supplier).Include(b => b.Category).AsNoTracking(), page ?? 1, pageSize));

            //var bags = _context.Bags
            // .Include(b => b.Supplier).Include(b => b.Category)
            // .AsNoTracking();
            ////var bags2 = _context.Bags
            ////.Include(b => b.Category)
            ////.AsNoTracking();
            //return View(await bags.ToListAsync());
        }

        // GET: Bags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bag = await _context.Bags.SingleOrDefaultAsync(m => m.ID == id);
            if (bag == null)
            {
                return NotFound();
            }

            return View(bag);
        }

        // GET: Bags/Create
        public IActionResult Create()
        {
            PopulateSuppliersDropDownList();
            PopulateCategoriesDropDownList();
            return View();
        }

        // POST: Bags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CategoryID,Description,Name,PathOfFile,Price,SupplierID")] Bag bag, IList<IFormFile> _files)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(bag);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}

            var relativeName = "";
            var fileName = "";

            PopulateSuppliersDropDownList(bag.SupplierID);
            PopulateCategoriesDropDownList(bag.CategoryID);

            if (_files.Count < 1)
            {
                relativeName = "/Images/Default.jpg";
            }
            else
            {
                foreach (var file in _files)
                {
                    fileName = ContentDispositionHeaderValue
                                      .Parse(file.ContentDisposition)
                                      .FileName
                                      .Trim('"');
                    //Path for localhost
                    relativeName = "/Images/BagImages/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;

                    using (FileStream fs = System.IO.File.Create(_hostingEnv.WebRootPath + relativeName))
                    {
                        await file.CopyToAsync(fs);
                        fs.Flush();
                    }
                }
            }
            bag.PathOfFile = relativeName;
            bag.StartDate = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(bag);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " + "see your system administrator.");
            }



            return View(bag);
        }

        // GET: Bags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bag = await _context.Bags.SingleOrDefaultAsync(m => m.ID == id);
            if (bag == null)
            {
                return NotFound();
            }
            PopulateSuppliersDropDownList(bag.SupplierID);
            PopulateCategoriesDropDownList(bag.CategoryID);
            return View(bag);
        }

        // POST: Bags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CategoryID,Description,Name,PathOfFile,Price,SupplierID")] Bag bag, IList<IFormFile> _files)
        {

            var relativeName = "";
            var fileName = "";

            if (id != bag.ID)
            {
                return NotFound();
            }             

            if (_files.Count < 1)
            {
                // if not file is selected, file path will not be changed
                //relativeName = "/Images/Default.jpg";
                //relativeName = bag.PathOfFile;
            }
            else
            {
                foreach (var file in _files)
                {
                    fileName = ContentDispositionHeaderValue
                                        .Parse(file.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                    //Path for localhost
                    relativeName = "/Images/BagImages/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;

                    using (FileStream fs = System.IO.File.Create(_hostingEnv.WebRootPath + relativeName))
                    {
                        await file.CopyToAsync(fs);
                        fs.Flush();
                    }
                }
                bag.PathOfFile = relativeName;
                bag.StartDate = DateTime.Now;
            }
            
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(bag);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " + "see your system administrator.");
            }

            //if (ModelState.IsValid)
            //{
                //try
                //{
                //    _context.Update(bag);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!BagExists(bag.ID))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                //return RedirectToAction("Index");
            //}
            PopulateSuppliersDropDownList(bag.SupplierID);
            PopulateCategoriesDropDownList(bag.CategoryID);
            return View(bag);
        }

        private void PopulateSuppliersDropDownList(object selectedSupplier = null)
        {
            var suppliersQuery = from d in _context.Suppliers
                                   orderby d.Name
                                   select d;
            ViewBag.SupplierID = new SelectList(suppliersQuery.AsNoTracking(), "ID", "Name",
            selectedSupplier);
        }

        private void PopulateCategoriesDropDownList(object selectedCategories = null)
        {
            var categoriesQuery = from d in _context.Categories
                                 orderby d.Name
                                 select d;
            ViewBag.CategoryID = new SelectList(categoriesQuery.AsNoTracking(), "ID", "Name",
            selectedCategories);
        }

        // GET: Bags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bag = await _context.Bags.SingleOrDefaultAsync(m => m.ID == id);
            if (bag == null)
            {
                return NotFound();
            }

            return View(bag);
        }

        // POST: Bags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bag = await _context.Bags.SingleOrDefaultAsync(m => m.ID == id);
            _context.Bags.Remove(bag);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                TempData["BagUsed"] = "The Bag being deleted has been used in previous orders.Delete those orders before trying again.";
                return RedirectToAction("Delete");
            }

            return RedirectToAction("Index");
        }

        private bool BagExists(int id)
        {
            return _context.Bags.Any(e => e.ID == id);
        }
    }
}
