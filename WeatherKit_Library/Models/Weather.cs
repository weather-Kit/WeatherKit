using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherKit_Library.Models
{
    public class Weather
    {
        int Id { get; set; }

        // Example: Rain
        string Main { get; set; }

        // Example: moderate rain
        string Description { get; set; }

        string Icon { get; set; }
    }
}
