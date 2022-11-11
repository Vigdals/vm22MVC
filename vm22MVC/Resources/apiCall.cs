using getAPIstuff.Models;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace getAPIstuff.Api
{
    public class ApiCall
    {
        public apiModel DoApiCall(string apiURL)
        {
            using var client = new HttpClient();
            // Get data response
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(apiURL).Result;
            var stringResponse = response.Content.ReadAsStringAsync().Result;
            // Adding data to the model - just for learning purposes
            var responseModel = new apiModel(stringResponse)
            {
                Url = apiURL,
                Response = response
            };
            return responseModel;
        }

        public static void CheckIfSuccess(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body
                //Console.WriteLine(apiResultAsModel.StringResponse);
                Debug.WriteLine(response.StatusCode);
            }
            else
            {
                Debug.WriteLine("Feilkode: {0} og grunnen til dette er: ({1})", (int)response.StatusCode,
                    response.ReasonPhrase);
            }
        }
    }
}
