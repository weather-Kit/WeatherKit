using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public class WeatherAPIService : IWeatherAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISettingService _settingService;
        private readonly string meterBySecUnit = "meter/sec";
        private readonly string milesByhourUnit = "miles/hour";

        public string JSONContent { get; set; }
        private string URL;

        public WeatherAPIService(IHttpClientFactory httpClientFactory,
            ISettingService settingService)
        {
            _httpClientFactory = httpClientFactory;
            _settingService = settingService;
        }

        public string GetJSONContent()
        {
            return JSONContent;
        }
        public string GetURL()
        {
            return URL;
        }

        /*
          * api.openweathermap.org/data/2.5/weather?q={city name}&appid={API key}   - WORKING
             api.openweathermap.org/data/2.5/weather?q={city name},{state code},{country code}&appid={API key}      - WORKING
             api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={API key}    - WORKING
             api.openweathermap.org/data/2.5/weather?zip={zip code}&appid={API key}     - WORKING
        * */
        public async Task<Forecast> GetWeatherForecasts(LocationInput locationInput)
        {
            // Create base uri
            UriBuilder builder = new UriBuilder("http://api.openweathermap.org/data/2.5/weather");
            builder.Query = "";

            // If City, State is provided
            if (!string.IsNullOrEmpty(locationInput.City))
            {
                builder.Query = $"q={locationInput.City}";

                if (!string.IsNullOrEmpty(locationInput.StateCode) && !string.IsNullOrEmpty(locationInput.CountryCode))
                {
                    builder.Query += $",{locationInput.StateCode},{locationInput.CountryCode}";
                }
            }
            // If GeoLocation is provided
            else if (locationInput.Latitude != 0 && locationInput.Longitude != 0)
            {
                builder.Query = $"lat={locationInput.Latitude}&lon={locationInput.Longitude}";
            }
            // If zipcode is provided
            else if (!string.IsNullOrEmpty(locationInput.ZipCode))
            {
                builder.Query = $"zip={locationInput.ZipCode}";
            }

            if (builder.Query.Length > 0)
            {
                Setting setting = _settingService.GetSetting();

                if (setting.Units != Units.Standard)
                {
                    string unitType = setting.Units == Units.Imperial ? "imperial" : "metric";
                    builder.Query += $"&units={unitType}";
                }

                builder.Query += "&appid=1e94cd79afa39de4db034e687033b2de";

                // Get the HttpClient & make the request call
                HttpClient client = _httpClientFactory.CreateClient("API Client");
                var result = await client.GetAsync(builder.Uri);
                URL = builder.Query;

                if (result.IsSuccessStatusCode)
                {
                    // Read all of the response and deserialise it into an instace of
                    // Forecast class
                    var content = await result.Content.ReadAsStringAsync();
                    JSONContent = content;
                    Forecast forecast = JsonConvert.DeserializeObject<Forecast>(content);

                    if (forecast != null)
                    {
                        forecast.Date = ConvertUnixTimestampToDate(forecast.dt, forecast.timezone);
                        forecast.sys.SunRise = ConvertUnixTimestampToDate(forecast.sys.sunrise, forecast.timezone);
                        forecast.sys.SunSet = ConvertUnixTimestampToDate(forecast.sys.sunset, forecast.timezone);

                        if (setting.Units == Units.Standard || setting.Units == Units.Metric)
                        {
                            forecast.wind.GustUnit = meterBySecUnit;
                            forecast.wind.SpeedUnit = meterBySecUnit;
                            forecast.main.TempUnit = "C";
                        }
                        else if (setting.Units == Units.Imperial)
                        {
                            forecast.wind.GustUnit = milesByhourUnit;
                            forecast.wind.SpeedUnit = milesByhourUnit;
                            forecast.main.TempUnit = "F";
                        }
                    }

                    return forecast;
                }
            }

            return null;
        }

        private DateTime ConvertUnixTimestampToDate(long timeStamp, int timezone)
        {
            timeStamp += timezone;  
            // DateTime object for UTC 
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            // Add the timestamp 
            dateTime = dateTime.AddSeconds(timeStamp);

            /**
            // Get Local timezone name  "timezone":-18000,
            string timeZoneName = TimeZoneInfo.Local.IsDaylightSavingTime(dateTime) ?
                        TimeZoneInfo.Local.DaylightName : TimeZoneInfo.Local.StandardName;
            
            // Set timeZone
            timeZone = timeZoneName;

            // Find TimeZoneInfo
            tst = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            // Convvert UTC to TimeZone
            DateTime converted = TimeZoneInfo.ConvertTimeFromUtc(dateTime, tst);
*/
            return dateTime;
        }
    }
}
