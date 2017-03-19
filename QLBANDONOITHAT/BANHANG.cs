using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using QLBANDONOITHAT.LQTOSQL;
using System.Data.SqlClient;

namespace QLBANDONOITHAT
{
    public partial class BANHANG : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        Class_banhang BH;
        public BANHANG()
        {
            InitializeComponent();
            BH = new Class_banhang();
            cbb_tenmh.Enabled = false;
            cbb_sl.Enabled = false;
            BH.loaddlcbb(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //LOAD DU LIEU KHI NHAN VIEN DAT HANG CHO KHÁCH
        private void bt_chon_Click(object sender, EventArgs e)
        {
            BH.insertcthd_hd(this, cbb_tenmh.Text, cbb_sl.Text);
        }

        //đống form
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //xóa sản phẩm đã chọn
        private void button2_Click(object sender, EventArgs e)
        {
            BH.xoa(this);
        }

        //truy van ten khach hang theo so dien thoai khi nhap vao
        private void textEdit2_EditValueChanged(object sender, EventArgs e)
        {
            BH.truyvantenkh(this, txt_sdt.Text);
        }

        //mỡ form thêm khách hàng
        public static string tenkh="";
        public static string sdtkh = "";
        private void label2_Click(object sender, EventArgs e)
        {
            tenkh = txt_tenkh.Text;
            sdtkh = txt_sdt.Text;
            THEMKH f = new THEMKH();
            f.Show();
        }

        private void bt_xacnhan_Click(object sender, EventArgs e)
        {
            BH.capshd(txt_tenkh.Text,txt_sdt.Text);
            cbb_tenmh.Enabled = true;
            cbb_sl.Enabled = true;

        }

        private void txt_khachtra_EditValueChanged(object sender, EventArgs e)
        {
            BH.tienthua(this, txt_khachtra.Text);
        }

  

 
    }
}