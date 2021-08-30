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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            List<Product> entities = await _context.Product.Include(p=>p.Category).ToListAsync();

            List<ProductViewModel> models = new();

            models.AddRange(entities.Select(e => new ProductViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                Category = new()
                {
                    ID = e.Category.ID,
                    Name = e.Category.Name
                }
            }));

            return View(models);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productEntity = await _context.Product.Include(p=>p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (productEntity == null)
            {
                return NotFound();
            }

            ProductViewModel model = new()
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Price = productEntity.Price,
                Category = new CategoryViewModel { ID = productEntity.Category.ID, Name = productEntity.Category.Name }
            };
            return View(model);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            List<Category> categoriesEntities= await _context.Category.ToListAsync();

            ProductViewModel model = new();

            model.AvailableCategories.AddRange(
                categoriesEntities.Select(c=>new SelectListItem { Text=c.Name, Value=c.ID.ToString()}));

            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SelectedCategoryId,Name,Price")] ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                Category categoryEntity = await _context.Category.FirstOrDefaultAsync(c => c.ID == product.SelectedCategoryId);
                if(categoryEntity is null)
                {
                    ModelState.AddModelError(nameof(ProductViewModel.SelectedCategoryId), "Category not found!");

                    return View(product);
                }

                Product productEntity = new()
                {
                    Name=product.Name,
                    Price=product.Price,
                    Category=categoryEntity
                };

                _context.Add(productEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(p=>p.Category).FirstOrDefaultAsync(p=>p.Id==id);
            if (product == null)
            {
                return NotFound();
            }
            ProductViewModel model = new()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = new() { ID = product.Category.ID, Name = product.Category.Name }

            };

            return View(model);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] ProductViewModel product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Product productEntity = new()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price
                    };

                    _context.Update(productEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(p=>p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ProductViewModel model = new()
            {
                Id=product.Id,
                Name=product.Name,
                Price=product.Price,
                Category = new()
                {
                    ID=product.Category.ID,
                    Name=product.Category.Name
                }
            };
            return View(model);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
