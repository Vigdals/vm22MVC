using getAPI;
using getAPIstuff.Api;
using getAPIstuff.Models;
using Newtonsoft.Json.Linq;

namespace vm22MVC.Service
{
    public class DoApiCallService : IDoApiCallService
    {
        public List<kampModel> DoVmApiCall(apiModel request)
        {
            var ligaId = request.ligaId;
            var teamId = request.teamId;
            var tournamentId = request.tournamentId;
            var date = request.date;

            var apiResultAsModel = new ApiCall().DoApiCall($"https://api.nifs.no/tournaments/{tournamentId}/matches?date={date}");
            var response = apiResultAsModel.Response;
            ApiCall.CheckIfSuccess(response);
            var jsonStringed = apiResultAsModel.StringResponse;
            var jsonSerialized = new getAPI.jsonConvertAndIteration().JsonSerialize(jsonStringed);
            List<kampModel> kampModels = jsonConvertAndIteration.JsonIteration(jsonSerialized);

            return kampModels;
        }
    }

    public interface IDoApiCallService
    {
        public List<kampModel> DoVmApiCall(apiModel request);
    }
}
