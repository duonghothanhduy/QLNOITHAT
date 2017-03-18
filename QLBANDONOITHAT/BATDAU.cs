using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QLBANDONOITHAT.LQTOSQL;

namespace QLBANDONOITHAT
{
    public partial class BATDAU : System.Windows.Forms.Form
    {
        public BATDAU()
        {
            InitializeComponent();
            txt_dangnhap.Visible = false;
            txtpass.Visible = false;
            bt_dangnhap.Visible = false;
            lb_thongbao.Visible = false;
            tli_dangnhap.Visible = false;
            lb_user.Visible = false;
            lb_pass.Visible = false;
        }

        //khai bao database
        dbQLNOITHATDataContext data = new dbQLNOITHATDataContext();

        // vị bang~ chọn chức năng hình tròn
        private void tileItem11_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            radialMenu1.ShowPopup(new Point(675, 370));
        }

        //mỡ form bán hàng
        private void tileItem4_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            BANHANG f = new BANHANG();
            f.Show();
        }

        //mỡ form mặt hàng
        private void tileItem14_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (phanquyen == 2)
            {
                form_mathang f = new form_mathang();
                f.Show();
            }
            else
            {
                XtraMessageBox.Show("Bạn Không Được Cấp Quyền Để Truy Cập Trang Này!", "Quyền Sử Dụng", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        // đóng form menu
        private void tileItem19_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            this.Close();
        }

        //thay đổi đinh dạng mật khẩu
        private void txtpass_TextChanged(object sender, EventArgs e)
        {
            this.txtpass.Properties.PasswordChar = '*';
        }

        // code nút đăng nhập
        private void tileItem20_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            txt_dangnhap.Visible = true;
            txtpass.Visible = true;
            bt_dangnhap.Visible = true;
            lb_thongbao.Visible = false;
            tli_dangnhap.Visible = true;
            lb_user.Visible = true;
            lb_pass.Visible = true;
        }

        //code nhập thông tin đăng nhập
        public int t = 0;
        public int phanquyen=0;
        public static string manv = "";
        private void bt_dangnhap_Click_1(object sender, EventArgs e)
        {
             string user = txt_dangnhap.Text;
            string matkhau = txtpass.Text;

            LOGIN tam = data.LOGINs.SingleOrDefault(a => a.TENDN == user && a.PASS == matkhau);

            

            if(tam==null)
            {
                t = 0;
            }
            else
            {
                t = 1;
            }

            if(t==0)
            {
                lb_thongbao.Visible = true;
                lb_thongbao.Text = "Sai Tên Đăng Nhập hoặc Mật Khẩu !";
                txt_dangnhap.Text="";
                txtpass.Text="";
            }
          
            if(t==1)
            {
                    manv = tam.MANV;
                    phanquyen = int.Parse(tam.PHANQUYEN.ToString());
                    txt_dangnhap.Visible = false;
                    txtpass.Visible = false;
                    bt_dangnhap.Visible = false;
                    lb_thongbao.Visible = false;
                    lb_user.Visible = false;
                    lb_pass.Visible = false;
                    string[] chuoi = tam.GHICHU.Split('-');
                    for (int i = 0; i < chuoi.Length;i++ )
                    {
                        tli_dangnhap.Text += chuoi[i] +"\n";
                    }
                    
                    tli_nutdangnhap.Text = "Đăng Xuất";
            }
        }

        private void tileItem16_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (phanquyen==2)
            {
                FORM_KHACHHANG f = new FORM_KHACHHANG();
                f.Show();
            }
            else
            {
                XtraMessageBox.Show("Bạn Không Được Cấp Quyền Để Truy Cập Trang Này!", "Quyền Sử Dụng", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }



    }


    
}
