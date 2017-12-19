using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace StarEdit.MysqlServices
{
    class OldTriDataPack
    {
        public List<string> header = new List<string>();
        public List<string> datatype = new List<string>();
        public List<int> datasize = new List<int>();
        public List<string> comment = new List<string>();
        public Dictionary<TriData, List<String>> data = new Dictionary<TriData, List<String>>();
        public Dictionary<int, int> keys = new Dictionary<int, int>();
        public String tablename;
        public DateTime cachTime;

        public OldTriDataPack(string table)
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

        public void AddPackData(List<String> item)
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

            MysqlService.InsertData(tablename, item);
        }

        public void RemovePackData(TriData dat)
        {
            data.Remove(dat);

            int keyid = dat.a;
            keys[keyid]--;
            if (keys[keyid] == 0)
            {
                keys.Remove(keyid);
            }

            MysqlService.RemoveData(tablename, header[0], dat.a, header[1], dat.b, header[2], dat.c);
        }

        public void EditPackData(TriData dat, String fieldname, String value)
        {
            int index = GetPackIndexByName(fieldname);
            data[dat][index] = value;

            MysqlService.UpdateData(tablename, header[0], dat.a, header[1], dat.b, header[2], dat.c, fieldname, value);
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

                AddPackData(new List<String>(infos));
            }
            sr.Close();
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
