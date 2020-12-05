using Microsoft.AspNetCore.Http;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public class LocationService : ILocationService
    {
        private LocationInput currentLocation;
        private bool cookieHasData = false;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocationService(IHttpContextAccessor httpContextAccessor)
        {
            currentLocation = new LocationInput();
            _httpContextAccessor = httpContextAccessor;

            // Read location fromthe cookie
            ReadLocation(httpContextAccessor.HttpContext);
        }

        public bool CookieHasData()
        {
            return cookieHasData;
        }

        public LocationInput GetLocation()
        {
            return currentLocation;
        }

        // Updates currentLocation & cookie
        public void UpdateLocation(LocationInput newLocation)
        { 
            currentLocation = newLocation;
            WriteLocation(_httpContextAccessor.HttpContext);
        }

        public void ReadLocation(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey("City"))
            {
                currentLocation.City = context.Request.Cookies["City"];
                cookieHasData = true;
            }
            if (context.Request.Cookies.ContainsKey("StateCode")) { 
                currentLocation.StateCode = context.Request.Cookies["StateCode"];
                cookieHasData = true;
            }
            if (context.Request.Cookies.ContainsKey("ZipCode")) { 
                currentLocation.ZipCode = context.Request.Cookies["ZipCode"];
                cookieHasData = true;
            }

            if (!(context.Request.Cookies["Latitude"] is null) && !(context.Request.Cookies["Longitude"] is null))
            {
                currentLocation.Latitude = long.Parse(context.Request.Cookies["Latitude"]);
                currentLocation.Longitude = long.Parse(context.Request.Cookies["Longitude"]);
                cookieHasData = true;
            }
        }

        public void WriteLocation(HttpContext context)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Domain = ".localhost";
            cookieOptions.Expires = System.DateTime.Now.AddDays(1);

            if (!string.IsNullOrEmpty(currentLocation.City))
                context.Response.Cookies.Append("City", currentLocation.City, cookieOptions);
            if (!string.IsNullOrEmpty(currentLocation.StateCode))
                context.Response.Cookies.Append("StateCode", currentLocation.StateCode, cookieOptions);
            if (!string.IsNullOrEmpty(currentLocation.ZipCode))
                context.Response.Cookies.Append("ZipCode", currentLocation.ZipCode, cookieOptions);
            if (currentLocation.Latitude != 0)
                context.Response.Cookies.Append("Latitude", currentLocation.Latitude.ToString(), cookieOptions);
            if (currentLocation.Longitude != 0)
                context.Response.Cookies.Append("Longitude", currentLocation.Longitude.ToString());
        }
    }
}
