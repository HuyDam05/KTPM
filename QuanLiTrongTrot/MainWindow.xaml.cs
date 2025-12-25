using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLiTrongTrot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // Xử lý click Menu Tab (thanh ngang)
        private void MenuTab_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();

            switch (tag)
            {
                case "TrangChu":
                    txtWelcome.Text = "Trang chủ";
                    break;
                case "PhanBon":
                    txtWelcome.Text = "Quản lý Phân Bón";
                    break;
                case "ThuocBVTV":
                    txtWelcome.Text = "Quản lý Thuốc BVTV";
                    break;
                case "VungTrong":
                    txtWelcome.Text = "Quản lý Vùng Trồng";
                    break;
                case "CoSoSanXuat":
                    txtWelcome.Text = "Quản lý Cơ Sở Sản Xuất";
                    break;
                case "CoSoBuonBan":
                    txtWelcome.Text = "Quản lý Cơ Sở Buôn Bán";
                    break;
                case "GiongCay":
                    txtWelcome.Text = "Quản lý Giống Cây";
                    break;
                case "SinhVatGayHai":
                    txtWelcome.Text = "Quản lý Sinh Vật Gây Hại";
                    break;
            }
        }

        // Xử lý click Sidebar (menu trái)
        private void Sidebar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();

            switch (tag)
            {
                case "QuanLyNguoiDung":
                    txtWelcome.Text = "Quản lý Người dùng";
                    break;
                case "LichSuDangNhap":
                    txtWelcome.Text = "Lịch sử đăng nhập";
                    break;
                case "QuanLyHanhChinh":
                    txtWelcome.Text = "Quản lý Hành chính";
                    break;
                case "DonViCapHuyen":
                    txtWelcome.Text = "Đơn vị cấp Huyện";
                    break;
                case "DonViCapXa":
                    txtWelcome.Text = "Đơn vị cấp Xã";
                    break;
                case "DoiMatKhau":
                    txtWelcome.Text = "Đổi mật khẩu";
                    break;
            }
        }

        // Đăng xuất
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}

