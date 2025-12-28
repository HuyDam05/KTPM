using QuanLiTrongTrot.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace QuanLiTrongTrot.View.CoSoSanXuat
{
    public partial class CoSoSanXuatView : UserControl
    {
        private string _currentTable = "CS_VG";
        private List<CS_VG> _listCoSo;

        public CoSoSanXuatView()
        {
            InitializeComponent();
            LoadCoSoVietGap();
        }

        #region Load Data Methods

        public void LoadCoSoVietGap()
        {
            try
            {
                _currentTable = "CS_VG";
                txtTitle.Text = "Danh mục Cơ sở đủ ATTP VietGap";

                dgCoSo.Columns.Clear();
                dgCoSo.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgCoSo.Columns.Add(new DataGridTextColumn { Header = "Tên cơ sở", Binding = new System.Windows.Data.Binding("Ten"), Width = 250 });
                dgCoSo.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 300 });
                dgCoSo.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });

                string query = "SELECT * FROM CS_VG";
                DataTable data = DataProvider.Instance.ExecuteQuery(query);

                _listCoSo = new List<CS_VG>();
                foreach (DataRow row in data.Rows)
                {
                    _listCoSo.Add(new CS_VG
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Ten = row["Ten"].ToString(),
                        DiaChi = row["DiaChi"].ToString(),
                        BanDoId = Convert.ToInt32(row["BanDoId"])
                    });
                }

                dgCoSo.ItemsSource = _listCoSo;
                txtTongSo.Text = _listCoSo.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void TimKiemCoSo()
        {
            txtTitle.Text = "Tìm kiếm thông tin Cơ sở đủ ATTP VietGap";
            txtSearch.Focus();
        }

        #endregion

        #region Event Handlers

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_listCoSo == null) return;

            string keyword = txtSearch.Text.ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                dgCoSo.ItemsSource = _listCoSo;
            }
            else
            {
                var filtered = _listCoSo.FindAll(c =>
                    c.Ten.ToLower().Contains(keyword) ||
                    c.DiaChi.ToLower().Contains(keyword));
                dgCoSo.ItemsSource = filtered;
            }
            txtTongSo.Text = ((List<CS_VG>)dgCoSo.ItemsSource).Count.ToString();
        }

        private void BtnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Thêm mới vào bảng: {_currentTable}", "Thông báo");
        }

        #endregion
    }
}