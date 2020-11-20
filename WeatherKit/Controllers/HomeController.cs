using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeatherKit.Models;
using WeatherKit.Services;

namespace WeatherKit.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISettingService _settingService;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, 
            ISettingService settingService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _settingService = settingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /*
         * api.openweathermap.org/data/2.5/weather?q={city name}&appid={API key}
            api.openweathermap.org/data/2.5/weather?q={city name},{state code}&appid={API key}
            api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={API key}
            api.openweathermap.org/data/2.5/weather?zip={zip code}&appid={API key}
         * */
        private async Task<Forecast> GetWeatherForecasts(LocationInput locationInput)
        {
            // Create base uri
            UriBuilder builder = new UriBuilder("http://api.openweathermap.org/data/2.5/weather");
            builder.Query = "";

            // If City, State is provided
            if (!string.IsNullOrEmpty(locationInput.City))
            {
                builder.Query = $"q={locationInput.City}";

                if (!string.IsNullOrEmpty(locationInput.StateCode))
                {
                    builder.Query += $",{locationInput.StateCode}";
                }
            }
            // If GeoLocation is provided
            else if (locationInput.Latitude != 0 && locationInput.Longitude != 0)
            {
                builder.Query = $"lat={locationInput.Latitude}&lon={locationInput.Longitude}";
            }
            // If zipcode is provided
            else if (!string.IsNullOrEmpty(locationInput.ZipCode))
            {
                builder.Query = $"zip={locationInput.ZipCode}";
            }

            if (builder.Query.Length > 0)
            {
                Setting setting = _settingService.GetSetting();

                if (setting.Units != Units.Standard)
                {
                    string unitType = setting.Units == Units.Imperial ? "imperial" : "metric";
                    builder.Query = $"units={unitType}";
                }

                builder.Query += "&appid=1e94cd79afa39de4db034e687033b2de";

                // Get the HttpClient & make the request call
                HttpClient client = _httpClientFactory.CreateClient("API Client");
                var result = await client.GetAsync(builder.Uri);

                if (result.IsSuccessStatusCode)
                {
                    // Read all of the response and deserialise it into an instace of
                    // Forecast class
                    var content = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Forecast>(content);
                }
            }

            return null;
        }

        private async Task<HttpResponseMessage> GetWeatherAsync(string query)
        {
            UriBuilder builder = new UriBuilder("http://api.openweathermap.org/data/2.5/weather");
            builder.Query = $"{query}&appid=1e94cd79afa39de4db034e687033b2de";

            var client = _httpClientFactory.CreateClient("API Client");

            return await client.GetAsync(builder.Uri);
        }

    }
}
