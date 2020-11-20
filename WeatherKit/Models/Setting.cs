namespace WeatherKit.Models
{
    public class Setting
    {
        public bool Is24HourTimeFormat { get; set; } = true;

        public LocationInput PreferredLocation { get; set; }

        public Units Units { get; set; } = Units.Imperial;

        //public WindDirection WindDirection { get; set; }

        public Setting()
        {
            Units = Units;
        }
    }

    public enum Units
    {
        Imperial,   // F
        Metric,     // C
        Standard      // Kelvin
    };

    /*
    public enum WindDirection
    {
        Cardinal,   // N, S, E, W
        Degrees     // 0 - 360
    }
    */
}
