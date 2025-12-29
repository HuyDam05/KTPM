using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiTrongTrot.View.ThuocBVTV
{
    public partial class ThuocBVTVView : UserControl
    {
        private string _currentTable = "ThuocBVTV";
        private DataTable _currentData;
        private bool _isLoading = false;

        public ThuocBVTVView()
        {
            InitializeComponent();
            LoadThuocBVTV();
        }

        #region Load Data Methods

        public void LoadThuocBVTV()
        {
            try
            {
                _isLoading = true;
                _currentTable = "ThuocBVTV";
                txtTitle.Text = "Danh sách Thuốc Bảo Vệ Thực Vật";
                txtSearch.Text = "";
                
                dgThuocBVTV.ItemsSource = null;

                dgThuocBVTV.Columns.Clear();
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Tên thuốc", Binding = new System.Windows.Data.Binding("Ten"), Width = 200 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Ngày sản xuất", Binding = new System.Windows.Data.Binding("NgaySX") { StringFormat = "dd/MM/yyyy" }, Width = 120 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Hạn sử dụng", Binding = new System.Windows.Data.Binding("HanSD") { StringFormat = "dd/MM/yyyy" }, Width = 120 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Vùng trồng ID", Binding = new System.Windows.Data.Binding("VTId"), Width = 100 });

                string query = "SELECT * FROM ThuocBVTV";
                _currentData = DataProvider.Instance.ExecuteQuery(query);

                dgThuocBVTV.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadCoSoSanXuat()
        {
            try
            {
                _isLoading = true;
                _currentTable = "CoSoSanXuatThuocBVTV";
                txtTitle.Text = "Cơ sở sản xuất thuốc BVTV";
                txtSearch.Text = "";
                
                dgThuocBVTV.ItemsSource = null;

                dgThuocBVTV.Columns.Clear();
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Tên cơ sở", Binding = new System.Windows.Data.Binding("Ten"), Width = 200 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 250 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Loại ID", Binding = new System.Windows.Data.Binding("LoaiId"), Width = 80 });

                string query = "SELECT * FROM CoSoSanXuatThuocBVTV";
                _currentData = DataProvider.Instance.ExecuteQuery(query);

                dgThuocBVTV.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadCoSoBan()
        {
            try
            {
                _isLoading = true;
                _currentTable = "CoSoBanThuocBVTV";
                txtTitle.Text = "Cơ sở bán thuốc BVTV";
                txtSearch.Text = "";
                
                dgThuocBVTV.ItemsSource = null;

                dgThuocBVTV.Columns.Clear();
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Tên cơ sở", Binding = new System.Windows.Data.Binding("Ten"), Width = 200 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 250 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Loại ID", Binding = new System.Windows.Data.Binding("LoaiId"), Width = 80 });

                string query = "SELECT * FROM CoSoBanThuocBVTV";
                _currentData = DataProvider.Instance.ExecuteQuery(query);

                dgThuocBVTV.ItemsSource = _currentData.DefaultView;
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
            MessageBox.Show($"Thêm mới vào bảng: {_currentTable}", "Thông báo");
        }

        #endregion
    }
}