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
            ReadSetting();  // Get stored settings values
        }

        public Setting GetSetting()
        {
            return currentSetting;
        }

        public void UpdateSetting(Setting setting)
        {
            currentSetting = setting;
            // Store updated values in cookie
        }

        private void ReadSetting()
        {
            // Read Setting from cookie
        }
    }
}
