using WeatherKit.Models;

namespace WeatherKit.Services
{
    public interface ISettingService 
    {
        public Setting GetSetting();

        public void UpdateSetting(Setting setting);

    }
}
