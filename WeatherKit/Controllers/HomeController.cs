using System.Diagnostics;
using System.Linq;
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
        private readonly ICityListService _cityListService;
        private LocationInput li = new LocationInput();

        public HomeController(ILogger<HomeController> logger, 
            ISettingService settingService, IWeatherAPIService weatherAPIService, 
            ILocationService locationService, 
            ICityListService cityListService)
        {
            _logger = logger;
            _settingService = settingService;
            _locationService = locationService;
            _weatherAPIService = weatherAPIService;
            _cityListService = cityListService;
        }

        public async Task<IActionResult> Index()
        {
            Forecast weatherForecast = null;

            if (_locationService.CookieHasData())
            {
                weatherForecast = await _weatherAPIService.GetWeatherForecasts(_locationService.GetLocation());
            }
            else // If there are no cookies set, try to get user's location from their IP address
            {
                // Try to retrieve user's geolocation and store it in cookie
                LocationInput userLocation = _locationService.RetrieveLocationFromDb();
                if (userLocation != null)
                {
                    weatherForecast = await _weatherAPIService.GetWeatherForecasts(_locationService.GetLocation());
                }
            }

            if (weatherForecast != null)
            {
                SetDateTimeInViewBag(weatherForecast);

                return View("GetWeatherDetails", weatherForecast);
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
            return Json(_cityListService.GetCityOptionsList());
        }


        [HttpGet]
        public async Task<IActionResult> GetWeatherDetails(string citySelected, string zipCode)
        {

            if (!string.IsNullOrEmpty(citySelected))
            {
                string[] cityInfoArray = citySelected.Split(',');
                li.City = cityInfoArray[0].Trim();

                if (cityInfoArray.Count() == 3)
                {
                    li.StateCode = cityInfoArray[1].Trim();
                    li.CountryCode = cityInfoArray[2].Trim();
                }
            }

            if (!string.IsNullOrEmpty(zipCode))
            {
                li.ZipCode = zipCode.Trim();
            }

            var weatherForecast = await _weatherAPIService.GetWeatherForecasts(li);
            if (weatherForecast == null)
            {
                ViewBag.InvalidMsg = "Incorrect location.";
                return await Index();    
            }

            SetDateTimeInViewBag(weatherForecast);

            // Save the location globally & to cookie
            _locationService.UpdateLocation(li);

            return View("GetWeatherDetails", weatherForecast);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private void SetDateTimeInViewBag(Forecast weatherForecast)
        {
            string dtFormat = "MMM dd, yyyy HH:mm";
            string sunRiseSetFormat = "HH:mm";
            if (!_settingService.GetSetting().Is24HourTimeFormat)
            {
                dtFormat = "MMM dd, yyyy hh:mm tt";
                sunRiseSetFormat = "hh:mm tt";
            }

            ViewBag.Time = weatherForecast.Date.ToString(dtFormat);
            ViewBag.SunRiseStr = weatherForecast.sys.SunRise.ToString(sunRiseSetFormat);
            ViewBag.SunSetStr = weatherForecast.sys.SunSet.ToString(sunRiseSetFormat);
        }
    }
}
