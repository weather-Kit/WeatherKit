using WeatherKit.Models;

namespace WeatherKit.Services
{
    public interface ILocationService
    {
        public LocationInput GetLocation();
        public void UpdateLocation(LocationInput newLocation);
    }
}
