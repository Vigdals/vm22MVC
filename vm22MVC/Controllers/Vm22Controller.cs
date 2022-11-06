using getAPI;
using getAPIstuff.Api;
using getAPIstuff.Models;
using Microsoft.AspNetCore.Mvc;
using vm22MVC.Models;
using Newtonsoft.Json;

namespace vm22MVC.Controllers
{
    public class vm22Controller : Controller
    {
        public IActionResult Index()
        {
            var apiResultAsModel = new ApiCall().DoApiCall("https://api.nifs.no/tournaments/56/matches?date=2022-11-21");
            var response = apiResultAsModel.Response;
            ApiCall.CheckIfSuccess(response);
            var jsonStringed = apiResultAsModel.StringResponse;
            var jsonSerialized = new getAPI.jsonConvertAndIteration().JsonSerialize(jsonStringed);
            List<kampModel> kampModels = jsonConvertAndIteration.JsonIteration(jsonSerialized);

            return View(apiResultAsModel);
        }
        public IActionResult Kamper()
        {
            //var apiResultAsModel = new ApiCall().DoApiCall("https://api.nifs.no/tournaments/56/matches?date=2018-06-25");
            var apiResultAsModel = new ApiCall().DoApiCall("https://api.nifs.no/stages/690408/matches?teamId=844");
            var response = apiResultAsModel.Response;
            ApiCall.CheckIfSuccess(response);
            var jsonStringed = apiResultAsModel.StringResponse;
            var jsonSerialized = new getAPI.jsonConvertAndIteration().JsonSerialize(jsonStringed);
            List<kampModel> kampModels = jsonConvertAndIteration.JsonIteration(jsonSerialized);
            return View(kampModels);
        }
        public IActionResult Create()
        {

            return View();
        }
    }
}
