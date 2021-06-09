using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmsCatalog.Data;
using FilmsCatalog.Models;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FilmsCatalog.Services;


namespace FilmsCatalog.Controllers
{
    [Authorize]
    public class FilmsController : Controller
    {
        private static readonly HashSet<String> AllowedExtensions = new HashSet<String> { ".jpg", ".jpeg", ".png" };

        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUserPermissionsService _userPermissions;

        public FilmsController(ApplicationDbContext context, UserManager<User> userManager, IHostingEnvironment hostingEnvironment, IUserPermissionsService userPermissions)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _userPermissions = userPermissions;
        }

        // GET: Films
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Films.Include(f => f.Creator).OrderBy(f => f.Name).ToListAsync());
        }

        // GET: Films/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.Include(p => p.Creator).FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create
        [Authorize]
        public IActionResult Create()
        {
            return View(new FilmCreateModel());
        }

        // POST: Films/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FilmCreateModel model)
        {
            if (model.Poster != null)
            {
                var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.Poster.ContentDisposition).FileName.Value.Trim('"'));
                var fileExt = Path.GetExtension(fileName);
                if (!AllowedExtensions.Contains(fileExt))
                {
                    this.ModelState.AddModelError(nameof(model.Poster), "This file type is prohibited");
                }
            }

            var user = await this._userManager.GetUserAsync(this.HttpContext.User);

            if (ModelState.IsValid)
            {
                var film = new Film
                {
                    Name = model.Name,
                    Description = model.Description,
                    Year = model.Year,
                    Director = model.Director,
                    CreatorId = user.Id
                };
                
                if (model.Poster != null)
                {
                    var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.Poster.ContentDisposition).FileName.Value.Trim('"'));
                    var fileExt = Path.GetExtension(fileName);

                    var posterPath = Path.Combine(this._hostingEnvironment.WebRootPath, "posters", film.Id.ToString("N") + fileExt);
                    film.PosterPath = $"/posters/{film.Id:N}{fileExt}";
                    using (var fileStream = new FileStream(posterPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                    {
                        await model.Poster.CopyToAsync(fileStream);
                    }
                }
                _context.Films.Add(film);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Details", "Films", new { id = film.Id });
            }
            return View(model);
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null || !_userPermissions.CanEditFilm(film))
            {
                return NotFound();
            }

            var model = new FilmCreateModel
            {
                Name = film.Name,
                Description = film.Description,
                Year = film.Year,
                Director = film.Director
            };
            //получается, что старую фотку мы даже не выводим... лучше бы ее вывести просто для вида что ли и не перетировать при не изменении
            ViewBag.FilmId = id;
            return View(model);
        }

        // POST: Films/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, FilmCreateModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.SingleOrDefaultAsync(f => f.Id == id);
            if (film == null || !_userPermissions.CanEditFilm(film)) 
            {
                return this.NotFound();
            }

            if (model.Poster != null)
            {
                var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.Poster.ContentDisposition).FileName.Value.Trim('"'));
                var fileExt = Path.GetExtension(fileName);

                if (!AllowedExtensions.Contains(fileExt))
                {
                    this.ModelState.AddModelError(nameof(model.Poster), "This file type is prohibited");
                }
            }

            if (ModelState.IsValid)
            {
                film.Name = model.Name;
                film.Description = model.Description;
                film.Year = model.Year;
                film.Director = model.Director;

                if (model.Poster != null)
                {
                    //удаляем старый постер
                    var posterPath = Path.Combine(this._hostingEnvironment.WebRootPath, "posters", film.Id.ToString("N") + Path.GetExtension(film.PosterPath));
                    System.IO.File.Delete(posterPath);
                    film.PosterPath = null;

                    //добавляем новый                   
                    var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.Poster.ContentDisposition).FileName.Value.Trim('"'));
                    var fileExt = Path.GetExtension(fileName);

                    posterPath = Path.Combine(this._hostingEnvironment.WebRootPath, "posters", film.Id.ToString("N") + fileExt);
                    film.PosterPath = $"/posters/{film.Id:N}{fileExt}";
                    using (var fileStream = new FileStream(posterPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                    {
                        await model.Poster.CopyToAsync(fileStream);
                    }
                }                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .Include(f => f.Creator)
                .Include(f => f.Catalogs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null || !_userPermissions.CanEditFilm(film))
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FirstOrDefaultAsync(m => m.Id == id);

            if (film == null || !_userPermissions.CanEditFilm(film))
            {
                return NotFound();
            }

            var posterPath = Path.Combine(this._hostingEnvironment.WebRootPath, "posters", film.Id.ToString("N") + Path.GetExtension(film.PosterPath));
            System.IO.File.Delete(posterPath);
            _context.Films.Remove(film);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
