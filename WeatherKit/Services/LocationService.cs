using WeatherKit.Models;

namespace WeatherKit.Services
{
    public class LocationService : ILocationService
    {
        private LocationInput Location;

        public LocationService()
        {
            Location = new LocationInput();
        }

        public LocationInput GetLocation()
        {
            return Location;
        }

        public void UpdateLocation(LocationInput newLocation)
        {
            Location = newLocation;
        }
    }
}
