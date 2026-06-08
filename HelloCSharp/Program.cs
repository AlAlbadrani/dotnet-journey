using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HelloCSharp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Fetching weather for multiple cities simultaneously...\n");

            string[] cities = { "Malmö", "Stockholm", "London", "Tokyo" };

            // Start all tasks simultaneously
            Task<string>[] tasks = Array.ConvertAll(cities, city => GetWeatherAsync(city));

            // Wait for ALL of them to complete
            string[] results = await Task.WhenAll(tasks);

            foreach (string result in results)
                Console.WriteLine(result);
        }

        static async Task<string> GetWeatherAsync(string city)
        {
            using HttpClient client = new HttpClient();

            // Step 1: Get coordinates
            string geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(city)}&count=1";
            string geoJson = await client.GetStringAsync(geoUrl);
            JObject geoData = JObject.Parse(geoJson);

            var results = geoData["results"];
            if (results == null || !results.HasValues)
                return $"{city}: Not found";

            double lat = results[0]["latitude"].Value<double>();
            double lon = results[0]["longitude"].Value<double>();
            string cityName = results[0]["name"].Value<string>();
            string country = results[0]["country"].Value<string>();

            // Step 2: Get weather
            string weatherUrl = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true";
            string weatherJson = await client.GetStringAsync(weatherUrl);
            JObject weatherData = JObject.Parse(weatherJson);

            double temp = weatherData["current_weather"]["temperature"].Value<double>();
            double wind = weatherData["current_weather"]["windspeed"].Value<double>();

            return $"{cityName}, {country}: {temp}°C, wind {wind} km/h";
        }
    }
}