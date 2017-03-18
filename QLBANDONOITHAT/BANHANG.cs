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
        public BANHANG()
        {
            InitializeComponent();
            cbb_tenmh.Enabled = true;
            cbb_sl.Enabled = true;
            loaddlcbb();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //load dữ liệu vào cbb tìm theo tên hàng
        string chuoiketnoi = @"Data Source=DESKTOP-DFC2SM5\DUY_MSSQL;Initial Catalog=QL_NOITHAT;User ID=sa;Password=Backhome1";
        SqlDataAdapter adapter;
        SqlConnection connec;
        DataTable datatable;

        public void loaddlcbb()
        {
            connec = new SqlConnection();
            connec.ConnectionString = chuoiketnoi;
            connec.Open();
            adapter = new SqlDataAdapter("select TENMH from MATHANG", connec);
            datatable = new DataTable();
            adapter.Fill(datatable);
            cbb_tenmh.DataSource = datatable;
            cbb_tenmh.DisplayMember = "TENMH";
            cbb_tenmh.ValueMember = "TENMH";
            cbb_tenmh.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbb_tenmh.AutoCompleteSource = AutoCompleteSource.ListItems;
            adapter.Dispose();
            connec.Close();
        }

        //ham cap nhat tong tri gia
        public void capnhattongtrigia(string m)
        {
            var sql2 = data.CTHDs;
            foreach (var item in sql2)
            {
                if (item.SOHD == sohd && item.MAMH == m)
                {
                    tongtrigia += (double)(item.DONGIA * item.SOLUONGMUA);
                }
            }
            lb_tongdongia.Text = Convert.ToString(tongtrigia);

        }


        //hàm load dữ liệu
        public string[] dschon = new string[100];
        public int[] soluongchon = new int[100];
        public int i = 0;
        public double tongtrigia = 0;
        public double khuyenmai = 0;
        dbQLNOITHATDataContext data= new dbQLNOITHATDataContext();
        public void loaddulieu()
        {
            List<Class_banhang> dsmh = new List<Class_banhang>();
            var sql_banhang = data.CTHDs;
            BindingSource bd = new BindingSource();
            foreach (var sql in sql_banhang)
            {

                if (sql.SOHD == sohd)
                {
                    Class_banhang tam = new Class_banhang();

                    //khai bao biến tạm chứa dữ liệu từng thuộc tính từ sql qua
                    tam.mamh = sql.MAMH;
                    MATHANG TAM = data.MATHANGs.SingleOrDefault(a => a.MAMH == sql.MAMH);
                    if (TAM != null)
                    {
                        tam.tenmh = TAM.TENMH;
                    }
                    tam.Giaban = (double)sql.DONGIA;
                    tam.Soluong = (Int32)sql.SOLUONGMUA;
                    dsmh.Add(tam);
                }
            }

            foreach (Class_banhang k in dsmh)
            {
                bd.Add(k);
            }

            //do du lieu ra gridcontrol
            gctrl_banhang.DataSource = bd;
                          
        }

        //cap so hoa don
        public string sohd="";
        public void capshd()
        {
            CAPMA tam = data.CAPMAs.SingleOrDefault(a => a.STT == 7);
            sohd = tam.KIHIEU + Convert.ToString(tam.KISO + 1);
            HOADON A = new HOADON();
            A.SOHD = sohd;
            A.NGAYLAP_HD = DateTime.Now;
            A.MANV = BATDAU.manv;
            A.TONGTG = 0;
            A.TT_THANHTOAN = "";
            KHACHHANG K = data.KHACHHANGs.SingleOrDefault(a=>a.TENKH==txt_tenkh.Text && a.DIENTHOAI_KH==txt_sdt.Text);
            if(K!=null)
            {
                A.MAKH = K.MAKH;
            }
            data.HOADONs.InsertOnSubmit(A);
            data.SubmitChanges();
            tam.KISO = tam.KISO + 1;
            data.SubmitChanges();
        }

        //LOAD DU LIEU KHI NHAN VIEN DAT HANG CHO KHÁCH
        public string mamathang = "";
        private void bt_chon_Click(object sender, EventArgs e)
        {

            CTHD C = new CTHD();
            C.SOHD = sohd;
            MATHANG MH = data.MATHANGs.SingleOrDefault(a => a.TENMH == cbb_tenmh.Text);
            if(MH!=null)
            {
                mamathang = MH.MAMH;
                C.MAMH = MH.MAMH;
                C.DONGIA = MH.GIABAN;
            }
            C.SOLUONGMUA = int.Parse(cbb_sl.Text);
            C.THUEVAT=10;

            //cap nhat tong tri gia
            tongtrigia += (double)((int.Parse(cbb_sl.Text)*C.DONGIA));
            khuyenmai +=(double)(((int.Parse(cbb_sl.Text)*C.DONGIA))*C.KH)
            data.CTHDs.InsertOnSubmit(C);
            data.SubmitChanges();

            

            int kt = 0;
            //load dữ liêu mặt hàng được chọn
            if(cbb_sl.Text=="")
            {
                XtraMessageBox.Show("Bạn Chưa Chọn Số Lượng Sản Phẩm!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                kt = 1;
            }
            if (kt == 0)
            {
                loaddulieu();
                lb_tongdongia.Text = Convert.ToString(tongtrigia);
            }
        }

        //đống form
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //xóa sản phẩm đã chọn
        private void button2_Click(object sender, EventArgs e)
        {
            int donghientai = gv_banhang.FocusedRowHandle;
            string mamh = gv_banhang.GetRowCellValue(donghientai, "Mamh").ToString();
            var sql = data.CTHDs;
            foreach(var item in sql)
            {
                if(item.SOHD==sohd && item.MAMH==mamh)
                {
                    tongtrigia = tongtrigia - (double)(item.DONGIA * item.SOLUONGMUA);
                    data.CTHDs.DeleteOnSubmit(item);
                    data.SubmitChanges();
                    loaddulieu();
                    lb_tongdongia.Text = Convert.ToString(tongtrigia);
                }
            }
        }

        private void textEdit2_EditValueChanged(object sender, EventArgs e)
        {
            int ktr = 0;
            var sql_khachhang = data.KHACHHANGs;
            foreach(var item in sql_khachhang)
            {
                if(item.DIENTHOAI_KH== txt_sdt.Text)
                {
                    txt_tenkh.Text = item.TENKH;
                    ktr = 1;
                }
                
            }
            if(ktr==0)
            {
                txt_tenkh.Text = "";
            }
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
            capshd();
            cbb_tenmh.Enabled = true;
            cbb_sl.Enabled = true;

        }

  

 
    }
}