using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace StarEdit.MysqlServices
{
    class OldDataPack
    {
        public List<string> header = new List<string>();
        public List<string> datatype = new List<string>();
        public List<int> datasize = new List<int>();
        public List<string> comment = new List<string>();
        public Dictionary<int, List<String>> data = new Dictionary<int,List<string>>();
        public int maxid = 0;
        public String tablename;
        public DateTime cachTime;

        public OldDataPack(string table)
        {
            tablename = table;
        }

        public int GetPackIndexByName(String name)
        {
            for(int i =0;i < header.Count;i ++)
            {
                if (header[i] == name)
                {
                    return i;
                }
            }
            return -1;
        }

        public void AddPackData(int id, List<String> item)
        {
            data.Add(id, item);

            MysqlService.InsertData(tablename, item);
        }

        public void RemovePackData(int id)
        {
            data.Remove(id);

            MysqlService.RemoveData(tablename, header[0], id);
        }

        public void EditPackData(int id, String fieldname, String value)
        {
            int index = GetPackIndexByName(fieldname);
            data[id][index] = value;
            MysqlService.UpdateData(tablename, header[0], id, fieldname, value);
        }

        public void ExportPackData(String path)
        {
            StreamWriter sw = new StreamWriter(path, false, Encoding.Default);
            sw.WriteLine(String.Join(",", header.ToArray()));
            foreach (List<String> infos in data.Values)
                sw.WriteLine(String.Join(",", infos.ToArray()));
            sw.Close();
        }

        public void ImportPackData(String path, bool doClear)
        {
            ExportPackData(String.Format("backup/{0} {1}.csv", tablename, DateTime.Now.Ticks));
            if (doClear)
            {
                MysqlService.ClearData(tablename);
                data.Clear();
            }

            StreamReader sr = new StreamReader(path, Encoding.Default);
            string line;
            int result;
            while ((line = sr.ReadLine()) != null)
            {
                string[] infos = line.Split(',');
                if (!int.TryParse(infos[0], out result))
                    continue;

                AddPackData(int.Parse(infos[0]), new List<String>(infos));
            }
            sr.Close();
        }
    }
}
