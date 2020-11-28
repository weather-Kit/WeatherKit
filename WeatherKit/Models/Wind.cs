namespace WeatherKit.Models
{
    public class Wind
    {
        // Unit Default: meter/sec, Metric: meter/sec, Imperial: miles/hour.
        public double speed { get; set; }

        // Wind direction: Degrees
        public int deg { get; set; }

        // Unit Default: meter/sec, Metric: meter/sec, Imperial: miles/hour
        public double gust { get; set; }

        public string SpeedUnit { get; set; }
        public string GustUnit { get; set; }
    }
}