using System.Threading.Tasks;
using WeatherKit.Models;

namespace WeatherKit.Services
{
    public interface IWeatherAPIService
    {
        public Task<Forecast> GetWeatherForecasts(LocationInput locationInput);
    }
}
