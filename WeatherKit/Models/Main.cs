namespace WeatherKit.Models
{
    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }

        // Atmospheric pressure (on the sea level, if there is no sea_level or grnd_level data), hPa
        public int pressure { get; set; }
        
        // Humidity %
        public int humidity { get; set; }

        // Units for the temperature
        public string TempUnit { get; set; }
    }
}