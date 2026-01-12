using QuanLiTrongTrot.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLiTrongTrot.View.TrangChu
{
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
            LoadQuyenData();
        }

        private void LoadQuyenData()
        {
            try
            {
                DataTable dtQuyen = new DataTable();
                dtQuyen.Columns.Add("QuyenID", typeof(int));
                dtQuyen.Columns.Add("TenQuyen", typeof(string));

                dtQuyen.Rows.Add(1, "Developer");
                dtQuyen.Rows.Add(2, "Admin");
                dtQuyen.Rows.Add(3, "Staff");

                cboQuyen.ItemsSource = dtQuyen.DefaultView;
                cboQuyen.SelectedIndex = 2; // Mặc định chọn Staff
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách quyền: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Tạo Hồ sơ mới trong bảng HoSo và trả về Id
        /// </summary>
        private int CreateNewHoSo()
        {
            // INSERT vào bảng HoSo và lấy Id vừa tạo
            string query = "INSERT INTO HoSo DEFAULT VALUES; SELECT SCOPE_IDENTITY();";
            int newId = DataProvider.ExecuteScalar(query);
            return newId;
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            var username = (txtUsername.Text ?? string.Empty).Trim();
            var password = pbPassword.Password ?? string.Empty;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cboQuyen.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn quyền cho tài khoản!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (DataProvider.CHECK_DATA_EXISTS(username, "Ten", "TaiKhoan"))
                {
                    MessageBox.Show("Tài khoản đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string quyenId = cboQuyen.SelectedValue.ToString();
                
                // Bước 1: Tạo Hồ sơ mới trong bảng HoSo trước
                int newHoSoId = CreateNewHoSo();

                // Bước 2: Tạo TaiKhoan với HoSoId vừa tạo
                string[] values = { username, password, quyenId, newHoSoId.ToString() };
                string[] data_names = { "Ten", "MatKhau", "QuyenID", "HoSoId" };

                int result = DataProvider.INSERT_DATA(values, data_names, "TaiKhoan");

                if (result > 0)
                {
                    MessageBox.Show($"Thêm mới thành công!\nHồ sơ ID: {newHoSoId}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
