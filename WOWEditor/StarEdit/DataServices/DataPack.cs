using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MessageBoxExLib;
using System.Drawing;

namespace StarEdit.DataServices
{
    class DataPack
    {
        public string tableName;
        public List<string> lines = new List<string>();
        public List<string> headers = new List<string>();
        public List<string> dataTypes = new List<string>();
        public List<int> dataSizes = new List<int>();
        public List<string> comment = new List<string>();
        public Dictionary<int, List<string>> data = new Dictionary<int, List<string>>();

        public DataPack(string table)
        {
            tableName = table;
        }

        public int GetPackIndexByName(String name)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                if (headers[i] == name)
                {
                    return i;
                }
            }
            return -1;
        }

        public void AddPackData(int id, List<String> item)
        {
            if (!DataService.checkOutModified(tableName, lines))
            {
                data.Add(id, item);

                DataService.resetData(tableName, this);
            }
        }

        public void RemovePackData(int id)
        {
            if (!DataService.checkOutModified(tableName, lines))
            {
                data.Remove(id);

                DataService.resetData(tableName, this);
            }
        }

        public void EditPackData(int id, String fieldname, String value)
        {
            if (!DataService.checkOutModified(tableName, lines))
            {
                int index = GetPackIndexByName(fieldname);
                data[id][index] = value;
                DataService.resetData(tableName, this);
            }
        }

        public void ExportPackData(String path)
        {
            StreamWriter sw = new StreamWriter(path, false, Encoding.Default);
            String line = String.Join(",", headers.ToArray());
            sw.WriteLine(line);
            foreach (List<String> infos in data.Values)
            {
                line = String.Join(",", infos.ToArray());
                sw.WriteLine(line);
            }
            sw.Close();
        }

        public void ImportPackData(String path, bool doClear)
        {
            if (!DataService.checkOutModified(tableName, lines))
            {
                if (doClear)
                {
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

                    data.Add(int.Parse(infos[0]), new List<String>(infos));
                }
                DataService.resetData(tableName, this);
                sr.Close();
            }
        }
    }
}
