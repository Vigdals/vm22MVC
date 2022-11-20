using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            foreach (var file in fileInfos)
            {

                Debug.WriteLine(file.Name);
                //System.Text.Encoding.Latin1
                using StreamReader r = new StreamReader(file.FullName, System.Text.Encoding.Default);
                string json = r.ReadToEnd();
                string jsonString = System.Text.Json.JsonSerializer.Serialize(json);
                Debug.WriteLine(jsonString);
                dynamic array = JsonConvert.DeserializeObject(jsonString);
                foreach (var item in array)
                {
                    Debug.WriteLine($"{item.kampModels.nifsKampId}. Svar: {item.TippeModels.HjemmeLag}-{item.TippeModels.Bortelag}:{item.TippeModels.Answer}");
                }
            }

            

            return View();
        }
    }
}
