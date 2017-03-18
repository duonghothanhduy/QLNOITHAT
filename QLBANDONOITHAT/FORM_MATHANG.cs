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
        public int i = 0;
        public form_mathang()
        {
            InitializeComponent();
            loaddulieu();
            dulieucombten();
            dulieucombsx();

            cbb_timtheoten.Text = "";
            //mã mặt hàng tự động
            CAPMA CM = data.CAPMAs.SingleOrDefault(a => a.STT == 1);
            i = ((Int32)CM.KISO) + 1;
            lb_mamh.Text = CM.KIHIEU + Convert.ToString(i);
        }

      
        public void capnhatsl()
        {
            var sql_mathang = data.MATHANGs;
            foreach(var item in sql_mathang)
            {
                //lay so luong tu CTPN
                int slnhap = 0;
                var sql_CTPN = data.CTPNs;
                foreach (var sql in sql_CTPN)
                {
                    if (sql.MAMH == item.MAMH)
                    {
                        slnhap += (Int32)sql.SLNHAP;
                    }
                }
                //lay so luong tu CHITIETKHO
                int slkho = 0;
                var sql_CTKHO = data.CHITIETKHOs;
                foreach (var sql in sql_CTKHO)
                {
                    if (sql.MAMH == item.MAMH)
                    {
                        slkho += (Int32)sql.SOLUONG_TONKHO;
                    }
                }
                //tính tổng số lượng hiện có của mặt hàng
                data.SLTON((int)slkho, (int)slnhap, item.MAMH);
                
            }
        }

        //DONG' FORM
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //LOAD DỮ LIỆU MẶT HÀNG
        dbQLNOITHATDataContext data = new dbQLNOITHATDataContext();
        public void loaddulieu()
        {
            capnhatsl();
            List<class_MatHang> dsmh = new List<class_MatHang>();

            //LAY DU LIEU TRONG SQL GÁN VÀO BIẾN sql_mh
            var sql_mh = data.MATHANGs;

            BindingSource bd = new BindingSource();

            foreach (var sql in sql_mh)
            {
                class_MatHang tam = new class_MatHang();

                //khai bao biến tạm chứa dữ liệu từng thuộc tính từ sql qua

                tam.mamh = sql.MAMH;
                tam.tenmh = sql.TENMH;
                tam.loai = sql.MALOAI;
                tam.noisx = sql.NOISX;
                tam.dvt = sql.DVT;
                tam.Giaban = (double)sql.GIABAN;
                tam.Giamua = (double)sql.GIAMUA;
                tam.Khuyenmai = (double)sql.KHUYENMAI;
                tam.Slt = (Int32)sql.SLTON;
                dsmh.Add(tam);
            }
            foreach (class_MatHang k in dsmh)
            {
                bd.Add(k);
            }

            //do du lieu ra gridcontrol
            gctrl_mathang.DataSource = bd;

        }

        //lay du lieu tu form dua vao sql khi thêm
        public void Them()
        {
            MATHANG M = new MATHANG();
            CAPMA CM = data.CAPMAs.SingleOrDefault(a => a.STT == 1);

            M.MAMH = lb_mamh.Text;
            M.TENMH = txt_tenmh.Text;

            if(cbb_loai.SelectedItem.ToString()=="Tủ")
                M.MALOAI = "LH1";
            if(cbb_loai.SelectedItem.ToString()=="Giường")
                M.MALOAI = "LH2";
            if(cbb_loai.SelectedItem.ToString()=="Bàn")
                M.MALOAI = "LH3";
            if(cbb_loai.SelectedItem.ToString()=="Ghế")
                M.MALOAI = "LH4";
            if (rdb_cai.Checked == true)
                M.DVT = "Cái";
            if (rdb_m2.Checked == true)
                M.DVT = "m2";
            M.NOISX = txt_noisx.Text;

            M.GIABAN = 0;
            M.GIAMUA = 0;
            M.KHUYENMAI = 0;
            M.SLTON = 0;

            //cap nhat du lieu bang mat hang
            data.MATHANGs.InsertOnSubmit(M);
            data.SubmitChanges();

            //cap nhat lại dữ liểu bảng cấp mã
            CM.KISO = i;
            data.SubmitChanges();
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
                Them();
                txt_tenmh.Text = "";
                txt_noisx.Text = "";
                cbb_loai.Text = "";
                rdb_cai.Checked = false;
                rdb_m2.Checked = false;
                loaddulieu();
            }
            
        }


        //lấy mã sản phẩm sau khi click chuột vào 1 dòng bất kì
        //public string mamh = "";
        private void button2_Click(object sender, EventArgs e)
        {
            string mamh = "";
            int donghientai = gv_mathang.FocusedRowHandle;
            mamh = gv_mathang.GetRowCellValue(donghientai, "Mamh").ToString();
            DialogResult thongbao;
            thongbao=XtraMessageBox.Show("Bạn có thực sự muốn xóa mặt hàng này ?", "thongbao",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(DialogResult.Yes==thongbao)
            {
                var sql_mathang = data.MATHANGs;
                foreach (var k in sql_mathang)
                {
                    if (k.MAMH == mamh)
                    {
                        data.MATHANGs.DeleteOnSubmit(k);
                        data.SubmitChanges();
                    }
                }
                loaddulieu();
            }
        }

        //tự dộng cập nhật dữ liệu khi sửa trực tiếp trên Gridcontrol
        public int dongdachon = 0;
        private void gv_mathang_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            string mamh = "";
            int donghientai = gv_mathang.FocusedRowHandle;
            if (donghientai != dongdachon)
            {
                mamh = gv_mathang.GetRowCellValue(donghientai, "Mamh").ToString();
                MATHANG tam = data.MATHANGs.SingleOrDefault(a => a.MAMH == mamh);
                if (tam != null)
                {
                    tam.TENMH = gv_mathang.GetRowCellValue(donghientai, "Tenmh").ToString();
                    tam.MALOAI = gv_mathang.GetRowCellValue(donghientai, "Loai").ToString();
                    tam.NOISX = gv_mathang.GetRowCellValue(donghientai, "Noisx").ToString();
                    tam.DVT = gv_mathang.GetRowCellValue(donghientai, "Dvt").ToString();
                    tam.GIABAN = (double)gv_mathang.GetRowCellValue(donghientai, "Giaban");
                    tam.GIAMUA = (double)gv_mathang.GetRowCellValue(donghientai, "Giamua");
                    tam.KHUYENMAI = (double)gv_mathang.GetRowCellValue(donghientai, "Khuyenmai");
                    tam.SLTON = (int)gv_mathang.GetRowCellValue(donghientai, "Slt");
                    data.SubmitChanges();
                    loaddulieu();
                    dongdachon = donghientai;
                }
            }
        }

        //khai bao chuoi~ ket noi dữ liệu
        string chuoiketnoi = @"Data Source=DESKTOP-DFC2SM5\DUY_MSSQL;Initial Catalog=QL_NOITHAT;User ID=sa;Password=Backhome1";
        SqlDataAdapter adapter;
        SqlConnection connec;
        DataTable datatable;
        //LOAD DU LIÊU TIM THEO TÊN
        private void bt_tiemkiemten_Click(object sender, EventArgs e)
        {
            List<class_MatHang> dsmh = new List<class_MatHang>();

            //LAY DU LIEU TRONG SQL GÁN VÀO BIẾN sql_mh
            var sql_mh = data.MATHANGs;

            BindingSource bd = new BindingSource();

            foreach (var sql in sql_mh)
            {
                class_MatHang tam = new class_MatHang();
                if (sql.TENMH == cbb_timtheoten.Text)
                {
                    //khai bao biến tạm chứa dữ liệu từng thuộc tính từ sql qua

                    tam.mamh = sql.MAMH;
                    tam.tenmh = sql.TENMH;
                    tam.loai = sql.MALOAI;
                    tam.noisx = sql.NOISX;
                    tam.dvt = sql.DVT;
                    tam.Giaban = (double)sql.GIABAN;
                    tam.Giamua = (double)sql.GIAMUA;
                    tam.Khuyenmai = (double)sql.KHUYENMAI;
                    tam.Slt = (Int32)sql.SLTON;
                    dsmh.Add(tam);
                }
            }
            foreach (class_MatHang k in dsmh)
            {
                bd.Add(k);
            }

            //do du lieu ra gridcontrol
            gctrl_mathang.DataSource = bd;
        }

        //đưa dữ liệu vào combobox VÀO COMBOBOX HIEN RA DANH SACH
       public void dulieucombten()
        {
            connec = new SqlConnection();
            connec.ConnectionString = chuoiketnoi;
            connec.Open();
            adapter = new SqlDataAdapter("select TENMH from MATHANG", connec);
            datatable = new DataTable();
            adapter.Fill(datatable);
            cbb_timtheoten.DataSource = datatable;
            cbb_timtheoten.DisplayMember = "TENMH";
            cbb_timtheoten.ValueMember = "TENMH";
            cbb_timtheoten.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbb_timtheoten.AutoCompleteSource = AutoCompleteSource.ListItems;
            adapter.Dispose();
            connec.Close();
            cbb_timtheoten.Text = "";
        }
        //ĐƯA DỮ LIỆU VÀO CBB nơi sản xuất
       public void dulieucombsx()
       {
           connec = new SqlConnection();
           connec.ConnectionString = chuoiketnoi;
           connec.Open();
           adapter = new SqlDataAdapter("select NOISX from MATHANG", connec);
           datatable = new DataTable();
           adapter.Fill(datatable);
           cbb_noisx.DataSource = datatable;
           cbb_noisx.DisplayMember = "NOISX";
           cbb_noisx.ValueMember = "NOISX";
           cbb_noisx.AutoCompleteMode = AutoCompleteMode.Suggest;
           cbb_noisx.AutoCompleteSource = AutoCompleteSource.ListItems;
           adapter.Dispose();
           connec.Close();
           cbb_noisx.Text = "";
       }
        //load dữ liệu theo nơi sản xuất
       private void bt_timtheonoisanxuat_Click(object sender, EventArgs e)
       {
           List<class_MatHang> dsmh = new List<class_MatHang>();

           //LAY DU LIEU TRONG SQL GÁN VÀO BIẾN sql_mh
           var sql_mh = data.MATHANGs;

           BindingSource bd = new BindingSource();

           foreach (var sql in sql_mh)
           {
               class_MatHang tam = new class_MatHang();
               if (sql.TENMH == cbb_noisx.Text)
               {
                   //khai bao biến tạm chứa dữ liệu từng thuộc tính từ sql qua

                   tam.mamh = sql.MAMH;
                   tam.tenmh = sql.TENMH;
                   tam.loai = sql.MALOAI;
                   tam.noisx = sql.NOISX;
                   tam.dvt = sql.DVT;
                   tam.Giaban = (double)sql.GIABAN;
                   tam.Giamua = (double)sql.GIAMUA;
                   tam.Khuyenmai = (double)sql.KHUYENMAI;
                   tam.Slt = (Int32)sql.SLTON;
                   dsmh.Add(tam);
               }
           }
           foreach (class_MatHang k in dsmh)
           {
               bd.Add(k);
           }

           //do du lieu ra gridcontrol
           gctrl_mathang.DataSource = bd;
       }

        
        //LOAD DU LIEU TIM KIẾM THEO LOẠI
        public string loai = "";
        private void rdb_tu_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_tu.Checked == true)
            {
                loai = "LH1";
                List<class_MatHang> dsmh = new List<class_MatHang>();

                //LAY DU LIEU TRONG SQL GÁN VÀO BIẾN sql_mh
                var sql_mh = data.MATHANGs;

                BindingSource bd = new BindingSource();

                foreach (var sql in sql_mh)
                {
                    class_MatHang tam = new class_MatHang();
                    if (sql.MALOAI == loai)
                    {
                        //khai bao biến tạm chứa dữ liệu từng thuộc tính từ sql qua

                        tam.mamh = sql.MAMH;
                        tam.tenmh = sql.TENMH;
                        tam.loai = sql.MALOAI;
                        tam.noisx = sql.NOISX;
                        tam.dvt = sql.DVT;
                        tam.Giaban = (double)sql.GIABAN;
                        tam.Giamua = (double)sql.GIAMUA;
                        tam.Khuyenmai = (double)sql.KHUYENMAI;
                        tam.Slt = (Int32)sql.SLTON;
                        dsmh.Add(tam);
                    }
                }
                foreach (class_MatHang k in dsmh)
                {
                    bd.Add(k);
                }

                //do du lieu ra gridcontrol
                gctrl_mathang.DataSource = bd;
            }
        }

        private void rdb_giuong_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_giuong.Checked == true)
            {
                loai = "LH2";
                List<class_MatHang> dsmh = new List<class_MatHang>();

                //LAY DU LIEU TRONG SQL GÁN VÀO BIẾN sql_mh
                var sql_mh = data.MATHANGs;

                BindingSource bd = new BindingSource();

                foreach (var sql in sql_mh)
                {
                    class_MatHang tam = new class_MatHang();
                    if (sql.MALOAI == loai)
                    {
                        //khai bao biến tạm chứa dữ liệu từng thuộc tính từ sql qua

                        tam.mamh = sql.MAMH;
                        tam.tenmh = sql.TENMH;
                        tam.loai = sql.MALOAI;
                        tam.noisx = sql.NOISX;
                        tam.dvt = sql.DVT;
                        tam.Giaban = (double)sql.GIABAN;
                        tam.Giamua = (double)sql.GIAMUA;
                        tam.Khuyenmai = (double)sql.KHUYENMAI;
                        tam.Slt = (Int32)sql.SLTON;
                        dsmh.Add(tam);
                    }
                }
                foreach (class_MatHang k in dsmh)
                {
                    bd.Add(k);
                }

                //do du lieu ra gridcontrol
                gctrl_mathang.DataSource = bd;
            }
        }

        private void rdb_ban_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_ban.Checked == true)
            {
                loai = "LH3";
                List<class_MatHang> dsmh = new List<class_MatHang>();

                //LAY DU LIEU TRONG SQL GÁN VÀO BIẾN sql_mh
                var sql_mh = data.MATHANGs;

                BindingSource bd = new BindingSource();

                foreach (var sql in sql_mh)
                {
                    class_MatHang tam = new class_MatHang();
                    if (sql.MALOAI == loai)
                    {
                        //khai bao biến tạm chứa dữ liệu từng thuộc tính từ sql qua

                        tam.mamh = sql.MAMH;
                        tam.tenmh = sql.TENMH;
                        tam.loai = sql.MALOAI;
                        tam.noisx = sql.NOISX;
                        tam.dvt = sql.DVT;
                        tam.Giaban = (double)sql.GIABAN;
                        tam.Giamua = (double)sql.GIAMUA;
                        tam.Khuyenmai = (double)sql.KHUYENMAI;
                        tam.Slt = (Int32)sql.SLTON;
                        dsmh.Add(tam);
                    }
                }
                foreach (class_MatHang k in dsmh)
                {
                    bd.Add(k);
                }

                //do du lieu ra gridcontrol
                gctrl_mathang.DataSource = bd;
            }
        }

        private void rdb_ghe_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_tu.Checked == true)
            {
                loai = "LH4";
                List<class_MatHang> dsmh = new List<class_MatHang>();

                //LAY DU LIEU TRONG SQL GÁN VÀO BIẾN sql_mh
                var sql_mh = data.MATHANGs;

                BindingSource bd = new BindingSource();

                foreach (var sql in sql_mh)
                {
                    class_MatHang tam = new class_MatHang();
                    if (sql.MALOAI == loai)
                    {
                        //khai bao biến tạm chứa dữ liệu từng thuộc tính từ sql qua

                        tam.mamh = sql.MAMH;
                        tam.tenmh = sql.TENMH;
                        tam.loai = sql.MALOAI;
                        tam.noisx = sql.NOISX;
                        tam.dvt = sql.DVT;
                        tam.Giaban = (double)sql.GIABAN;
                        tam.Giamua = (double)sql.GIAMUA;
                        tam.Khuyenmai = (double)sql.KHUYENMAI;
                        tam.Slt = (Int32)sql.SLTON;
                        dsmh.Add(tam);
                    }
                }
                foreach (class_MatHang k in dsmh)
                {
                    bd.Add(k);
                }

                //do du lieu ra gridcontrol
                gctrl_mathang.DataSource = bd;
            }
        }


        //load du lieu theo tìm kiếm về số lượng
        public int soluong;
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked == true)
            {
                soluong = 10;
                List<class_MatHang> dsmh = new List<class_MatHang>();

                //LAY DU LIEU TRONG SQL GÁN VÀO BIẾN sql_mh
                var sql_mh = data.MATHANGs;

                BindingSource bd = new BindingSource();

                foreach (var sql in sql_mh)
                {
                    class_MatHang tam = new class_MatHang();
                    if (sql.SLTON > soluong)
                    {
                        //khai bao biến tạm chứa dữ liệu từng thuộc tính từ sql qua

                        tam.mamh = sql.MAMH;
                        tam.tenmh = sql.TENMH;
                        tam.loai = sql.MALOAI;
                        tam.noisx = sql.NOISX;
                        tam.dvt = sql.DVT;
                        tam.Giaban = (double)sql.GIABAN;
                        tam.Giamua = (double)sql.GIAMUA;
                        tam.Khuyenmai = (double)sql.KHUYENMAI;
                        tam.Slt = (Int32)sql.SLTON;
                        dsmh.Add(tam);
                    }
                }
                foreach (class_MatHang k in dsmh)
                {
                    bd.Add(k);
                }

                //do du lieu ra gridcontrol
                gctrl_mathang.DataSource = bd;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked == true)
            {
                soluong = 10;
                List<class_MatHang> dsmh = new List<class_MatHang>();

                //LAY DU LIEU TRONG SQL GÁN VÀO BIẾN sql_mh
                var sql_mh = data.MATHANGs;

                BindingSource bd = new BindingSource();

                foreach (var sql in sql_mh)
                {
                    class_MatHang tam = new class_MatHang();
                    if (sql.SLTON < soluong)
                    {
                        //khai bao biến tạm chứa dữ liệu từng thuộc tính từ sql qua

                        tam.mamh = sql.MAMH;
                        tam.tenmh = sql.TENMH;
                        tam.loai = sql.MALOAI;
                        tam.noisx = sql.NOISX;
                        tam.dvt = sql.DVT;
                        tam.Giaban = (double)sql.GIABAN;
                        tam.Giamua = (double)sql.GIAMUA;
                        tam.Khuyenmai = (double)sql.KHUYENMAI;
                        tam.Slt = (Int32)sql.SLTON;
                        dsmh.Add(tam);
                    }
                }
                foreach (class_MatHang k in dsmh)
                {
                    bd.Add(k);
                }

                //do du lieu ra gridcontrol
                gctrl_mathang.DataSource = bd;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
            {
                soluong = 0;
                List<class_MatHang> dsmh = new List<class_MatHang>();

                //LAY DU LIEU TRONG SQL GÁN VÀO BIẾN sql_mh
                var sql_mh = data.MATHANGs;

                BindingSource bd = new BindingSource();

                foreach (var sql in sql_mh)
                {
                    class_MatHang tam = new class_MatHang();
                    if (sql.SLTON ==soluong)
                    {
                        //khai bao biến tạm chứa dữ liệu từng thuộc tính từ sql qua

                        tam.mamh = sql.MAMH;
                        tam.tenmh = sql.TENMH;
                        tam.loai = sql.MALOAI;
                        tam.noisx = sql.NOISX;
                        tam.dvt = sql.DVT;
                        tam.Giaban = (double)sql.GIABAN;
                        tam.Giamua = (double)sql.GIAMUA;
                        tam.Khuyenmai = (double)sql.KHUYENMAI;
                        tam.Slt = (Int32)sql.SLTON;
                        dsmh.Add(tam);
                    }
                }
                foreach (class_MatHang k in dsmh)
                {
                    bd.Add(k);
                }

                //do du lieu ra gridcontrol
                gctrl_mathang.DataSource = bd;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_xuatfile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx |RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    switch (fileExtenstion)
                    {
                        case ".xls":
                            gv_mathang.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            gv_mathang.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            gv_mathang.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            gv_mathang.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            gv_mathang.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            gv_mathang.ExportToMht(exportFilePath);
                            break;
                        default:
                            break;
                    }

                    if (File.Exists(exportFilePath))
                    {
                        try
                        {
                            //Try to open the file and let windows decide how to open it.
                            System.Diagnostics.Process.Start(exportFilePath);
                        }
                        catch
                        {
                            String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                            MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}