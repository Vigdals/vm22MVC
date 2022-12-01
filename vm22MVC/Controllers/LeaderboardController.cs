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
            var tournamentModelList = GetUserData();
            var resultatListe = GetVmResultater();
            //tournamentModelList.AddRange(resultatListe);
            //Just for logging

            //Setting enum value (Home, Tie, Away) into the KampModel based on resultat. Standard value is "NotPlayed"
            foreach (var gruppe in resultatListe)
            {
                foreach (var kamp in gruppe.kampModels)
                {
                    Debug.WriteLine($"{kamp.HomeTeam} mot {kamp.AwayTeam} vart {kamp.HomeScore}-{kamp.AwayScore}. nifsID: {kamp.nifsKampId}");
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
            }
            //Går i gjennom alle users in tournamentModelList
            foreach (var brukernamn in tournamentModelList)
            {
                foreach (var kamp in brukernamn.TippeModels)
                {
                    Debug.WriteLine($"{brukernamn.userName} tippa {kamp.Answer} i kampen: {kamp.HjemmeLag} mot {kamp.BorteLag}. nifsID: {kamp.nifsKampId}");
                    var currentNifsKampId = kamp.nifsKampId;
                    if (resultatListe.Select(x => x.kampModels).Contains(currentNifsKampId))
                    {

                    }
                }
            }


            return View(tournamentModelList);
        }
            
        List<TournamentModel> GetUserData()
        {
            var filePath = "c:\\home\\json\\correctJsonFolder\\";
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] fileInfos = di.GetFiles();
            var tournamentModelList = new List<TournamentModel>();

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
                    var listTippeModels = GetTippeModels(keyValuePair);
                    tournamentModelList.Add(new TournamentModel()
                    {
                        TippeModels = listTippeModels,
                        groupName = keyValuePair.Key,
                        userName = file.Name
                    });
                }
            }
            return tournamentModelList;
        }
        List<TippeModel> GetTippeModels(KeyValuePair<string, JToken?> keyValuePair)
        {
            var tippeModelList = new List<TippeModel>();
            if (keyValuePair.Value != null)
            {
                var keyValueList = keyValuePair.Value.ToList();
            }
            else return tippeModelList;

            foreach (var jToken in keyValuePair.Value)
            {
                var deserializedTippeModel = JsonSerializer.Deserialize<TippeModel>(jToken.ToString());
                if (deserializedTippeModel != null) tippeModelList.Add(deserializedTippeModel);
            }

            return tippeModelList;
        }
        //Forstår ikkje heilt kva dette _callService er
        private readonly IDoApiCallService _callService = new DoApiCallService();
        List<TournamentModel> GetVmResultater()
        {
            var year = "2022";
            var apiTournamentModel = new ApiCall().DoApiCall("https://api.nifs.no/tournaments/56/stages/");
            var apiTournamentReponse = apiTournamentModel.Response;
            ApiCall.CheckIfSuccess(apiTournamentReponse);

            var listModel = new List<TournamentModel>();
            var jsonSerialized = new jsonConvertAndIteration().JsonSerialize(apiTournamentModel.StringResponse);
            var jToken = JToken.Parse(jsonSerialized);

            foreach (var item in jToken)
            {
                if ((int)item.SelectToken("yearStart") != int.Parse(year)) continue;
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
            return listModel;
        }
    }
}