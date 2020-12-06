using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly HomeController _homeController;
        static private Uri referer;

        public SettingsController(IWeatherAPIService weatherAPIService, 
            ISettingService settingService, ILocationService locationService, HomeController homeController)
        {
            _weatherAPIService = weatherAPIService;
            _settingService = settingService;
            _locationService = locationService;
            _homeController = homeController;

            // Update settingOption with GetSettings()
            UpdateSettingOptionsWithSetting();
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
            UpdateToSettingModel(unitOption, timeFormatOption);

            if (referer.AbsolutePath == "/")
            {
                return (IActionResult)_homeController.Index();
            }
            else
            {
                // Get the location input data from cookie
                _locationService.ReadLocation(HttpContext);

                // Make API call to get updated forecast 
                return _homeController.GetWeatherDetails(_locationService.GetLocation().City,_locationService.GetLocation().ZipCode).Result;
            }
        }

        // Update properties of SettingOptions with Setting
        private void UpdateSettingOptionsWithSetting()
        {
            // Get Setting
            Setting currSetting = _settingService.GetSetting();

            // Update properties of SettingOptions with Setting
            so.TimeFormatOption = currSetting.Is24HourTimeFormat ? so.TimeFormatOptions[1] : so.TimeFormatOptions[0];
            if (currSetting.Units == Units.Standard)
                so.UnitOption = so.UnitOptions[0];
            else if (currSetting.Units == Units.Metric)
                so.UnitOption = so.UnitOptions[1];
            else if (currSetting.Units == Units.Imperial)
                so.UnitOption = so.UnitOptions[2];

            return;
        }

        // Updates global Setting model
        private void UpdateToSettingModel(string unitOption, string timeFormatOption)
        {
            Setting settings = _settingService.GetSetting();
            // Set Units
            if (unitOption == so.UnitOptions[0])    // "Standard C & F"
            {
                settings.Units = Units.Standard;
            }
            else if (unitOption == so.UnitOptions[1])   // "Metric Celsius"
            {
                settings.Units = Units.Metric;
            }
            else
            {
                settings.Units = Units.Imperial;
            }

            // Set time format
            if (timeFormatOption == so.TimeFormatOptions[0])    // 12 Hour
            {
                settings.Is24HourTimeFormat = false;
            }
            else
            {
                settings.Is24HourTimeFormat = true;
            }

            _settingService.UpdateSetting(settings);

            return;
        }
    }
}
