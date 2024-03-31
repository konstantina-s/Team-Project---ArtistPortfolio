using ArtistPortfolio.Data;
using ArtistPortfolio.Models.Models;
using ArtistPortfolio.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtistPortfolio.Controllers
{
    [Authorize]
    public class BiographyDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public BiographyDashboardController(ApplicationDbContext context, IImageService imageService) 
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: /BiographyDashboard
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_context.Biography == null)
            {
                return NotFound();
            }

            return View(await _context.Biography!.ToListAsync());
        }

        // GET: /BiographyDashboard/AddBiography
        [HttpGet]
        public IActionResult AddBiography()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBiography([Bind("BiographyContentMK,BiographyContentEN,ImageFile")] Biography biography)
        {
            if (ModelState.IsValid)
            {
                var base64String = _imageService.ConvertImageToBase64(Request.Form.Files["imageFile"]);

                biography.ImageData = base64String;
                _context.Biography.Add(biography);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(biography);
        }

        // GET: /BiographyDashboard/UpdateBiography/{id}
        [HttpGet]
        public async Task<IActionResult> UpdateBiography(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biography = await _context.Biography!.FirstOrDefaultAsync(b => b.Id == id);

            if (biography == null)
            {
                return NotFound();
            }

            return View(biography);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBiography(long? id, [Bind("Id,BiographyContentMK,BiographyContentEN,ImageFile")] Biography biography)
        {
            if (id != biography.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var base64String = _imageService.ConvertImageToBase64(Request.Form.Files["imageFile"]);

                    biography.ImageData = base64String;
                    _context.Biography.Add(biography);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var currentImage = await _context.Images!.FirstOrDefaultAsync(x => x.Id == biography.Id);
                    if (currentImage != null)
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
            return View(biography);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBiography(long? id)
        {
            var biography = await _context.Biography!.FirstOrDefaultAsync(x => x.Id == id);
            if (biography != null)
            {
                _context.Entry(biography).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}