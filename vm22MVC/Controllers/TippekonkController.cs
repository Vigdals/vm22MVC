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
        
        public IActionResult Index(string groupName)
        {
            //if groupname is empty the rest of the code will not me excecuted - kul syntax
            if (string.IsNullOrWhiteSpace(groupName)) return View(new TournamentModel(){kampModels = new List<kampModel>()});

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
                    groupName = (string)item.SelectToken("groupName"),
                    yearStart = (int)item.SelectToken("yearStart"),
                    id = (int)item.SelectToken("id")
                };
                var TournamentMatches = new ApiCall().DoApiCall($"https://api.nifs.no/stages/{model.id}/matches/");
                List<kampModel> kampModelsList = jsonConvertAndIteration.JsonIteration(TournamentMatches.StringResponse);

                model.kampModels = kampModelsList;

                listModel.Add(model);
            }

            //Using Linq here med input fra drop down list:
            return View(listModel.First(x => x.groupName == groupName));
            
        }
    }
}
