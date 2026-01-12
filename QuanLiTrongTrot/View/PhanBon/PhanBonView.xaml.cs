using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                _currentData = DataProvider.ExecuteQuery(query);

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
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Loại phân bón ID", Binding = new System.Windows.Data.Binding("DanhSachLoaiId"), Width = 150 });

                // Query lấy danh sách tất cả ID phân bón đã liên kết
                string query = @"
                    SELECT 
                        cs.Id, 
                        cs.Ten, 
                        cs.DiaChi, 
                        cs.BanDoId,
                        ISNULL(
                            (SELECT STRING_AGG(CAST(tg.PhanBonId AS VARCHAR), ', ') 
                             FROM PhanBon_CoSoSanXuat tg 
                             WHERE tg.CoSoSanXuatId = cs.Id), 
                            CAST(cs.LoaiId AS VARCHAR)
                        ) AS DanhSachLoaiId
                    FROM CoSoSanXuatPhanBon cs";

                _currentData = DataProvider.ExecuteQuery(query);

                dgPhanBon.ItemsSource = _currentData.DefaultView;
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

                string query = "SELECT * FROM CoSoSanXuatPhanBon";
                _currentData = DataProvider.ExecuteQuery(query);

                // Thêm cột DanhSachLoaiId
                _currentData.Columns.Add("DanhSachLoaiId", typeof(string));

                foreach (DataRow row in _currentData.Rows)
                {
                    int coSoId = Convert.ToInt32(row["Id"]);
                    string listIds = GetPhanBonIdsByCoSoSanXuat(coSoId);
                    row["DanhSachLoaiId"] = string.IsNullOrEmpty(listIds) ? row["LoaiId"].ToString() : listIds;
                }

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

        private string GetPhanBonIdsByCoSoSanXuat(int coSoId)
        {
            try
            {
                string query = "SELECT PhanBonId FROM PhanBon_CoSoSanXuat WHERE CoSoSanXuatId = @CoSoId";
                DataTable dt = DataProvider.ExecuteQuery(query, new object[] { coSoId });

                List<string> ids = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    ids.Add(row["PhanBonId"].ToString());
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
                _currentTable = "CoSoBanPhanBon";
                txtTitle.Text = "Cơ sở bán phân bón";
                txtSearch.Text = "";

                dgPhanBon.ItemsSource = null;

                dgPhanBon.Columns.Clear();
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Tên cơ sở", Binding = new System.Windows.Data.Binding("Ten"), Width = 200 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 250 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Loại phân bón ID", Binding = new System.Windows.Data.Binding("DanhSachLoaiId"), Width = 150 });

                // Query lấy danh sách tất cả ID phân bón đã liên kết
                string query = @"
                    SELECT 
                        cs.Id, 
                        cs.Ten, 
                        cs.DiaChi, 
                        cs.BanDoId,
                        ISNULL(
                            (SELECT STRING_AGG(CAST(tg.PhanBonId AS VARCHAR), ', ') 
                             FROM PhanBon_CoSoBan tg 
                             WHERE tg.CoSoBanId = cs.Id), 
                            CAST(cs.LoaiId AS VARCHAR)
                        ) AS DanhSachLoaiId
                    FROM CoSoBanPhanBon cs";

                _currentData = DataProvider.ExecuteQuery(query);

                dgPhanBon.ItemsSource = _currentData.DefaultView;
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

                string query = "SELECT * FROM CoSoBanPhanBon";
                _currentData = DataProvider.ExecuteQuery(query);

                _currentData.Columns.Add("DanhSachLoaiId", typeof(string));

                foreach (DataRow row in _currentData.Rows)
                {
                    int coSoId = Convert.ToInt32(row["Id"]);
                    string listIds = GetPhanBonIdsByCoSoBan(coSoId);
                    row["DanhSachLoaiId"] = string.IsNullOrEmpty(listIds) ? row["LoaiId"].ToString() : listIds;
                }

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

        private string GetPhanBonIdsByCoSoBan(int coSoId)
        {
            try
            {
                string query = "SELECT PhanBonId FROM PhanBon_CoSoBan WHERE CoSoBanId = @CoSoId";
                DataTable dt = DataProvider.ExecuteQuery(query, new object[] { coSoId });

                List<string> ids = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    ids.Add(row["PhanBonId"].ToString());
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
                case "PhanBon":
                    LoadPhanBon();
                    break;
                case "CoSoSanXuatPhanBon":
                    LoadCoSoSanXuat();
                    break;
                case "CoSoBanPhanBon":
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

        /// <summary>
        /// Xóa các liên kết trong bảng trung gian trước khi xóa bản ghi chính
        /// </summary>
        private void DeleteRelatedLinks(int id)
        {
            try
            {
                switch (_currentTable)
                {
                    case "CoSoSanXuatPhanBon":
                        DataProvider.ExecuteNonQuery(
                            "DELETE FROM PhanBon_CoSoSanXuat WHERE CoSoSanXuatId = @Id",
                            new SqlParameter("@Id", id));
                        break;

                    case "CoSoBanPhanBon":
                        DataProvider.ExecuteNonQuery(
                            "DELETE FROM PhanBon_CoSoBan WHERE CoSoBanId = @Id",
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
                case "PhanBon":
                    var addPhanBonWindow = new AddPhanBonWindow();
                    if (addPhanBonWindow.ShowDialog() == true)
                    {
                        LoadPhanBon();
                    }
                    break;

                case "CoSoSanXuatPhanBon":
                    var addCoSoSXWindow = new AddCoSoSanXuatPhanBonWindow();
                    if (addCoSoSXWindow.ShowDialog() == true)
                    {
                        LoadCoSoSanXuat();
                    }
                    break;

                case "CoSoBanPhanBon":
                    var addCoSoBanWindow = new AddCoSoBanPhanBonWindow();
                    if (addCoSoBanWindow.ShowDialog() == true)
                    {
                        LoadCoSoBan();
                    }
                    break;
            }
        }

        private void dgPhanBon_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgPhanBon.SelectedItem == null) return;

            var selectedRow = dgPhanBon.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            switch (_currentTable)
            {
                case "PhanBon":
                    var editPhanBonWindow = new AddPhanBonWindow(selectedRow);
                    if (editPhanBonWindow.ShowDialog() == true)
                    {
                        LoadPhanBon();
                    }
                    break;

                case "CoSoSanXuatPhanBon":
                    var editCoSoSXWindow = new AddCoSoSanXuatPhanBonWindow(selectedRow);
                    if (editCoSoSXWindow.ShowDialog() == true)
                    {
                        LoadCoSoSanXuat();
                    }
                    break;

                case "CoSoBanPhanBon":
                    var editCoSoBanWindow = new AddCoSoBanPhanBonWindow(selectedRow);
                    if (editCoSoBanWindow.ShowDialog() == true)
                    {
                        LoadCoSoBan();
                    }
                    break;
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgPhanBon.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedRow = dgPhanBon.SelectedItem as DataRowView;
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
                    // Xóa liên kết trong bảng trung gian trước
                    DeleteRelatedLinks(id);

                    // Xóa bản ghi chính bằng ID
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
