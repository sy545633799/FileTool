using System;
using System.Collections.Generic;
using System.Text;

namespace StarEdit.MysqlServices
{
    class OldDataPackBook
    {
        static Dictionary<String, OldDataPack> datas = new Dictionary<string, OldDataPack>();

        OldDataPackBook()
        {
        }

        public static OldDataPack GetPack(String packname)
        {
            if (!datas.ContainsKey(packname))
            {
                OldDataPack data = MysqlService.GetAllData(packname);
                datas.Add(packname, data);
            }

            return datas[packname];
        }
    }
}
