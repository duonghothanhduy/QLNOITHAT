using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using QLBANDONOITHAT.LQTOSQL;

namespace QLBANDONOITHAT
{
    class Class_banhang
    {
        Class_conext data;
        public Class_banhang()
        {
            data = new Class_conext();
        }
        //dua ra ten khach hang khi dua vao sdt
        public void truyvantenkh(BANHANG f,string sdt)
        {
            int ktr = 0;
            var sql_khachhang = data.database().KHACHHANGs.ToList();
            foreach (var item in sql_khachhang)
            {
                if (item.DIENTHOAI_KH == sdt)
                {
                    f.txt_tenkh.Text = item.TENKH;
                    ktr = 1;
                }

            }
            if (ktr == 0)
            {
                f.txt_tenkh.Text = "";
            }
        }
        //load du lieu vao combb
        public void loaddlcbb(BANHANG f)
        {
            var sql = data.database().MATHANGs.ToList();
            f.cbb_tenmh.DataSource = sql;
            f.cbb_tenmh.DisplayMember = "TENMH";
            f.cbb_tenmh.ValueMember = "MAMH";
            f.cbb_tenmh.AutoCompleteMode = AutoCompleteMode.Suggest;
            f.cbb_tenmh.AutoCompleteSource = AutoCompleteSource.ListItems;
            
        }

        //cap so hoa don
        public string sohd = "";
        public void capshd(string tenkh,string sdth)
        {
            CAPMA tam = data.database().CAPMAs.SingleOrDefault(a => a.STT == 7);
            sohd = tam.KIHIEU + Convert.ToString(tam.KISO + 1);
            HOADON A = new HOADON();
            A.SOHD = sohd;
            A.NGAYLAP_HD = DateTime.Now;
            //  A.MANV = BATDAU.manv;
            A.TONGTG = 0;
            A.TT_THANHTOAN = "";
            KHACHHANG K = data.database().KHACHHANGs.SingleOrDefault(a => a.TENKH == tenkh && a.DIENTHOAI_KH == sdth);
            if (K != null)
            {
                A.MAKH = K.MAKH;
            }
            data.database().HOADONs.InsertOnSubmit(A);
            data.database().SubmitChanges();
            tam.KISO = tam.KISO + 1;
            data.database().SubmitChanges();
        }

        //LOAD DU LIEU DANH SACH CHON KHI CHON HANG HOA
        public void loaddulieu(BANHANG f)
        {
            var sql = data.database().PROC_BANHANG().Where(a => a.SOHD == sohd).ToList();
            f.gctrl_banhang.DataSource = sql;
        }

        //dua du lieu vao CTHD va hoa don
        public string mamathang = "";
        public double tongtrigia = 0;
        public double khuyenmai = 0;
        public void insertcthd_hd(BANHANG f,string tenmh,string sl)
        {
            CTHD C = new CTHD();
            C.SOHD = sohd;
            MATHANG MH = data.database().MATHANGs.SingleOrDefault(a => a.TENMH == tenmh);
            if (MH != null)
            {
                mamathang = MH.MAMH;
                C.MAMH = MH.MAMH;
                C.DONGIA = MH.GIABAN;
            }
            C.SOLUONGMUA = int.Parse(sl);
            C.THUEVAT = 10;

            //cap nhat tong tri gia
            tongtrigia += (double)((int.Parse(sl) * C.DONGIA));
            khuyenmai += (double)(((int.Parse(sl) * C.DONGIA))*MH.KHUYENMAI/100);
            data.database().CTHDs.InsertOnSubmit(C);
            data.database().SubmitChanges();
            int kt = 0;
            //load dữ liêu mặt hàng được chọn
            if (sl == "")
            {
                XtraMessageBox.Show("Bạn Chưa Chọn Số Lượng Sản Phẩm!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                kt = 1;
            }
            if (kt == 0)
            {
                loaddulieu(f);
                f.lb_tongdongia.Text = Convert.ToString(tongtrigia);
                f.lb_khuyenmai.Text = Convert.ToString(khuyenmai);
                f.lb_tiencantra.Text = Convert.ToString(tongtrigia - khuyenmai);
                
            }
        }

        //xoa mat hang da chon
        public void xoa(BANHANG f)
        {
            int donghientai = f.gv_banhang.FocusedRowHandle;
            string mamh = f.gv_banhang.GetRowCellValue(donghientai, "MAMH").ToString();
            var sql = data.database().CTHDs.SingleOrDefault(a=>a.SOHD == sohd && a.MAMH == mamh);
            tongtrigia = tongtrigia - (double)(sql.DONGIA * sql.SOLUONGMUA);
            data.database().CTHDs.DeleteOnSubmit(sql);
            data.database().SubmitChanges();
            loaddulieu(f);
            f.lb_tongdongia.Text = Convert.ToString(tongtrigia);
                
            
        }

        //tinh tien thua cho khach
        public void tienthua(BANHANG f,string tien)
        {
            if (tien == "")
            {
                f.lb_tienthua.Text = "0";
            }
            else
            {
                f.lb_tienthua.Text = Convert.ToString((tongtrigia - khuyenmai) - int.Parse(f.txt_khachtra.Text));
            }
        }
    }
}
