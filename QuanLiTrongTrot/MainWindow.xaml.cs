using QuanLiTrongTrot.Model;
using QuanLiTrongTrot.View.CoSoSanXuat;
using QuanLiTrongTrot.View.GiongCay;
using QuanLiTrongTrot.View.PhanBon;
using QuanLiTrongTrot.View.SinhVatGayHai;
using QuanLiTrongTrot.View.ThuocBVTV;
using QuanLiTrongTrot.View.TrangChu;
using QuanLiTrongTrot.View.VungTrong;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace QuanLiTrongTrot
{
    public partial class MainWindow : Window
    {
        private readonly string _username;
        private GiongCayView _giongCayView;
        private ThuocBVTVView _thuocBVTVView;
        private PhanBonView _phanBonView;
        private VungTrongView _vungTrongView;
        private SinhVatGayHaiView _sinhVatGayHaiView;
        private CoSoVietGapView _coSoVietGapView;
        private TrangChu _trangChuView;
        private TrangchuView _trangchuView;

        public MainWindow() : this(string.Empty)
        {
        }
        public MainWindow(string username)
        {
            InitializeComponent();
            LoadTrangChuContent();
            _username = username;
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
                    LoadTrangChuContent();
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

                case "SinhVatGayHai":
                    LoadSinhVatGayHaiSidebar();
                    LoadSinhVatGayHaiContent();
                    break;
            }
        }

        #region Sidebar Methods

        private void LoadDefaultSidebar()
        {
            SidebarContent.Children.Clear();
            SidebarContent.Children.Add(CreateSidebarHeader("Người dùng"));

            var btnQuanliusers = CreateSidebarButton("Quản lý người dùng", "QuanLyNguoiDung");
            btnQuanliusers.Click += btnQuanliuser;
            SidebarContent.Children.Add(btnQuanliusers);

            SidebarContent.Children.Add(CreateSidebarHeader("Hành chính", 20));
            var btnHanhChinh = CreateSidebarButton("Quản lý hành chính", "QuanLyHanhChinh");
            btnHanhChinh.Click += btnQuanliHanhChinh;
            SidebarContent.Children.Add(btnHanhChinh);

            SidebarContent.Children.Add(CreateSidebarHeader("Profile", 20));
            var btnChangePassword = CreateSidebarButton("Đổi mật khẩu", "DoiMatKhau");
            btnChangePassword.Click += Sidebar_Click;
            SidebarContent.Children.Add(btnChangePassword);
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
        }

        private void LoadCoSoSanXuatSidebar()
        {
            SidebarContent.Children.Clear();
            SidebarContent.Children.Add(CreateSidebarHeader("Cơ sở đủ ATTP VietGap"));

            var btnDanhMuc = CreateSidebarButton("Quản lý danh mục cơ sở", "DanhMucCoSo");
            btnDanhMuc.Click += BtnDanhMucCoSo_Click;
            SidebarContent.Children.Add(btnDanhMuc);
        }

        private void LoadSinhVatGayHaiSidebar()
        {
            SidebarContent.Children.Clear();
            SidebarContent.Children.Add(CreateSidebarHeader("Quản lý Sinh Vật Gây Hại"));

            var btnDanhMuc = CreateSidebarButton("Quản lý danh mục sinh vật gây hại", "DanhMucSVGH");
            btnDanhMuc.Click += BtnDanhMucSVGH_Click;
            SidebarContent.Children.Add(btnDanhMuc);

            var btnCapNhat = CreateSidebarButton("Cập nhật tình hình sinh vật gây hại", "CapNhatSVGH");
            btnCapNhat.Click += BtnCapNhatSVGH_Click;
            SidebarContent.Children.Add(btnCapNhat);

            var btnTuoiSau = CreateSidebarButton("Quản lý danh mục tuổi sâu, cấp độ phổ biến", "TuoiSau");
            btnTuoiSau.Click += BtnTuoiSau_Click;
            SidebarContent.Children.Add(btnTuoiSau);
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
        #region Trang chủ Events
        private void LoadTrangChuContent()
        {
            MainContent.Children.Clear();
            _trangChuView = new TrangChu();
            MainContent.Children.Add(_trangChuView);

        }
        public void LoadQuanliUser(string os)
        {
            MainContent.Children.Clear();
            _trangchuView = new TrangchuView(os);
            MainContent.Children.Add(_trangchuView);
        }
        private void btnLichSuDangNhap(object sender, EventArgs e)
        {
            LoadQuanliUser("QuanliLichSu");
        }
        private void btnQuanliHanhChinh(object sender, EventArgs e) 
        {
            LoadQuanliUser("QuanliHanhChinh");
        }
        private void btnDonViCapHuyen(object sender, EventArgs e)
        {
            LoadQuanliUser("DonViCaphuyen");
        }
        private void btnDonViCapXa(object sender, EventArgs e)
        {
            LoadQuanliUser("DonViCapXa");
        }
        private void btnQuanliuser(object sender, RoutedEventArgs e)
        {
            string[] strings = { _username , "3" };
            string[] data_name = { "Ten", "QuyenID" };
            bool a = DataProvider.CHECK_SAME_ROW_DATA( strings , data_name , "TaiKhoan" );
            if( a )
            {
                MessageBox.Show("Bạn không có quyền sử dụng tính năng này!", "Thông báo",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                LoadQuanliUser("Quanliuser");
            }
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

        #endregion

        #region Cơ Sở Sản Xuất Events

        private void LoadCoSoSanXuatContent()
        {
            MainContent.Children.Clear();
            _coSoVietGapView = new CoSoVietGapView();
            MainContent.Children.Add(_coSoVietGapView);
        }

        private void BtnDanhMucCoSo_Click(object sender, RoutedEventArgs e) => _coSoVietGapView?.LoadData();

        #endregion

        #region Sinh Vật Gây Hại Events

        private void LoadSinhVatGayHaiContent()
        {
            MainContent.Children.Clear();
            _sinhVatGayHaiView = new SinhVatGayHaiView();
            MainContent.Children.Add(_sinhVatGayHaiView);
        }

        private void BtnDanhMucSVGH_Click(object sender, RoutedEventArgs e) => _sinhVatGayHaiView?.LoadSinhVatGayHai();
        private void BtnCapNhatSVGH_Click(object sender, RoutedEventArgs e) => _sinhVatGayHaiView?.LoadCapNhatSVGH();
        private void BtnTuoiSau_Click(object sender, RoutedEventArgs e) => _sinhVatGayHaiView?.LoadTuoiSau();

        #endregion

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                new LoginWindow().Show();
                this.Close();
            }
        }

        private void Sidebar_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(_username);
            changePasswordWindow.ShowDialog();
        }
    }
}

