using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DevExpress.XtraGrid;
using QLBANDONOITHAT.LQTOSQL;
using System.IO;

namespace QLBANDONOITHAT
{
    public partial class form_mathang : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        class_MatHang MH;

        public int i = 0;
        public form_mathang()
        {
            InitializeComponent();
            MH = new class_MatHang();
            //load du lieu len form
            MH.loaddulieu(this);
            //CAP MA TU DONG
            MH.capmatudong(this);
            //LOAD DU LIEU VAO CBB
            MH.loadcbb(this);
            //tim theo loai
           
        }

        //DONG' FORM
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       //lay gia tri khi chọn vào radiobox
        public string donvitinh = "";
        private void rdb_cai_CheckedChanged(object sender, EventArgs e)
        {
            donvitinh = "Cai";
        }

        private void rdb_m2_CheckedChanged(object sender, EventArgs e)
        {
            donvitinh = "m2";
        }


        //NÚT THÊM
        private void bt_them_Click_1(object sender, EventArgs e)
        {
            //lấy hàm kiểm tra từ class_Checker
            if (Class_Checker.ktrthem_MATHANG(txt_tenmh.Text,cbb_loai.Text,donvitinh, txt_noisx.Text) == true)
            {
                MH.them(this, txt_tenmh.Text, cbb_loai.Text,txt_noisx.Text);
                txt_tenmh.Text = "";
                txt_noisx.Text = "";
                cbb_loai.Text = "";
                rdb_cai.Checked = false;
                rdb_m2.Checked = false;
            }
            
        }

        //xoa dong da chon 
        private void button2_Click(object sender, EventArgs e)
        {
            MH.xoa(this);
        }

        //tự dộng cập nhật dữ liệu khi sửa trực tiếp trên Gridcontrol
        private void gv_mathang_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            MH.sua(this);
        }

        //LOAD DU LIÊU TIM THEO TÊN
        private void bt_tiemkiemten_Click(object sender, EventArgs e)
        {
            MH.timtheoten(this, cbb_timtheoten.Text);
        }

       //load dữ liệu theo nơi sản xuất
       private void bt_timtheonoisanxuat_Click(object sender, EventArgs e)
       {
           MH.timtheonsx(this, cbb_noisx.Text);
       }
        
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_xuatfile_Click(object sender, EventArgs e)
        {
            MH.xuatfile(this);
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            MH.timsoluongtren10(this);
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            MH.timsoluongnho10(this);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            MH.timsoluonghh(this);
        }

        private void rdb_tu_CheckedChanged(object sender, EventArgs e)
        {
            MH.timtheoloai(this, "Tủ");
        }

        private void rdb_giuong_CheckedChanged(object sender, EventArgs e)
        {
            MH.timtheoloai(this, "Giường");
        }

        private void rdb_ban_CheckedChanged(object sender, EventArgs e)
        {
            MH.timtheoloai(this, "Bàn");
        }

        private void rdb_ghe_CheckedChanged(object sender, EventArgs e)
        {
            MH.timtheoloai(this, "Ghế");
        }

    }
}