using System.Collections.Generic;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public interface ICityListService
    {
        public List<CityOptions> GetCityOptionsList();
        public void ReadCityOptionsList();
    }
}
