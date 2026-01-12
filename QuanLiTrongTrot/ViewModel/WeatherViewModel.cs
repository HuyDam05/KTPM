using QuanLiTrongTrot.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using QuanLiTrongTrot.Services;

namespace QuanLiTrongTrot.ViewModel
{
    public class WeatherViewModel : INotifyPropertyChanged
    {
        WeatherService service = new WeatherService();

        private string _cityName;
        public string CityName
        {
            get { return _cityName; }
            set { _cityName = value; OnPropertyChanged(); }
        }

        private string _temperature;
        public string Temperature
        {
            get { return _temperature; }
            set { _temperature = value; OnPropertyChanged(); }
        }

        private string _humidity;
        public string Humidity
        {
            get { return _humidity; }
            set { _humidity = value; OnPropertyChanged(); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }

        public async Task LoadWeatherAsync()
        {
            var data = await service.GetWeatherAsync("Hanoi");

            CityName = data.CityName;
            Temperature = $"Nhiệt độ: {data.Main.Temperature} °C";
            Humidity = $"Độ ẩm: {data.Main.Humidity} %";
            Description = $"Thời tiết: {data.Weather[0].Description}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
