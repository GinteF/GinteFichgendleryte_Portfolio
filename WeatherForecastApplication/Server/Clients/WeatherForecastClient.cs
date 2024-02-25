using WeatherForecastApplication.Shared;

namespace WeatherForecastApplication.Server.Clients
{
    public class WeatherForecastClient
    {
        private readonly HttpClient _httpClient;

        public WeatherForecastClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WeatherForecastApplication/1.0");
        }

        public async Task<WeatherForecastModel> GetWeatherForecast(CityModel cityModel)
        {
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/complete?lat={cityModel.Latitude}&lon={cityModel.Longitude}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<WeatherForecastResponse>();

            var weatherForecast = result.properties.timeseries.FirstOrDefault().data.instant.details;
            weatherForecast.city_name = cityModel.City;
            return weatherForecast;
        }
    }
}

public class Instant
{
    public WeatherForecastModel details { get; set; }
}

public class Data
{
    public Instant instant { get; set; }
}

public class TimeSeries
{
    public DateTime time { get; set; }
    public Data data { get; set; }
}

public class Properties
{
    public List<TimeSeries> timeseries { get; set; }
}

public class WeatherForecastResponse
{
    public Properties properties { get; set; }
}
