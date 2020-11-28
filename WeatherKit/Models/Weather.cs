namespace WeatherKit.Models
{
    public class Weather
    {
        public int id { get; set; }
        // Example: Rain
        public string main { get; set; }
        // Example: moderate rain
        public string description { get; set; }
        // To get Icon for value "10d", 
        // URL is http://openweathermap.org/img/wn/10d@2x.png
        // Supportive Sizes : 2x.png& 4x.png
        public string icon { get; set; }
    }
}