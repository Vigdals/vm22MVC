using System.Diagnostics;
using System.Text.Json;
using getAPI;
using getAPIstuff.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using vm22MVC.Models;


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
            //Hugs å fikse dette litt funky med sluttspel når den tid kjem
            string[] arrayGroup = { "Gruppe A", "Gruppe B", "Gruppe C", "Gruppe D", "Gruppe E", "Gruppe F", "Gruppe G", "Gruppe H"};
            var username = new HttpContextAccessor().HttpContext?.User.Identity?.Name?.ToUpperInvariant();
            //using Linq
            var bettingGroup = new HttpContextAccessor().HttpContext?.User.Claims.Where(x => x.Type == "Group").Select(x => x.Value).FirstOrDefault();
            var jsonresult = "";
            for (int i = 0; i < tournamentModel.kampModels.Count; i++)
            {
                var kampModel = tournamentModel.kampModels[i];
                var tippeModel = tournamentModel.TippeModels[i];

                Debug.WriteLine($"Bruker: {username} tippa {tippeModel.Answer} på kamp {tippeModel.HjemmeLag}-{tippeModel.BorteLag}." +
                                $" I gruppe: {bettingGroup}. Og kampID er {kampModel.nifsKampId}");
                jsonresult = JsonConvert.SerializeObject(tournamentModel);
                //Vil legge data'en til ein SQL DB eller json her. .json fil er godt nok for no?
            }
            
            Debug.WriteLine($"Username: {username}. BettingGroup: {bettingGroup}. Json:\n{jsonresult}");
            var filename = $"c:\\home\\json\\{bettingGroup}_{username}.json";
            System.IO.File.AppendAllText(filename, jsonresult);
            
            //Gets the group name of the current form and redirects to the index with the group name as a parameter
            var currentGroup = tournamentModel.TippeModels.FirstOrDefault()?.Gruppe;
            //var index = arrayGroup.IndexOf(arrayGroup, currentGroup);
            var index = Array.FindIndex(arrayGroup, row => row.Contains(currentGroup));
            //kanskje legg inn  || arrayGroup[arrayGroup.Length] == ("Sluttspel") i iffen?
            Debug.WriteLine($"ArrayGroup.lenght = {arrayGroup.Length}");
            if (index == arrayGroup.Length-1) return RedirectToAction("FinishedTipping", "Tippekonk", tournamentModel);
            var changeToGroup = arrayGroup[index+1];
            return RedirectToAction("Index", new { groupName = changeToGroup });
        }
        public IActionResult Leaderboard()
        {
            return View();
        }
        public IActionResult FinishedTipping(TournamentModel tournamentModel)
        {
            
            return View(tournamentModel);
        }
    }
}