using System.Diagnostics;
using getAPI;
using getAPIstuff.Api;
using getAPIstuff.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
using vm22MVC.Models;
using Newtonsoft.Json.Linq;

namespace vm22MVC.Controllers
{
    public class TippekonkController : Controller
    {
        public IActionResult Index()
        {
            var apiTournamentModel = new ApiCall().DoApiCall("https://api.nifs.no/tournaments/56/stages/");
            var apiTournamentReponse = apiTournamentModel.Response;
            ApiCall.CheckIfSuccess(apiTournamentReponse);

            var listModel = new List<TournamentModel>();
            var jsonSerialized = new jsonConvertAndIteration().JsonSerialize(apiTournamentModel.StringResponse);
            var jToken = JToken.Parse(jsonSerialized);

            foreach (var item in jToken)
            {
                if ((int)item.SelectToken("yearStart") != 2022) continue;
                var model = new TournamentModel()
                {
                    fullName = (string)item.SelectToken("fullName"),
                    yearStart = (int)item.SelectToken("yearStart"),
                    id = (int)item.SelectToken("id")
                };
                listModel.Add(model);
            }

            var GroupMatches = new List<List<kampModel>>();
            foreach (var item in listModel)
            {
                var TournamentMatches = new ApiCall().DoApiCall($"https://api.nifs.no/stages/{item.id}/matches/");
                List<kampModel> kampModelsList = jsonConvertAndIteration.JsonIteration(TournamentMatches.StringResponse);
                GroupMatches.Add(kampModelsList);
            }

            return View(GroupMatches);
        }
    }
}
