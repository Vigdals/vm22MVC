using getAPI;
using getAPIstuff.Api;
using getAPIstuff.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
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
            if (string.IsNullOrWhiteSpace(groupName))
            {
                return View(listModel.First());
            }

            //If the list model contains a group name that has a alphabetical identifier it returns said model with that group name.
            //If not it will attempt to find the corresponding group with a numerical identifier.
            if (listModel.Any(x => x.groupName.Contains(groupName))) return View(listModel.First(x => x.groupName == groupName));
            {
                try
                {
                    //Switch that returns the corresponding group name with the group name numerical identifier.
                    //This is done since some values int he api does not use group as a alphabetical identifier but rather as a numerical.
                    return groupName.ToLower() switch
                    {
                        "gruppe a" => View(listModel.First(x => x.groupName == "Gruppe 1")),
                        "gruppe b" => View(listModel.First(x => x.groupName == "Gruppe 2")),
                        "gruppe c" => View(listModel.First(x => x.groupName == "Gruppe 3")),
                        "gruppe d" => View(listModel.First(x => x.groupName == "Gruppe 4")),
                        "gruppe e" => View(listModel.First(x => x.groupName == "Gruppe 5")),
                        "gruppe f" => View(listModel.First(x => x.groupName == "Gruppe 6")),
                        "gruppe g" => View(listModel.First(x => x.groupName == "Gruppe 7")),
                        "gruppe h" => View(listModel.First(x => x.groupName == "Gruppe 8")),
                        _ => View(listModel.First())
                    };
                }
                catch
                {
                    //If the switch fails to return a view due to an exception from not finding a numerical group in the incoming
                    //api request. It returns an empty TournamentModel and sends a message to the user with Viewbag
                    ViewBag.Message = $"Fant ikkje {groupName}!";
                    return View(new TournamentModel() { kampModels = new List<kampModel>() });
                }
            }
        }
    }
}
