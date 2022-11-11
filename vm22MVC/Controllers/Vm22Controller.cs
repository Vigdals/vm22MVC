using getAPI;
using getAPIstuff.Api;
using getAPIstuff.Models;
using Microsoft.AspNetCore.Mvc;
using vm22MVC.Service;

namespace vm22MVC.Controllers
{
    public class vm22Controller : Controller
    {
        ////injecting the interface from DoApiCallService
        private readonly IDoApiCallService _callService = new DoApiCallService();

        public IActionResult Index()
        {
            var apiResultAsModel = new ApiCall().DoApiCall("https://api.nifs.no/stages/683902/matches/");
            var response = apiResultAsModel.Response;
            ApiCall.CheckIfSuccess(response);
            var jsonStringed = apiResultAsModel.StringResponse;
            var jsonSerialized = new getAPI.jsonConvertAndIteration().JsonSerialize(jsonStringed);
            List<kampModel> kampModels = jsonConvertAndIteration.JsonIteration(jsonSerialized);

            return View(kampModels);
        }

        public IActionResult Create()
        {
            var apiViewModel = new apiModel();
            return View(apiViewModel);
        }

        [HttpPost]
        public IActionResult Create([Bind("stagesid, teamId, tournamentId, date")] apiModel responseModel)
        {
            var kampModels = _callService.DoVmApiCall(responseModel);

            return View("Index", kampModels);
        }
    }
}
