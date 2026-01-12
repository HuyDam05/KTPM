using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiTrongTrot.View.GiongCay
{
    public partial class GiongCayView : UserControl
    {
        private string _currentTable = "GiongCayChinh";
        private DataTable _currentData;
        private bool _isLoading = false;

        public GiongCayView()
        {
            InitializeComponent();
            LoadGiongCayChinh();
        }

        #region Load Data Methods

        public void LoadGiongCayChinh()
        {
            try
            {
                _isLoading = true;
                _currentTable = "GiongCayChinh";
                txtTitle.Text = "Danh sách Giống Cây Trồng Chính";
                txtSearch.Text = "";
                
                dgGiongCay.ItemsSource = null;

                dgGiongCay.Columns.Clear();
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Tên", Binding = new System.Windows.Data.Binding("Ten"), Width = 150 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Phân loại", Binding = new System.Windows.Data.Binding("PhanLoai"), Width = 150 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Mùa vụ", Binding = new System.Windows.Data.Binding("MuaVu"), Width = 120 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Sản lượng", Binding = new System.Windows.Data.Binding("SanLuong"), Width = 100 });

                string query = "SELECT * FROM GiongCayChinh";
                _currentData = DataProvider.ExecuteQuery(query);

                dgGiongCay.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadGiongCayLuuHanh()
        {
            try
            {
                _isLoading = true;
                _currentTable = "GiongCayLuuHanh";
                txtTitle.Text = "Danh sách Giống Cây Lưu Hành";
                txtSearch.Text = "";
                
                dgGiongCay.ItemsSource = null;

                dgGiongCay.Columns.Clear();
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Loại cây", Binding = new System.Windows.Data.Binding("Ten"), Width = 150 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Nơi phổ biến", Binding = new System.Windows.Data.Binding("NoiPhoBien"), Width = 150 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Công dụng", Binding = new System.Windows.Data.Binding("CongDung"), Width = 200 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Đặc điểm", Binding = new System.Windows.Data.Binding("DacDiem"), Width = 200 });

                string query = "SELECT * FROM GiongCayLuuHanh";
                _currentData = DataProvider.ExecuteQuery(query);

                dgGiongCay.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadGiongCayDauDong()
        {
            try
            {
                _isLoading = true;
                _currentTable = "GiongCayDauDong";
                txtTitle.Text = "Danh sách Giống Cây Đầu Dòng";
                txtSearch.Text = "";
                
                dgGiongCay.ItemsSource = null;

                dgGiongCay.Columns.Clear();
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Tên", Binding = new System.Windows.Data.Binding("Ten"), Width = 150 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Nguồn gốc", Binding = new System.Windows.Data.Binding("NguonGoc"), Width = 120 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Đặc tính", Binding = new System.Windows.Data.Binding("DacTinh"), Width = 200 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Thời gian thu hoạch", Binding = new System.Windows.Data.Binding("ThoiGianThuHoach"), Width = 140 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Giống ID", Binding = new System.Windows.Data.Binding("GiongId"), Width = 80 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Vùng trồng ID", Binding = new System.Windows.Data.Binding("VTId"), Width = 100 });

                string query = "SELECT * FROM GiongCayDauDong";
                _currentData = DataProvider.ExecuteQuery(query);

                dgGiongCay.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReloadCurrentData()
        {
            switch (_currentTable)
            {
                case "GiongCayChinh":
                    LoadGiongCayChinh();
                    break;
                case "GiongCayLuuHanh":
                    LoadGiongCayLuuHanh();
                    break;
                case "GiongCayDauDong":
                    LoadGiongCayDauDong();
                    break;
            }
        }

        #endregion

        #region Event Handlers

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isLoading || _currentData == null) return;

            string keyword = txtSearch.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                _currentData.DefaultView.RowFilter = "";
            }
            else
            {
                string filter = BuildRowFilter(keyword);
                try
                {
                    _currentData.DefaultView.RowFilter = filter;
                }
                catch
                {
                    _currentData.DefaultView.RowFilter = "";
                }
            }

            txtTongSo.Text = _currentData.DefaultView.Count.ToString();
        }

        private string BuildRowFilter(string keyword)
        {
            List<string> conditions = new List<string>();
            
            foreach (DataColumn col in _currentData.Columns)
            {
                if (col.DataType == typeof(string))
                {
                    conditions.Add($"CONVERT([{col.ColumnName}], 'System.String') LIKE '%{keyword}%'");
                }
                else if (col.DataType == typeof(int) || col.DataType == typeof(double))
                {
                    conditions.Add($"CONVERT([{col.ColumnName}], 'System.String') LIKE '%{keyword}%'");
                }
            }

            return string.Join(" OR ", conditions);
        }

        private void BtnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentTable)
            {
                case "GiongCayDauDong":
                    var addDauDongWindow = new AddGiongCayWindow();
                    if (addDauDongWindow.ShowDialog() == true)
                    {
                        LoadGiongCayDauDong();
                    }
                    break;
                    
                case "GiongCayChinh":
                    var addChinhWindow = new AddGiongCayChinhWindow();
                    if (addChinhWindow.ShowDialog() == true)
                    {
                        LoadGiongCayChinh();
                    }
                    break;
                    
                case "GiongCayLuuHanh":
                    var addLuuHanhWindow = new AddGiongCayLuuHanhWindow();
                    if (addLuuHanhWindow.ShowDialog() == true)
                    {
                        LoadGiongCayLuuHanh();
                    }
                    break;
            }
        }

        private void dgGiongCay_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgGiongCay.SelectedItem == null) return;

            var selectedRow = dgGiongCay.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            switch (_currentTable)
            {
                case "GiongCayDauDong":
                    var editDauDongWindow = new AddGiongCayWindow(selectedRow);
                    if (editDauDongWindow.ShowDialog() == true)
                    {
                        LoadGiongCayDauDong();
                    }
                    break;
                    
                case "GiongCayChinh":
                    var editChinhWindow = new AddGiongCayChinhWindow(selectedRow);
                    if (editChinhWindow.ShowDialog() == true)
                    {
                        LoadGiongCayChinh();
                    }
                    break;
                    
                case "GiongCayLuuHanh":
                    var editLuuHanhWindow = new AddGiongCayLuuHanhWindow(selectedRow);
                    if (editLuuHanhWindow.ShowDialog() == true)
                    {
                        LoadGiongCayLuuHanh();
                    }
                    break;
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn không
            if (dgGiongCay.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedRow = dgGiongCay.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            int id = Convert.ToInt32(selectedRow["Id"]);
            string tenHienThi = "";

            // Lấy tên hiển thị tùy theo bảng
            switch (_currentTable)
            {
                case "GiongCayChinh":
                    tenHienThi = selectedRow["Ten"].ToString();
                    break;
                case "GiongCayDauDong":
                    tenHienThi = selectedRow["Ten"].ToString();
                    break;
                case "GiongCayLuuHanh":
                    tenHienThi = selectedRow["Ten"].ToString();
                    break;
            }

            // Xác nhận xóa
            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa \"{tenHienThi}\" (ID: {id})?\n\nThao tác này không thể hoàn tác!", 
                "Xác nhận xóa", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    int deleteResult = DataProvider.DELETE_DATA(tenHienThi,"Ten",_currentTable);

                    if (deleteResult > 0)
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo", 
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                        ReloadCurrentData(); // Reload lại dữ liệu
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa dữ liệu!", "Lỗi", 
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", 
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion
    }
}
