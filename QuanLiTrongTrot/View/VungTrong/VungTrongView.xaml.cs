using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiTrongTrot.View.VungTrong
{
    public partial class VungTrongView : UserControl
    {
        private string _currentTable = "VungTrong";
        private DataTable _currentData;
        private bool _isLoading = false;

        public VungTrongView()
        {
            InitializeComponent();
            LoadVungTrong();
        }

        #region Load Data Methods

        public void LoadVungTrong()
        {
            try
            {
                _isLoading = true;
                _currentTable = "VungTrong";
                txtTitle.Text = "Danh mục Vùng Trồng";
                txtSearch.Text = "";

                dgVungTrong.ItemsSource = null;

                dgVungTrong.Columns.Clear();
                dgVungTrong.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgVungTrong.Columns.Add(new DataGridTextColumn { Header = "Quy mô (m²)", Binding = new System.Windows.Data.Binding("QuyMo"), Width = 150 });
                dgVungTrong.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 300 });
                dgVungTrong.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });

                string query = "SELECT * FROM VungTrong";
                _currentData = DataProvider.Instance.ExecuteQuery(query);

                dgVungTrong.ItemsSource = _currentData.DefaultView;
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