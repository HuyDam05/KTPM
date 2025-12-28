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
        private List<Model.VungTrong> _listVungTrong;

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
                _currentTable = "VungTrong";
                txtTitle.Text = "Danh mục Vùng Trồng";

                dgVungTrong.Columns.Clear();
                dgVungTrong.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new System.Windows.Data.Binding("Id"), Width = 50 });
                dgVungTrong.Columns.Add(new DataGridTextColumn { Header = "Tên vùng trồng", Binding = new System.Windows.Data.Binding("Ten"), Width = 250 });
                dgVungTrong.Columns.Add(new DataGridTextColumn { Header = "Địa chỉ", Binding = new System.Windows.Data.Binding("DiaChi"), Width = 300 });
                dgVungTrong.Columns.Add(new DataGridTextColumn { Header = "Bản đồ ID", Binding = new System.Windows.Data.Binding("BanDoId"), Width = 100 });

                string query = "SELECT * FROM VungTrong";
                DataTable data = DataProvider.Instance.ExecuteQuery(query);

                _listVungTrong = new List<Model.VungTrong>();
                foreach (DataRow row in data.Rows)
                {
                    _listVungTrong.Add(new Model.VungTrong
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Ten = row["Ten"].ToString(),
                        DiaChi = row["DiaChi"].ToString(),
                        BanDoId = Convert.ToInt32(row["BanDoId"])
                    });
                }

                dgVungTrong.ItemsSource = _listVungTrong;
                txtTongSo.Text = _listVungTrong.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void TimKiemVungTrong()
        {
            txtTitle.Text = "Tìm kiếm thông tin Vùng Trồng";
            txtSearch.Focus();
        }

        #endregion

        #region Event Handlers

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_listVungTrong == null) return;

            string keyword = txtSearch.Text.ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                dgVungTrong.ItemsSource = _listVungTrong;
            }
            else
            {
                var filtered = _listVungTrong.FindAll(v =>
                    v.Ten.ToLower().Contains(keyword) ||
                    v.DiaChi.ToLower().Contains(keyword));
                dgVungTrong.ItemsSource = filtered;
            }
            txtTongSo.Text = ((List<Model.VungTrong>)dgVungTrong.ItemsSource).Count.ToString();
        }

        private void BtnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Thêm mới vào bảng: {_currentTable}", "Thông báo");
        }

        #endregion
    }
}