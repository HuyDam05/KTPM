using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiTrongTrot.View.CoSoSanXuat
{
    public partial class CoSoVietGapView : UserControl
    {
        private DataTable _currentData;
        private bool _isLoading = false;

        public CoSoVietGapView()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                _isLoading = true;
                txtSearch.Text = "";

                dgCoSoVietGap.ItemsSource = null;
                dgCoSoVietGap.Columns.Clear();

                dgCoSoVietGap.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 60 });
                dgCoSoVietGap.Columns.Add(new DataGridTextColumn { Header = "Tên cơ sở", Binding = new System.Windows.Data.Binding("Ten"), Width = 250 });
                dgCoSoVietGap.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 300 });
                dgCoSoVietGap.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });

                string query = "SELECT * FROM CS_VG ORDER BY Id";
                _currentData = DataProvider.ExecuteQuery(query);

                dgCoSoVietGap.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
            var addWindow = new AddCoSoVietGapWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void dgCoSoVietGap_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgCoSoVietGap.SelectedItem == null) return;

            var selectedRow = dgCoSoVietGap.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            var editWindow = new AddCoSoVietGapWindow(selectedRow);
            if (editWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgCoSoVietGap.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedRow = dgCoSoVietGap.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            int id = Convert.ToInt32(selectedRow["Id"]);
            string ten = selectedRow["Ten"]?.ToString() ?? "";

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa \"{ten}\" (ID: {id})?\n\nThao tác này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    int deleteResult = DataProvider.DELETE_DATA(id.ToString(), "Id", "CS_VG");

                    if (deleteResult > 0)
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
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
    }
}
