using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherKit.Models;
using WeatherKit.Services;

namespace WeatherKit.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISettingService _settingService;
        private readonly ILocationService _locationService;
        private readonly IWeatherAPIService _weatherAPIService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, 
            ISettingService settingService, IWeatherAPIService weatherAPIService, 
            ILocationService locationService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _settingService = settingService;
            _locationService = locationService;
            _weatherAPIService = weatherAPIService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            if (_locationService.CookieHasData())
            {
                var weatherForecast = await _weatherAPIService.GetWeatherForecasts(_locationService.GetLocation());
                if (weatherForecast != null)
                {
                    string time = _settingService.GetSetting().Is24HourTimeFormat ?
                        weatherForecast.Date.ToString("HH:mm") : weatherForecast.Date.ToString("hh:mm tt");

                    ViewBag.URL = _weatherAPIService.GetURL();
                    ViewBag.JSONContent = _weatherAPIService.GetJSONContent();
                    ViewBag.Time = time;

                    return View("GetWeatherDetails", weatherForecast);
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherDetails(string cityState, string zipCode)
        {
            LocationInput li = new LocationInput();
            //******** Test with city name, zipcode - WORKING
            li.City = cityState;
            li.ZipCode = zipCode;

            var weatherForecast = await _weatherAPIService.GetWeatherForecasts(li);
            if (weatherForecast == null)
            {
                ViewBag.InvalidMsg = "Incorrect location or format.";
                return View("Index");
            }

            string time = _settingService.GetSetting().Is24HourTimeFormat ? 
                weatherForecast.Date.ToString("HH:mm") : weatherForecast.Date.ToString("hh:mm tt");

            ViewBag.URL = _weatherAPIService.GetURL();
            ViewBag.JSONContent = _weatherAPIService.GetJSONContent();
            ViewBag.Time = time;

            // Save the location globally & to cookie
            _locationService.UpdateLocation(li);

            return View("GetWeatherDetails", weatherForecast);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private LocationInput IsInputValid(string cityState, string zipCode)
        {
            LocationInput li = null;

            if (!string.IsNullOrEmpty(zipCode))
            {
                li = new LocationInput();
                li.ZipCode = zipCode.Trim();
            }

            if (!string.IsNullOrEmpty(cityState))
            {
                if (li == null)
                    li = new LocationInput();

                string[] items;
                if (cityState.Contains(','))
                {
                    items = cityState.Split(',');
                    List<string> list = new List<string>();
                    foreach(string i in items)
                    {
                        string str = i.Trim();
                        if (!string.IsNullOrEmpty(str))
                            list.Add(str);
                    }

                    if (list.Count == 0)
                        return li;

                    li.City = list[0];
                    if (list.Count > 1)
                        li.StateCode = list[1];
                    if (list.Count > 2)
                        li.CountryCode = list[2];
                }
                else
                {
                    li.City = cityState;
                }
            }

            return li;
        }
    }
}
