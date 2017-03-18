using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBANDONOITHAT
{
    class Class_banhang
    {
        public string mamh, tenmh;
       
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

        public double giaban;

        public double Giaban
        {
            get { return giaban; }
            set { giaban = value; }
        }

        private int soluong;

        public int Soluong
        {
            get { return soluong; }
            set { soluong = value; }
        }



        public Class_banhang()
        {
            Mamh = "";
            Tenmh = "";
            Giaban = 0;
            Soluong = 0;
        }
    }
}
