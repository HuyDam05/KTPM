using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiTrongTrot.View.PhanBon
{
    public partial class AddCoSoSanXuatPhanBonWindow : Window
    {
        private int? _editId = null;
        private List<int> _selectedPhanBonIds = new List<int>();

        public AddCoSoSanXuatPhanBonWindow()
        {
            InitializeComponent();
            LoadListBoxData();
        }

        public AddCoSoSanXuatPhanBonWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);
                txtTen.Text = row["Ten"].ToString();
                txtDiaChi.Text = row["DiaChi"].ToString();

                // Load các phân bón đã liên kết
                LoadSelectedPhanBon(_editId.Value);

                this.Title = "Sửa Cơ Sở Sản Xuất Phân Bón";
            }
        }

        private void LoadListBoxData()
        {
            try
            {
                string query = "SELECT Id, Ten FROM PhanBon ORDER BY Id";
                DataTable dt = DataProvider.ExecuteQuery(query);
                lstPhanBon.ItemsSource = dt.DefaultView;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Chưa có phân bón nào trong danh mục!\nVui lòng thêm phân bón trước.",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSelectedPhanBon(int coSoId)
        {
            try
            {
                // Lấy danh sách phân bón đã liên kết với cơ sở này
                string query = "SELECT PhanBonId FROM PhanBon_CoSoSanXuat WHERE CoSoSanXuatId = @CoSoId";
                DataTable dt = DataProvider.ExecuteQuery(query, new object[] { coSoId });

                _selectedPhanBonIds.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    _selectedPhanBonIds.Add(Convert.ToInt32(row["PhanBonId"]));
                }

                // Check các checkbox tương ứng
                lstPhanBon.UpdateLayout();
                foreach (var item in lstPhanBon.Items)
                {
                    var row = item as DataRowView;
                    if (row != null)
                    {
                        int id = Convert.ToInt32(row["Id"]);
                        if (_selectedPhanBonIds.Contains(id))
                        {
                            // Tìm và check checkbox
                            var container = lstPhanBon.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                            if (container != null)
                            {
                                var checkbox = FindVisualChild<CheckBox>(container);
                                if (checkbox != null)
                                    checkbox.IsChecked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Bảng trung gian có thể chưa có data
                System.Diagnostics.Debug.WriteLine("LoadSelectedPhanBon: " + ex.Message);
            }
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                    return result;
                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }

        private void PhanBon_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox?.Tag != null)
            {
                int id = Convert.ToInt32(checkbox.Tag);
                if (!_selectedPhanBonIds.Contains(id))
                    _selectedPhanBonIds.Add(id);
            }
        }

        private void PhanBon_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox?.Tag != null)
            {
                int id = Convert.ToInt32(checkbox.Tag);
                _selectedPhanBonIds.Remove(id);
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

            if (_selectedPhanBonIds.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một loại phân bón!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_editId == null)
                {
                    // Thêm mới
                    int newId = DataProvider.GetNextId("CoSoSanXuatPhanBon");
                    int newBanDoId = newId;
                    int firstLoaiId = _selectedPhanBonIds.First(); // Lấy loại đầu tiên cho cột LoaiId

                    string queryInsert = @"
                        SET IDENTITY_INSERT CoSoSanXuatPhanBon ON;
                        INSERT INTO CoSoSanXuatPhanBon (Id, Ten, DiaChi, BanDoId, LoaiId) 
                        VALUES (@Id, @Ten, @DiaChi, @BanDoId, @LoaiId);
                        SET IDENTITY_INSERT CoSoSanXuatPhanBon OFF;";

                    int result = DataProvider.ExecuteNonQuery(queryInsert,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@DiaChi", txtDiaChi.Text),
                        new SqlParameter("@BanDoId", newBanDoId),
                        new SqlParameter("@LoaiId", firstLoaiId));

                    if (result > 0)
                    {
                        // Thêm vào bảng trung gian
                        InsertPhanBonLinks(newId);

                        MessageBox.Show($"Thêm mới thành công!\nID: {newId}\nSố loại phân bón: {_selectedPhanBonIds.Count}",
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
                    // Cập nhật
                    int firstLoaiId = _selectedPhanBonIds.First();

                    string queryUpdate = @"UPDATE CoSoSanXuatPhanBon 
                                           SET Ten = @Ten, DiaChi = @DiaChi, LoaiId = @LoaiId 
                                           WHERE Id = @Id";

                    int result = DataProvider.ExecuteNonQuery(queryUpdate,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@Ten", txtTen.Text),
                        new SqlParameter("@DiaChi", txtDiaChi.Text),
                        new SqlParameter("@LoaiId", firstLoaiId));

                    if (result > 0)
                    {
                        // Xóa liên kết cũ và thêm mới
                        DeletePhanBonLinks(_editId.Value);
                        InsertPhanBonLinks(_editId.Value);

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

        private void InsertPhanBonLinks(int coSoId)
        {
            foreach (int phanBonId in _selectedPhanBonIds)
            {
                try
                {
                    string query = "INSERT INTO PhanBon_CoSoSanXuat (PhanBonId, CoSoSanXuatId) VALUES (@PhanBonId, @CoSoId)";
                    DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@PhanBonId", phanBonId),
                        new SqlParameter("@CoSoId", coSoId));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"InsertPhanBonLinks error: {ex.Message}");
                }
            }
        }

        private void DeletePhanBonLinks(int coSoId)
        {
            try
            {
                string query = "DELETE FROM PhanBon_CoSoSanXuat WHERE CoSoSanXuatId = @CoSoId";
                DataProvider.ExecuteNonQuery(query, new SqlParameter("@CoSoId", coSoId));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeletePhanBonLinks error: {ex.Message}");
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (_editId == null)
            {
                txtTen.Text = "";
                txtDiaChi.Text = "";
                _selectedPhanBonIds.Clear();
                // Uncheck all
                foreach (var item in lstPhanBon.Items)
                {
                    var container = lstPhanBon.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                    if (container != null)
                    {
                        var checkbox = FindVisualChild<CheckBox>(container);
                        if (checkbox != null)
                            checkbox.IsChecked = false;
                    }
                }
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
                        // Xóa liên kết trước
                        DeletePhanBonLinks(_editId.Value);

                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "CoSoSanXuatPhanBon");
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
