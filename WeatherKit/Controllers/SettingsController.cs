using Microsoft.AspNetCore.Mvc;
using WeatherKit.Models;
using WeatherKit.Services;

namespace WeatherKit.Controllers
{
    public class SettingsController : Controller
    {
        private SettingOptions so = new SettingOptions();

        Forecast newForecast = new Forecast();

        public IActionResult Index()
        {
            return View(so);
        }

        [HttpPost]

        public IActionResult SaveSettings(string unitOption, string timeFormatOption)
        {
            // Get the location input data from cookie
            // Update setting model
            // Make API call to get updated forecast 
            // return View("GetWeatherDetails", newForecast);

            return View("Index", so);// (Placeholder for temp functionality) Delete this line when other updates have been made
        }
    }
}
