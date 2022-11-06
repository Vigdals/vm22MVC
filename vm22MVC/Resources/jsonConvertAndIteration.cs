using Newtonsoft.Json.Linq;
using getAPIstuff.Models;

namespace getAPI
{
    public class jsonConvertAndIteration
    {
        //convert jsonString to an actual json
        public string JsonSerialize(string jsonString)
        {
            //dependency here - nugt pkg
            var jsonSerialized = JToken.Parse(jsonString);
            return jsonSerialized.ToString();
        }
        //Creates a model out of the json
        public static List<kampModel> JsonIteration(string jsonSerializedString)
        {
            var listModel = new List<kampModel>();
            var jToken = JToken.Parse(jsonSerializedString);

            foreach (var item in jToken)
            {
                var model = new kampModel()
                {
                    Name = (string)item.SelectToken("name"),
                    HomeTeam = (string)item.SelectToken("homeTeam.name"),
                    AwayTeam = (string)item.SelectToken("awayTeam.name"),
                    HomeScore = (string)item.SelectToken("result.homeScore90"),
                    AwayScore = (string)item.SelectToken("result.awayScore90"),
                    HomeTeamLogo = (string)item.SelectToken("homeTeam.logo.url") == null ? "No logo found" : item.SelectToken("homeTeam.logo.url").ToString(),
                    Stadium = (string)item.SelectToken("stadium.name")
                };
                listModel.Add(model);
            }
            return listModel;
        }
    }
}