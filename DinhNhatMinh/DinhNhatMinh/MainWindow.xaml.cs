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
using DinhNhatMinh.Models;
using System.Text.RegularExpressions;
using System.Reflection;

namespace DinhNhatMinh

{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        QlsachContext db = new QlsachContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiDL();
            HienThiCB();
        }
        private void HienThiCB()
        {
            var query = from t in db.Tacgia
                        select t;
            cboTG.ItemsSource = query.ToList();
            cboTG.DisplayMemberPath = "Tentacgia";
            cboTG.SelectedValuePath = "Matg";
            cboTG.SelectedIndex = 0;    
        }
        private List<TT> LayDL()
        {
            var query = from t in db.Saches
                        where t.Sotrang >= 0
                        orderby t.Sotrang descending
                        select new TT
                        {
                            MaSach = t.Masach,
                            TenSach = t.Tensach,
                            MaTG = t.Matg,
                            NamXB = t.Namxuatban,
                            SoTrang = t.Sotrang,
                            TongTien = t.Sotrang * 80000

                        };
            return query.ToList<TT>();
        }
        private void HienThiDL()
        {
            dgv_dsSach.ItemsSource = LayDL();
        }

        private void dgv_dsSach_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(dgv_dsSach.SelectedItem != null)
            {
                try
                {
                    Type t = dgv_dsSach.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txtMaSach.Text = p[0].GetValue(dgv_dsSach.SelectedValue).ToString();
                    txtTenSach.Text = p[1].GetValue(dgv_dsSach.SelectedValue).ToString();
                    txtNamXB.Text = p[2].GetValue(dgv_dsSach.SelectedValue).ToString();
                    txtSotrang.Text = p[3].GetValue(dgv_dsSach.SelectedValue).ToString();
                    cboTG.SelectedValue = p[4].GetValue(dgv_dsSach.SelectedValue).ToString();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi:" + ex.Message,"Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            var query = db.Saches.SingleOrDefault(t=> t.Masach.Equals(txtMaSach.Text));
            if(query != null)
            {
                MessageBox.Show("Đã tồn tại mã sách "+txtMaSach.Text, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                Sach a = new Sach();
                a.Masach = txtMaSach.Text;
                a.Tensach = txtTenSach.Text;
                a.Sotrang = int.Parse(txtSotrang.Text);
                a.Namxuatban = int.Parse(txtNamXB.Text);
                Tacgium dm = (Tacgium)cboTG.SelectedItem;
                a.Matg = dm.Matg;
                db.Saches.Add(a);
                db.SaveChanges();
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButton.OK);
                HienThiDL();
            }
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            var query = db.Saches.SingleOrDefault(t => t.Masach==txtMaSach.Text);
            if(query != null)
            {
                query.Tensach = txtTenSach.Text;
                query.Sotrang = int.Parse(txtSotrang.Text);
                query.Namxuatban = int.Parse(txtNamXB.Text);
                query.Matg = ((Tacgium)cboTG.SelectedItem).Matg;
                db.Saches.Add(query);
                HienThiDL();
            } else
            {
                MessageBox.Show("Mã " + txtMaSach.Text+" không tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            var query = db.Saches.SingleOrDefault(t => t.Masach.Equals(txtMaSach.Text));
            if (query != null)
            {
                db.Saches.Remove(query);
                db.SaveChanges();
                HienThiDL();
            }
            else
            {
                MessageBox.Show("Mã sách " + txtMaSach.Text+" không tồn tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}