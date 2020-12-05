using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherKit.Models
{
    public class SettingOptions : PageModel
    {
        public Uri referer;

        [BindProperty]
        public string UnitOption { get; set; }
        public string[] UnitOptions = new[] { "Standard C & F", "Metric Celsius", "Imperial Fahrenheit"};

        [BindProperty]
        public string TimeFormatOption { get; set; }
        public string[] TimeFormatOptions = new[] { "12 Hour", "24 Hour"};
    }
}
