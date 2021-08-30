using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commerce.Data;
using E_commerce.Models;

namespace E_commerce.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            List<Category> categoryEntities = await _context.Category.ToListAsync();

            List<CategoryViewModel> model = new();
            model.AddRange(
                categoryEntities
                    .Select(ce => new CategoryViewModel 
                    { 
                        ID=ce.ID, 
                        Name=ce.Name
                    }));

            return View(model);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context
                .Category
                .FirstOrDefaultAsync(m => m.ID == id);

            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModel model = new() { ID=category.ID, Name=category.Name};

            return View(model);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    ModelState.AddModelError(nameof(CategoryViewModel.Name), "Category name is nrequired!");
                    return View(category);
                };

                Category categoryEntity = new()
                { 
                    Name=category.Name
                };
                _context.Add(categoryEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModel model = new() { ID = category.ID, Name = category.Name };

            return View(model);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] CategoryViewModel category)
        {
            if (id != category.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    ModelState.AddModelError(nameof(CategoryViewModel.Name), "Category name is nrequired!");
                    return View(category);
                };

                try
                {
                    Category entity = new()
                    {
                        ID = category.ID,
                        Name = category.Name
                    }; 

                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.ID == id);

            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModel model = new() { ID = category.ID, Name = category.Name };

            return View(model);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);

            _context.Category.Remove(category);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.ID == id);
        }
    }
}
