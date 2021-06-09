using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsCatalog.Data;
using FilmsCatalog.Models;
using Microsoft.AspNetCore.Authorization;

namespace FilmsCatalog.Controllers
{
    [Authorize]
    public class CatalogFilmsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogFilmsController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        // GET: CatalogFilms/Create
        public async Task<IActionResult> Create(Guid? catalogId)
        {
            if(catalogId == null)
            {
                return this.NotFound();
            }

            var catalog = await _context.Catalogs.Include(c => c.Films).SingleOrDefaultAsync(c => c.Id == catalogId);

            if(catalog == null)
            {
                return this.NotFound();
            }

           /* var ListOfAvaliableFilms = _context.Films
                .Where(x => !catalog.Films.Any(f => f.FilmId == x.Id)); */

            var ListOfAvaliableFilms = this._context.Films
                .Where(x => !x.Catalogs.Any(z => z.CatalogId == catalogId)); //отсеяли тех, кто уже есть в каталоге
             
            //ViewData["CatalogId"] = new SelectList(_context.Catalogs, "Id", "Name"); //заменить на id конкретного каталога??????????!!!!!!!
            ViewData["FilmId"] = new SelectList(ListOfAvaliableFilms, "Id", "Name");
            ViewBag.CatalogId = catalogId;
            ViewBag.CatalogName = catalog.Name;
            ViewBag.FilmId = new SelectList(ListOfAvaliableFilms, "Id", "Name");
            return View(new CatalogFilmCreateModel());
        }

        // POST: CatalogFilms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? catalogId, CatalogFilmCreateModel model)
        {
            if (catalogId == null)
            {
                return this.NotFound();
            }

            var catalog = await _context.Catalogs.Include(c => c.Films).SingleOrDefaultAsync(c => c.Id == catalogId);

            if (catalog == null)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                var catalogFilm = new CatalogFilm
                {
                    CatalogId = catalog.Id,
                    FilmId = model.FilmId
                };

                _context.Add(catalogFilm);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Catalogs", new { id = catalog.Id });
            }
            this.ViewBag.Catalog = catalog;
            ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Name", model.FilmId);
            return View(model);
        }

        

        // GET: CatalogFilms/Delete/5
        public async Task<IActionResult> Delete(Guid? catalogId, Guid? filmId)
        {
            if (catalogId == null || filmId == null)
            {
                return NotFound();
            }

            var catalogFilm = await _context.CatalogFilms
                .Include(c => c.Catalog)
                .Include(c => c.Film)
                .SingleOrDefaultAsync(m => m.CatalogId == catalogId && m.FilmId == filmId);
            if (catalogFilm == null)
            {
                return NotFound();
            }
            ViewBag.CatalogId = catalogId;
            return View(catalogFilm);
        }

        // POST: CatalogFilms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid catalogId, Guid filmId)
        {
            var catalogFilm = await _context.CatalogFilms.SingleOrDefaultAsync(m => m.CatalogId == catalogId && m.FilmId == filmId);
            _context.CatalogFilms.Remove(catalogFilm);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Catalogs", new { id = catalogId });
        }
    }
}
