using System;
using System.Threading.Tasks;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public interface IWeatherAPIService
    {
        public Task<Forecast> GetWeatherForecasts(LocationInput locationInput);
        public string GetJSONContent();
        public string GetURL();
        public string GetTimeZone();
        public TimeZoneInfo GetTimeZoneInfo();


    }
}
