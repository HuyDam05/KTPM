using QuanLiTrongTrot.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLiTrongTrot.View.SinhVatGayHai
{
    /// <summary>
    /// Interaction logic for AddCapNhatSVGHWindow.xaml
    /// </summary>
    public partial class AddCapNhatSVGHWindow : Window
    {
        private int? _editId = null;

        public AddCapNhatSVGHWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
            dpNgay.SelectedDate = DateTime.Today;
            txtGio.Text = DateTime.Now.ToString("HH:mm");
        }

        public AddCapNhatSVGHWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);

                if (row["NgayGioCN"] != DBNull.Value)
                {
                    DateTime ngayGio = Convert.ToDateTime(row["NgayGioCN"]);
                    dpNgay.SelectedDate = ngayGio.Date;
                    txtGio.Text = ngayGio.ToString("HH:mm");
                }

                txtTienDo.Text = row["TienDo"]?.ToString() ?? "";

                if (row["SVId"] != DBNull.Value)
                {
                    cboSinhVat.SelectedValue = Convert.ToInt32(row["SVId"]);
                }

                this.Title = "Sửa Cập Nhật Tình Hình SVGH";
            }
        }

        private void LoadComboBoxData()
        {
            try
            {
                string query = "SELECT Id, Ten FROM SinhVatGayHai ORDER BY Id";
                DataTable dt = DataProvider.ExecuteQuery(query);
                cboSinhVat.ItemsSource = dt.DefaultView;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Chưa có sinh vật gây hại nào!\nVui lòng thêm sinh vật gây hại trước.",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            if (cboSinhVat.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn sinh vật gây hại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpNgay.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày cập nhật!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTienDo.Text))
            {
                MessageBox.Show("Vui lòng nhập tiến độ/tình hình!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTienDo.Focus();
                return;
            }

            // Parse giờ
            TimeSpan gio;
            if (!TimeSpan.TryParse(txtGio.Text, out gio))
            {
                MessageBox.Show("Định dạng giờ không hợp lệ! (VD: 08:00)", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtGio.Focus();
                return;
            }

            try
            {
                DateTime ngayGioCN = dpNgay.SelectedDate.Value.Date.Add(gio);

                if (_editId == null)
                {
                    // Thêm mới với ID tự động lấp gap
                    int newId = DataProvider.GetNextId("CapNhat_SVGH");

                    string query = @"
                        SET IDENTITY_INSERT CapNhat_SVGH ON;
                        INSERT INTO CapNhat_SVGH (Id, NgayGioCN, TienDo, SVId) 
                        VALUES (@Id, @NgayGioCN, @TienDo, @SVId);
                        SET IDENTITY_INSERT CapNhat_SVGH OFF;";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@NgayGioCN", ngayGioCN),
                        new SqlParameter("@TienDo", txtTienDo.Text),
                        new SqlParameter("@SVId", cboSinhVat.SelectedValue));

                    if (result > 0)
                    {
                        MessageBox.Show($"Thêm mới thành công! (ID: {newId})", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thao tác không thành công!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Cập nhật
                    string query = @"UPDATE CapNhat_SVGH 
                                     SET NgayGioCN = @NgayGioCN, TienDo = @TienDo, SVId = @SVId 
                                     WHERE Id = @Id";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@NgayGioCN", ngayGioCN),
                        new SqlParameter("@TienDo", txtTienDo.Text),
                        new SqlParameter("@SVId", cboSinhVat.SelectedValue));

                    if (result > 0)
                    {
                        MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thao tác không thành công!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thao tác: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (_editId == null)
            {
                cboSinhVat.SelectedIndex = -1;
                dpNgay.SelectedDate = DateTime.Today;
                txtGio.Text = DateTime.Now.ToString("HH:mm");
                txtTienDo.Text = "";
            }
            else
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa bản cập nhật này?", "Xác nhận",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "CapNhat_SVGH");
                        if (deleteResult > 0)
                        {
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
