using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherKit.Models
{
    public class CityOptions
    {
        public CityOptions()
        {        }

        public CityOptions(string nm, string st, string co)
        {
            name = nm;
            state = st;
            country = co;
        }

        public string name { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }
}
