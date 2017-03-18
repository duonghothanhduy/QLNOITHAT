using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBANDONOITHAT
{
    class class_MatHang
    {
        public string mamh, tenmh, loai, noisx, dvt;
       
        public string Mamh
        {
            get { return mamh; }
            set { mamh = value; }
        }

        public string Tenmh
        {
            get { return tenmh; }
            set { tenmh = value; }
        }

        public string Loai
        {
            get { return loai; }
            set { loai = value; }
        }

        public string Noisx
        {
            get { return noisx; }
            set { noisx = value; }
        }

        public string Dvt
        {
            get { return dvt; }
            set { dvt = value; }
        }

        public double giaban, giamua, khuyenmai;

        public double Giaban
        {
            get { return giaban; }
            set { giaban = value; }
        }

        public double Giamua
        {
            get { return giamua; }
            set { giamua = value; }
        }

        public double Khuyenmai
        {
            get { return khuyenmai; }
            set { khuyenmai = value; }
        }

       
        private int slt;

        public int Slt
        {
            get { return slt; }
            set { slt = value; }
        }


        public class_MatHang()
        {
            Mamh = "";
            Tenmh = "";
            Noisx = "";
            Loai = "";
            Dvt = "";

            Giaban = 0;
            Giamua = 0;
            Khuyenmai = 0;
            Slt = 0;
        }
    }
}
