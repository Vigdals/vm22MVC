using System.Diagnostics;
using System.Linq;
using getAPI;
using getAPIstuff.Api;
using getAPIstuff.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using vm22MVC.Models;
using System.Data.SqlClient;
using System.Security.Claims;

namespace vm22MVC.Controllers
{
    [Authorize]
    public class TippekonkController : Controller
    {

        public IActionResult Index(TournamentModel tournamentModel)
        {
            //if groupname is empty the rest of the code will not be excecuted - kul syntax
            //if (string.IsNullOrWhiteSpace(groupName)) return View(new TournamentModel() { kampModels = new List<kampModel>() });
            //gets tournament infomation. Important to get this because it gives us the ID for each group. From group A to H. 56 = world cup
            var apiTournamentModel = new ApiCall().DoApiCall("https://api.nifs.no/tournaments/56/stages/");
            var apiTournamentReponse = apiTournamentModel.Response;
            ApiCall.CheckIfSuccess(apiTournamentReponse);

            var listModel = new List<TournamentModel>();
            var jsonSerialized = new jsonConvertAndIteration().JsonSerialize(apiTournamentModel.StringResponse);
            var jToken = JToken.Parse(jsonSerialized);

            //iterates through the tournament matches in 2022. Gotta change this in 2026 when next world cup is
            foreach (var item in jToken)
            {
                if ((int)item.SelectToken("yearStart") != 2022) continue;
                var model = new TournamentModel()
                {
                    groupName = (string)item.SelectToken("groupName"),
                    yearStart = (int)item.SelectToken("yearStart"),
                    id = (int)item.SelectToken("id")
                };
                //Gets all matches for all the groups through api call
                var TournamentMatches = new ApiCall().DoApiCall($"https://api.nifs.no/stages/{model.id}/matches/");
                var kampModelsList = jsonConvertAndIteration.JsonIteration(TournamentMatches.StringResponse);

                model.kampModels = kampModelsList;

                listModel.Add(model);
            }

            //Using Linq here with input fra drop down list in the index.cshtml:
            //return View(listModel.First(x => x.groupName == groupName));

            tournamentModel = string.IsNullOrWhiteSpace(tournamentModel.groupName)
                ? listModel.First()
                : listModel.First(x => x.groupName.Equals(tournamentModel.groupName));

            return View(tournamentModel);
        }

        public IActionResult Submit([FromForm] TournamentModel tournamentModel)
        {
            var cnString =
                "Server=(localdb)\\mssqllocaldb;Database=vm22MVC;Trusted_Connection=True;MultipleActiveResultSets=true";
            var username = new HttpContextAccessor().HttpContext?.User.Identity?.Name?.ToUpperInvariant();
            var bettingGroup = "";
            var kampModels = new List<kampModel>();
            foreach (var item in tournamentModel.TippeModels)
            {
                //Save items to database
                //Kvifor er kampModels tom????
                Debug.WriteLine($"Bruker: {username} tippa {item.Answer} på kamp {item.HjemmeLag}-{item.BorteLag}." +
                                $" I gruppe: {tournamentModel.groupName}. Og kampID er {tournamentModel.kampModels[0].nifsKampId}");
                
            }

            //Gets the group name of the current form and redirects to the index with the group name as a parameter
            var group = tournamentModel.TippeModels.FirstOrDefault()?.Gruppe;
            return RedirectToAction("Index", new { groupName = group });
        }
        public IActionResult Leaderboard()
        {
            return View();
        }
    }
}