using QuanLiTrongTrot.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLiTrongTrot.View.PhanBon
{
    public partial class AddPhanBonWindow : Window
    {
        private int? _editId = null;

        public AddPhanBonWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        public AddPhanBonWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);
                txtTen.Text = row["Ten"].ToString();
                txtThanhPhan.Text = row["ThanhPhan"].ToString();
                txtPhanLoai.Text = row["PhanLoai"].ToString();
                cboVungTrong.SelectedValue = Convert.ToInt32(row["VTId"]);

                this.Title = "Sửa Phân Bón";
            }
        }

        private void LoadComboBoxData()
        {
            try
            {
                string queryVungTrong = "SELECT Id, DiaChi FROM VungTrong";
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
                MessageBox.Show("Vui lòng nhập tên phân bón!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTen.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtThanhPhan.Text))
            {
                MessageBox.Show("Vui lòng nhập thành phần!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtThanhPhan.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhanLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập phân loại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhanLoai.Focus();
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
                    int newId = DataProvider.GetNextId("PhanBon");

                    string query = @"
                        SET IDENTITY_INSERT PhanBon ON;
                        INSERT INTO PhanBon (Id, Ten, ThanhPhan, PhanLoai, VTId) 
                        VALUES (@Id, @Ten, @ThanhPhan, @PhanLoai, @VTId);
                        SET IDENTITY_INSERT PhanBon OFF;";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@ThanhPhan", txtThanhPhan.Text),
                        new SqlParameter("@PhanLoai", txtPhanLoai.Text),
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
                    string query = @"UPDATE PhanBon 
                                     SET Ten = @Ten, ThanhPhan = @ThanhPhan, PhanLoai = @PhanLoai, VTId = @VTId 
                                     WHERE Id = @Id";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@ThanhPhan", txtThanhPhan.Text),
                        new SqlParameter("@PhanLoai", txtPhanLoai.Text),
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
                txtThanhPhan.Text = "";
                txtPhanLoai.Text = "";
                cboVungTrong.SelectedIndex = -1;
                txtTen.Focus();
            }
            else
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa phân bón này?", "Xác nhận",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "PhanBon");
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
