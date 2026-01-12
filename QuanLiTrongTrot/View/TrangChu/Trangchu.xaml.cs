using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using QuanLiTrongTrot.Model;
using QuanLiTrongTrot.ViewModel;

namespace QuanLiTrongTrot.View.TrangChu
{
    /// <summary>
    /// Interaction logic for Trangchu.xaml
    /// </summary>
    public partial class TrangChu : UserControl
    {
        private readonly WeatherViewModel _vm = new WeatherViewModel();
        public TrangChu()
        {
            
            InitializeComponent();
            DataContext = _vm;
            Loaded += async (_, __) => await _vm.LoadWeatherAsync();
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Update_Timer;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        public void Update_Timer(object sender, EventArgs e)
        {
            DisplayTime.Text = DateTime.Now.ToString();
        }
    }
}
