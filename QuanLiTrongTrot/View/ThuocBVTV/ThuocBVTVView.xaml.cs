using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                _currentData = DataProvider.ExecuteQuery(query);

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
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Loại thuốc ID", Binding = new System.Windows.Data.Binding("DanhSachLoaiId"), Width = 150 });

                // Query lấy danh sách tất cả ID thuốc đã liên kết
                string query = @"
                    SELECT 
                        cs.Id, 
                        cs.Ten, 
                        cs.DiaChi, 
                        cs.BanDoId,
                        ISNULL(
                            (SELECT STRING_AGG(CAST(tg.ThuocBVTVId AS VARCHAR), ', ') 
                             FROM ThuocBVTV_CoSoSanXuat tg 
                             WHERE tg.CoSoSanXuatId = cs.Id), 
                            CAST(cs.LoaiId AS VARCHAR)
                        ) AS DanhSachLoaiId
                    FROM CoSoSanXuatThuocBVTV cs";

                _currentData = DataProvider.ExecuteQuery(query);

                dgThuocBVTV.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                // Fallback nếu STRING_AGG không được hỗ trợ (SQL Server < 2017)
                LoadCoSoSanXuatFallback();
            }
        }

        private void LoadCoSoSanXuatFallback()
        {
            try
            {
                _isLoading = true;
                
                // Query cơ bản
                string query = "SELECT * FROM CoSoSanXuatThuocBVTV";
                _currentData = DataProvider.ExecuteQuery(query);

                // Thêm cột DanhSachLoaiId
                _currentData.Columns.Add("DanhSachLoaiId", typeof(string));

                foreach (DataRow row in _currentData.Rows)
                {
                    int coSoId = Convert.ToInt32(row["Id"]);
                    string listIds = GetThuocIdsByCoSoSanXuat(coSoId);
                    row["DanhSachLoaiId"] = string.IsNullOrEmpty(listIds) ? row["LoaiId"].ToString() : listIds;
                }

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

        private string GetThuocIdsByCoSoSanXuat(int coSoId)
        {
            try
            {
                string query = "SELECT ThuocBVTVId FROM ThuocBVTV_CoSoSanXuat WHERE CoSoSanXuatId = @CoSoId";
                DataTable dt = DataProvider.ExecuteQuery(query, new object[] { coSoId });

                List<string> ids = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    ids.Add(row["ThuocBVTVId"].ToString());
                }
                return string.Join(", ", ids);
            }
            catch
            {
                return "";
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
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Loại thuốc ID", Binding = new System.Windows.Data.Binding("DanhSachLoaiId"), Width = 150 });

                // Query lấy danh sách tất cả ID thuốc đã liên kết
                string query = @"
                    SELECT 
                        cs.Id, 
                        cs.Ten, 
                        cs.DiaChi, 
                        cs.BanDoId,
                        ISNULL(
                            (SELECT STRING_AGG(CAST(tg.ThuocBVTVId AS VARCHAR), ', ') 
                             FROM ThuocBVTV_CoSoBan tg 
                             WHERE tg.CoSoBanId = cs.Id), 
                            CAST(cs.LoaiId AS VARCHAR)
                        ) AS DanhSachLoaiId
                    FROM CoSoBanThuocBVTV cs";

                _currentData = DataProvider.ExecuteQuery(query);

                dgThuocBVTV.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                // Fallback nếu STRING_AGG không được hỗ trợ
                LoadCoSoBanFallback();
            }
        }

        private void LoadCoSoBanFallback()
        {
            try
            {
                _isLoading = true;
                
                string query = "SELECT * FROM CoSoBanThuocBVTV";
                _currentData = DataProvider.ExecuteQuery(query);

                _currentData.Columns.Add("DanhSachLoaiId", typeof(string));

                foreach (DataRow row in _currentData.Rows)
                {
                    int coSoId = Convert.ToInt32(row["Id"]);
                    string listIds = GetThuocIdsByCoSoBan(coSoId);
                    row["DanhSachLoaiId"] = string.IsNullOrEmpty(listIds) ? row["LoaiId"].ToString() : listIds;
                }

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

        private string GetThuocIdsByCoSoBan(int coSoId)
        {
            try
            {
                string query = "SELECT ThuocBVTVId FROM ThuocBVTV_CoSoBan WHERE CoSoBanId = @CoSoId";
                DataTable dt = DataProvider.ExecuteQuery(query, new object[] { coSoId });

                List<string> ids = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    ids.Add(row["ThuocBVTVId"].ToString());
                }
                return string.Join(", ", ids);
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region Helper Methods

        private void ReloadCurrentData()
        {
            switch (_currentTable)
            {
                case "ThuocBVTV":
                    LoadThuocBVTV();
                    break;
                case "CoSoSanXuatThuocBVTV":
                    LoadCoSoSanXuat();
                    break;
                case "CoSoBanThuocBVTV":
                    LoadCoSoBan();
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

        private void DeleteRelatedLinks(int id)
        {
            try
            {
                switch (_currentTable)
                {
                    case "CoSoSanXuatThuocBVTV":
                        DataProvider.ExecuteNonQuery(
                            "DELETE FROM ThuocBVTV_CoSoSanXuat WHERE CoSoSanXuatId = @Id",
                            new SqlParameter("@Id", id));
                        break;

                    case "CoSoBanThuocBVTV":
                        DataProvider.ExecuteNonQuery(
                            "DELETE FROM ThuocBVTV_CoSoBan WHERE CoSoBanId = @Id",
                            new SqlParameter("@Id", id));
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteRelatedLinks error: {ex.Message}");
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

        private void BtnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentTable)
            {
                case "ThuocBVTV":
                    var addThuocWindow = new AddThuocBVTVWindow();
                    if (addThuocWindow.ShowDialog() == true)
                    {
                        LoadThuocBVTV();
                    }
                    break;

                case "CoSoSanXuatThuocBVTV":
                    var addCoSoSXWindow = new AddCoSoSanXuatThuocWindow();
                    if (addCoSoSXWindow.ShowDialog() == true)
                    {
                        LoadCoSoSanXuat();
                    }
                    break;

                case "CoSoBanThuocBVTV":
                    var addCoSoBanWindow = new AddCoSoBanThuocWindow();
                    if (addCoSoBanWindow.ShowDialog() == true)
                    {
                        LoadCoSoBan();
                    }
                    break;
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgThuocBVTV.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedRow = dgThuocBVTV.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            int id = Convert.ToInt32(selectedRow["Id"]);
            string tenHienThi = selectedRow["Ten"].ToString();

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa \"{tenHienThi}\" (ID: {id})?\n\nThao tác này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DeleteRelatedLinks(id);
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

        private void dgThuocBVTV_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgThuocBVTV.SelectedItem == null) return;

            var selectedRow = dgThuocBVTV.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            switch (_currentTable)
            {
                case "ThuocBVTV":
                    var editThuocWindow = new AddThuocBVTVWindow(selectedRow);
                    if (editThuocWindow.ShowDialog() == true)
                    {
                        LoadThuocBVTV();
                    }
                    break;

                case "CoSoSanXuatThuocBVTV":
                    var editCoSoSXWindow = new AddCoSoSanXuatThuocWindow(selectedRow);
                    if (editCoSoSXWindow.ShowDialog() == true)
                    {
                        LoadCoSoSanXuat();
                    }
                    break;

                case "CoSoBanThuocBVTV":
                    var editCoSoBanWindow = new AddCoSoBanThuocWindow(selectedRow);
                    if (editCoSoBanWindow.ShowDialog() == true)
                    {
                        LoadCoSoBan();
                    }
                    break;
            }
        }

        #endregion
    }
}