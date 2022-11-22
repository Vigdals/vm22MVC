using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using vm22MVC.Models;
using getAPIstuff.Models;

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
            var listModel = new List<TippeModel>();

            foreach (var file in fileInfos)
            {
                Debug.WriteLine(file.Name);
                //System.Text.Encoding.Default gives me ÆØÅ - good
                using StreamReader r = new StreamReader(file.FullName, System.Text.Encoding.Default);
                string json = r.ReadToEnd();
                string jsonSerializedString = System.Text.Json.JsonSerializer.Serialize(json);
                Debug.WriteLine(jsonSerializedString);
                //var jsonModels = System.Text.Json.JsonSerializer.Deserialize<TippeModel>(jsonSerializedString);
                var jToken = JToken.Parse(jsonSerializedString);
                Debug.WriteLine("WriteLine before ForEach");
                foreach (var item in jToken)
                {
                    Debug.WriteLine((string)item.SelectToken("TippeModels.Answer"));
                }
                

                //foreach (var item in jsonModels)
                //{
                //    Debug.WriteLine((int)item.SelectToken("kampModels.nifsKampId"));
                //    var model = new TippeModel()
                //    {
                //        NifsKampId = (string)item.SelectToken("kampModels.nifsKampId")
                //    };
                //    listModel.Add(model);
                //    //Debug.WriteLine(item.kampModels.nifsKampId);
                //}
            }
            return View();
        }
    }
}
