using System.Diagnostics;
using System.Text.Json;
using getAPI;
using getAPIstuff.Api;
using getAPIstuff.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            //hardcoding in sluttspill, kinda wonky
            tournamentModel.groupName = "Sluttspill";
            tournamentModel = string.IsNullOrWhiteSpace(tournamentModel.groupName)
                ? listModel.First()
                : listModel.First(x => x.groupName.Equals(tournamentModel.groupName));

            return View(tournamentModel);
        }

        public IActionResult Submit([FromForm] TournamentModel tournamentModel)
        {
            //Hugs å fikse dette litt funky med sluttspel når den tid kjem
            string[] arrayGroup =
                { "Gruppe A", "Gruppe B", "Gruppe C", "Gruppe D", "Gruppe E", "Gruppe F", "Gruppe G", "Gruppe H", "Sluttspill" };
            var username = new HttpContextAccessor().HttpContext?.User.Identity?.Name?.ToUpperInvariant();
            var bettingGroup = new HttpContextAccessor().HttpContext?.User.Claims.Where(x => x.Type == "Group")
                .Select(x => x.Value).FirstOrDefault();

            //move nifsID from tournamentModel.KampModels into tournamentmodel.Tippemodels and adding username
            for (int i = 0; i < tournamentModel.TippeModels.Count; i++)
            {
                tournamentModel.TippeModels[i].nifsKampId = tournamentModel.kampModels[i].nifsKampId;
                tournamentModel.TippeModels[i].userName = username;
            }
            //put tippemodel into json format
            var jsonResult = JsonConvert.SerializeObject(tournamentModel.TippeModels);          
            var filename = $"c:\\home\\json\\sluttspel2\\{bettingGroup}_{username}.json";

            System.IO.File.AppendAllText(filename, jsonResult);

            //Gets the group name of the current form and redirects to the index with the group name as a parameter
            var currentGroup = tournamentModel.TippeModels.FirstOrDefault()?.Gruppe;


            //var index = arrayGroup.IndexOf(arrayGroup, currentGroup);

            var index = Array.FindIndex(arrayGroup, row => row.Contains(currentGroup));

            if (index == arrayGroup.Length - 1)
                return RedirectToAction("FinishedTipping", "Tippekonk", tournamentModel);

            var changeToGroup = arrayGroup[index + 1];

            return RedirectToAction("Index", new { groupName = changeToGroup });
        }

        public IActionResult FinishedTipping(TournamentModel tournamentModel)
        {
            //var username = new HttpContextAccessor().HttpContext?.User.Identity?.Name?.ToUpperInvariant();
            //var bettingGroup = new HttpContextAccessor().HttpContext?.User.Claims.Where(x => x.Type == "Group")
            //    .Select(x => x.Value).FirstOrDefault();
            //var turnering = new TournamentModel();
            //var filePath = $"c:\\home\\json\\sluttspel2\\{bettingGroup}_{username}.json";

            //var tippeModelList = new List<TippeModel>();
            //DirectoryInfo di = new DirectoryInfo(filePath);
            //FileInfo[] fileInfos = di.GetFiles();


            //foreach (var file in fileInfos)
            //{
            //    //Debug.WriteLine($"----{file.Name}----");
            //    //System.Text.Encoding.Default gives me ÆØÅ - good
            //    using StreamReader r = new StreamReader(file.FullName, System.Text.Encoding.Default);
            //    string json = r.ReadToEnd();

            //    var deserialized = JObject.Parse(json);

            //    foreach (var keyValuePair in deserialized)
            //    {
            //        //Debug.WriteLine(keyValuePair.Key);
            //        //Debug.WriteLine(keyValuePair.Value);
            //        var listTippeModels = GetTippeModels(keyValuePair, file.Name);
            //        tippeModelList.AddRange(listTippeModels);
            //    }
            //}
            //turnering.TippeModels = tippeModelList;
            //return View(turnering);
            return View();
        }

        List<TippeModel> GetTippeModels(KeyValuePair<string, JToken?> keyValuePair, string fileName)
        {
            fileName = fileName.Substring(10);
            fileName = fileName.Substring(0, fileName.Length - 5);
            var tippeModelList = new List<TippeModel>();
            if (keyValuePair.Value != null)
            {
                var keyValueList = keyValuePair.Value.ToList();
            }
            else return tippeModelList;

            foreach (var jToken in keyValuePair.Value)
            {
                var deserializedTippeModel = System.Text.Json.JsonSerializer.Deserialize<TippeModel>(jToken.ToString());
                if (deserializedTippeModel != null)
                {
                    deserializedTippeModel.userName = fileName;
                    tippeModelList.Add(deserializedTippeModel);
                }
            }

            return tippeModelList;
        }
    }
}