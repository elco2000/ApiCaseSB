using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ApiCase.Model.ApicallGeometry;
using Newtonsoft.Json;

namespace ApiCase
{
    public class Helper
    {
        private static readonly HttpClient client = new HttpClient();


        public static async Task<string> GetDistanceAsync(string postalCodeOne, string postalCodeTwo)
        {
            // I used the api: https://openrouteservice.org/

            // Calling two requests for the coordinates.
            var requestOne = client.GetAsync($"https://api.openrouteservice.org/geocode/search?api_key=5b3ce3597851110001cf624835c2299196e44f44bfc3605b3e011117&text={postalCodeOne}");
            var requestTwo = client.GetAsync($"https://api.openrouteservice.org/geocode/search?api_key=5b3ce3597851110001cf624835c2299196e44f44bfc3605b3e011117&text={postalCodeTwo}");

            // Get the response out of the request.
            string responseDataOne = await requestOne.Result.Content.ReadAsStringAsync();
            string responseDataTwo = await requestTwo.Result.Content.ReadAsStringAsync();

            // Convert the JSON to an object.
            Root responseOne = JsonConvert.DeserializeObject<Root>(responseDataOne);
            Root responseTwo = JsonConvert.DeserializeObject<Root>(responseDataTwo);

            // Get a valid format for the next Request for the distance.
            string coordinateOne = responseOne.features[0].geometry.coordinates[0] + "," +
                                   responseOne.features[0].geometry.coordinates[1];
            string coordinateTwo = responseTwo.features[0].geometry.coordinates[0] + "," +
                                   responseTwo.features[0].geometry.coordinates[1];

            // Calling a request for the distance.
            var requestDistance = client.GetAsync(
                $"https://api.openrouteservice.org/v2/directions/driving-car?api_key=5b3ce3597851110001cf624835c2299196e44f44bfc3605b3e011117&start={coordinateOne}&end={coordinateTwo}");
            
            // Get the response out of the request.
            string responseDataDistance = await requestDistance.Result.Content.ReadAsStringAsync();

            // Convert the JSON to an object.
            Model.ApicallDistance.Root responseDistance = JsonConvert.DeserializeObject<Model.ApicallDistance.Root>(responseDataDistance);

            // Returns the distance in KM.
            return "Afstand: " + responseDistance.features[0].properties.summary.distance / 1000 + "km";
        }
    }
}
