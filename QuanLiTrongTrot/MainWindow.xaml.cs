using QuanLiTrongTrot.View.GiongCay;
using QuanLiTrongTrot.View.ThuocBVTV;
using QuanLiTrongTrot.View.PhanBon;
using QuanLiTrongTrot.View.VungTrong;
using QuanLiTrongTrot.View.CoSoSanXuat;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuanLiTrongTrot
{
    public partial class MainWindow : Window
    {
        private GiongCayView _giongCayView;
        private ThuocBVTVView _thuocBVTVView;
        private PhanBonView _phanBonView;
        private VungTrongView _vungTrongView;
        private CoSoSanXuatView _coSoSanXuatView;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuTab_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();

            txtWelcome.Visibility = Visibility.Collapsed;

            switch (tag)
            {
                case "TrangChu":
                    LoadDefaultSidebar();
                    MainContent.Children.Clear();
                    MainContent.Children.Add(txtWelcome);
                    txtWelcome.Text = "Chào mừng bạn đến với hệ thống!";
                    txtWelcome.Visibility = Visibility.Visible;
                    break;

                case "GiongCay":
                    LoadGiongCaySidebar();
                    LoadGiongCayContent();
                    break;

                case "ThuocBVTV":
                    LoadThuocBVTVSidebar();
                    LoadThuocBVTVContent();
                    break;

                case "PhanBon":
                    LoadPhanBonSidebar();
                    LoadPhanBonContent();
                    break;

                case "VungTrong":
                    LoadVungTrongSidebar();
                    LoadVungTrongContent();
                    break;

                case "CoSoSanXuat":
                    LoadCoSoSanXuatSidebar();
                    LoadCoSoSanXuatContent();
                    break;

                case "CoSoBuonBan":
                    LoadDefaultSidebar();
                    txtWelcome.Text = "Quản lý Cơ Sở Buôn Bán";
                    txtWelcome.Visibility = Visibility.Visible;
                    break;

                case "SinhVatGayHai":
                    LoadDefaultSidebar();
                    txtWelcome.Text = "Quản lý Sinh Vật Gây Hại";
                    txtWelcome.Visibility = Visibility.Visible;
                    break;
            }
        }

        #region Sidebar Methods

        private void LoadDefaultSidebar()
        {
            SidebarContent.Children.Clear();

            SidebarContent.Children.Add(CreateSidebarHeader("Người dùng"));
            SidebarContent.Children.Add(CreateSidebarButton("Quản lý người dùng", "QuanLyNguoiDung"));
            SidebarContent.Children.Add(CreateSidebarButton("Lịch sử đăng nhập", "LichSuDangNhap"));

            SidebarContent.Children.Add(CreateSidebarHeader("Hành chính", 20));
            SidebarContent.Children.Add(CreateSidebarButton("Quản lý hành chính", "QuanLyHanhChinh"));
            SidebarContent.Children.Add(CreateSidebarButton("Đơn vị cấp huyện", "DonViCapHuyen"));
            SidebarContent.Children.Add(CreateSidebarButton("Đơn vị cấp xã", "DonViCapXa"));

            SidebarContent.Children.Add(CreateSidebarHeader("Profile", 20));
            SidebarContent.Children.Add(CreateSidebarButton("Đổi mật khẩu", "DoiMatKhau"));
        }

        private void LoadGiongCaySidebar()
        {
            SidebarContent.Children.Clear();
            SidebarContent.Children.Add(CreateSidebarHeader("Danh mục Giống Cây"));

            var btnGiongCayChinh = CreateSidebarButton("Giống cây trồng chính", "GiongCayChinh");
            btnGiongCayChinh.Click += BtnGiongCayChinh_Click;
            SidebarContent.Children.Add(btnGiongCayChinh);

            var btnGiongCayLuuHanh = CreateSidebarButton("Giống cây lưu hành", "GiongCayLuuHanh");
            btnGiongCayLuuHanh.Click += BtnGiongCayLuuHanh_Click;
            SidebarContent.Children.Add(btnGiongCayLuuHanh);

            var btnGiongCayDauDong = CreateSidebarButton("Giống cây đầu dòng", "GiongCayDauDong");
            btnGiongCayDauDong.Click += BtnGiongCayDauDong_Click;
            SidebarContent.Children.Add(btnGiongCayDauDong);
        }

        private void LoadThuocBVTVSidebar()
        {
            SidebarContent.Children.Clear();
            SidebarContent.Children.Add(CreateSidebarHeader("Quản lý Thuốc BVTV"));

            var btnThuocBVTV = CreateSidebarButton("Danh mục thuốc BVTV", "ThuocBVTV");
            btnThuocBVTV.Click += BtnThuocBVTV_Click;
            SidebarContent.Children.Add(btnThuocBVTV);

            var btnCoSoSanXuat = CreateSidebarButton("Cơ sở sản xuất thuốc", "CoSoSanXuatThuoc");
            btnCoSoSanXuat.Click += BtnCoSoSanXuatThuoc_Click;
            SidebarContent.Children.Add(btnCoSoSanXuat);

            var btnCoSoBan = CreateSidebarButton("Cơ sở bán thuốc", "CoSoBanThuoc");
            btnCoSoBan.Click += BtnCoSoBanThuoc_Click;
            SidebarContent.Children.Add(btnCoSoBan);
        }

        private void LoadPhanBonSidebar()
        {
            SidebarContent.Children.Clear();
            SidebarContent.Children.Add(CreateSidebarHeader("Quản lý Phân Bón"));

            var btnPhanBon = CreateSidebarButton("Danh mục phân bón", "PhanBon");
            btnPhanBon.Click += BtnPhanBon_Click;
            SidebarContent.Children.Add(btnPhanBon);

            var btnCoSoSanXuat = CreateSidebarButton("Cơ sở sản xuất phân bón", "CoSoSanXuatPB");
            btnCoSoSanXuat.Click += BtnCoSoSanXuatPhanBon_Click;
            SidebarContent.Children.Add(btnCoSoSanXuat);

            var btnCoSoBan = CreateSidebarButton("Cơ sở bán phân bón", "CoSoBanPB");
            btnCoSoBan.Click += BtnCoSoBanPhanBon_Click;
            SidebarContent.Children.Add(btnCoSoBan);
        }

        private void LoadVungTrongSidebar()
        {
            SidebarContent.Children.Clear();
            SidebarContent.Children.Add(CreateSidebarHeader("Quản lý Vùng Trồng"));

            var btnDanhMuc = CreateSidebarButton("Quản lý danh mục vùng trồng", "DanhMucVungTrong");
            btnDanhMuc.Click += BtnDanhMucVungTrong_Click;
            SidebarContent.Children.Add(btnDanhMuc);

            var btnTimKiem = CreateSidebarButton("Tìm kiếm thông tin vùng trồng", "TimKiemVungTrong");
            btnTimKiem.Click += BtnTimKiemVungTrong_Click;
            SidebarContent.Children.Add(btnTimKiem);
        }

        private void LoadCoSoSanXuatSidebar()
        {
            SidebarContent.Children.Clear();
            SidebarContent.Children.Add(CreateSidebarHeader("Cơ sở đủ ATTP VietGap"));

            var btnDanhMuc = CreateSidebarButton("Quản lý danh mục cơ sở", "DanhMucCoSo");
            btnDanhMuc.Click += BtnDanhMucCoSo_Click;
            SidebarContent.Children.Add(btnDanhMuc);

            var btnTimKiem = CreateSidebarButton("Tìm kiếm thông tin cơ sở", "TimKiemCoSo");
            btnTimKiem.Click += BtnTimKiemCoSo_Click;
            SidebarContent.Children.Add(btnTimKiem);
        }

        private TextBlock CreateSidebarHeader(string text, double topMargin = 10)
        {
            return new TextBlock
            {
                Text = text,
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E7D32")),
                Margin = new Thickness(15, topMargin, 15, 5)
            };
        }

        private Button CreateSidebarButton(string content, string tag)
        {
            return new Button
            {
                Content = content,
                Tag = tag,
                Style = (Style)FindResource("SidebarButtonStyle")
            };
        }

        #endregion

        #region Giống Cây Events

        private void LoadGiongCayContent()
        {
            MainContent.Children.Clear();
            _giongCayView = new GiongCayView();
            MainContent.Children.Add(_giongCayView);
        }

        private void BtnGiongCayChinh_Click(object sender, RoutedEventArgs e) => _giongCayView?.LoadGiongCayChinh();
        private void BtnGiongCayLuuHanh_Click(object sender, RoutedEventArgs e) => _giongCayView?.LoadGiongCayLuuHanh();
        private void BtnGiongCayDauDong_Click(object sender, RoutedEventArgs e) => _giongCayView?.LoadGiongCayDauDong();

        #endregion

        #region Thuốc BVTV Events

        private void LoadThuocBVTVContent()
        {
            MainContent.Children.Clear();
            _thuocBVTVView = new ThuocBVTVView();
            MainContent.Children.Add(_thuocBVTVView);
        }

        private void BtnThuocBVTV_Click(object sender, RoutedEventArgs e) => _thuocBVTVView?.LoadThuocBVTV();
        private void BtnCoSoSanXuatThuoc_Click(object sender, RoutedEventArgs e) => _thuocBVTVView?.LoadCoSoSanXuat();
        private void BtnCoSoBanThuoc_Click(object sender, RoutedEventArgs e) => _thuocBVTVView?.LoadCoSoBan();

        #endregion

        #region Phân Bón Events

        private void LoadPhanBonContent()
        {
            MainContent.Children.Clear();
            _phanBonView = new PhanBonView();
            MainContent.Children.Add(_phanBonView);
        }

        private void BtnPhanBon_Click(object sender, RoutedEventArgs e) => _phanBonView?.LoadPhanBon();
        private void BtnCoSoSanXuatPhanBon_Click(object sender, RoutedEventArgs e) => _phanBonView?.LoadCoSoSanXuat();
        private void BtnCoSoBanPhanBon_Click(object sender, RoutedEventArgs e) => _phanBonView?.LoadCoSoBan();

        #endregion

        #region Vùng Trồng Events

        private void LoadVungTrongContent()
        {
            MainContent.Children.Clear();
            _vungTrongView = new VungTrongView();
            MainContent.Children.Add(_vungTrongView);
        }

        private void BtnDanhMucVungTrong_Click(object sender, RoutedEventArgs e) => _vungTrongView?.LoadVungTrong();
        private void BtnTimKiemVungTrong_Click(object sender, RoutedEventArgs e) => _vungTrongView?.TimKiemVungTrong();

        #endregion

        #region Cơ Sở Sản Xuất Events

        private void LoadCoSoSanXuatContent()
        {
            MainContent.Children.Clear();
            _coSoSanXuatView = new CoSoSanXuatView();
            MainContent.Children.Add(_coSoSanXuatView);
        }

        private void BtnDanhMucCoSo_Click(object sender, RoutedEventArgs e) => _coSoSanXuatView?.LoadCoSoVietGap();
        private void BtnTimKiemCoSo_Click(object sender, RoutedEventArgs e) => _coSoSanXuatView?.TimKiemCoSo();

        #endregion

        private void Sidebar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string tag = btn.Tag.ToString();

            txtWelcome.Visibility = Visibility.Visible;
            MainContent.Children.Clear();
            MainContent.Children.Add(txtWelcome);

            switch (tag)
            {
                case "QuanLyNguoiDung": txtWelcome.Text = "Quản lý Người dùng"; break;
                case "LichSuDangNhap": txtWelcome.Text = "Lịch sử đăng nhập"; break;
                case "QuanLyHanhChinh": txtWelcome.Text = "Quản lý Hành chính"; break;
                case "DonViCapHuyen": txtWelcome.Text = "Đơn vị cấp Huyện"; break;
                case "DonViCapXa": txtWelcome.Text = "Đơn vị cấp Xã"; break;
                case "DoiMatKhau": txtWelcome.Text = "Đổi mật khẩu"; break;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                new LoginWindow().Show();
                this.Close();
            }
        }
    }
}

