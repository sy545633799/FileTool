using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarEdit.Tools;
using StarEdit.MysqlServices;
using System.IO;
using MessageBoxExLib;
using System.Drawing;

namespace StarEdit.DataServices
{
    class DataService
    {
        static string respath;
        static string datapath;

        static DataService()
        {
            IniFile ini = new IniFile("./config.ini");
            respath = ini.IniReadValue("DataService", "respath");
            datapath = ini.IniReadValue("DataService", "datapath");
        }

        public static DataPack GetAllData(string tableName)
        {
            DataPack pack = new DataPack(tableName);
            pack.lines.Clear();
            StreamReader sr = new StreamReader(datapath + "/" + tableName + ".cfg", Encoding.UTF8);
            string line = "";
            bool inHeader = false;
            bool inData = false;
            while ((line = sr.ReadLine()) != null)
            {
                pack.lines.Add(line);
                if (line == "[header]")
                {
                    inHeader = true;
                    inData = false;
                }
                else if (line == "[data]")
                {
                    inHeader = false;
                    inData = true;
                }
                else if (inHeader)
                {
                    string[] items = line.Split('\t');
                    if (items.Length >= 3)
                    {
                        pack.headers.Add(items[0]);
                        pack.dataTypes.Add(items[1]);
                        pack.dataSizes.Add(int.Parse(items[2]));
                    }
                    if (items.Length >= 4)
                    {
                        pack.comment.Add(items[3]);
                    }
                    else
                    {
                        pack.comment.Add("");
                    }
                }
                else if (inData)
                {
                    string[] items = line.Split('\t');
                    List<string> strList = items.ToList();

                    int id = int.Parse(items[0]);
                    pack.data.Add(id, strList);
                }
            }
            sr.Close();

            return pack;
        }

        public static BiDataPack GetAllBiData(string tableName)
        {
            BiDataPack pack = new BiDataPack(tableName);
            pack.lines.Clear();
            StreamReader sr = new StreamReader(datapath + "/" + tableName + ".cfg", Encoding.UTF8);
            string line = "";
            bool inHeader = false;
            bool inData = false;
            while ((line = sr.ReadLine()) != null)
            {
                pack.lines.Add(line);
                if (line == "[header]")
                {
                    inHeader = true;
                    inData = false;
                }
                else if (line == "[data]")
                {
                    inHeader = false;
                    inData = true;
                }
                else if (inHeader)
                {
                    string[] items = line.Split('\t');
                    if (items.Length >= 3)
                    {
                        pack.headers.Add(items[0]);
                        pack.dataTypes.Add(items[1]);
                        pack.dataSizes.Add(int.Parse(items[2]));
                    }
                    if (items.Length >= 4)
                    {
                        pack.comment.Add(items[3]);
                    }
                    else
                    {
                        pack.comment.Add("");
                    }
                }
                else if (inData)
                {
                    string[] items = line.Split('\t');
                    List<string> strList = items.ToList();

                    int keyid = int.Parse(items[0]);
                    BiData dat = new BiData(int.Parse(items[0]), int.Parse(items[1]));
                    pack.data.Add(dat, new List<string>(items));
                    if (pack.keys.ContainsKey(keyid))
                    {
                        pack.keys[keyid]++;
                    }
                    else
                    {
                        pack.keys.Add(keyid, 1);
                    }
                }
            }
            sr.Close();

            return pack;
        }

        public static TriDataPack GetAllTriData(string tableName)
        {
            TriDataPack pack = new TriDataPack(tableName);
            pack.lines.Clear();
            StreamReader sr = new StreamReader(datapath + "/" + tableName + ".cfg", Encoding.UTF8);
            string line = "";
            bool inHeader = false;
            bool inData = false;
            while ((line = sr.ReadLine()) != null)
            {
                pack.lines.Add(line);
                if (line == "[header]")
                {
                    inHeader = true;
                    inData = false;
                }
                else if (line == "[data]")
                {
                    inHeader = false;
                    inData = true;
                }
                else if (inHeader)
                {
                    string[] items = line.Split('\t');
                    if (items.Length >= 3)
                    {
                        pack.headers.Add(items[0]);
                        pack.dataTypes.Add(items[1]);
                        pack.dataSizes.Add(int.Parse(items[2]));
                    }
                    if (items.Length >= 4)
                    {
                        pack.comment.Add(items[3]);
                    }
                    else
                    {
                        pack.comment.Add("");
                    }
                }
                else if (inData)
                {
                    string[] items = line.Split('\t');
                    List<string> strList = items.ToList();

                    int keyid = int.Parse(items[0]);
                    TriData dat = new TriData(int.Parse(items[0]), int.Parse(items[1]), int.Parse(items[2]));
                    pack.data.Add(dat, new List<string>(items));
                    if (pack.keys.ContainsKey(keyid))
                    {
                        pack.keys[keyid]++;
                    }
                    else
                    {
                        pack.keys.Add(keyid, 1);
                    }
                }
            }
            sr.Close();

            return pack;
        }

        public static void resetData(string tableName, DataPack pack)
        {
            String line;
            StreamWriter sw = new StreamWriter(datapath + "/" + tableName + ".cfg", false, Encoding.UTF8);
            sw.WriteLine("[header]");
            for (int i = 0; i < pack.headers.Count; ++i)
            {
                line = pack.headers[i] + "\t" + pack.dataTypes[i] + "\t" + pack.dataSizes[i] + "\t" + pack.comment[i];
                sw.WriteLine(line);
            }
            sw.WriteLine();
            sw.WriteLine("[data]");
            //按照key顺序输出
            List<int> keys = new List<int>();
            foreach (int key in pack.data.Keys)
            {
                keys.Add(key);
            }
            keys.Sort();
            for (int i = 0; i < keys.Count; ++i)
            {
                sw.WriteLine(String.Join("\t", pack.data[keys[i]].ToArray()));
            }
            sw.Close();

            pack.lines.Clear();
            StreamReader sr = new StreamReader(datapath + "/" + tableName + ".cfg", Encoding.UTF8);
            while ((line = sr.ReadLine()) != null)
            {
                pack.lines.Add(line);
            }
            sr.Close();
        }

        public static void resetData(string tableName, BiDataPack pack)
        {
            String line;
            StreamWriter sw = new StreamWriter(datapath + "/" + tableName + ".cfg", false, Encoding.UTF8);
            sw.WriteLine("[header]");
            for (int i = 0; i < pack.headers.Count; ++i)
            {
                line = pack.headers[i] + "\t" + pack.dataTypes[i] + "\t" + pack.dataSizes[i] + "\t" + pack.comment[i];
                sw.WriteLine(line);
            }
            sw.WriteLine();
            sw.WriteLine("[data]");
            //按照key顺序输出
            List<int> key1List = new List<int>();
            Dictionary<int, List<BiData>> key1Map = new Dictionary<int, List<BiData>>();
            foreach (BiData bi in pack.data.Keys)
            {
                if (!key1Map.ContainsKey(bi.a))
                {
                    key1Map.Add(bi.a, new List<BiData>());
                    key1List.Add(bi.a);
                }
                key1Map[bi.a].Add(bi);
            }
            key1List.Sort();
            for (int i = 0; i < key1List.Count; ++i)
            {
                int key1 = key1List[i];
                Dictionary<int, BiData> key2Map = new Dictionary<int, BiData>();
                List<int> key2List = new List<int>();
                foreach (BiData bi in key1Map[key1])
                {
                    key2List.Add(bi.b);
                    key2Map.Add(bi.b, bi);
                }
                key2List.Sort();
                for (int j = 0; j < key2List.Count; ++j)
                {
                    sw.WriteLine(String.Join("\t", pack.data[key2Map[key2List[j]]].ToArray()));
                }
            }
            sw.Close();

            pack.lines.Clear();
            StreamReader sr = new StreamReader(datapath + "/" + tableName + ".cfg", Encoding.UTF8);
            while ((line = sr.ReadLine()) != null)
            {
                pack.lines.Add(line);
            }
            sr.Close();
        }

        public static void resetData(string tableName, TriDataPack pack)
        {
            String line;
            StreamWriter sw = new StreamWriter(datapath + "/" + tableName + ".cfg", false, Encoding.UTF8);

            sw.WriteLine("[header]");
            for (int i = 0; i < pack.headers.Count; ++i)
            {
                line = pack.headers[i] + "\t" + pack.dataTypes[i] + "\t" + pack.dataSizes[i] + "\t" + pack.comment[i];
                sw.WriteLine(line);
            }
            sw.WriteLine();
            sw.WriteLine("[data]");
            //按照key顺序输出
            List<int> key1List = new List<int>();
            Dictionary<int, List<TriData>> key1Map = new Dictionary<int, List<TriData>>();
            foreach (TriData tri in pack.data.Keys)
            {
                if (!key1Map.ContainsKey(tri.a))
                {
                    key1Map.Add(tri.a, new List<TriData>());
                    key1List.Add(tri.a);
                }
                key1Map[tri.a].Add(tri);
            }
            key1List.Sort();
            for (int i = 0; i < key1List.Count; ++i)
            {
                List<int> key2List = new List<int>();
                Dictionary<int, List<TriData>> key2Map = new Dictionary<int, List<TriData>>();
                foreach (TriData tri in key1Map[key1List[i]])
                {
                    if (!key2Map.ContainsKey(tri.b))
                    {
                        key2Map.Add(tri.b, new List<TriData>());
                        key2List.Add(tri.b);
                    }
                    key2Map[tri.b].Add(tri);
                }
                key2List.Sort();
                
                for (int j = 0; j < key2List.Count; ++j)
                {
                    List<int> key3List = new List<int>();
                    Dictionary<int, TriData> key3Map = new Dictionary<int, TriData>();
                    foreach (TriData tri in key2Map[key2List[j]])
                    {
                        key3Map.Add(tri.c, tri);
                        key3List.Add(tri.c);
                    }
                    key3List.Sort();
                    for (int k = 0; k < key3List.Count; ++k)
                    {
                        sw.WriteLine(String.Join("\t", pack.data[key3Map[key3List[k]]].ToArray()));
                    }
                }
            }
            sw.Close();

            pack.lines.Clear();
            StreamReader sr = new StreamReader(datapath + "/" + tableName + ".cfg", Encoding.UTF8);
            while ((line = sr.ReadLine()) != null)
            {
                pack.lines.Add(line);
            }
            sr.Close();
        }

        public static bool checkOutModified(string tableName, List<string> lines)
        {
            StreamReader sr = new StreamReader(datapath + "/" + tableName + ".cfg", Encoding.UTF8);
            String line;
            int iLine = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (lines.Count <= iLine || line != lines[iLine++])
                {
                    sr.Close();
                    MessageBoxEx.Show("数据已在外部被修改，无法保存，请重新打开工具开始修改", "错误", SystemIcons.Error);
                    return true;
                }
            }
            sr.Close();
            return false;
        }
    }
}
