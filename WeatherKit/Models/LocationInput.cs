using System;

namespace WeatherKit.Models
{
    public class LocationInput
    {
        // Example: Bellevue
        public string City { get; set; }

        // Example: WA 
        public string StateCode { get; set; }

        // 2 char code. Example: US
        public string CountryCode { get; set; }

        public string ZipCode { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
