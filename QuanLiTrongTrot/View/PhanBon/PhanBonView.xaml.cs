using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiTrongTrot.View.PhanBon
{
    public partial class PhanBonView : UserControl
    {
        private string _currentTable = "PhanBon";
        private DataTable _currentData;
        private bool _isLoading = false;

        public PhanBonView()
        {
            InitializeComponent();
            LoadPhanBon();
        }

        #region Load Data Methods

        public void LoadPhanBon()
        {
            try
            {
                _isLoading = true;
                _currentTable = "PhanBon";
                txtTitle.Text = "Danh sách Phân Bón";
                txtSearch.Text = "";
                
                dgPhanBon.ItemsSource = null;

                dgPhanBon.Columns.Clear();
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Tên phân bón", Binding = new System.Windows.Data.Binding("Ten"), Width = 180 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Thành phần", Binding = new System.Windows.Data.Binding("ThanhPhan"), Width = 150 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Phân loại", Binding = new System.Windows.Data.Binding("PhanLoai"), Width = 120 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Vùng trồng ID", Binding = new System.Windows.Data.Binding("VTId"), Width = 100 });

                string query = "SELECT * FROM PhanBon";
                _currentData = DataProvider.Instance.ExecuteQuery(query);

                dgPhanBon.ItemsSource = _currentData.DefaultView;
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
                _currentTable = "CoSoSanXuatPhanBon";
                txtTitle.Text = "Cơ sở sản xuất phân bón";
                txtSearch.Text = "";
                
                dgPhanBon.ItemsSource = null;

                dgPhanBon.Columns.Clear();
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Tên cơ sở", Binding = new System.Windows.Data.Binding("Ten"), Width = 200 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 250 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Loại ID", Binding = new System.Windows.Data.Binding("LoaiId"), Width = 80 });

                string query = "SELECT * FROM CoSoSanXuatPhanBon";
                _currentData = DataProvider.Instance.ExecuteQuery(query);

                dgPhanBon.ItemsSource = _currentData.DefaultView;
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
                _currentTable = "CoSoBanPhanBon";
                txtTitle.Text = "Cơ sở bán phân bón";
                txtSearch.Text = "";
                
                dgPhanBon.ItemsSource = null;

                dgPhanBon.Columns.Clear();
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Tên cơ sở", Binding = new System.Windows.Data.Binding("Ten"), Width = 200 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 250 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Loại ID", Binding = new System.Windows.Data.Binding("LoaiId"), Width = 80 });

                string query = "SELECT * FROM CoSoBanPhanBon";
                _currentData = DataProvider.Instance.ExecuteQuery(query);

                dgPhanBon.ItemsSource = _currentData.DefaultView;
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