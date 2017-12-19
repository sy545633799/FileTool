using System;
using System.Collections.Generic;
using System.Text;

namespace StarEdit.MysqlServices
{
    class OldTriDataPackBook
    {
        static Dictionary<String, OldTriDataPack> datas = new Dictionary<string, OldTriDataPack>();

        OldTriDataPackBook()
        {
        }

        public static OldTriDataPack GetPack(String packname)
        {
            if (!datas.ContainsKey(packname))
            {
                OldTriDataPack data = MysqlService.GetAllTriData(packname);                
                datas.Add(packname, data);
            }

            return datas[packname];
        }
    }
}
