using QuanLiTrongTrot.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLiTrongTrot.View.SinhVatGayHai
{
    /// <summary>
    /// Interaction logic for AddTuoiSauWindow.xaml
    /// </summary>
    public partial class AddTuoiSauWindow : Window
    {
        private int? _editId = null;

        public AddTuoiSauWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        public AddTuoiSauWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);
                txtTen.Text = row["Ten"]?.ToString() ?? "";
                txtTuoiSau.Text = row["TuoiSau"]?.ToString() ?? "";
                txtDoPhoBien.Text = row["DoPhoBien"]?.ToString() ?? "";

                if (row["SVId"] != DBNull.Value)
                {
                    cboSinhVat.SelectedValue = Convert.ToInt32(row["SVId"]);
                }

                this.Title = "Sửa Tuổi Sâu - Cấp Độ Phổ Biến";
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
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTen.Focus();
                return;
            }

            if (!int.TryParse(txtTuoiSau.Text, out int tuoiSau) || tuoiSau < 0)
            {
                MessageBox.Show("Tuổi sâu phải là số nguyên không âm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTuoiSau.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDoPhoBien.Text))
            {
                MessageBox.Show("Vui lòng nhập độ phổ biến!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDoPhoBien.Focus();
                return;
            }

            if (cboSinhVat.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn sinh vật gây hại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_editId == null)
                {
                    int newId = DataProvider.GetNextId("TuoiSau");

                    string query = @"
                        SET IDENTITY_INSERT TuoiSau ON;
                        INSERT INTO TuoiSau (Id, Ten, TuoiSau, DoPhoBien, SVId) 
                        VALUES (@Id, @Ten, @TuoiSau, @DoPhoBien, @SVId);
                        SET IDENTITY_INSERT TuoiSau OFF;";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@TuoiSau", tuoiSau),
                        new SqlParameter("@DoPhoBien", txtDoPhoBien.Text),
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
                    string query = @"UPDATE TuoiSau 
                                     SET Ten = @Ten, TuoiSau = @TuoiSau, DoPhoBien = @DoPhoBien, SVId = @SVId 
                                     WHERE Id = @Id";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@TuoiSau", tuoiSau),
                        new SqlParameter("@DoPhoBien", txtDoPhoBien.Text),
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
                txtTen.Text = "";
                txtTuoiSau.Text = "";
                txtDoPhoBien.Text = "";
                cboSinhVat.SelectedIndex = -1;
                txtTen.Focus();
            }
            else
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "TuoiSau");
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
