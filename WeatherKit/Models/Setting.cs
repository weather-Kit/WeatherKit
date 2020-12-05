namespace WeatherKit.Models
{
    public class Setting
    {
        public bool Is24HourTimeFormat { get; set; } = true;

        public LocationInput PreferredLocation { get; set; }

        public Units Units { get; set; } = Units.Standard;

        public Setting()
        {
        }
    }

    public enum Units
    {
        Imperial,   // F
        Metric,     // C
        Standard      // C & F
    };
}
