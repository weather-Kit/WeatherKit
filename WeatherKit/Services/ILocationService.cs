using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public interface ILocationService
    {
        public LocationInput GetLocation();
        public void UpdateLocation(LocationInput newLocation);

        public void ReadLocation();
        public void WriteLocation();
        public bool CookieHasData();
        public System.Net.IPAddress GetIP();
        public List<CityOptions> GetCityOptionsList();
        public void ReadCityOptionsList();

    }
}
