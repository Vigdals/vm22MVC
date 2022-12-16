using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using vm22MVC.Models;
using System.Text.Json;
using System.Diagnostics;
using getAPIstuff.Api;
using getAPI;
using getAPIstuff.Models;
using vm22MVC.Service;

namespace vm22MVC.Controllers
{
    public class LeaderboardController : Controller
    {
        public IActionResult Index()
        {
            var turnering = new TournamentModel();
            turnering.kampModels = HentVmResultat();
            turnering.TippeModels = HentTippeModels();
            //Populates the kampModels.kampstatus with home, away or tie. Also sets NotPlayed if match has not been played
            foreach (var kamp in turnering.kampModels)
            {
                Debug.WriteLine(
                    $"{kamp.HomeTeam} mot {kamp.AwayTeam} vart {kamp.HomeScore}-{kamp.AwayScore}. nifsID: {kamp.nifsKampId}");
                //Hopper over kamper som ikkje er blitt spilt enda
                if (string.IsNullOrWhiteSpace(kamp.HomeScore)) continue;
                if (int.Parse(kamp.HomeScore) > int.Parse(kamp.AwayScore))
                {
                    kamp.KampStatus = KampStatus.Home;
                    Debug.WriteLine(kamp.KampStatus.ToString());
                }
                else if (int.Parse(kamp.HomeScore) == int.Parse(kamp.AwayScore))
                {
                    kamp.KampStatus = KampStatus.Tie;
                    Debug.WriteLine(kamp.KampStatus.ToString());
                }
                else
                {
                    kamp.KampStatus = KampStatus.Away;
                    Debug.WriteLine(kamp.KampStatus.ToString());
                }
            }

            var pointsOverViewList = new List<pointsOverView>();
            //Iterates through each user in turneringModel and updates score on each match for each user
            foreach (var brukernamn in turnering.TippeModels.Select(t => t.userName).Distinct())
            {
                foreach (var kamp in turnering.TippeModels.Where(t => t.userName == brukernamn))
                {
                    Debug.WriteLine(
                        $"{brukernamn} tippa {kamp.Answer} i kampen: {kamp.HjemmeLag} mot {kamp.BorteLag}. nifsID: {kamp.nifsKampId}");
                    var currentNifsKampId = kamp.nifsKampId;
                    var kampResultat = turnering.kampModels.FirstOrDefault(x => x.nifsKampId == kamp.nifsKampId);
                    if (kampResultat != null)
                    {
                        kamp.UpdateScore(kampResultat);
                    }
                }
                pointsOverViewList.Add(new pointsOverView{UserName = brukernamn, TotalScore = turnering.CalculateScoreByUserName(brukernamn)});
            }

            return View(pointsOverViewList);
        }
        //A model that should be in its own file
        public class pointsOverView
        {
            public string UserName { get; set; }
            public int TotalScore { get; set; }
        }
        
        private List<TippeModel> HentTippeModels()
        {
            var tippeModelList = new List<TippeModel>();
            var filePath = "c:\\home\\json\\correctJsonFolder\\";
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] fileInfos = di.GetFiles();


            foreach (var file in fileInfos)
            {
                //Debug.WriteLine($"----{file.Name}----");
                //System.Text.Encoding.Default gives me ÆØÅ - good
                using StreamReader r = new StreamReader(file.FullName, System.Text.Encoding.Default);
                string json = r.ReadToEnd();

                var deserialized = JObject.Parse(json);

                foreach (var keyValuePair in deserialized)
                {
                    //Debug.WriteLine(keyValuePair.Key);
                    //Debug.WriteLine(keyValuePair.Value);
                    var listTippeModels = GetTippeModels(keyValuePair, file.Name);
                    tippeModelList.AddRange(listTippeModels);
                }
            }

            return tippeModelList;
        }

        private List<kampModel> HentVmResultat()
        {
            var kampModels = new List<kampModel>();
            var year = "2022";
            var apiTournamentModel = new ApiCall().DoApiCall("https://api.nifs.no/tournaments/56/stages/");
            var apiTournamentReponse = apiTournamentModel.Response;
            ApiCall.CheckIfSuccess(apiTournamentReponse);
            
            var jsonSerialized = new jsonConvertAndIteration().JsonSerialize(apiTournamentModel.StringResponse);
            var vmStages = JToken.Parse(jsonSerialized);

            foreach (var stage in vmStages)
            {
                if ((int)stage.SelectToken("yearStart") != int.Parse(year)) continue;
                var model = new TournamentModel()
                {
                    groupName = (string)stage.SelectToken("groupName"),
                    yearStart = (int)stage.SelectToken("yearStart"),
                    id = (int)stage.SelectToken("id")
                };
                //Gets all matches for all the groups through api call
                var TournamentMatches = new ApiCall().DoApiCall($"https://api.nifs.no/stages/{model.id}/matches/");
                List<kampModel> kampModelsList =
                    jsonConvertAndIteration.JsonIteration(TournamentMatches.StringResponse);
                kampModels.AddRange(kampModelsList);
            }

            return kampModels;
        }

        private List<PlayerModel> HentPlayerStats()
        {
            var playerModels = new List<PlayerModel>();

            //do stuff here

            return playerModels;
        }

        //List<TournamentModel> GetUserData()
        //{
        //    var filePath = "c:\\home\\json\\correctJsonFolder\\";
        //    DirectoryInfo di = new DirectoryInfo(filePath);
        //    FileInfo[] fileInfos = di.GetFiles();
        //    var tournamentModelList = new List<TournamentModel>();

        //    foreach (var file in fileInfos)
        //    {
        //        //Debug.WriteLine($"----{file.Name}----");
        //        //System.Text.Encoding.Default gives me ÆØÅ - good
        //        using StreamReader r = new StreamReader(file.FullName, System.Text.Encoding.Default);
        //        string json = r.ReadToEnd();

        //        var deserialized = JObject.Parse(json);

        //        foreach (var keyValuePair in deserialized)
        //        {
        //            //Debug.WriteLine(keyValuePair.Key);
        //            //Debug.WriteLine(keyValuePair.Value);
        //            //var listTippeModels = GetTippeModels(keyValuePair);
        //            //tournamentModelList.Add(new TournamentModel()
        //            //{
        //            //    TippeModels = listTippeModels,
        //            //    groupName = keyValuePair.Key,
        //            //    userName = file.Name
        //            //});
        //        }
        //    }
        //    return tournamentModelList;
        //}
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
                var deserializedTippeModel = JsonSerializer.Deserialize<TippeModel>(jToken.ToString());
                if (deserializedTippeModel != null)
                {
                    deserializedTippeModel.userName = fileName;
                    tippeModelList.Add(deserializedTippeModel);
                }
            }

            return tippeModelList;
        }

        //Forstår ikkje heilt kva dette _callService er
        private readonly IDoApiCallService _callService = new DoApiCallService();
        //List<TournamentModel> GetVmResultater()
        //{
        //    var year = "2022";
        //    var apiTournamentModel = new ApiCall().DoApiCall("https://api.nifs.no/tournaments/56/stages/");
        //    var apiTournamentReponse = apiTournamentModel.Response;
        //    ApiCall.CheckIfSuccess(apiTournamentReponse);

        //    var listModel = new List<TournamentModel>();
        //    var jsonSerialized = new jsonConvertAndIteration().JsonSerialize(apiTournamentModel.StringResponse);
        //    var jToken = JToken.Parse(jsonSerialized);

        //    foreach (var item in jToken)
        //    {
        //        if ((int)item.SelectToken("yearStart") != int.Parse(year)) continue;
        //        var model = new TournamentModel()
        //        {
        //            groupName = (string)item.SelectToken("groupName"),
        //            yearStart = (int)item.SelectToken("yearStart"),
        //            id = (int)item.SelectToken("id")
        //        };
        //        //Gets all matches for all the groups through api call
        //        var TournamentMatches = new ApiCall().DoApiCall($"https://api.nifs.no/stages/{model.id}/matches/");
        //        List<kampModel> kampModelsList = jsonConvertAndIteration.JsonIteration(TournamentMatches.StringResponse);

        //        model.kampModels = kampModelsList;

        //        listModel.Add(model);
        //    }

        //    //Using Linq here with input fra drop down list in the index.cshtml:
        //    return listModel;
        //}
    }
}