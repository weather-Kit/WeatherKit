using System;

namespace WeatherKit.Models
{
    public class Sys
    {
        public int type { get; set; }   // ignore
        public int id { get; set; } // ignore

        // Country code (US, GB, JP etc.)
        public string country { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }

        public DateTime SunRise { get; set; }
        public DateTime SunSet { get; set; }
    }
}