using System;
using System.Collections.Generic;
using System.Text;

namespace StarEdit.MysqlServices
{
    class OldBiDataPackBook
    {
        static Dictionary<String, OldBiDataPack> datas = new Dictionary<string, OldBiDataPack>();

        OldBiDataPackBook()
        {
        }

        public static OldBiDataPack GetPack(String packname)
        {
            if (!datas.ContainsKey(packname))
            {
                OldBiDataPack data = MysqlService.GetAllBiData(packname);                
                datas.Add(packname, data);
            }

            return datas[packname];
        }
    }
}
