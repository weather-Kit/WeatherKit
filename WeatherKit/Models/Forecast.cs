using System.Collections.Generic;

namespace WeatherKit.Models
{
    public class Forecast
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        //Time of data calculation, unix, UTC
        public int dt { get; set; }
        // Example: Country Code, Sunrise/Sunset time
        public Sys sys { get; set; }
        // UTC
        public int timezone { get; set; }
        // City ID
        public int id { get; set; }
        // City Name
        public string name { get; set; }
        public int cod { get; set; }
    }
}
