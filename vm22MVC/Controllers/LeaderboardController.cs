using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using vm22MVC.Models;
using System.Text.Json;
using System.Diagnostics;

namespace vm22MVC.Controllers
{
    public class LeaderboardController : Controller
    {
        public IActionResult Index()
        {
            var tournamentModelList = GetUserData();
            foreach (var tournament in tournamentModelList)
            {
                Debug.WriteLine(tournament.userName);
            }
            foreach (var item in tournamentModelList.Where(f=>f.userName == "Odds konk_VIGDAL VED OG VEL.json"))
            {
                Debug.WriteLine(item.TippeModels);
            }

            foreach (var item in TippeModel)
            {

            }

            return View();
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
    }
}