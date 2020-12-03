using Microsoft.AspNetCore.Mvc;
using System;
using WeatherKit.Models;
using WeatherKit.Services;

namespace WeatherKit.Controllers
{
    public class SettingsController : Controller
    {
        private SettingOptions so = new SettingOptions();
        private readonly IWeatherAPIService _weatherAPIService;
        private readonly ISettingService _settingService;
        private readonly ILocationService _locationService;
        static private Uri referer;

        public SettingsController(IWeatherAPIService weatherAPIService, ISettingService settingService, ILocationService locationService)
        {
            _weatherAPIService = weatherAPIService;
            _settingService = settingService;
            _locationService = locationService;
        }

        public IActionResult Settings()
        {
            referer = new Uri(HttpContext.Request.Headers["Referer"].ToString());
            return View(so);
        }

        [HttpPost]

        public IActionResult SaveSettings(string unitOption, string timeFormatOption)
        {
            
            // Update setting model
            Setting settings = _settingService.GetSetting();
            if (unitOption == "Standard Kelvin")
            {
                settings.Units = Units.Standard;
            }
            else if (unitOption == "Metric Celsius")
            {
                settings.Units = Units.Metric;
            }
            else
            {
                settings.Units = Units.Imperial;
            }

            if (referer.AbsolutePath == "/")
            {
                return View("Index");
            }
            else
            {
                // Get the location input data from cookie
                _locationService.ReadLocation(HttpContext);

                // Make API call to get updated forecast 
                Forecast newForecast = _weatherAPIService.GetWeatherForecasts(_locationService.GetLocation()).Result;
                return View("GetWeatherDetails", newForecast);
            }
            
        }
    }
}
