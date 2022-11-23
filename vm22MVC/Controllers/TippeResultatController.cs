using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using vm22MVC.Models;

namespace vm22MVC.Controllers
{
    public class TippeResultatController : Controller
    {
        public IActionResult Index(string userName)
        {
            ViewBag.UserName = userName;
            //filsti til alle jsons i virtuell mappe i Azure static webapp
            if (string.IsNullOrWhiteSpace(userName)) return View(new List<TournamentModel>());
            var enivornment = Environment.UserDomainName;
            var filePath = "c:\\home\\json\\correctJsonFolder\\";
            Debug.WriteLine(enivornment);
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] fileInfos = di.GetFiles();
            var prefixedFilename = $"Odds konk_{userName.ToUpper()}.json";
            var tournamentModelList = new List<TournamentModel>();

            foreach (var file in fileInfos.Where(f => f.Name == prefixedFilename))
            {
                Debug.WriteLine($"----{file.Name}----");
                //System.Text.Encoding.Default gives me ÆØÅ - good
                using StreamReader r = new StreamReader(file.FullName, System.Text.Encoding.Default);
                string json = r.ReadToEnd();

                if (string.IsNullOrWhiteSpace(json)) return View("Error");

                var deserialized = JObject.Parse(json);

                foreach (var keyValuePair in deserialized)
                {
                    Debug.WriteLine(keyValuePair.Key);
                    Debug.WriteLine(keyValuePair.Value);
                    var listTippeModels = GetTippeModels(keyValuePair);
                    tournamentModelList.Add(new TournamentModel()
                    {
                        TippeModels = listTippeModels,
                        groupName = keyValuePair.Key,
                        userName = file.Name
                    });
                }
            }
            return View(tournamentModelList);
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
