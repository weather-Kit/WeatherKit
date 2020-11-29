using Microsoft.AspNetCore.Http;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public class SettingService : ISettingService
    {
        private Setting currentSetting;

        public SettingService()
        {
            currentSetting = new Setting();
        }

        public SettingService(Setting setting)
        {
            currentSetting = setting;
        }

        public Setting GetSetting()
        {
            return currentSetting;
        }

        public void UpdateSetting(Setting setting)
        {
            //update currentSetting with external setting object
            currentSetting = setting;
        }

        public void ReadSetting(HttpContext context)
        {
            currentSetting.Is24HourTimeFormat = bool.Parse(context.Request.Cookies["Is24HourTimeFormat"]);
            currentSetting.Units = (Units)int.Parse(context.Request.Cookies["Units"]);
        }

        public void WriteSetting(HttpContext context)
        {
            context.Response.Cookies.Append("Is24HourTimeFormat", currentSetting.Is24HourTimeFormat.ToString());
            context.Response.Cookies.Append("Units", ((int)currentSetting.Units).ToString());
        }
    }
}
