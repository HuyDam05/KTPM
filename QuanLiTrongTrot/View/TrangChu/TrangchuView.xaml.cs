using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

//dgTrangChu
namespace QuanLiTrongTrot.View.TrangChu
{
    public partial class TrangchuView : UserControl
    {
        private string tenHienThi = "";
        private string _currentTable = "";
        private DataTable _currentData;
        private bool _isLoading = false;
        
        public TrangchuView(string os)
        {
            InitializeComponent();
            switch (os) {
                case "QuanliHanhChinh":
                    LoadHanhChinh();
                    break;
                case "Quanliuser":
                    LoadUser();
                    break;
            }
            if (_currentTable != "TaiKhoan" && _currentTable != "Xa" )
            {
                btnXoa.Visibility = Visibility.Collapsed;
            }
        }
        #region Load data
        public void LoadHanhChinh()
        {
            try
            {
                _isLoading = true;
                _currentTable = "Tinh";
                txtTitle.Text = "Đơn vị Hành Chính";
                txtSearch.Text = "";

                dgTrangChu.ItemsSource = null;

                dgTrangChu.Columns.Clear();
                dgTrangChu.Columns.Add(new DataGridTextColumn { Header = "Mã Vùng", Binding = new System.Windows.Data.Binding("MaVung"), Width = 250 });
                dgTrangChu.Columns.Add(new DataGridTextColumn { Header = "Tên Tỉnh/Thành", Binding = new System.Windows.Data.Binding("Ten"), Width = 300 });

                string query = $"SELECT * FROM {_currentTable}";
                _currentData = DataProvider.ExecuteQuery(query);

                dgTrangChu.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void LoadUser()
        {
            try
            {
                _isLoading = true;
                _currentTable = "TaiKhoan";
                txtTitle.Text = "Danh sách Tài khoản";
                txtSearch.Text = "";

                dgTrangChu.ItemsSource = null;

                dgTrangChu.Columns.Clear();
                dgTrangChu.Columns.Add(new DataGridTextColumn { Header = "Tên", Binding = new System.Windows.Data.Binding("Ten"), Width = 250 });
                dgTrangChu.Columns.Add(new DataGridTextColumn { Header = "Hạng quyền hạn", Binding = new System.Windows.Data.Binding("QuyenID"), Width = 150 });
                dgTrangChu.Columns.Add(new DataGridTextColumn { Header = "Hồ sơ Id", Binding = new System.Windows.Data.Binding("HoSoId"), Width = 100 });
                string query = $"SELECT * FROM {_currentTable}";
                _currentData = DataProvider.ExecuteQuery(query);

                dgTrangChu.ItemsSource = _currentData.DefaultView;
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
                case "QuanliHanhChinh":
                    LoadHanhChinh();
                    break;
                case "TaiKhoan":
                    LoadUser();
                    break;
                case "Xa":
                    LoadXa(tenHienThi);
                    break;
            }
            if (_currentTable != "TaiKhoan" && _currentTable != "Xa")
            {
                btnXoa.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
        #region Nút và Thanh công cụ
        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn không
            if (dgTrangChu.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedRow = dgTrangChu.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            string tenHienThi = "";

            // Lấy tên hiển thị tùy theo bảng
            switch (_currentTable)
            {
                case "TaiKhoan":
                    tenHienThi = selectedRow["Ten"].ToString();
                    break;
                case "Xa":
                    tenHienThi = selectedRow["Ten"].ToString();
                    break;

            }

            // Xác nhận xóa
            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa \"{tenHienThi}\"\nThao tác này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    int deleteResult = DataProvider.DELETE_DATA(tenHienThi, "Ten", _currentTable);

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

        private void BtnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentTable == "TaiKhoan")
                {
                    var w = new AddUserWindow { Owner = Window.GetWindow(this) };
                    if (w.ShowDialog() == true)
                        LoadUser();
                    return;
                }

                if (_currentTable == "Xa")
                {
                    var w = new AddXaWindow { Owner = Window.GetWindow(this) };
                    if (w.ShowDialog() == true)
                    {
                        ReloadCurrentData();
                    }
                    return;
                }

                MessageBox.Show("Chức năng thêm mới cho mục này chưa hỗ trợ.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        
        private void Xa(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(_currentTable != "Tinh") return;
            var selectedRow = dgTrangChu.SelectedItem as DataRowView;
            _currentTable = "Xa";
            tenHienThi = selectedRow["Ten"].ToString();
            LoadXa(tenHienThi);
        }
        private void LoadXa(string tenTinh) {
            try
            {
                btnXoa.Visibility = Visibility.Visible;
                _isLoading = true;
                txtTitle.Text = $"Danh sách Xã của {tenTinh}";
                txtSearch.Text = "";

                dgTrangChu.ItemsSource = null;

                dgTrangChu.Columns.Clear();
                dgTrangChu.Columns.Add(new DataGridTextColumn { Header = "STT", Binding = new System.Windows.Data.Binding("STT"), Width = 100 });
                dgTrangChu.Columns.Add(new DataGridTextColumn { Header = "Tên Xã", Binding = new System.Windows.Data.Binding("Ten"), Width = 300 });

                // Sử dụng ROW_NUMBER() để tạo STT liên tục
                string query = @"SELECT ROW_NUMBER() OVER (ORDER BY Xa.Id) AS STT, Xa.Id, Xa.Ten 
                                 FROM Xa 
                                 INNER JOIN Tinh ON Xa.MaTinh = Tinh.MaVung 
                                 WHERE Tinh.Ten LIKE N'%" + tenTinh + "%'";
                
                _currentData = DataProvider.ExecuteQuery(query);
                dgTrangChu.ItemsSource = _currentData.DefaultView;
                txtTongSo.Text = _currentData.Rows.Count.ToString();
                _isLoading = false;
            }
            catch (Exception ex)
            {
                _isLoading = false;
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
    }
}
