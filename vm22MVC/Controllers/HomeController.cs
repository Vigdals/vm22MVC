using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using vm22MVC.Models;
using NuGet.Configuration;

namespace vm22MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.User.Identity?.Name == null) return Redirect("/login");
            return View();
        }
        public IActionResult Tippekonk()
        {
            //@Debug.WriteLine($"Brukernavn {userModel.BrukerNavn} og gruppenamn er {userModel.GruppeNavn}");

            //TempData["Collective"] = JsonConvert.SerializeObject(new CollectiveModel(userModel));

            return RedirectToAction("Index", "Tippekonk" );
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("/login")]
        public IActionResult GetLoginScreen()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> LoginUser(
            string userName, string password, string returnUrl, string bettingGroup)
        {
            var loginValidation = ValidateLogin(userName, password);

            if (!loginValidation)
            {
                TempData["LoginFailed"] = $"The username or password is incorrect";

                return Redirect("/login");
            }

            await SignInUser(userName, bettingGroup);

            if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/"))
            {
                returnUrl = "/";
            }

            return Redirect(returnUrl);
        }

        private bool ValidateLogin(string userName, string password)
        {
            return true;
        }

        private async Task SignInUser(string userName, string bettingGroup)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                //new Claim("Group", bettingGroup)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}