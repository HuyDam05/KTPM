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
using System.Windows.Shapes;
using QuanLiTrongTrot.Model;

namespace QuanLiTrongTrot
{
    /// <summary>
    /// Interaction logic for Regproblems.xaml
    /// </summary>
    public partial class RegisterPb : Window
    {
        public RegisterPb()
        {
            InitializeComponent();
        }

        private void BtnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            string username = txtForgotUsername.Text;
            username.ToLowerInvariant();
            string newPassword = txtForgotNewPassword.Password;
            string confirmNewPassword = txtForgotConfirmPassword.Password;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmNewPassword))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DataProvider.CHECK_DATA_EXISTS(username, "Ten", "TaiKhoan") == false)
            {
                MessageBox.Show("Tên đăng nhập không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (newPassword != confirmNewPassword)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                try
                {
                    string[] set = { "Ten", "MatKhau" };
                    string[] values = { username, newPassword };
                    int resulttk = DataProvider.CHANGE_DATA(values, set, "TaiKhoan");
                    if (resulttk > 0)
                    {
                        MessageBox.Show("Đặt lại mật khẩu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi kết nối database: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtNewUsername.Text;
            username.ToLowerInvariant();
            string password = txtNewPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                bool userExists = DataProvider.CHECK_DATA_EXISTS(username, "Ten", "TaiKhoan");
                if (userExists)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string[] set = { "Ten", "MatKhau" };
                string[] values = { username, password };
                int resulttk = DataProvider.INSERT_DATA(values, set, "TaiKhoan");
                if (resulttk > 0)
                {
                    MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối database: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
