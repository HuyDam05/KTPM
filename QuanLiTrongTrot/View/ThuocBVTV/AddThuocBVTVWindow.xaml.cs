using QuanLiTrongTrot.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLiTrongTrot.View.ThuocBVTV
{
    public partial class AddThuocBVTVWindow : Window
    {
        private int? _editId = null;

        public AddThuocBVTVWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
            dpNgaySX.SelectedDate = DateTime.Today;
            dpHanSD.SelectedDate = DateTime.Today.AddYears(1);
        }

        public AddThuocBVTVWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);
                txtTen.Text = row["Ten"].ToString();
                dpNgaySX.SelectedDate = Convert.ToDateTime(row["NgaySX"]);
                dpHanSD.SelectedDate = Convert.ToDateTime(row["HanSD"]);
                cboVungTrong.SelectedValue = Convert.ToInt32(row["VTId"]);

                this.Title = "Sửa Thuốc BVTV";
            }
        }

        private void LoadComboBoxData()
        {
            try
            {
                string query = "SELECT Id, DiaChi FROM VungTrong ORDER BY Id";
                DataTable dt = DataProvider.ExecuteQuery(query);
                cboVungTrong.ItemsSource = dt.DefaultView;
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
                MessageBox.Show("Vui lòng nhập tên thuốc!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTen.Focus();
                return;
            }

            if (dpNgaySX.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày sản xuất!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpHanSD.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn hạn sử dụng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpHanSD.SelectedDate <= dpNgaySX.SelectedDate)
            {
                MessageBox.Show("Hạn sử dụng phải sau ngày sản xuất!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    int newId = DataProvider.GetNextId("ThuocBVTV");

                    string query = @"
                        SET IDENTITY_INSERT ThuocBVTV ON;
                        INSERT INTO ThuocBVTV (Id, Ten, NgaySX, HanSD, VTId) 
                        VALUES (@Id, @Ten, @NgaySX, @HanSD, @VTId);
                        SET IDENTITY_INSERT ThuocBVTV OFF;";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@NgaySX", dpNgaySX.SelectedDate.Value),
                        new SqlParameter("@HanSD", dpHanSD.SelectedDate.Value),
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
                    string query = @"UPDATE ThuocBVTV 
                                     SET Ten = @Ten, NgaySX = @NgaySX, HanSD = @HanSD, VTId = @VTId 
                                     WHERE Id = @Id";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@NgaySX", dpNgaySX.SelectedDate.Value),
                        new SqlParameter("@HanSD", dpHanSD.SelectedDate.Value),
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
                dpNgaySX.SelectedDate = DateTime.Today;
                dpHanSD.SelectedDate = DateTime.Today.AddYears(1);
                cboVungTrong.SelectedIndex = -1;
                txtTen.Focus();
            }
            else
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa thuốc này?", "Xác nhận",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "ThuocBVTV");
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
