using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiTrongTrot.View.SinhVatGayHai
{
    public partial class SinhVatGayHaiView : UserControl
    {
        private string _currentTable = "SinhVatGayHai";
        private DataTable _currentData;
        private bool _isLoading = false;

        public SinhVatGayHaiView()
        {
            InitializeComponent();
            LoadSinhVatGayHai();
        }

        #region Load Data Methods

        public void LoadSinhVatGayHai()
        {
            try
            {
                _isLoading = true;
                _currentTable = "SinhVatGayHai";
                txtTitle.Text = "Danh mục Sinh Vật Gây Hại";
                txtSearch.Text = "";
                
                dgSinhVatGayHai.ItemsSource = null;

                dgSinhVatGayHai.Columns.Clear();
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Tên sinh vật", Binding = new System.Windows.Data.Binding("Ten"), Width = 150 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Phân loại", Binding = new System.Windows.Data.Binding("PhanLoai"), Width = 120 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Cây ảnh hưởng", Binding = new System.Windows.Data.Binding("CayAH"), Width = 120 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Vùng trồng ID", Binding = new System.Windows.Data.Binding("VTId"), Width = 100 });

                string query = "SELECT * FROM SinhVatGayHai";
                _currentData = DataProvider.ExecuteQuery(query);

                dgSinhVatGayHai.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadCapNhatSVGH()
        {
            try
            {
                _isLoading = true;
                _currentTable = "CapNhat_SVGH";
                txtTitle.Text = "Cập nhật tình hình Sinh Vật Gây Hại";
                txtSearch.Text = "";
                
                dgSinhVatGayHai.ItemsSource = null;

                dgSinhVatGayHai.Columns.Clear();
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Ngày giờ cập nhật", Binding = new System.Windows.Data.Binding("NgayGioCN") { StringFormat = "dd/MM/yyyy HH:mm" }, Width = 150 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Tiến độ", Binding = new System.Windows.Data.Binding("TienDo"), Width = 250 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Sinh vật ID", Binding = new System.Windows.Data.Binding("SVId"), Width = 100 });

                string query = "SELECT * FROM CapNhat_SVGH ORDER BY NgayGioCN DESC";
                _currentData = DataProvider.ExecuteQuery(query);

                dgSinhVatGayHai.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadTuoiSau()
        {
            try
            {
                _isLoading = true;
                _currentTable = "TuoiSau";
                txtTitle.Text = "Danh mục Tuổi Sâu - Cấp độ phổ biến";
                txtSearch.Text = "";
                
                dgSinhVatGayHai.ItemsSource = null;

                dgSinhVatGayHai.Columns.Clear();
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Tên", Binding = new System.Windows.Data.Binding("Ten"), Width = 150 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Tuổi sâu", Binding = new System.Windows.Data.Binding("TuoiSau"), Width = 80 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Độ phổ biến", Binding = new System.Windows.Data.Binding("DoPhoBien"), Width = 120 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Sinh vật ID", Binding = new System.Windows.Data.Binding("SVId"), Width = 100 });

                string query = "SELECT * FROM TuoiSau";
                _currentData = DataProvider.ExecuteQuery(query);

                dgSinhVatGayHai.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Helper Methods

        private void ReloadCurrentData()
        {
            switch (_currentTable)
            {
                case "SinhVatGayHai":
                    LoadSinhVatGayHai();
                    break;
                case "CapNhat_SVGH":
                    LoadCapNhatSVGH();
                    break;
                case "TuoiSau":
                    LoadTuoiSau();
                    break;
            }
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

        private void BtnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentTable)
            {
                case "SinhVatGayHai":
                    var addSVGHWindow = new AddSinhVatGayHaiWindow();
                    if (addSVGHWindow.ShowDialog() == true)
                    {
                        LoadSinhVatGayHai();
                    }
                    break;

                case "CapNhat_SVGH":
                    var addCapNhatWindow = new AddCapNhatSVGHWindow();
                    if (addCapNhatWindow.ShowDialog() == true)
                    {
                        LoadCapNhatSVGH();
                    }
                    break;

                case "TuoiSau":
                    var addTuoiSauWindow = new AddTuoiSauWindow();
                    if (addTuoiSauWindow.ShowDialog() == true)
                    {
                        LoadTuoiSau();
                    }
                    break;
            }
        }

        private void dgSinhVatGayHai_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgSinhVatGayHai.SelectedItem == null) return;

            var selectedRow = dgSinhVatGayHai.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            switch (_currentTable)
            {
                case "SinhVatGayHai":
                    var editSVGHWindow = new AddSinhVatGayHaiWindow(selectedRow);
                    if (editSVGHWindow.ShowDialog() == true)
                    {
                        LoadSinhVatGayHai();
                    }
                    break;

                case "CapNhat_SVGH":
                    var editCapNhatWindow = new AddCapNhatSVGHWindow(selectedRow);
                    if (editCapNhatWindow.ShowDialog() == true)
                    {
                        LoadCapNhatSVGH();
                    }
                    break;

                case "TuoiSau":
                    var editTuoiSauWindow = new AddTuoiSauWindow(selectedRow);
                    if (editTuoiSauWindow.ShowDialog() == true)
                    {
                        LoadTuoiSau();
                    }
                    break;
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgSinhVatGayHai.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedRow = dgSinhVatGayHai.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            int id = Convert.ToInt32(selectedRow["Id"]);
            string tenHienThi = "";

            switch (_currentTable)
            {
                case "SinhVatGayHai":
                    tenHienThi = selectedRow["Ten"].ToString();
                    break;
                case "CapNhat_SVGH":
                    tenHienThi = Convert.ToDateTime(selectedRow["NgayGioCN"]).ToString("dd/MM/yyyy HH:mm");
                    break;
                case "TuoiSau":
                    tenHienThi = selectedRow["Ten"].ToString();
                    break;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa \"{tenHienThi}\" (ID: {id})?\n\nThao tác này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    int deleteResult = DataProvider.DELETE_DATA(id.ToString(), "Id", _currentTable);

                    if (deleteResult > 0)
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                        ReloadCurrentData();
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