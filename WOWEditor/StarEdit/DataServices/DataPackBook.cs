using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarEdit.DataServices
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
                DataPack data = DataService.GetAllData(packname);
                datas.Add(packname, data);
            }

            return datas[packname];
        }
    }
}
