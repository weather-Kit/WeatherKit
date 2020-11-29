using Microsoft.AspNetCore.Http;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public interface ISettingService 
    {
        public Setting GetSetting();

        public void UpdateSetting(Setting setting);

        public void ReadSetting(HttpContext context);
        public void WriteSetting(HttpContext context);
    }
}
