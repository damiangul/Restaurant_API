using System.Collections.Generic;

namespace Restaurant_API
{
    public interface IWeatherForcastService
    {
        IEnumerable<WeatherForecast> Get(int results, int minTemp, int maxTemp);
    }
}