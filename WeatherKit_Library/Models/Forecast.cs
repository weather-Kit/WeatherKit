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
        public Clouds CloudinessPercentage{ get; set; }

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
}
