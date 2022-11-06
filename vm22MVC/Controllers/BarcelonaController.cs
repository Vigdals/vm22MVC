﻿using getAPI;
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
            //var apiResultAsModel = new ApiCall().DoApiCall("https://api.nifs.no/tournaments/56/matches?date=2018-06-25");
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
