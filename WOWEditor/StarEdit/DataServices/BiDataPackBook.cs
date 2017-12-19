using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarEdit.DataServices
{
    class BiDataPackBook
    {
        static Dictionary<String, BiDataPack> datas = new Dictionary<string, BiDataPack>();

        BiDataPackBook()
        {
        }

        public static BiDataPack GetPack(String packname)
        {
            if (!datas.ContainsKey(packname))
            {
                BiDataPack data = DataService.GetAllBiData(packname);
                datas.Add(packname, data);
            }

            return datas[packname];
        }
    }
}
