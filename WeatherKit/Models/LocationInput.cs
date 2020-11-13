using System;

namespace WeatherKit.Models
{
    public class LocationInput
    {
        // Example: Bellevue
        public string City { get; set; }

        // Example: WA 
        public string StateCode { get; set; }

        public string ZipCode { get; set; }
    }
}
