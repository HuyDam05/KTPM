using QuanLiTrongTrot.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLiTrongTrot.View.GiongCay
{
    public partial class AddGiongCayLuuHanhWindow : Window
    {
        private int? _editId = null;

        public AddGiongCayLuuHanhWindow()
        {
            InitializeComponent();
        }

        public AddGiongCayLuuHanhWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);
                txtTen.Text = row["Ten"].ToString();
                txtNoiPhoBien.Text = row["NoiPhoBien"].ToString();
                txtCongDung.Text = row["CongDung"].ToString();
                txtDacDiem.Text = row["DacDiem"].ToString();

                this.Title = "Sửa Giống Cây Lưu Hành";
            }
        }

        /// <summary>
        /// Lấy ID nhỏ nhất còn trống hoặc ID tiếp theo
        /// </summary>
        private int GetNextId()
        {
            string gapQuery = @"
                SELECT TOP 1 t1.Id + 1 AS NextId
                FROM GiongCayLuuHanh t1
                LEFT JOIN GiongCayLuuHanh t2 ON t1.Id + 1 = t2.Id
                WHERE t2.Id IS NULL
                ORDER BY t1.Id";

            int gapId = DataProvider.ExecuteScalar(gapQuery);
            if (gapId > 0)
                return gapId;

            int maxId = DataProvider.ExecuteScalar("SELECT ISNULL(MAX(Id), 0) FROM GiongCayLuuHanh");
            if (maxId == 0)
                return 1;

            int hasId1 = DataProvider.ExecuteScalar("SELECT COUNT(*) FROM GiongCayLuuHanh WHERE Id = 1");
            if (hasId1 == 0)
                return 1;

            return maxId + 1;
        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập loại cây!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTen.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNoiPhoBien.Text))
            {
                MessageBox.Show("Vui lòng nhập nơi phổ biến!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNoiPhoBien.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCongDung.Text))
            {
                MessageBox.Show("Vui lòng nhập công dụng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtCongDung.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDacDiem.Text))
            {
                MessageBox.Show("Vui lòng nhập đặc điểm!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDacDiem.Focus();
                return;
            }

            try
            {
                if (_editId == null)
                {
                    // Thêm mới với ID tự động
                    int newId = GetNextId();

                    string query = @"
                        SET IDENTITY_INSERT GiongCayLuuHanh ON;
                        INSERT INTO GiongCayLuuHanh (Id, Ten, NoiPhoBien, CongDung, DacDiem) 
                        VALUES (@Id, @Ten, @NoiPhoBien, @CongDung, @DacDiem);
                        SET IDENTITY_INSERT GiongCayLuuHanh OFF;";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@NoiPhoBien", txtNoiPhoBien.Text),
                        new SqlParameter("@CongDung", txtCongDung.Text),
                        new SqlParameter("@DacDiem", txtDacDiem.Text));

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
                    string query = @"UPDATE GiongCayLuuHanh 
                                     SET Ten = @Ten, NoiPhoBien = @NoiPhoBien, CongDung = @CongDung, DacDiem = @DacDiem 
                                     WHERE Id = @Id";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@NoiPhoBien", txtNoiPhoBien.Text),
                        new SqlParameter("@CongDung", txtCongDung.Text),
                        new SqlParameter("@DacDiem", txtDacDiem.Text));

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
                // Clear form
                txtTen.Text = "";
                txtNoiPhoBien.Text = "";
                txtCongDung.Text = "";
                txtDacDiem.Text = "";
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
                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "GiongCayLuuHanh");
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