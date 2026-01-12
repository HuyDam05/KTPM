using System.Collections.Generic;
using Newtonsoft.Json;
using QuanLiTrongTrot.ViewModel;


namespace QuanLiTrongTrot.Model
{
    public class WeatherResponse
    {
        [JsonProperty("main")]
        public MainInfo Main { get; set; }

        [JsonProperty("weather")]
        public List<WeatherInfo> Weather { get; set; }

        [JsonProperty("name")]
        public string CityName { get; set; }
    }

    public class MainInfo
    {
        [JsonProperty("temp")]
        public double Temperature { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }
    }
    public class WeatherInfo
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
