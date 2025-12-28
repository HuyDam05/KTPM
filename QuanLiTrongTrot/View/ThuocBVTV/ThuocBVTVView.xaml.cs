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

        public ThuocBVTVView()
        {
            InitializeComponent();
            LoadThuocBVTV();
        }

        #region Load Data Methods

        // Load Danh sách thuốc BVTV
        public void LoadThuocBVTV()
        {
            try
            {
                _currentTable = "ThuocBVTV";
                txtTitle.Text = "Danh sách Thuốc Bảo Vệ Thực Vật";

                dgThuocBVTV.Columns.Clear();
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Tên thuốc", Binding = new System.Windows.Data.Binding("Ten"), Width = 200 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Ngày sản xuất", Binding = new System.Windows.Data.Binding("NgaySX") { StringFormat = "dd/MM/yyyy" }, Width = 120 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Hạn sử dụng", Binding = new System.Windows.Data.Binding("HanSD") { StringFormat = "dd/MM/yyyy" }, Width = 120 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Vùng trồng ID", Binding = new System.Windows.Data.Binding("VTId"), Width = 100 });

                string query = "SELECT * FROM ThuocBVTV";
                DataTable data = DataProvider.Instance.ExecuteQuery(query);

                var list = new List<Model.ThuocBVTV>();
                foreach (DataRow row in data.Rows)
                {
                    list.Add(new Model.ThuocBVTV
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Ten = row["Ten"].ToString(),
                        NgaySX = Convert.ToDateTime(row["NgaySX"]),
                        HanSD = Convert.ToDateTime(row["HanSD"]),
                        VTId = Convert.ToInt32(row["VTId"])
                    });
                }

                dgThuocBVTV.ItemsSource = list;
                txtTongSo.Text = list.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Load Cơ sở sản xuất thuốc BVTV
        public void LoadCoSoSanXuat()
        {
            try
            {
                _currentTable = "CoSoSanXuatThuocBVTV";
                txtTitle.Text = "Cơ sở sản xuất thuốc BVTV";

                dgThuocBVTV.Columns.Clear();
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Tên cơ sở", Binding = new System.Windows.Data.Binding("Ten"), Width = 200 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 250 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Loại ID", Binding = new System.Windows.Data.Binding("LoaiId"), Width = 80 });

                string query = "SELECT * FROM CoSoSanXuatThuocBVTV";
                DataTable data = DataProvider.Instance.ExecuteQuery(query);

                var list = new List<CoSoSanXuatThuocBVTV>();
                foreach (DataRow row in data.Rows)
                {
                    list.Add(new CoSoSanXuatThuocBVTV
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Ten = row["Ten"].ToString(),
                        DiaChi = row["DiaChi"].ToString(),
                        BanDoId = Convert.ToInt32(row["BanDoId"]),
                        LoaiId = Convert.ToInt32(row["LoaiId"])
                    });
                }

                dgThuocBVTV.ItemsSource = list;
                txtTongSo.Text = list.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Load Cơ sở bán thuốc BVTV
        public void LoadCoSoBan()
        {
            try
            {
                _currentTable = "CoSoBanThuocBVTV";
                txtTitle.Text = "Cơ sở bán thuốc BVTV";

                dgThuocBVTV.Columns.Clear();
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Tên cơ sở", Binding = new System.Windows.Data.Binding("Ten"), Width = 200 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 250 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });
                dgThuocBVTV.Columns.Add(new DataGridTextColumn { Header = "Loại ID", Binding = new System.Windows.Data.Binding("LoaiId"), Width = 80 });

                string query = "SELECT * FROM CoSoBanThuocBVTV";
                DataTable data = DataProvider.Instance.ExecuteQuery(query);

                var list = new List<CoSoBanThuocBVTV>();
                foreach (DataRow row in data.Rows)
                {
                    list.Add(new CoSoBanThuocBVTV
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Ten = row["Ten"].ToString(),
                        DiaChi = row["DiaChi"].ToString(),
                        BanDoId = Convert.ToInt32(row["BanDoId"]),
                        LoaiId = Convert.ToInt32(row["LoaiId"])
                    });
                }

                dgThuocBVTV.ItemsSource = list;
                txtTongSo.Text = list.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Event Handlers

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO: Implement search
        }

        private void BtnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Thêm mới vào bảng: {_currentTable}", "Thông báo");
        }

        #endregion
    }
}