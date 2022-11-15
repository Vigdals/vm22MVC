using getAPI;
using getAPIstuff.Api;
using getAPIstuff.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using vm22MVC.Models;
using vm22MVC.Service;

namespace vm22MVC.Controllers
{
    public class vm22Controller : Controller
    {
        ////injecting the interface from DoApiCallService
        private readonly IDoApiCallService _callService = new DoApiCallService();

        public IActionResult Index(string groupName, string year)
        {
            if (string.IsNullOrWhiteSpace(groupName)) return View(new TournamentModel() { kampModels = new List<kampModel>() });
            var apiTournamentModel = new ApiCall().DoApiCall("https://api.nifs.no/tournaments/56/stages/");
            var apiTournamentReponse = apiTournamentModel.Response;
            ApiCall.CheckIfSuccess(apiTournamentReponse);

            var listModel = new List<TournamentModel>();
            var jsonSerialized = new jsonConvertAndIteration().JsonSerialize(apiTournamentModel.StringResponse);
            var jToken = JToken.Parse(jsonSerialized);

            //iterates through the tournament matches in 2022. Gotta change this in 2026 when next world cup is
            foreach (var item in jToken)
            {
                if ((int)item.SelectToken("yearStart") != Int32.Parse(year)) continue;
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

            //Using Linq here with input fra drop down list in the index.cshtml:
            return View(listModel.First(x => x.groupName == groupName));

        }

        public IActionResult Submit([FromForm] TournamentModel tournamentModel)
        {
            foreach (var item in tournamentModel.TippeModels)
            {
                Debug.WriteLine(item.Answer);
            }

            //Gets the group name of the current form and redirects to the index with the group name as a parameter
            var gruppe = tournamentModel.TippeModels.FirstOrDefault()?.Gruppe;
            return RedirectToAction("Index", new { groupName = gruppe });
        }
        //public IActionResult Create()
        //{
        //    var apiViewModel = new apiModel();
        //    return View(apiViewModel);
        //}

        //[HttpPost]
        //public IActionResult Create([Bind("stagesid, teamId, tournamentId, date")] apiModel responseModel)
        //{
        //    var kampModels = _callService.DoVmApiCall(responseModel);

        //    return View("Index", kampModels);
        //}
    }
}
