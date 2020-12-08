using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MaxMind.GeoIP2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly ISettingService _settingService;
        private readonly ILocationService _locationService;
        private readonly IWeatherAPIService _weatherAPIService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private LocationInput li = new LocationInput();
        private List<CityOptions> cityOptionsList = new List<CityOptions>();
        private List<CityInfo> cityInfoList = null;

        public HomeController(ILogger<HomeController> logger, 
            ISettingService settingService, IWeatherAPIService weatherAPIService, 
            ILocationService locationService, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _settingService = settingService;
            _locationService = locationService;
            _weatherAPIService = weatherAPIService;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = webHostEnvironment;
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

                    //ViewBag.URL = _weatherAPIService.GetURL();
                    //ViewBag.JSONContent = _weatherAPIService.GetJSONContent();
                    ViewBag.Time = time;

                    return View("GetWeatherDetails_Debug", weatherForecast);
                }
            }
            else // If there are no cookies set, try to get user's location from their IP address
            {
                try
                {
                    using (var reader = new DatabaseReader(_hostingEnvironment.ContentRootPath + "\\GeoLite2-City.mmdb"))
                    {
                        var ipAddress = HttpContext.Connection.RemoteIpAddress;    
                        //ipAddress = System.Net.IPAddress.Parse("104.199.123.16");
                        var city = reader.City(ipAddress);

                        LocationInput location = new LocationInput();
                        if ((city.Location.Longitude != null) && (city.Location.Latitude != null))
                        {
                            location.Longitude = (double)city.Location.Longitude;
                            location.Latitude = (double)city.Location.Latitude;

                            _locationService.UpdateLocation(location);
                            _locationService.WriteLocation(HttpContext);
                            _locationService.ReadLocation(HttpContext); //sets CookieHasData to true

                            Forecast forecast = await _weatherAPIService.GetWeatherForecasts(_locationService.GetLocation());
                            if (forecast != null)
                            {
                                string time = _settingService.GetSetting().Is24HourTimeFormat ?
                                    forecast.Date.ToString("HH:mm") : forecast.Date.ToString("hh:mm tt");

                                forecast.TimeZone = city.Location.TimeZone;

                                //ViewBag.URL = _weatherAPIService.GetURL();
                                //ViewBag.JSONContent = _weatherAPIService.GetJSONContent();
                                ViewBag.Time = time;

                                return View("GetWeatherDetails_Debug", forecast);
                            }
                        }
                    }
                }
                catch { }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public JsonResult CityList()
        {

            using (StreamReader r = new StreamReader("wwwroot/json/city.list.json"))
            {
                string json = r.ReadToEnd();
                cityInfoList = JsonConvert.DeserializeObject<List<CityInfo>>(json);
            }

            foreach (var city in cityInfoList)
            {
                CityOptions co = new CityOptions();
                co.name = city.name;
                co.state = city.state;
                co.country = city.country;
                cityOptionsList.Add(co);
            }

            return Json(cityOptionsList);
        }


        [HttpGet]
        public async Task<IActionResult> GetWeatherDetails(string citySelected, string zipCode)
        {

            if (!string.IsNullOrEmpty(citySelected))
            {
                string[] cityInfoArray = citySelected.Split(',');
                li.City = cityInfoArray[0];

                if (cityInfoArray.Count() == 3)
                {
                    li.StateCode = cityInfoArray[1];
                    li.CountryCode = cityInfoArray[2];
                }
            }

            if (!string.IsNullOrEmpty(zipCode))
            {
                li.ZipCode = zipCode;
            }

            var weatherForecast = await _weatherAPIService.GetWeatherForecasts(li);
            if (weatherForecast == null)
            {
                ViewBag.InvalidMsg = "Incorrect location or format.";
                return View("Index");
            }

            string time = _settingService.GetSetting().Is24HourTimeFormat ?
                weatherForecast.Date.ToString("HH:mm") : weatherForecast.Date.ToString("hh:mm tt");

            ViewBag.Time = time;

            // Save the location globally & to cookie
            _locationService.UpdateLocation(li);

            return View("GetWeatherDetails_Debug", weatherForecast);
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
                    foreach (string i in items)
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
