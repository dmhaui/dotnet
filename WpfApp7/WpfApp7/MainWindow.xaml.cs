using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Reflection;
using WpfApp7.Models;
using System.Globalization;
using dm;

namespace WpfApp7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        QlduAn05Context db = new QlduAn05Context();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiDL();
        }
        private List<TT> LayDL()
        {
           var query = from t in db.NhanViens
                       where t.HeSoLuong >= 0
                        orderby t.HeSoLuong descending
                        select new TT
                        {
                            MaNV = t.MaNv,
                          HoTen = t.HoTen,
                            NgayTD = t.NgayTuyenDung,
                            HeSoLuong = (float?)t.HeSoLuong
                        };
            return query.ToList<TT>();
        }
        private void HienThiDL()
        {
            dgv_dsNhanVien.ItemsSource = LayDL();
        }


        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            var query = db.NhanViens.SingleOrDefault(t => t.MaNv.Equals(txtMaNV.Text));
            if (query != null)
            {
                MessageBox.Show("Đã tồn tại mã nhân viên " + txtMaNV.Text + " trong dữ liệu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                NhanVien nv = new NhanVien();
                nv.MaNv = txtMaNV.Text;
                nv.HoTen = txtHoTen.Text;
                nv.NgayTuyenDung = dpkNgayTD.SelectedDate.HasValue ? dpkNgayTD.SelectedDate.Value.ToString() : null;
                nv.HeSoLuong = float.Parse(txtHeSoLuong.Text, CultureInfo.InvariantCulture.NumberFormat);
                db.NhanViens.Add(nv);
                db.SaveChanges();

                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButton.OK);
                HienThiDL();
            }
        }

        private void dgv_dsNhanVien_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(dgv_dsNhanVien.SelectedItem != null)
            {
                try
                {
                    Type t = dgv_dsNhanVien.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txtMaNV.Text = p[0].GetValue(dgv_dsNhanVien.SelectedValue).ToString();
                    txtHoTen.Text = p[1].GetValue(dgv_dsNhanVien.SelectedValue).ToString();
                    if (DateTime.TryParse(p[2].GetValue(dgv_dsNhanVien.SelectedValue).ToString(), out DateTime ngayTD))
                    {
                        dpkNgayTD.SelectedDate = ngayTD;
                    }
                    else
                    {
                        // Xử lý khi giá trị không thể chuyển đổi
                        MessageBox.Show("Lỗi chuyển đổi ngày tuyển dụng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    txtHeSoLuong.Text = p[3].GetValue(dgv_dsNhanVien.SelectedValue).ToString();



                } catch (Exception ex)
                {
                    MessageBox.Show("Lỗi:" + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgv_dsNhanVien.SelectedItem != null)
            {
                try
                {
                    // Lấy mã nhân viên từ dòng được chọn
                    string maNV = (dgv_dsNhanVien.SelectedItem as TT)?.MaNV;

                    // Xác nhận người dùng muốn xoá
                    MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn xoá nhân viên có mã {maNV} không?", "Xác nhận xoá", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Thực hiện xoá trong cơ sở dữ liệu
                        var nvToDelete = db.NhanViens.SingleOrDefault(t => t.MaNv.Equals(maNV));
                        if (nvToDelete != null)
                        {
                            db.NhanViens.Remove(nvToDelete);
                            db.SaveChanges();

                            // Cập nhật hiển thị trên DataGrid
                            HienThiDL();

                            MessageBox.Show("Xoá thành công", "Thông báo", MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show($"Không tìm thấy nhân viên có mã {maNV}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi:" + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xoá", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        // Phương thức thống kê
        public List<NhanVienThongKe> ThongKeNhanVien()
        {
            var query = from nhanVien in db.NhanViens
                        join thamGia in db.ThamGia on nhanVien.MaNv equals thamGia.MaNhanVien
                        group nhanVien by nhanVien.MaNv into g
                        where g.Count() > 1
                        select new NhanVienThongKe
                        {
                            MaNV = g.Key,
                            HoTen = g.First().HoTen,
                            NgayTD = g.First().NgayTuyenDung,
                            HeSoLuong = (float?)g.First().HeSoLuong,
                            SoDuAnThamGia = g.Count()
                        };

            return query.ToList();
        }

        private void btnThongKe_Click(object sender, RoutedEventArgs e)
        {

            var danhSachThongKe = ThongKeNhanVien();


            Window2 thongKeWindow = new Window2(danhSachThongKe);


            thongKeWindow.ShowDialog();
        }
    }
}