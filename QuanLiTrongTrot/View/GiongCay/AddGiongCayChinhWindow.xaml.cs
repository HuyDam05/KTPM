using QuanLiTrongTrot.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiTrongTrot.View.GiongCay
{
    public partial class AddGiongCayChinhWindow : Window
    {
        private int? _editId = null;

        public AddGiongCayChinhWindow()
        {
            InitializeComponent();
        }

        public AddGiongCayChinhWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);
                txtTen.Text = row["Ten"].ToString();
                txtPhanLoai.Text = row["PhanLoai"].ToString();
                txtSanLuong.Text = row["SanLuong"].ToString();

                // Set mùa vụ trong ComboBox
                string muaVu = row["MuaVu"].ToString();
                foreach (ComboBoxItem item in cboMuaVu.Items)
                {
                    if (item.Content.ToString() == muaVu)
                    {
                        cboMuaVu.SelectedItem = item;
                        break;
                    }
                }

                this.Title = "Sửa Giống Cây Trồng Chính";
            }
        }

        /// <summary>
        /// Lấy ID nhỏ nhất còn trống hoặc ID tiếp theo
        /// </summary>
        private int GetNextId()
        {
            string gapQuery = @"
                SELECT TOP 1 t1.Id + 1 AS NextId
                FROM GiongCayChinh t1
                LEFT JOIN GiongCayChinh t2 ON t1.Id + 1 = t2.Id
                WHERE t2.Id IS NULL
                ORDER BY t1.Id";

            int gapId = DataProvider.ExecuteScalar(gapQuery);
            if (gapId > 0)
                return gapId;

            int maxId = DataProvider.ExecuteScalar("SELECT ISNULL(MAX(Id), 0) FROM GiongCayChinh");
            if (maxId == 0)
                return 1;

            int hasId1 = DataProvider.ExecuteScalar("SELECT COUNT(*) FROM GiongCayChinh WHERE Id = 1");
            if (hasId1 == 0)
                return 1;

            return maxId + 1;
        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên giống cây!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTen.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhanLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập phân loại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhanLoai.Focus();
                return;
            }

            if (cboMuaVu.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn mùa vụ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtSanLuong.Text, out int sanLuong) || sanLuong <= 0)
            {
                MessageBox.Show("Sản lượng phải là số nguyên dương!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSanLuong.Focus();
                return;
            }

            try
            {
                string muaVu = ((ComboBoxItem)cboMuaVu.SelectedItem).Content.ToString();

                if (_editId == null)
                {
                    // Thêm mới với ID tự động
                    int newId = GetNextId();

                    string query = @"
                        SET IDENTITY_INSERT GiongCayChinh ON;
                        INSERT INTO GiongCayChinh (Id, Ten, PhanLoai, MuaVu, SanLuong) 
                        VALUES (@Id, @Ten, @PhanLoai, @MuaVu, @SanLuong);
                        SET IDENTITY_INSERT GiongCayChinh OFF;";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@PhanLoai", txtPhanLoai.Text),
                        new SqlParameter("@MuaVu", muaVu),
                        new SqlParameter("@SanLuong", sanLuong));

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
                    string query = @"UPDATE GiongCayChinh 
                                     SET Ten = @Ten, PhanLoai = @PhanLoai, MuaVu = @MuaVu, SanLuong = @SanLuong 
                                     WHERE Id = @Id";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@PhanLoai", txtPhanLoai.Text),
                        new SqlParameter("@MuaVu", muaVu),
                        new SqlParameter("@SanLuong", sanLuong));

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
                txtPhanLoai.Text = "";
                cboMuaVu.SelectedIndex = -1;
                txtSanLuong.Text = "";
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
                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "GiongCayChinh");
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