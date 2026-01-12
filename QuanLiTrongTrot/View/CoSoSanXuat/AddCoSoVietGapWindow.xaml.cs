using QuanLiTrongTrot.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLiTrongTrot.View.CoSoSanXuat
{
    public partial class AddCoSoVietGapWindow : Window
    {
        private int? _editId = null;

        public AddCoSoVietGapWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        public AddCoSoVietGapWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);
                txtTen.Text = row["Ten"]?.ToString() ?? "";
                
                // Set địa chỉ theo BanDoId (liên kết với VungTrong)
                if (row["BanDoId"] != DBNull.Value)
                {
                    cboDiaChi.SelectedValue = Convert.ToInt32(row["BanDoId"]);
                }

                this.Title = "Sửa Cơ Sở Đủ ATTP VietGap";
            }
        }

        private void LoadComboBoxData()
        {
            try
            {
                // Lấy danh sách địa chỉ từ bảng VungTrong
                string query = "SELECT Id, DiaChi FROM VungTrong ORDER BY DiaChi";
                DataTable dt = DataProvider.ExecuteQuery(query);
                cboDiaChi.ItemsSource = dt.DefaultView;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Chưa có vùng trồng nào!\nVui lòng thêm vùng trồng trước.",
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
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên cơ sở!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTen.Focus();
                return;
            }

            if (cboDiaChi.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn địa chỉ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int diaChiId = Convert.ToInt32(cboDiaChi.SelectedValue);
                
                // Lấy DiaChi text để lưu vào cột DiaChi
                var selectedItem = cboDiaChi.SelectedItem as DataRowView;
                string diaChiText = selectedItem?["DiaChi"]?.ToString() ?? "";

                if (_editId == null)
                {
                    // Thêm mới với ID tự động lấp gap
                    int newId = DataProvider.GetNextId("CS_VG");

                    string query = @"
                        SET IDENTITY_INSERT CS_VG ON;
                        INSERT INTO CS_VG (Id, Ten, DiaChi, BanDoId) 
                        VALUES (@Id, @Ten, @DiaChi, @BanDoId);
                        SET IDENTITY_INSERT CS_VG OFF;";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@DiaChi", diaChiText),
                        new SqlParameter("@BanDoId", diaChiId));

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
                    string query = @"UPDATE CS_VG 
                                     SET Ten = @Ten, DiaChi = @DiaChi, BanDoId = @BanDoId 
                                     WHERE Id = @Id";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@DiaChi", diaChiText),
                        new SqlParameter("@BanDoId", diaChiId));

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
                txtTen.Text = "";
                cboDiaChi.SelectedIndex = -1;
                txtTen.Focus();
            }
            else
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa cơ sở này?", "Xác nhận",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "CS_VG");
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
