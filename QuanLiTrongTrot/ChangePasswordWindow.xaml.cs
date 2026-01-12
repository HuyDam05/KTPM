using System;
using System.Windows;
using QuanLiTrongTrot.Model;

namespace QuanLiTrongTrot
{
 public partial class ChangePasswordWindow : Window
 {
 // Có thể truyền username vào nếu cần
 public string Username { get; set; }
 public ChangePasswordWindow(string username = null)
 {
 InitializeComponent();
 Username = username;
 }

 private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
 {
 if (string.IsNullOrWhiteSpace(pbOldPassword.Password) ||
 string.IsNullOrWhiteSpace(pbNewPassword.Password) ||
 string.IsNullOrWhiteSpace(pbConfirmPassword.Password))
 {
 MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
 return;
 }

 if (pbNewPassword.Password != pbConfirmPassword.Password)
 {
 MessageBox.Show("Mật khẩu mới không khớp.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
 return;
 }
 else
            {
                string[] values = {Username, pbNewPassword.Password };
                string[] datanames = {"Ten","MatKhau"};
                int a = DataProvider.CHANGE_DATA(values,datanames,"TaiKhoan");
                if ( a > 0)
                {
                    MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
            }
                // Tạm thời: đóng window như thao tác thành công.
               
 }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
