using WeatherForecastApplication.Shared;
namespace WeatherForecastApplication.Server.Services
{
    public class CityCSVService
    {
        public List<CityModel> ReadCityData()
        {
            List<CityModel> cityDataList = new List<CityModel>();

            using (var reader = new StreamReader("./Resources/worldcities.csv"))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.Replace("\"", "");
                    var values = line.Split(",");

                    string city = values[0];
                    string lat = values[2].Split(".")[0];
                    string lng = values[3].Split(".")[0];

                    cityDataList.Add(new CityModel
                    {
                        City = city,
                        Latitude = lat,
                        Longitude = lng
                    });
                }
            }

            return cityDataList;
        }
    }
}
