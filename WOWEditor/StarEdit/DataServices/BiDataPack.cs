using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MessageBoxExLib;
using System.Drawing;

namespace StarEdit.DataServices
{
    class BiDataPack
    {
        public List<string> lines = new List<string>();
        public List<string> headers = new List<string>();
        public List<string> dataTypes = new List<string>();
        public List<int> dataSizes = new List<int>();
        public List<string> comment = new List<string>();
        public Dictionary<BiData, List<String>> data = new Dictionary<BiData, List<String>>();
        public Dictionary<int, int> keys = new Dictionary<int, int>();
        public String tablename;

        public BiDataPack(string table)
        {
            tablename = table;
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

        public void AddPackData(List<String> item)
        {
            if (!DataService.checkOutModified(tablename, lines))
            {
                BiData dat = new BiData(int.Parse(item[0]), int.Parse(item[1]));
                data.Add(dat, item);

                int keyid = int.Parse(item[0]);
                if (keys.ContainsKey(keyid))
                {
                    keys[keyid]++;
                }
                else
                {
                    keys.Add(keyid, 1);
                }

                DataService.resetData(tablename, this);
            }
        }

        public void RemovePackData(BiData dat)
        {
            if (!DataService.checkOutModified(tablename, lines))
            {
                data.Remove(dat);

                int keyid = dat.a;
                keys[keyid]--;
                if (keys[keyid] == 0)
                {
                    keys.Remove(keyid);
                }

                DataService.resetData(tablename, this);
            }
        }

        public void EditPackData(BiData dat, String fieldname, String value)
        {
            if (!DataService.checkOutModified(tablename, lines))
            {
                int index = GetPackIndexByName(fieldname);
                data[dat][index] = value;

                DataService.resetData(tablename, this);
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
            if (!DataService.checkOutModified(tablename, lines))
            {
                if (doClear)
                {
                    keys.Clear();
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

                    int keyid = int.Parse(infos[0]);
                    BiData dat = new BiData(int.Parse(infos[0]), int.Parse(infos[1]));
                    data.Add(dat, new List<string>(infos));

                    if (keys.ContainsKey(keyid))
                    {
                        keys[keyid]++;
                    }
                    else
                    {
                        keys.Add(keyid, 1);
                    }
                }
                DataService.resetData(tablename, this);
                sr.Close();
            }
        }
    }

    struct BiData
    {
        public int a;
        public int b;

        public BiData(int pa, int pb)
        {
            a = pa;
            b = pb;
        }
    }
}
