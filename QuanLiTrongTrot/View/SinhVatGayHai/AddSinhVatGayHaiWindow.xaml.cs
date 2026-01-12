    using QuanLiTrongTrot.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLiTrongTrot.View.SinhVatGayHai
{
    public partial class AddSinhVatGayHaiWindow : Window
    {
        private int? _editId = null;

        public AddSinhVatGayHaiWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        public AddSinhVatGayHaiWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);
                txtTen.Text = row["Ten"].ToString();
                txtPhanLoai.Text = row["PhanLoai"].ToString();
                
                // Set lại ComboBox theo tên cây (CayAH là nvarchar)
                string cayAH = row["CayAH"]?.ToString() ?? "";
                foreach (DataRowView item in cboCayAH.ItemsSource)
                {
                    if (item["Ten"].ToString() == cayAH)
                    {
                        cboCayAH.SelectedItem = item;
                        break;
                    }
                }
                
                if (row["VTId"] != DBNull.Value)
                {
                    cboVungTrong.SelectedValue = Convert.ToInt32(row["VTId"]);
                }
                
                this.Title = "Sửa Sinh Vật Gây Hại";
            }
        }

        private void LoadComboBoxData()
        {
            try
            {
                string queryCay = "SELECT Id, Ten FROM GiongCayDauDong ORDER BY Id";
                DataTable dtCay = DataProvider.ExecuteQuery(queryCay);
                cboCayAH.ItemsSource = dtCay.DefaultView;

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
                MessageBox.Show("Vui lòng nhập tên sinh vật!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTen.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhanLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập phân loại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhanLoai.Focus();
                return;
            }

            if (cboCayAH.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn cây ảnh hưởng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cboVungTrong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn vùng trồng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // CayAH là nvarchar => lấy TÊN cây, không phải Id
                var selectedCay = cboCayAH.SelectedItem as DataRowView;
                string tenCay = selectedCay?["Ten"]?.ToString() ?? "";

                if (_editId == null)
                {
                    // Thêm mới với ID tự động lấp gap
                    int newId = DataProvider.GetNextId("SinhVatGayHai");

                    string query = @"
                        SET IDENTITY_INSERT SinhVatGayHai ON;
                        INSERT INTO SinhVatGayHai (Id, Ten, PhanLoai, CayAH, VTId) 
                        VALUES (@Id, @Ten, @PhanLoai, @CayAH, @VTId);
                        SET IDENTITY_INSERT SinhVatGayHai OFF;";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@PhanLoai", txtPhanLoai.Text),
                        new SqlParameter("@CayAH", tenCay),
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
                    string query = @"UPDATE SinhVatGayHai 
                                     SET Ten = @Ten, PhanLoai = @PhanLoai, CayAH = @CayAH, VTId = @VTId 
                                     WHERE Id = @Id";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@PhanLoai", txtPhanLoai.Text),
                        new SqlParameter("@CayAH", tenCay),
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
                txtPhanLoai.Text = "";
                cboCayAH.SelectedIndex = -1;
                cboVungTrong.SelectedIndex = -1;
                txtTen.Focus();
            }
            else
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa sinh vật gây hại này?", "Xác nhận",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "SinhVatGayHai");
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
