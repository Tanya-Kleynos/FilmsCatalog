using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsCatalog.Data;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FilmsCatalog.Services;

namespace FilmsCatalog.Controllers
{
    [Authorize]
    public class CatalogsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserPermissionsService _userPermissions;

        public CatalogsController(ApplicationDbContext context, UserManager<User> userManager, IUserPermissionsService userPermissions)
        {
            _context = context;
            _userManager = userManager;
            _userPermissions = userPermissions;
        }

        // GET: Catalogs
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var item = await this._context.Catalogs
                .Include(c => c.Films)
                .Include(c => c.Creator)
                .OrderBy(c => c.Date)
                .ToListAsync();
            return View(item);
        }

        // GET: Catalogs/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalogs
                .Include(c => c.Films)
                .ThenInclude(c => c.Film)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catalog == null)
            {
                return NotFound();
            }

            return View(catalog);
        }

        // GET: Catalogs/Create
        [Authorize]
        public IActionResult Create()
        {
            return View(new CatalogCreateModel());
        }

        // POST: Catalogs/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CatalogCreateModel model)
        {
            var user = await this._userManager.GetUserAsync(this.HttpContext.User);
            if (ModelState.IsValid)
            {
                var now = DateTime.UtcNow;
                var catalog = new Catalog
                {
                    Name = model.Name,
                    Description = model.Description,
                    CreatorId = user.Id,
                    Date = now
                };
                
                _context.Catalogs.Add(catalog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Catalogs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalogs.FindAsync(id);
            if (catalog == null || !_userPermissions.CanEditCatalog(catalog))
            {
                return NotFound();
            }

            var model = new CatalogCreateModel
            {
                Name = catalog.Name,
                Description = catalog.Description
            };
            ViewBag.CatalogId = id;
            return View(model);
        }

        // POST: Catalogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, CatalogCreateModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalog = await _context.Catalogs.SingleOrDefaultAsync(c => c.Id == id);
            if (catalog == null || !_userPermissions.CanEditCatalog(catalog))
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                catalog.Name = model.Name;
                catalog.Description = model.Description;
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Catalogs", new { id = catalog.Id });
            }
            return View(model);
        }

        // GET: Catalogs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var catalog = await _context.Catalogs
                .Include(c => c.Creator)
                .Include(c => c.Films)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catalog == null || !_userPermissions.CanEditCatalog(catalog))
            {
                return NotFound();
            }
            ViewBag.CatalogId = id;
            return View(catalog);
        }

        // POST: Catalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var catalog = await _context.Catalogs.FirstOrDefaultAsync(m => m.Id == id);
            if (catalog == null || !_userPermissions.CanEditCatalog(catalog))
            {
                return NotFound();
            }

            _context.Catalogs.Remove(catalog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
