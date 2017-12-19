using System;
using System.Collections.Generic;
using System.Text;

namespace StarEdit.MysqlServices
{
    class DataPackBook
    {
        static Dictionary<String, DataPack> datas = new Dictionary<string, DataPack>();

        DataPackBook()
        {
        }

        public static DataPack GetPack(String packname)
        {
            if (!datas.ContainsKey(packname))
            {
                DataPack data = MysqlService.GetAllData(packname);
                datas.Add(packname, data);
            }

            return datas[packname];
        }
    }
}
