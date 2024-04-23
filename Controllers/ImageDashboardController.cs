using ArtistPortfolio.Data;
using ArtistPortfolio.Models.Models;
using ArtistPortfolio.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtistPortfolio.Controllers
{
    [Authorize]
    public class ImageDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public ImageDashboardController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: /ImageDashboard
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_context.Images == null)
            {
                return NotFound();
            }

            return View(await _context.Images!.OrderByDescending(p => p.Id).ToListAsync());
        }

        // GET: /ImageDashboard/ImageDetail/{id}
        [HttpGet]
        public async Task<IActionResult> ImageDetail(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images!.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: /ImageDashboard/AddImage
        [HttpGet]
        public IActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage([Bind("TitleMK,TitleEN,DescMK,DescEN,TechniqueMK,TechniqueEN,Format,ImageFile,IsForSale")] Image image)
        {
            if (ModelState.IsValid)
            {
                var base64String = _imageService.ConvertImageToBase64(Request.Form.Files["imageFile"]);

                image.Data = base64String;
                _context.Images.Add(image);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(image);
        }

        // GET: /ImageDashboard/UpdateImage/{id}
        [HttpGet]
        public async Task<IActionResult> UpdateImage(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images!.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateImage(long? id, [Bind("Id,TitleMK,TitleEN,DescMK,DescEN,TechniqueMK,TechniqueEN,Format,ImageFile,IsForSale")] Image image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var base64String = _imageService.ConvertImageToBase64(Request.Form.Files["imageFile"]);

                    image.Data = base64String;
                    _context.Images.Add(image);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var currentImage = await _context.Images!.FirstOrDefaultAsync(x => x.Id == image.Id);
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
            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(long? id)
        {
            var image = await _context.Images!.FirstOrDefaultAsync(x => x.Id == id);
            if (image != null)
            {
                _context.Entry(image).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}