using System.Diagnostics;
using System.Linq;
using getAPI;
using getAPIstuff.Api;
using getAPIstuff.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using vm22MVC.Models;

namespace vm22MVC.Controllers
{
    public class TippekonkController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Index(CollectiveModel collectiveModel, string groupName)
        {
            //if groupname is empty the rest of the code will not be excecuted - kul syntax
            //if (string.IsNullOrWhiteSpace(groupName)) return View(new TournamentModel() { kampModels = new List<kampModel>() });
            //gets tournament infomation. Important to get this because it gives us the ID for each group. From group A to H. 56 = world cup
            if (collectiveModel.User == null)
                collectiveModel = GetCollectiveModelFromTemp();
                
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
                List<kampModel> kampModelsList = jsonConvertAndIteration.JsonIteration(TournamentMatches.StringResponse);

                model.kampModels = kampModelsList;

                listModel.Add(model);
            }

            if (!string.IsNullOrWhiteSpace(groupName)) collectiveModel.TournamentModel.groupName = groupName;

            //Using Linq here with input fra drop down list in the index.cshtml:
            //return View(listModel.First(x => x.groupName == groupName));
            collectiveModel.TournamentModelList = new List<TournamentModel>();
            collectiveModel.TournamentModelList.AddRange(listModel);

            if (collectiveModel.TournamentModel == null) collectiveModel.TournamentModel = new TournamentModel();

            collectiveModel.TournamentModel = string.IsNullOrWhiteSpace(collectiveModel.TournamentModel.groupName)
                ? listModel.First()
                : listModel.First(x => x.groupName.Equals(collectiveModel.TournamentModel.groupName));

            return View(collectiveModel);
        }

        private CollectiveModel GetCollectiveModelFromTemp()
        {
            var collectiveModel =
                JsonConvert.DeserializeObject<CollectiveModel>((TempData["Collective"]?.ToString() ?? string.Empty));

            return collectiveModel ?? new CollectiveModel();
        }

        public IActionResult Submit([FromForm] TournamentModel tournamentModel)
        {
            foreach (var item in tournamentModel.TippeModels)
            {
                Debug.WriteLine(item.Answer);
            }

            //Gets the group name of the current form and redirects to the index with the group name as a parameter
            var gruppe = tournamentModel.TippeModels.FirstOrDefault()?.Gruppe;
            return RedirectToAction("Index", new { groupName = gruppe});
        }
        public IActionResult Leaderboard()
        {
            return View();
        }
    }
}