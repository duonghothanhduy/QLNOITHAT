using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBANDONOITHAT.LQTOSQL;
using DevExpress.XtraEditors;
using System.IO;

namespace QLBANDONOITHAT
{
    class class_MatHang
    {
        Class_conext data;
        public class_MatHang()
        {
            data = new Class_conext();
        }

        // load du lieu len form
        public void loaddulieu(form_mathang f)
        {
            //cap nhat so luong
            capnhatsl();
            var sql = data.database().MATHANGs.ToList();
            f.gctrl_mathang.DataSource = sql;

        }
        public void loadcbb(form_mathang f)
        {
            var mathang = data.database().MATHANGs.ToList();
            f.cbb_timtheoten.DataSource = mathang;
            f.cbb_timtheoten.DisplayMember = "TENMH";
            f.cbb_timtheoten.ValueMember = "MAMH";
            f.cbb_timtheoten.AutoCompleteMode = AutoCompleteMode.Suggest;
            f.cbb_timtheoten.AutoCompleteSource = AutoCompleteSource.ListItems;

            f.cbb_noisx.DataSource = mathang;
            f.cbb_noisx.DisplayMember = "NOISX";
            f.cbb_noisx.ValueMember = "MAMH";
            f.cbb_noisx.AutoCompleteMode = AutoCompleteMode.Suggest;
            f.cbb_noisx.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        public void xuatfile(form_mathang f)
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
                            f.gv_mathang.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            f.gv_mathang.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            f.gv_mathang.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            f.gv_mathang.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            f.gv_mathang.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            f.gv_mathang.ExportToMht(exportFilePath);
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

        //mã mặt hàng tự động
        public int i = 0;
        public void capmatudong(form_mathang f)
        {
            CAPMA CM = data.database().CAPMAs.SingleOrDefault(a => a.STT == 1);
            i = ((Int32)CM.KISO) + 1;
            f.lb_mamh.Text = CM.KIHIEU + Convert.ToString(i);

        }
        
        //ham them
        public void them(form_mathang f,string tenmh,string loai,string noisx)
        {
            MATHANG M = new MATHANG();
            CAPMA CM = data.database().CAPMAs.SingleOrDefault(a => a.STT == 1);
            M.MAMH = f.lb_mamh.Text;
            M.TENMH = tenmh;

            if(loai=="Tủ")
                M.MALOAI = "LH1";
            if(loai=="Giường")
                M.MALOAI = "LH2";
            if (loai == "Bàn")
                M.MALOAI = "LH3";
            if (loai == "Ghế")
                M.MALOAI = "LH4";
            if (f.rdb_cai.Checked == true)
                M.DVT = "Cái";
            if (f.rdb_m2.Checked == true)
                M.DVT = "m2";
            M.NOISX = noisx;

            M.GIABAN = 0;
            M.GIAMUA = 0;
            M.KHUYENMAI = 0;
            M.SLTON = 0;

            //cap nhat du lieu bang mat hang
            data.database().MATHANGs.InsertOnSubmit(M);
            data.database().SubmitChanges();

            //cap nhat lại dữ liểu bảng cấp mã
            CM.KISO = i+1;
            data.database().SubmitChanges();

            loaddulieu(f);
        }

        //ham xoa
        public void xoa(form_mathang f)
        {
            string mamh = "";
            int donghientai = f.gv_mathang.FocusedRowHandle;
            mamh = f.gv_mathang.GetRowCellValue(donghientai, "MAMH").ToString();
            DialogResult thongbao;
            thongbao = XtraMessageBox.Show("Bạn có thực sự muốn xóa mặt hàng này ?", "thongbao", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == thongbao)
            {
                var sql_mathang = data.database().MATHANGs;
                foreach (var k in sql_mathang)
                {
                    if (k.MAMH == mamh)
                    {
                        data.database().MATHANGs.DeleteOnSubmit(k);
                        data.database().SubmitChanges();
                    }
                }
                loaddulieu(f);
            }
        }

        public void capnhatsl()
        {
            var sql_mathang = data.database().MATHANGs;
            foreach (var item in sql_mathang)
            {
                //lay so luong tu CTPN
                int slnhap = 0;
                var sql_CTPN = data.database().CTPNs;
                foreach (var sql in sql_CTPN)
                {
                    if (sql.MAMH == item.MAMH)
                    {
                        slnhap += (Int32)sql.SLNHAP;
                    }
                }
                //lay so luong tu CHITIETKHO
                int slkho = 0;
                var sql_CTKHO = data.database().CHITIETKHOs;
                foreach (var sql in sql_CTKHO)
                {
                    if (sql.MAMH == item.MAMH)
                    {
                        slkho += (Int32)sql.SOLUONG_TONKHO;
                    }
                }
                //tính tổng số lượng hiện có của mặt hàng
                data.database().SLTON((int)slkho, (int)slnhap, item.MAMH);

            }
        }

        //ham sua
        public int dongdachon = 0;
        public void sua(form_mathang f)
        {
            string mamh = "";
            int donghientai = f.gv_mathang.FocusedRowHandle;
            if (donghientai != dongdachon)
            {
                mamh = f.gv_mathang.GetRowCellValue(donghientai, "MAMH").ToString();
                MATHANG tam = data.database().MATHANGs.SingleOrDefault(a => a.MAMH == mamh);
                if (tam != null)
                {
                    tam.TENMH = f.gv_mathang.GetRowCellValue(donghientai, "TENMH").ToString();
                    tam.MALOAI = f.gv_mathang.GetRowCellValue(donghientai, "MALOAI").ToString();
                    tam.NOISX = f.gv_mathang.GetRowCellValue(donghientai, "NOISX").ToString();
                    tam.DVT = f.gv_mathang.GetRowCellValue(donghientai, "DVT").ToString();
                    tam.GIABAN = (double)f.gv_mathang.GetRowCellValue(donghientai, "GIABAN");
                    tam.GIAMUA = (double)f.gv_mathang.GetRowCellValue(donghientai, "GIAMUA");
                    tam.KHUYENMAI = (double)f.gv_mathang.GetRowCellValue(donghientai, "KHUYENMAI");
                    tam.SLTON = (int)f.gv_mathang.GetRowCellValue(donghientai, "SLTON");
                    data.database().SubmitChanges();
                    loaddulieu(f);
                    dongdachon = donghientai;
                }
            }
        }
        
        //ham tim theo ten
        public void timtheoten(form_mathang f,string ten)
        {
            var sql = data.database().MATHANGs.Where(a => a.TENMH == ten).ToList();
            f.gctrl_mathang.DataSource = sql;
        }
        //ham tim theo noi san xuat
        public void timtheonsx(form_mathang f, string nsx)
        {
            var sql = data.database().MATHANGs.Where(a => a.NOISX == nsx).ToList();
            f.gctrl_mathang.DataSource = sql;
        }

        //ham tim theo loai
        public void timtheoloai(form_mathang f, string tenl)
        {
            string ml = "";
            var sql = data.database().LOAI_MHs.SingleOrDefault(a => a.TENLOAI == tenl);
            ml = sql.MALOAI;
            
            var sql1 = data.database().MATHANGs.Where(a => a.MALOAI == ml).ToList();
            f.gctrl_mathang.DataSource = sql1;
        }

        //ham loc theo so luong
        public void timsoluongtren10(form_mathang f)
        {
            var sql = data.database().MATHANGs.Where(a => a.SLTON >10).ToList();
            f.gctrl_mathang.DataSource = sql;
        }
        public void timsoluongnho10(form_mathang f)
        {
            var sql = data.database().MATHANGs.Where(a => a.SLTON < 10).ToList();
            f.gctrl_mathang.DataSource = sql;
        }
        public void timsoluonghh(form_mathang f)
        {
            var sql = data.database().MATHANGs.Where(a => a.SLTON <=0).ToList();
            f.gctrl_mathang.DataSource = sql;
        }
    }
}
