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
using QLBANDONOITHAT.LQTOSQL;

namespace QLBANDONOITHAT
{
    public partial class THEMKH : DevExpress.XtraEditors.XtraForm
    {
        public THEMKH()
        {
            InitializeComponent();
            lb_tenkh.Text = BANHANG.tenkh;
            lb_sdt.Text = BANHANG.sdtkh;
        }
        public int ktr = 0;
        //DONG FORM THI BẤM THÊM
        private void button3_Click(object sender, EventArgs e)
        {
            if (ktr == 0)
            {
                Them();
                ktr = 1;
            }
            if(ktr==1)
                this.Close();
        }

        //THEM DỮ LIỆU VÀO SQL
        dbQLNOITHATDataContext data = new dbQLNOITHATDataContext();
        public void Them()
        {
            KHACHHANG M = new KHACHHANG();

            CAPMA CM = data.CAPMAs.SingleOrDefault(a => a.STT == 2);
            int kiso = (Int32)CM.KISO+1;
            M.MAKH = CM.KIHIEU + Convert.ToString(kiso.ToString());
            M.TENKH = lb_tenkh.Text;

            if (rdb_nam.Checked==true)
                M.GIOITINH_KH = "Nam";
            if (rdb_nu.Checked==true)
                M.GIOITINH_KH = "Nữ";
            M.DIENTHOAI_KH = lb_sdt.Text;
            M.DIACHI_KH = txt_diachi.Text; ;

            //cap nhat du lieu bang mat hang
            data.KHACHHANGs.InsertOnSubmit(M);
            data.SubmitChanges();

            //cap nhat lại dữ liểu bảng cấp mã
            CM.KISO = kiso;
            data.SubmitChanges();
        }
    }
}