using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBANDONOITHAT.LQTOSQL;

namespace QLBANDONOITHAT
{
    class Class_dangnhap
    {
        Class_conext data;
        public Class_dangnhap()
        {
            data = new Class_conext();
        }

        //HAM DANG NHAP
        public int t = 0;
        public int phanquyen = 0;
        public static string manv = "";
        public void dangnhap(BATDAU f,string tendn,string mk)
        {
            string user = tendn;
            string matkhau = mk;

            LOGIN tam = data.database().LOGINs.SingleOrDefault(a => a.TENDN == user && a.PASS == matkhau);
            
            if (tam == null)
            {
                t = 0;
            }
            else
            {
                t = 1;
            }

            if (t == 0)
            {
                f.lb_thongbao.Visible = true;
                f.lb_thongbao.Text = "Sai Tên Đăng Nhập hoặc Mật Khẩu !";
                f.txt_dangnhap.Text = "";
                f.txtpass.Text = "";
            }

            if (t == 1)
            {
                manv = tam.MANV;
                phanquyen = int.Parse(tam.PHANQUYEN.ToString());
                f.txt_dangnhap.Visible = false;
                f.txtpass.Visible = false;
                f.bt_dangnhap.Visible = false;
                f.lb_thongbao.Visible = false;
                f.lb_user.Visible = false;
                f.lb_pass.Visible = false;
                string[] chuoi = tam.GHICHU.Split('-');
                for (int i = 0; i < chuoi.Length; i++)
                {
                    f.tli_dangnhap.Text += chuoi[i] + "\n";
                }

                f.tli_nutdangnhap.Text = "Đăng Xuất";
            }
        }

        public int capphanquyen()
        {
            return phanquyen;
        }
    }
}
