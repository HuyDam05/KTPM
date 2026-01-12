using QuanLiTrongTrot.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace QuanLiTrongTrot.View.VungTrong
{
    public partial class AddVungTrongWindow : Window
    {
        private int? _editId = null;

        public AddVungTrongWindow()
        {
            InitializeComponent();
        }

        public AddVungTrongWindow(DataRowView row) : this()
        {
            if (row != null)
            {
                _editId = Convert.ToInt32(row["Id"]);
                txtQuyMo.Text = row["QuyMo"].ToString();
                txtDiaChi.Text = row["DiaChi"].ToString();

                this.Title = "Sửa Vùng Trồng";
            }
        }

        /// <summary>
        /// Lấy ID nhỏ nhất còn trống hoặc ID tiếp theo
        /// </summary>
        private int GetNextId()
        {
            // Tìm ID nhỏ nhất còn trống (gap)
            string gapQuery = @"
                SELECT TOP 1 t1.Id + 1 AS NextId
                FROM VungTrong t1
                LEFT JOIN VungTrong t2 ON t1.Id + 1 = t2.Id
                WHERE t2.Id IS NULL
                ORDER BY t1.Id";

            int gapId = DataProvider.ExecuteScalar(gapQuery);
            if (gapId > 0)
                return gapId;

            // Kiểm tra bảng rỗng
            int maxId = DataProvider.ExecuteScalar("SELECT ISNULL(MAX(Id), 0) FROM VungTrong");
            if (maxId == 0)
                return 1;

            // Kiểm tra ID = 1 có tồn tại không
            int hasId1 = DataProvider.ExecuteScalar("SELECT COUNT(*) FROM VungTrong WHERE Id = 1");
            if (hasId1 == 0)
                return 1;

            return maxId + 1;
        }

        /// <summary>
        /// Đảm bảo BanDoPhanBo tồn tại, nếu không thì tạo mới
        /// </summary>
        private void EnsureBanDoPhanBoExists(int banDoId)
        {
            // Kiểm tra BanDoPhanBo có tồn tại không
            int exists = DataProvider.ExecuteScalar(
                "SELECT COUNT(*) FROM BanDoPhanBo WHERE Id = @Id",
                new SqlParameter("@Id", banDoId));

            if (exists == 0)
            {
                // Tạo mới BanDoPhanBo với tọa độ mặc định
                string insertQuery = @"
                    SET IDENTITY_INSERT BanDoPhanBo ON;
                    INSERT INTO BanDoPhanBo (Id, ToaDoX, ToaDoY) VALUES (@Id, @ToaDoX, @ToaDoY);
                    SET IDENTITY_INSERT BanDoPhanBo OFF;";

                DataProvider.ExecuteNonQuery(insertQuery,
                    new SqlParameter("@Id", banDoId),
                    new SqlParameter("@ToaDoX", 21.0 + banDoId * 0.1),  // Tọa độ X mặc định
                    new SqlParameter("@ToaDoY", 105.0 + banDoId * 0.1)); // Tọa độ Y mặc định
            }
        }

        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(txtQuyMo.Text, out double quyMo) || quyMo <= 0)
            {
                MessageBox.Show("Quy mô phải là số dương!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQuyMo.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDiaChi.Focus();
                return;
            }

            try
            {
                if (_editId == null)
                {
                    // Thêm mới với ID và BanDoId tự động
                    int newId = GetNextId();
                    int newBanDoId = newId;

                    // Đảm bảo BanDoPhanBo tồn tại trước khi insert VungTrong
                    EnsureBanDoPhanBoExists(newBanDoId);
                    
                    string query = @"
                        SET IDENTITY_INSERT VungTrong ON;
                        INSERT INTO VungTrong (Id, QuyMo, DiaChi, BanDoId) VALUES (@Id, @QuyMo, @DiaChi, @BanDoId);
                        SET IDENTITY_INSERT VungTrong OFF;";

                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", newId),
                        new SqlParameter("@QuyMo", quyMo),
                        new SqlParameter("@DiaChi", txtDiaChi.Text),
                        new SqlParameter("@BanDoId", newBanDoId));

                    if (result > 0)
                    {
                        MessageBox.Show($"Thêm mới thành công!\nID: {newId}\nBản đồ ID: {newBanDoId}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thao tác không thành công!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Cập nhật
                    string query = "UPDATE VungTrong SET QuyMo = @QuyMo, DiaChi = @DiaChi WHERE Id = @Id";
                    
                    int result = DataProvider.ExecuteNonQuery(query,
                        new SqlParameter("@Id", _editId.Value),
                        new SqlParameter("@QuyMo", quyMo),
                        new SqlParameter("@DiaChi", txtDiaChi.Text));

                    if (result > 0)
                    {
                        MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thao tác không thành công!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thao tác: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (_editId == null)
            {
                // Clear form
                txtQuyMo.Text = "";
                txtDiaChi.Text = "";
                txtQuyMo.Focus();
            }
            else
            {
                var result = MessageBox.Show("Bạn có chắc muốn xóa vùng trồng này?", "Xác nhận",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int deleteResult = DataProvider.DELETE_DATA(_editId.ToString(), "Id", "VungTrong");
                        if (deleteResult > 0)
                        {
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
