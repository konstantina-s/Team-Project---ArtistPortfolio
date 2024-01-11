using ArtistPortfolio.Models.DTO;
using ArtistPortfolio.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtistPortfolio.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public IActionResult Register()
        //{
        //    UserRegistrationDTO model = new UserRegistrationDTO();
        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register(UserRegistrationDTO request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userCheck = await _userManager.FindByEmailAsync(request.Email);
        //        if (userCheck == null)
        //        {
        //            var user = new ApplicationUser
        //            {
        //                FirstName = request.FirstName,
        //                LastName = request.LastName,
        //                UserName = request.Email,
        //                NormalizedUserName = request.Email,
        //                Email = request.Email
        //            };
        //            var result = await _userManager.CreateAsync(user, request.Password);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Login");
        //            }
        //            else
        //            {
        //                if (result.Errors.Count() > 0)
        //                {
        //                    foreach (var error in result.Errors)
        //                    {
        //                        ModelState.AddModelError("message", error.Description);
        //                    }
        //                }
        //                return View(request);
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("message", "Email already exists.");
        //            return View(request);
        //        }
        //    }
        //    return View(request);
        //}

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDTO model = new UserLoginDTO();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "ImageDashboard");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}