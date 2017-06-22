using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBagsECommerce.Data;
using QualityBagsECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using QualityBagsECommerce.Models.BagCategoryModels;

namespace QualityBagsECommerce.Controllers
{
    [AllowAnonymous]
    [Authorize(Roles = "Member")]
    public class MemberBagsController : Controller
    {
        private readonly BagContext _context;

        public MemberBagsController(BagContext context)
        {
            _context = context;    
        }

        // GET: MemberBags
        public IActionResult Index(int? CategoryID, string searchString )
        {
            //var bagContext = _context.Bags.Include(b => b.Category).Include(b => b.Supplier);
            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                var bags = _context.Bags.Where(b => b.Name.Contains(searchString) || b.Description.Contains(searchString)).ToList();
                var category = _context.Categories.Where(c => c.ID == CategoryID).AsNoTracking().ToList();
                return View(new BagCategory { Bag = bags, Category = category });
            }
            
            if (CategoryID != null)
            {
                var cates1 = _context.Categories.Where(c => c.ID == CategoryID).AsNoTracking().ToList();
                var bagContext1 = _context.Bags.Where(c => c.Category.ID == CategoryID).AsNoTracking().ToList();
                return View(new BagCategory { Bag = bagContext1, Category = cates1 });
            }
            else
            { 
            var bagContext2 = _context.Bags.AsNoTracking().ToList();
            var cates2 = _context.Categories.AsNoTracking().ToList();


            return View(new BagCategory { Bag = bagContext2, Category = cates2});
            }
        }

        // GET: MemberBags/Details/5
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

        // GET: MemberBags/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "MobileNumber");
            return View();
        }

        // POST: MemberBags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CategoryID,Description,Name,PathOfFile,Price,StartDate,SupplierID")] Bag bag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bag);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", bag.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "MobileNumber", bag.SupplierID);
            return View(bag);
        }

        // GET: MemberBags/Edit/5
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", bag.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "MobileNumber", bag.SupplierID);
            return View(bag);
        }

        // POST: MemberBags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CategoryID,Description,Name,PathOfFile,Price,StartDate,SupplierID")] Bag bag)
        {
            if (id != bag.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BagExists(bag.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", bag.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "MobileNumber", bag.SupplierID);
            return View(bag);
        }

        // GET: MemberBags/Delete/5
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

        // POST: MemberBags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bag = await _context.Bags.SingleOrDefaultAsync(m => m.ID == id);
            _context.Bags.Remove(bag);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool BagExists(int id)
        {
            return _context.Bags.Any(e => e.ID == id);
        }
    }
}
