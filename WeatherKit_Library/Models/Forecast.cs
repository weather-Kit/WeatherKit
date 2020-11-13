using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml;

namespace WeatherKit_Library.Models
{
    public class Forecast
    {
        public Coordinates Coord {get; set;}
        public Weather WeatherConditions { get; set; }
        public string Base { get; set; }
        public Main TempDetails { get; set; }
        public int Visibility { get; set; }
        public Wind WindDetails { get; set; }
        public Rain RainVolume { get; set; }
        public Clouds ClodinessPercentage{ get; set; }

        //Time of data calculation, unix, UTC
        public long Dt { get; set; }

        // Example: Country Code, Sunrise/Sunset time
        public Sys OtherDetails { get; set; }

        // UTC
        public int Timezone { get; set; }

        // City ID
        public int Id { get; set; }

        // City Name
        public string Name { get; set; }

        public int Cod { get; set; }
    }

    public class Coordinates
    {
        double Longitude { get; set; }

        double Latitude { get; set; }
    }

    public class Weather
    {
        int Id { get; set; }

        // Example: Rain
        string Main { get; set; }

        // Example: moderate rain
        string Description { get; set; }

        string Icon { get; set; }
    }

    public class Main
    {
        double Temp { get; set; }

        double Feels_Like { get; set; }

        double Temp_Min { get; set; }

        double Temp_Max { get; set; }

        int Pressure { get; set; }

        int Humidity { get; set; }
    }

    public class Wind
    {
        double Speed { get; set; }

        // Wind direction: Degrees
        int Deg { get; set; }
    }

    public class Rain
    {
        // Rain volume for the last 1 hour, mm
        double OneH { get; set; }
    }

    public class Clouds
    {
        // Cloudiness, %
        int All { get; set; }
    }

    public class Sys
    {
        int Type {get; set; }
        int Id {get; set; }
        string Country {get; set; }
        long Sunrise {get; set; }
        long Sunset {get; set; }
    }
}
