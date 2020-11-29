using Microsoft.AspNetCore.Http;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public interface ILocationService
    {
        public LocationInput GetLocation();
        public void UpdateLocation(LocationInput newLocation);

        public void ReadLocation(HttpContext context);
        public void WriteLocation(HttpContext context);
    }
}
