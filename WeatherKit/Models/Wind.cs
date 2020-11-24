namespace WeatherKit.Models
{
    public class Wind
    {
        public double speed { get; set; }
        // Wind direction: Degrees
        public int deg { get; set; }
        public double gust { get; set; }
    }
}