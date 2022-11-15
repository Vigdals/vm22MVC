using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using vm22MVC.Models;

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
            return View();
        }
        public IActionResult Tippekonk(UserModel userModel)
        {
            @Debug.WriteLine($"Brukernavn {userModel.BrukerNavn} og gruppenamn er {userModel.GruppeNavn}");
            return RedirectToAction("Index", "Tippekonk", new {collectiveModel = new CollectiveModel(userModel)}, null);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}