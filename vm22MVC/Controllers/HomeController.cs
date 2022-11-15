using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
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

            TempData["Collective"] = JsonConvert.SerializeObject(new CollectiveModel(userModel));

            return RedirectToAction("Index", "Tippekonk" );
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