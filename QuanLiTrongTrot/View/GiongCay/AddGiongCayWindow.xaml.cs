using QuanLiTrongTrot.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLiTrongTrot.View.GiongCay
{
    public partial class AddGiongCayWindow : Window
    {
        private int? _editId = null;

        public AddGiongCayWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        public AddGiongCayWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);
                txtTen.Text = row["Ten"].ToString();
                txtNguonGoc.Text = row["NguonGoc"].ToString();
                txtDacTinh.Text = row["DacTinh"].ToString();
                txtThoiGianThuHoach.Text = row["ThoiGianThuHoach"].ToString();
                cboGiong.SelectedValue = Convert.ToInt32(row["GiongId"]);
                cboVungTrong.SelectedValue = Convert.ToInt32(row["VTId"]);

                this.Title = "Sửa Giống Cây Đầu Dòng";
            }
        }

        private void LoadComboBoxData()
        {
            try
            {
                string queryGiong = "SELECT Id, Ten FROM GiongCayChinh ORDER BY Id";
                DataTable dtGiong = DataProvider.ExecuteQuery(queryGiong);
                cboGiong.ItemsSource = dtGiong.DefaultView;

                string queryVungTrong = "SELECT Id, DiaChi FROM VungTrong ORDER BY Id";
                DataTable dtVungTrong = DataProvider.ExecuteQuery(queryVungTrong);
                cboVungTrong.ItemsSource = dtVungTrong.DefaultView;
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
                MessageBox.Show("Vui lòng nhập tên giống cây!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTen.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNguonGoc.Text))
            {
                MessageBox.Show("Vui lòng nhập nguồn gốc!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNguonGoc.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDacTinh.Text))
            {
                MessageBox.Show("Vui lòng nhập đặc tính!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDacTinh.Focus();
                return;
            }

            if (!int.TryParse(txtThoiGianThuHoach.Text, out int thoiGian) || thoiGian <= 0)
            {
                MessageBox.Show("Thời gian thu hoạch phải là số nguyên dương!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtThoiGianThuHoach.Focus();
                return;
            }

            if (cboGiong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn giống!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cboVungTrong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn vùng trồng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_editId == null)
                {
                    // Thêm mới với ID tự động lấp gap
                    int newId = DataProvider.GetNextId("GiongCayDauDong");

                    string query = @"
                        SET IDENTITY_INSERT GiongCayDauDong ON;
                        INSERT INTO GiongCayDauDong (Id, Ten, NguonGoc, DacTinh, ThoiGianThuHoach, GiongId, VTId) 
                        VALUES (@Id, @Ten, @NguonGoc, @DacTinh, @ThoiGianThuHoach, @GiongId, @VTId);
                        SET IDENTITY_INSERT GiongCayDauDong OFF;";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@NguonGoc", txtNguonGoc.Text),
                        new SqlParameter("@DacTinh", txtDacTinh.Text),
                        new SqlParameter("@ThoiGianThuHoach", thoiGian),
                        new SqlParameter("@GiongId", cboGiong.SelectedValue),
                        new SqlParameter("@VTId", cboVungTrong.SelectedValue));

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
                    string query = @"UPDATE GiongCayDauDong 
                                     SET Ten = @Ten, NguonGoc = @NguonGoc, DacTinh = @DacTinh, 
                                         ThoiGianThuHoach = @ThoiGianThuHoach, GiongId = @GiongId, VTId = @VTId 
                                     WHERE Id = @Id";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@NguonGoc", txtNguonGoc.Text),
                        new SqlParameter("@DacTinh", txtDacTinh.Text),
                        new SqlParameter("@ThoiGianThuHoach", thoiGian),
                        new SqlParameter("@GiongId", cboGiong.SelectedValue),
                        new SqlParameter("@VTId", cboVungTrong.SelectedValue));

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
                txtNguonGoc.Text = "";
                txtDacTinh.Text = "";
                txtThoiGianThuHoach.Text = "";
                cboGiong.SelectedIndex = -1;
                cboVungTrong.SelectedIndex = -1;
                txtTen.Focus();
            }
            else
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa giống cây này?", "Xác nhận",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "GiongCayDauDong");
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