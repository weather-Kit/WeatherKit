namespace WeatherKit.Models
{
    public class Weather
    {
        public int id { get; set; }
        // Example: Rain
        public string main { get; set; }
        // Example: moderate rain
        public string description { get; set; }
        public string icon { get; set; }
    }
}
