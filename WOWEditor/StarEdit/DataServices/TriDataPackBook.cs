using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarEdit.DataServices
{
    class TriDataPackBook
    {
        static Dictionary<String, TriDataPack> datas = new Dictionary<string, TriDataPack>();

        TriDataPackBook()
        {
        }

        public static TriDataPack GetPack(String packname)
        {
            if (!datas.ContainsKey(packname))
            {
                TriDataPack data = DataService.GetAllTriData(packname);
                datas.Add(packname, data);
            }

            return datas[packname];
        }
    }
}
