using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLiTrongTrot.View.GiongCay
{
    public partial class GiongCayView : UserControl
    {
        private string _currentTable = "GiongCayChinh";

        public GiongCayView()
        {
            InitializeComponent();
            LoadGiongCayChinh(); // Mặc định load Giống cây trồng chính
        }

        #region Load Data Methods

        // Load Giống cây trồng chính
        private void LoadGiongCayChinh()
        {
            try
            {
                _currentTable = "GiongCayChinh";
                txtTitle.Text = "Danh sách Giống Cây Trồng Chính";

                // Cập nhật columns cho DataGrid
                dgGiongCay.Columns.Clear();
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Tên", Binding = new System.Windows.Data.Binding("Ten"), Width = 150 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Phân loại", Binding = new System.Windows.Data.Binding("PhanLoai"), Width = 150 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Mùa vụ", Binding = new System.Windows.Data.Binding("MuaVu"), Width = 120 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Sản lượng", Binding = new System.Windows.Data.Binding("SanLuong"), Width = 100 });

                string query = "SELECT * FROM GiongCayChinh";
                DataTable data = DataProvider.Instance.ExecuteQuery(query);

                var list = new List<GiongCayChinh>();
                foreach (DataRow row in data.Rows)
                {
                    list.Add(new GiongCayChinh
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Ten = row["Ten"].ToString(),
                        PhanLoai = row["PhanLoai"].ToString(),
                        MuaVu = row["MuaVu"].ToString(),
                        SanLuong = Convert.ToInt32(row["SanLuong"])
                    });
                }

                dgGiongCay.ItemsSource = list;
                txtTongSo.Text = list.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Load Giống cây lưu hành
        private void LoadGiongCayLuuHanh()
        {
            try
            {
                _currentTable = "GiongCayLuuHanh";
                txtTitle.Text = "Danh sách Giống Cây Lưu Hành";

                // Cập nhật columns cho DataGrid
                dgGiongCay.Columns.Clear();
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Loại cây", Binding = new System.Windows.Data.Binding("LoaiCay"), Width = 150 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Nơi phổ biến", Binding = new System.Windows.Data.Binding("NoiPhoBien"), Width = 150 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Công dụng", Binding = new System.Windows.Data.Binding("CongDung"), Width = 200 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Đặc điểm", Binding = new System.Windows.Data.Binding("DacDiem"), Width = 200 });

                string query = "SELECT * FROM GiongCayLuuHanh";
                DataTable data = DataProvider.Instance.ExecuteQuery(query);

                var list = new List<GiongCayLuuHanh>();
                foreach (DataRow row in data.Rows)
                {
                    list.Add(new GiongCayLuuHanh
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        LoaiCay = row["LoaiCay"].ToString(),
                        NoiPhoBien = row["NoiPhoBien"].ToString(),
                        CongDung = row["CongDung"].ToString(),
                        DacDiem = row["DacDiem"].ToString()
                    });
                }

                dgGiongCay.ItemsSource = list;
                txtTongSo.Text = list.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Load Giống cây đầu dòng
        private void LoadGiongCayDauDong()
        {
            try
            {
                _currentTable = "GiongCayDauDong";
                txtTitle.Text = "Danh sách Giống Cây Đầu Dòng";

                // Cập nhật columns cho DataGrid
                dgGiongCay.Columns.Clear();
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Tên", Binding = new System.Windows.Data.Binding("Ten"), Width = 150 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Nguồn gốc", Binding = new System.Windows.Data.Binding("NguonGoc"), Width = 120 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Đặc tính", Binding = new System.Windows.Data.Binding("DacTinh"), Width = 200 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Thời gian thu hoạch", Binding = new System.Windows.Data.Binding("ThoiGianThuHoach"), Width = 140 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Giống ID", Binding = new System.Windows.Data.Binding("GiongId"), Width = 80 });
                dgGiongCay.Columns.Add(new DataGridTextColumn { Header = "Vùng trồng ID", Binding = new System.Windows.Data.Binding("VTId"), Width = 100 });

                string query = "SELECT * FROM GiongCayDauDong";
                DataTable data = DataProvider.Instance.ExecuteQuery(query);

                var list = new List<GiongCayDauDong>();
                foreach (DataRow row in data.Rows)
                {
                    list.Add(new GiongCayDauDong
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Ten = row["Ten"].ToString(),
                        NguonGoc = row["NguonGoc"].ToString(),
                        DacTinh = row["DacTinh"].ToString(),
                        ThoiGianThuHoach = Convert.ToInt32(row["ThoiGianThuHoach"]),
                        GiongId = Convert.ToInt32(row["GiongId"]),
                        VTId = Convert.ToInt32(row["VTId"])
                    });
                }

                dgGiongCay.ItemsSource = list;
                txtTongSo.Text = list.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Event Handlers

        private void BtnGiongCayChinh_Click(object sender, RoutedEventArgs e)
        {
            LoadGiongCayChinh();
        }

        private void BtnGiongCayLuuHanh_Click(object sender, RoutedEventArgs e)
        {
            LoadGiongCayLuuHanh();
        }

        private void BtnGiongCayDauDong_Click(object sender, RoutedEventArgs e)
        {
            LoadGiongCayDauDong();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO: Implement search functionality
            string keyword = txtSearch.Text.ToLower();
            // Tìm kiếm theo bảng hiện tại
        }

        private void BtnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Thêm mới vào bảng: {_currentTable}", "Thông báo");
            // TODO: Mở form thêm mới tương ứng
        }

        #endregion
    }
}
