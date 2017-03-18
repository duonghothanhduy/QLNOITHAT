using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using QLBANDONOITHAT.LQTOSQL;

namespace QLBANDONOITHAT
{
    class Class_Checker
    {
        dbQLNOITHATDataContext data = new dbQLNOITHATDataContext();

        //ham kiem tra them Mat hang
        public static bool ktrthem_MATHANG(string ten,string loai,string dvt,string noisx)
        {
            int KT = 0;
            string KQ = "------------THÔNG BÁO------------\n\n\n";
            if(ten=="")
            {
                KT = 1;
                KQ += "Tên mặt hàng còn trống\n";
            }
            if (loai == "")
            {
                KT = 1;
                KQ += "Loại mặt hàng còn trống\n";
            } 
            if (dvt == "")
            {
                KT = 1;
                KQ += "Đơn vị tính mặt hàng còn trống\n";
            }
            if (noisx == "")
            {
                KT = 1;
                KQ += "Nơi Sản Xuất mặt hàng còn trống\n";
            }
            if(KT==1)
            {
                XtraMessageBox.Show(KQ, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; 
            }
            else
                return true;
        }
        
    }
}
