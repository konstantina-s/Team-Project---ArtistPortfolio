using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ArtistPortfolio.Data;
using ArtistPortfolio.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace ArtistPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ApplicationDbContext context, IStringLocalizer<HomeController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        // GET: /
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_context.Images == null)
            {
                return NotFound();
            }

            return View(await _context.Images.Where(p => p.IsForSale == true).ToListAsync());
        }

        // GET: /Home/Exhibits
        [HttpGet]
        public async Task<IActionResult> Exhibits()
        {
            if (_context.Images == null)
            {
                return NotFound();
            }

            return View(await _context.Images.Where(p => p.IsForSale == false).ToListAsync());
        }

        // GET: /Home/Detail/{id}
        [HttpGet]
        public async Task<IActionResult> Detail(long? id)
        {
            if (_context.Images == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FirstOrDefaultAsync(p => p.Id == id);

            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: /Home/Biography
        [HttpGet]
        public async Task<IActionResult> Biography()
        {
            if (_context.Biography == null)
            {
                return NotFound();
            }

            return View(await _context.Biography!.ToListAsync());
        }

        // GET: /Home/Contact
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        // POST: Change Language
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}