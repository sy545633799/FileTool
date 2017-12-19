using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StarEdit.DataServices
{
    class TriDataPack
    {
        public List<string> lines = new List<string>();
        public List<string> headers = new List<string>();
        public List<string> dataTypes = new List<string>();
        public List<int> dataSizes = new List<int>();
        public List<string> comment = new List<string>();
        public Dictionary<TriData, List<String>> data = new Dictionary<TriData, List<String>>();
        public Dictionary<int, int> keys = new Dictionary<int, int>();
        public String tablename;

        public TriDataPack(string table)
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
                TriData dat = new TriData(int.Parse(item[0]), int.Parse(item[1]), int.Parse(item[2]));
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

        public void RemovePackData(TriData dat)
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

        public void EditPackData(TriData dat, String fieldname, String value)
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
            sw.WriteLine(String.Join(",", headers.ToArray()));
            foreach (List<String> infos in data.Values)
                sw.WriteLine(String.Join(",", infos.ToArray()));
            sw.Close();
        }

        public void ImportPackData(String path, bool doClear)
        {
            if (!DataService.checkOutModified(tablename, lines))
            {
                if (doClear)
                {
                    data.Clear();
                    keys.Clear();
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
                    TriData dat = new TriData(int.Parse(infos[0]), int.Parse(infos[1]), int.Parse(infos[2]));
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

    struct TriData
    {
        public int a;
        public int b;
        public int c;

        public TriData(int pa, int pb, int pc)
        {
            a = pa;
            b = pb;
            c = pc;
        }
    }
}
