using QuanLiTrongTrot.Model;
using System;
using System.Windows;

namespace QuanLiTrongTrot.View.TrangChu
{
    public partial class AddXaWindow : Window
    {
        public AddXaWindow(string tinh = null)
        {
            InitializeComponent();
            if (!string.IsNullOrWhiteSpace(tinh))
                txtTinh.Text = tinh;
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            var tenXa = (txtTenXa.Text ?? string.Empty).Trim();
            var tinh = (txtTinh.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(tenXa) || string.IsNullOrWhiteSpace(tinh))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Lấy MaVung từ tên Tỉnh
                string queryMaVung = "SELECT MaVung FROM Tinh WHERE Ten = @Ten";
                var dt = DataProvider.ExecuteQuery(queryMaVung, new object[] { tinh });
                
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy tỉnh!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string maVung = dt.Rows[0]["MaVung"].ToString();
                
                string[] values = { tenXa, maVung };
                string[] data_names = { "Ten", "MaTinh" };

                int result = DataProvider.INSERT_DATA(values, data_names, "Xa");
                if (result > 0)
                {
                    MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                    return;
                }

                MessageBox.Show("Thao tác không thành công!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thao tác: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
