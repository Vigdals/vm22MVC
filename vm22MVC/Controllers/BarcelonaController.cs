using getAPI;
using getAPIstuff.Api;
using getAPIstuff.Models;
using Microsoft.AspNetCore.Mvc;
using vm22MVC.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace vm22MVC.Controllers
{
    public class BarcelonaController : Controller
    {
        public IActionResult Index()
        {
            //All matches of one group in vm22
            //var apiResultAsModel = new ApiCall().DoApiCall("https://api.nifs.no/stages/683902/matches/");
            //all the matches of barcelona
            var apiResultAsModel = new ApiCall().DoApiCall("https://api.nifs.no/stages/690408/matches?teamId=844");
            var response = apiResultAsModel.Response;
            ApiCall.CheckIfSuccess(response);
            var jsonStringed = apiResultAsModel.StringResponse;
            var jsonSerialized = new getAPI.jsonConvertAndIteration().JsonSerialize(jsonStringed);
            List<kampModel> kampModels = jsonConvertAndIteration.JsonIteration(jsonSerialized);
            return View(kampModels);
        }
    }
}
