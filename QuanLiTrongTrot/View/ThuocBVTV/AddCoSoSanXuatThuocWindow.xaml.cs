using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiTrongTrot.View.ThuocBVTV
{
    /// <summary>
    /// Interaction logic for AddCoSoSanXuatThuocWindow.xaml
    /// </summary>
    public partial class AddCoSoSanXuatThuocWindow : Window
    {
        private int? _editId = null;
        private List<int> _selectedThuocIds = new List<int>();

        public AddCoSoSanXuatThuocWindow()
        {
            InitializeComponent();
            LoadListBoxData();
        }

        public AddCoSoSanXuatThuocWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);
                txtTen.Text = row["Ten"].ToString();
                txtDiaChi.Text = row["DiaChi"].ToString();

                LoadSelectedThuoc(_editId.Value);
                this.Title = "Sửa Cơ Sở Sản Xuất Thuốc BVTV";
            }
        }

        private void LoadListBoxData()
        {
            try
            {
                string query = "SELECT Id, Ten FROM ThuocBVTV ORDER BY Id";
                DataTable dt = DataProvider.ExecuteQuery(query);
                lstThuoc.ItemsSource = dt.DefaultView;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Chưa có thuốc BVTV nào trong danh mục!\nVui lòng thêm thuốc trước.",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSelectedThuoc(int coSoId)
        {
            try
            {
                string query = "SELECT ThuocBVTVId FROM ThuocBVTV_CoSoSanXuat WHERE CoSoSanXuatId = @CoSoId";
                DataTable dt = DataProvider.ExecuteQuery(query, new object[] { coSoId });

                _selectedThuocIds.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    _selectedThuocIds.Add(Convert.ToInt32(row["ThuocBVTVId"]));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadSelectedThuoc: " + ex.Message);
            }
        }

        private void Thuoc_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox?.Tag != null)
            {
                int id = Convert.ToInt32(checkbox.Tag);
                if (!_selectedThuocIds.Contains(id))
                    _selectedThuocIds.Add(id);
            }
        }

        private void Thuoc_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox?.Tag != null)
            {
                int id = Convert.ToInt32(checkbox.Tag);
                _selectedThuocIds.Remove(id);
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

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDiaChi.Focus();
                return;
            }

            if (_selectedThuocIds.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một loại thuốc!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_editId == null)
                {
                    int newId = DataProvider.GetNextId("CoSoSanXuatThuocBVTV");
                    int newBanDoId = newId;
                    int firstLoaiId = _selectedThuocIds.First();

                    string queryInsert = @"
                        SET IDENTITY_INSERT CoSoSanXuatThuocBVTV ON;
                        INSERT INTO CoSoSanXuatThuocBVTV (Id, Ten, DiaChi, BanDoId, LoaiId) 
                        VALUES (@Id, @Ten, @DiaChi, @BanDoId, @LoaiId);
                        SET IDENTITY_INSERT CoSoSanXuatThuocBVTV OFF;";

                    int result = DataProvider.ExecuteNonQuery(queryInsert,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@DiaChi", txtDiaChi.Text),
                        new SqlParameter("@BanDoId", newBanDoId),
                        new SqlParameter("@LoaiId", firstLoaiId));

                    if (result > 0)
                    {
                        InsertThuocLinks(newId);
                        MessageBox.Show($"Thêm mới thành công!\nID: {newId}\nSố loại thuốc: {_selectedThuocIds.Count}",
                            "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    int firstLoaiId = _selectedThuocIds.First();

                    string queryUpdate = @"UPDATE CoSoSanXuatThuocBVTV 
                                           SET Ten = @Ten, DiaChi = @DiaChi, LoaiId = @LoaiId 
                                           WHERE Id = @Id";

                    int result = DataProvider.ExecuteNonQuery(queryUpdate,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@DiaChi", txtDiaChi.Text),
                        new SqlParameter("@LoaiId", firstLoaiId));

                    if (result > 0)
                    {
                        DeleteThuocLinks(_editId.Value);
                        InsertThuocLinks(_editId.Value);
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

        private void InsertThuocLinks(int coSoId)
        {
            foreach (int thuocId in _selectedThuocIds)
            {
                try
                {
                    string query = "INSERT INTO ThuocBVTV_CoSoSanXuat (ThuocBVTVId, CoSoSanXuatId) VALUES (@ThuocId, @CoSoId)";
                    DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@ThuocId", thuocId),
                        new SqlParameter("@CoSoId", coSoId));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"InsertThuocLinks error: {ex.Message}");
                }
            }
        }

        private void DeleteThuocLinks(int coSoId)
        {
            try
            {
                string query = "DELETE FROM ThuocBVTV_CoSoSanXuat WHERE CoSoSanXuatId = @CoSoId";
                DataProvider.ExecuteNonQuery(query, new SqlParameter("@CoSoId", coSoId));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteThuocLinks error: {ex.Message}");
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (_editId == null)
            {
                txtTen.Text = "";
                txtDiaChi.Text = "";
                _selectedThuocIds.Clear();
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
                        DeleteThuocLinks(_editId.Value);
                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "CoSoSanXuatThuocBVTV");
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
