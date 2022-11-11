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
            var stagesId = request.stagesid;
            var teamId = request.teamId;
            var tournamentId = request.tournamentId;
            var date = request.date;
            Console.WriteLine($"LigaID er: {stagesId}. TeamID er {teamId}. Tournament id er {tournamentId}. Dato er: {date}.");

            //var apiResultAsModel = new ApiCall().DoApiCall($"https://api.nifs.no/tournaments/{tournamentId}/matches?date={date}");
            var apiResultAsModel = new ApiCall().DoApiCall($"https://api.nifs.no/stages/{stagesId}/matches/");
            var response = apiResultAsModel.Response;
            ApiCall.CheckIfSuccess(response);
            var jsonStringed = apiResultAsModel.StringResponse;
            var jsonSerialized = new getAPI.jsonConvertAndIteration().JsonSerialize(jsonStringed);
            List<kampModel> kampModels = jsonConvertAndIteration.JsonIteration(jsonSerialized);

            return kampModels;
        }
        public List<kampModel> DoStagesApiCall(apiModel request)
        {
            var stagesId = request.stagesid;
            var teamId = request.teamId;
            var tournamentId = request.tournamentId;
            var date = request.date;
            Console.WriteLine($"LigaID er: {stagesId}. TeamID er {teamId}. Tournament id er {tournamentId}. Dato er: {date}.");

            var apiResultAsModel = new ApiCall().DoApiCall($"https://api.nifs.no/stages/{stagesId}matches?teamId={teamId}");
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
