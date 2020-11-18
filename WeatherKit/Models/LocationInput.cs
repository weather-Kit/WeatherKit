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

        public long Longitude { get; set; }

        public long Latitude { get; set; }
    }
}
