using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using vm22MVC.Models;

namespace vm22MVC.Controllers
{
    public class LeaderboardController : Controller
    {
        public IActionResult Index()
        {
            var filePath = "c:\\home\\json\\";
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] fileInfos = di.GetFiles();
            Debug.WriteLine("before foreach?");

            var tournamentModelList = new List<TournamentModel>();

            foreach (var file in fileInfos)
            {
                Debug.WriteLine(file.Name);
                //System.Text.Encoding.Default gives me ÆØÅ - good
                using StreamReader r = new StreamReader(file.FullName, System.Text.Encoding.Default);
                string json = r.ReadToEnd();

                if (string.IsNullOrWhiteSpace(json)) return View();
                Debug.WriteLine(json);

                var deserialized = JObject.Parse(json);
                Debug.WriteLine(deserialized);

                foreach (var keyValuePair in deserialized)
                {
                    var listTippeModels = GetTippeModels(keyValuePair);
                    tournamentModelList.Add(new TournamentModel()
                    {
                        TippeModels = listTippeModels,
                        groupName = keyValuePair.Key,
                        userName = file.FullName
                    });
                }
            }

            return View(tournamentModelList);
        }

        List<TippeModel> GetTippeModels(KeyValuePair<string, JToken?> keyValuePair)
        {
            var tippeModelList = new List<TippeModel>();

            if (keyValuePair.Value == null) throw new Exception("No tippe models found");

            var keyValueList = keyValuePair.Value.ToList();

            for (var index = 0; index < keyValueList.Count; index++)
            {
                tippeModelList.Add(JsonSerializer.Deserialize<TippeModel>(keyValuePair.Value[index]?.ToString() ?? string.Empty) ?? new TippeModel());
                if (keyValuePair.Value.Count() >= index && tippeModelList.Count != 0)
                {
                    return tippeModelList;
                }
            }

            throw new Exception("No tippe models found");
        }
    }
}
