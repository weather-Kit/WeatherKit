using Microsoft.AspNetCore.Http;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public class SettingService : ISettingService
    {
        private Setting currentSetting;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SettingService(IHttpContextAccessor httpContextAccessor)
        {
            currentSetting = new Setting();
            _httpContextAccessor = httpContextAccessor;

            // Read the saved Settings
            ReadSetting(httpContextAccessor.HttpContext);
        }

        public Setting GetSetting()
        {
            return currentSetting;
        }

        // Updates Setting model & cookie
        public void UpdateSetting(Setting setting)
        {
            //update currentSetting with external setting object
            currentSetting = setting;
            // Update/Write updated content to cookie
            WriteSetting(_httpContextAccessor.HttpContext);
        }

        public void ReadSetting(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey("Is24HourTimeFormat"))
                currentSetting.Is24HourTimeFormat = bool.Parse(context.Request.Cookies["Is24HourTimeFormat"]);
            if (context.Request.Cookies.ContainsKey("Units"))
                currentSetting.Units = (Units)int.Parse(context.Request.Cookies["Units"]);

            // If Settings are not saved in cookie, save it
            if (!context.Request.Cookies.ContainsKey("Is24HourTimeFormat") &&
                !context.Request.Cookies.ContainsKey("Units"))
            {
                WriteSetting(context);
            }
        }

        public void WriteSetting(HttpContext context)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Domain = ".localhost";
            cookieOptions.Expires = System.DateTime.Now.AddDays(1);

            context.Response.Cookies.Append("Is24HourTimeFormat", currentSetting.Is24HourTimeFormat.ToString(), cookieOptions);
            context.Response.Cookies.Append("Units", ((int)currentSetting.Units).ToString(), cookieOptions);
        }
    }
}