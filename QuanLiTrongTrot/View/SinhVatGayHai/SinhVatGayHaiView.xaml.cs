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
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Vùng trồng", Binding = new System.Windows.Data.Binding("Vung"), Width = 150 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Tọa độ X", Binding = new System.Windows.Data.Binding("ToaDoX"), Width = 80 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Tọa độ Y", Binding = new System.Windows.Data.Binding("ToaDoY"), Width = 80 });

                // Sử dụng View thay vì bảng trực tiếp
                string query = "SELECT * FROM ViewSinhVatGayHai";
                _currentData = DataProvider.Instance.ExecuteQuery(query);

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
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Ngày giờ cập nhật", Binding = new System.Windows.Data.Binding("NgayGioCN") { StringFormat = "dd/MM/yyyy HH:mm" }, Width = 180 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Tiến độ", Binding = new System.Windows.Data.Binding("TienDo"), Width = 200 });
                dgSinhVatGayHai.Columns.Add(new DataGridTextColumn { Header = "Sinh vật ID", Binding = new System.Windows.Data.Binding("SVId"), Width = 100 });

                string query = "SELECT * FROM CapNhat_SVGH";
                _currentData = DataProvider.Instance.ExecuteQuery(query);

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
                _currentData = DataProvider.Instance.ExecuteQuery(query);

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

        #region Event Handlers

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isLoading || _currentData == null) return;

            string keyword = txtSearch.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                // Hiển thị tất cả
                _currentData.DefaultView.RowFilter = "";
            }
            else
            {
                // Tạo filter cho tất cả các cột
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
            // Tạo filter dựa trên các cột của DataTable
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