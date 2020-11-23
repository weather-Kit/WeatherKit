using WeatherKit.Models;

namespace WeatherKit.Services
{
    interface ILocationService
    {
        public LocationInput GetLocation();
    }
}
