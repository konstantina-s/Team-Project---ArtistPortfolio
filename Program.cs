using ArtistPortfolio.Data;
using ArtistPortfolio.Models.Identity;
using ArtistPortfolio.Services.Implementations;
using ArtistPortfolio.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var serverVersion = ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddDbContext<ApplicationDbContext>(
            dbContextOptions => dbContextOptions.
                UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/Login";
    options.Cookie.Name = "ArtistPortfolioCookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Account/Login";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddRazorRuntimeCompilation();

builder.Services.AddPortableObjectLocalization()
    .Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("mk-MK"),
            new CultureInfo("en-US")
        };

        options.DefaultRequestCulture = new RequestCulture("mk-MK");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
        options.RequestCultureProviders = new List<IRequestCultureProvider>
        {
            new QueryStringRequestCultureProvider(),
            new CookieRequestCultureProvider()
        };
    });

builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

