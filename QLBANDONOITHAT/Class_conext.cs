using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBANDONOITHAT.LQTOSQL;

namespace QLBANDONOITHAT
{
    class Class_conext
    {
        //khai bao bien ket noi database
        dbQLNOITHATDataContext data;
        public Class_conext()
        {
            data = new dbQLNOITHATDataContext();
        }

        public dbQLNOITHATDataContext database()
        {
            return data;
        }
    }
}
