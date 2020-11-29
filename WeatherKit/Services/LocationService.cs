using Microsoft.AspNetCore.Http;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public class LocationService : ILocationService
    {
        private LocationInput currentLocation;

        public LocationService()
        {
            currentLocation = new LocationInput();
        }

        public LocationInput GetLocation()
        {
            return currentLocation;
        }

        public void UpdateLocation(LocationInput newLocation)
        {
            currentLocation = newLocation;
        }

        public void ReadLocation(HttpContext context)
        {
            currentLocation.City = context.Request.Cookies["City"];
            currentLocation.StateCode = context.Request.Cookies["StateCode"];
            currentLocation.ZipCode = context.Request.Cookies["ZipCode"];
            currentLocation.Latitude = long.Parse(context.Request.Cookies["Latitude"]);
            currentLocation.Longitude = long.Parse(context.Request.Cookies["Longitude"]);
        }

        public void WriteLocation(HttpContext context)
        {
            context.Response.Cookies.Append("City", currentLocation.City);
            context.Response.Cookies.Append("StateCode", currentLocation.StateCode);
            context.Response.Cookies.Append("ZipCode", currentLocation.ZipCode);
            context.Response.Cookies.Append("Latitude", currentLocation.Latitude.ToString());
            context.Response.Cookies.Append("Longitude", currentLocation.Longitude.ToString());
        }
    }
}
