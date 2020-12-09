using Microsoft.AspNetCore.Http;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public class LocationService : ILocationService
    {
        private LocationInput currentLocation;
        private bool cookieHasData = false;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private System.Net.IPAddress IPAddress;

        public LocationService(IHttpContextAccessor httpContextAccessor)
        {
            currentLocation = new LocationInput();
            _httpContextAccessor = httpContextAccessor;

            // Read location fromthe cookie
            ReadLocation();
            IPAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
        }

        public bool CookieHasData()
        {
            return cookieHasData;
        }

        public LocationInput GetLocation()
        {
            return currentLocation;
        }

        public System.Net.IPAddress GetIP()
        {
            return IPAddress;
        }

        // Updates currentLocation & cookie
        public void UpdateLocation(LocationInput newLocation)
        { 
            currentLocation = newLocation;
            WriteLocation();
        }

        public void ReadLocation()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("City"))
            {
                currentLocation.City = _httpContextAccessor.HttpContext.Request.Cookies["City"];
                cookieHasData = true;
            }
            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("StateCode")) { 
                currentLocation.StateCode = _httpContextAccessor.HttpContext.Request.Cookies["StateCode"];
                cookieHasData = true;
            }
            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("ZipCode")) { 
                currentLocation.ZipCode = _httpContextAccessor.HttpContext.Request.Cookies["ZipCode"];
                cookieHasData = true;
            }

            if (!(_httpContextAccessor.HttpContext.Request.Cookies["Latitude"] is null) && !(_httpContextAccessor.HttpContext.Request.Cookies["Longitude"] is null))
            {
                currentLocation.Latitude = double.Parse(_httpContextAccessor.HttpContext.Request.Cookies["Latitude"]);
                currentLocation.Longitude = double.Parse(_httpContextAccessor.HttpContext.Request.Cookies["Longitude"]);
                cookieHasData = true;
            }
        }

        public void WriteLocation()
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Domain = ".localhost";
            cookieOptions.Expires = System.DateTime.Now.AddDays(1);

            if (!string.IsNullOrEmpty(currentLocation.City))
                _httpContextAccessor.HttpContext.Response.Cookies.Append("City", currentLocation.City, cookieOptions);
            if (!string.IsNullOrEmpty(currentLocation.StateCode))
                _httpContextAccessor.HttpContext.Response.Cookies.Append("StateCode", currentLocation.StateCode, cookieOptions);
            if (!string.IsNullOrEmpty(currentLocation.ZipCode))
                _httpContextAccessor.HttpContext.Response.Cookies.Append("ZipCode", currentLocation.ZipCode, cookieOptions);
            if (currentLocation.Latitude != 0)
                _httpContextAccessor.HttpContext.Response.Cookies.Append("Latitude", currentLocation.Latitude.ToString(), cookieOptions);
            if (currentLocation.Longitude != 0)
                _httpContextAccessor.HttpContext.Response.Cookies.Append("Longitude", currentLocation.Longitude.ToString(), cookieOptions);
        }
    }
}
