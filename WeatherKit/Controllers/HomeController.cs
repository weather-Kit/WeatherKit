using System.Diagnostics;
using System.Threading.Tasks;
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

        public HomeController(ILogger<HomeController> logger, 
            ISettingService settingService, IWeatherAPIService weatherAPIService, ILocationService locationService)
        {
            _logger = logger;
            _settingService = settingService;
            _locationService = locationService;
            _weatherAPIService = weatherAPIService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SplashPage()
        {
            return View("SplashPage");
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

            ViewBag.URL = _weatherAPIService.GetURL();
            ViewBag.JSONContent = _weatherAPIService.GetJSONContent();
            ViewBag.TimeZoneName = _weatherAPIService.GetTimeZone();
            //ViewBag.TimeZoneInfo = _weatherAPIService.GetTimeZoneInfo().DisplayName;

            string time = "";
            if (_settingService.GetSetting().Is24HourTimeFormat)
            {
                // set time format for 24 hr format
                time = weatherForecast.Date.ToString("HH:mm");
            }
            else
            {
                // set time for 12 hr format
                time = weatherForecast.Date.ToString("hh:mm tt");
            }
            ViewBag.Time = time;


            return View("GetWeatherDetails", weatherForecast);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
