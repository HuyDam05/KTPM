using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                
                // Bỏ cột STT, chỉ dùng ID làm số thứ tự
                dgVungTrong.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 60 });
                dgVungTrong.Columns.Add(new DataGridTextColumn { Header = "Quy mô (m²)", Binding = new System.Windows.Data.Binding("QuyMo"), Width = 150 });
                dgVungTrong.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 350 });

                string query = "SELECT Id, QuyMo, DiaChi FROM VungTrong ORDER BY Id";
                _currentData = DataProvider.ExecuteQuery(query);

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
            AddVungTrongWindow addWindow = new AddVungTrongWindow();
            addWindow.ShowDialog();
            ReloadCurrentData();
        }

        #endregion

        private void ReloadCurrentData()
        {
            switch (_currentTable)
            {
                case "VungTrong":
                    LoadVungTrong();
                    break;
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgVungTrong.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedRow = dgVungTrong.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            int id = Convert.ToInt32(selectedRow["Id"]);
            string diaChi = selectedRow["DiaChi"]?.ToString() ?? "";

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa vùng trồng \"{diaChi}\" (ID: {id})?\n\n" +
                "⚠️ CẢNH BÁO: Tất cả dữ liệu liên quan (Phân bón, Thuốc BVTV, Sinh vật gây hại, Giống cây...) cũng sẽ bị xóa!\n\n" +
                "Thao tác này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    int deleteResult = DeleteVungTrongCascade(id);

                    if (deleteResult > 0)
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                        ReloadCurrentData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Xóa VungTrong và tất cả dữ liệu liên quan
        /// </summary>
        private int DeleteVungTrongCascade(int vungTrongId)
        {
            using (var connection = new SqlConnection(DataProvider.connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Cấp 3: Bảng trung gian
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM PhanBon_CoSoBan WHERE PhanBonId IN (SELECT Id FROM PhanBon WHERE VTId = @VTId)", vungTrongId);
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM PhanBon_CoSoSanXuat WHERE PhanBonId IN (SELECT Id FROM PhanBon WHERE VTId = @VTId)", vungTrongId);
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM ThuocBVTV_CoSoBan WHERE ThuocBVTVId IN (SELECT Id FROM ThuocBVTV WHERE VTId = @VTId)", vungTrongId);
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM ThuocBVTV_CoSoSanXuat WHERE ThuocBVTVId IN (SELECT Id FROM ThuocBVTV WHERE VTId = @VTId)", vungTrongId);

                        // Cấp 2: Phụ thuộc SinhVatGayHai
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM CapNhat_SVGH WHERE SVId IN (SELECT Id FROM SinhVatGayHai WHERE VTId = @VTId)", vungTrongId);
                        ExecuteInTransaction(connection, transaction,
                            "DELETE FROM TuoiSau WHERE SVId IN (SELECT Id FROM SinhVatGayHai WHERE VTId = @VTId)", vungTrongId);

                        // Cấp 1: Phụ thuộc trực tiếp VungTrong
                        ExecuteInTransaction(connection, transaction, "DELETE FROM SinhVatGayHai WHERE VTId = @VTId", vungTrongId);
                        ExecuteInTransaction(connection, transaction, "DELETE FROM PhanBon WHERE VTId = @VTId", vungTrongId);
                        ExecuteInTransaction(connection, transaction, "DELETE FROM ThuocBVTV WHERE VTId = @VTId", vungTrongId);
                        ExecuteInTransaction(connection, transaction, "DELETE FROM GiongCayDauDong WHERE VTId = @VTId", vungTrongId);

                        // Cuối cùng: Xóa VungTrong
                        ExecuteInTransaction(connection, transaction, "DELETE FROM VungTrong WHERE Id = @VTId", vungTrongId);

                        transaction.Commit();
                        return 1;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private void ExecuteInTransaction(SqlConnection connection, SqlTransaction transaction, string query, int vungTrongId)
        {
            try
            {
                using (var command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.AddWithValue("@VTId", vungTrongId);
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                if (!ex.Message.Contains("Invalid object name"))
                    throw;
            }
        }
    }
}