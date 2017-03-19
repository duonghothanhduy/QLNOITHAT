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
        Class_dangnhap DN;
        public BATDAU()
        {
            InitializeComponent();
            DN = new Class_dangnhap();
            txt_dangnhap.Visible = false;
            txtpass.Visible = false;
            bt_dangnhap.Visible = false;
            lb_thongbao.Visible = false;
            tli_dangnhap.Visible = false;
            lb_user.Visible = false;
            lb_pass.Visible = false;
        }

        // vị tri bang~ chọn chức năng hình tròn
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
            if (DN.capphanquyen() == 2)
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
        private void bt_dangnhap_Click_1(object sender, EventArgs e)
        {
            DN.dangnhap(this,txt_dangnhap.Text,txtpass.Text);
        }

        private void tileItem16_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (DN.capphanquyen()==2)
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
