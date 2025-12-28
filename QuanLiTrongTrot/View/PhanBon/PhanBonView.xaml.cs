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

        public PhanBonView()
        {
            InitializeComponent();
            LoadPhanBon();
        }

        #region Load Data Methods

        // Load Danh sách phân bón
        public void LoadPhanBon()
        {
            try
            {
                _currentTable = "PhanBon";
                txtTitle.Text = "Danh sách Phân Bón";

                dgPhanBon.Columns.Clear();
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Tên phân bón", Binding = new System.Windows.Data.Binding("Ten"), Width = 180 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Thành phần", Binding = new System.Windows.Data.Binding("ThanhPhan"), Width = 150 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Phân loại", Binding = new System.Windows.Data.Binding("PhanLoai"), Width = 120 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Vùng trồng ID", Binding = new System.Windows.Data.Binding("VTId"), Width = 100 });

                string query = "SELECT * FROM PhanBon";
                DataTable data = DataProvider.Instance.ExecuteQuery(query);

                var list = new List<Model.PhanBon>();
                foreach (DataRow row in data.Rows)
                {
                    list.Add(new Model.PhanBon
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Ten = row["Ten"].ToString(),
                        ThanhPhan = row["ThanhPhan"].ToString(),
                        PhanLoai = row["PhanLoai"].ToString(),
                        VTId = Convert.ToInt32(row["VTId"])
                    });
                }

                dgPhanBon.ItemsSource = list;
                txtTongSo.Text = list.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Load Cơ sở sản xuất phân bón
        public void LoadCoSoSanXuat()
        {
            try
            {
                _currentTable = "CoSoSanXuatPhanBon";
                txtTitle.Text = "Cơ sở sản xuất phân bón";

                dgPhanBon.Columns.Clear();
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Tên cơ sở", Binding = new System.Windows.Data.Binding("Ten"), Width = 200 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 250 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Loại ID", Binding = new System.Windows.Data.Binding("LoaiId"), Width = 80 });

                string query = "SELECT * FROM CoSoSanXuatPhanBon";
                DataTable data = DataProvider.Instance.ExecuteQuery(query);

                var list = new List<CoSoSanXuatPhanBon>();
                foreach (DataRow row in data.Rows)
                {
                    list.Add(new CoSoSanXuatPhanBon
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Ten = row["Ten"].ToString(),
                        DiaChi = row["DiaChi"].ToString(),
                        BanDoId = Convert.ToInt32(row["BanDoId"]),
                        LoaiId = Convert.ToInt32(row["LoaiId"])
                    });
                }

                dgPhanBon.ItemsSource = list;
                txtTongSo.Text = list.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Load Cơ sở bán phân bón
        public void LoadCoSoBan()
        {
            try
            {
                _currentTable = "CoSoBanPhanBon";
                txtTitle.Text = "Cơ sở bán phân bón";

                dgPhanBon.Columns.Clear();
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Tên cơ sở", Binding = new System.Windows.Data.Binding("Ten"), Width = 200 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 250 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });
                dgPhanBon.Columns.Add(new DataGridTextColumn { Header = "Loại ID", Binding = new System.Windows.Data.Binding("LoaiId"), Width = 80 });

                string query = "SELECT * FROM CoSoBanPhanBon";
                DataTable data = DataProvider.Instance.ExecuteQuery(query);

                var list = new List<CoSoBanPhanBon>();
                foreach (DataRow row in data.Rows)
                {
                    list.Add(new CoSoBanPhanBon
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Ten = row["Ten"].ToString(),
                        DiaChi = row["DiaChi"].ToString(),
                        BanDoId = Convert.ToInt32(row["BanDoId"]),
                        LoaiId = Convert.ToInt32(row["LoaiId"])
                    });
                }

                dgPhanBon.ItemsSource = list;
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