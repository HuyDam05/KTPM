using Newtonsoft.Json;
using QuanLiTrongTrot.Model;
using System.Net.Http;
using System.Threading.Tasks;
using QuanLiTrongTrot.Model;

namespace QuanLiTrongTrot.Services
{
    public class WeatherService
    {
        public async Task<WeatherResponse> GetWeatherAsync(string city)
        {
            string apiKey = "bdf9f78488fe15f8846027d64183cd37";
            string url =
                $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=vi";

            HttpClient client = new HttpClient();
            string json = await client.GetStringAsync(url);

            return JsonConvert.DeserializeObject<WeatherResponse>(json);
        }
    }
}
