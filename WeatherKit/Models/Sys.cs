namespace WeatherKit.Models
{
    public class Sys
    {
        int Type { get; set; }
        int Id { get; set; }
        string Country { get; set; }
        long Sunrise { get; set; }
        long Sunset { get; set; }
    }
}
