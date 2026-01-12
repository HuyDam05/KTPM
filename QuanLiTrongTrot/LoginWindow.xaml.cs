using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLiTrongTrot
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        public string Normal_text()
        {
            return (txtUsername.Text ?? string.Empty).ToLowerInvariant();
        }
        private bool Track_Login()
        {
            try
            {
                var username = Normal_text();
                var password = txtPassword.Password;
                bool a = DataProvider.CHECK_DATA_EXISTS(username, "Ten", "TaiKhoan");
                bool b = DataProvider.CHECK_DATA_EXISTS(password, "MatKhau", "TaiKhoan");
                bool c = DataProvider.CHECK_SAME_ROW_DATA(new string[] { username, password }, new string[] { "Ten", "MatKhau" }, "TaiKhoan");
                return a && b && c;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối database: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if ( Track_Login() )
            {
                MainWindow mainWindow = new MainWindow(txtUsername.Text);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterPb registerPb = new RegisterPb();
            registerPb.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

