using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherKit.Models
{
    public class CityInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public Coord coord { get; set; }
    }
}
