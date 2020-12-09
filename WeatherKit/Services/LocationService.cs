using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public class LocationService : ILocationService
    {
        private LocationInput currentLocation;
        private bool cookieHasData = false;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private System.Net.IPAddress IPAddress;
        private List<CityOptions> cityOptionsList;


        public LocationService(IHttpContextAccessor httpContextAccessor)
        {
            //currentLocation = new LocationInput();
            _httpContextAccessor = httpContextAccessor;

            ReadCityOptionsList();
            // Read location fromthe cookie
            ReadLocation();
            IPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
        }

        public bool CookieHasData()
        {
            // cookie contains City, or lat & long, then true else false
            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("City") ||
                (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("Latitude")
                && _httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("Longitude")))
                return true;
            else
                return false;
            //return cookieHasData;
        }

        public LocationInput GetLocation()
        {
            if (currentLocation == null)
                ReadLocation();
            return currentLocation;
        }

        public System.Net.IPAddress GetIP()
        {
            return IPAddress;
        }

        public List<CityOptions> GetCityOptionsList()
        {
            return cityOptionsList;
        }

        // Updates currentLocation & cookie
        public void UpdateLocation(LocationInput newLocation)
        { 
            currentLocation = newLocation;
            WriteLocation();
        }

        public void ReadLocation()
        {
            if (currentLocation == null)
                currentLocation = new LocationInput();

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
            if (currentLocation == null)
                return;

            CookieOptions cookieOptions = new CookieOptions();
            //cookieOptions.Domain = "https://weatherkit.azurewebsites.net"; // .weatherkit.azurewebsites.net
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

        public void ReadCityOptionsList()
        {
            cityOptionsList = new List<CityOptions>();
            List<CityInfo> tempList = new List<CityInfo>();

            using (StreamReader r = new StreamReader("wwwroot/json/city.list.json"))
            {
                string json = r.ReadToEnd();
                tempList = JsonConvert.DeserializeObject<List<CityInfo>>(json);
            }

            foreach (var city in tempList)
            {
                CityOptions co = new CityOptions();
                co.name = city.name;
                co.state = city.state;
                co.country = city.country;
                cityOptionsList.Add(co);
            }

            return;
        }
    }
}
