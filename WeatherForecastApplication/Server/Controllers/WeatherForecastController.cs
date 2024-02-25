using Microsoft.AspNetCore.Mvc;
using WeatherForecastApplication.Server.Clients;
using WeatherForecastApplication.Server.Services;
using WeatherForecastApplication.Shared;

namespace WeatherForecastApplication.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherForecastClient _forecastClient;
        private readonly Dictionary<string, CityModel> _cache = new Dictionary<string, CityModel>();

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherForecastClient forecastClient)
        {
            _logger = logger;
            _forecastClient = forecastClient;
            var csvReader = new CityCSVService();
            var cities = csvReader.ReadCityData();
            foreach (var city in cities) {
                _cache.TryAdd(city.City, city);
            }
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecastModel>> Get()
        {
            var cities = _cache.Values.ToList();
            var someCities = cities.GetRange(0, 10);
            var tasks = someCities.Select(cityData => _forecastClient.GetWeatherForecast(cityData)).ToArray();
            var forecasts = await Task.WhenAll(tasks);
            return forecasts;
        }
    }
}
