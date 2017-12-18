using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TableExportTool.Common
{
    class PathManager
    {
        private static PathManager _instance;
        public readonly List<ISheet> _generateList = new List<ISheet>();
        public readonly List<ISheet> _tableList = new List<ISheet>();
        public readonly Dictionary<string, List<string>> _filesPath = new Dictionary<string, List<string>>();
        public readonly Dictionary<string, string> _configPath = new Dictionary<string, string>();

        public readonly Dictionary<string, List<string>> _configPathNew = new Dictionary<string, List<string>>();

        //是否生成PB
        public bool GenerateProtobuf { get; set; }

        public static PathManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PathManager();
                }
                return _instance;
            }
        }



        public Dictionary<string, List<string>> LoadDefultConfigNew(string startUpPath)
        {
            if (File.Exists(startUpPath + "\\newPath.txt"))
            {
                StreamReader sr = null;
                try
                {
                    sr = File.OpenText(startUpPath + "\\newPath.txt");
                    string temp;
                    while ((temp = sr.ReadLine()) != null)
                    {
                        string[] p = temp.Split('=');
                        if (p.Length > 1)
                        {

                            string[] pp = p[1].Split('|');
                            foreach (string s in pp)
                            {
                                if (_configPathNew.ContainsKey(p[0]))
                                {
                                    _configPathNew[p[0]].Add(s);
                                }
                                else
                                {
                                    _configPathNew.Add(p[0], new List<string>() { s });
                                }
                            }
                        }
                    }

                    return _configPathNew;

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    if (sr != null)
                        sr.Close();
                }
            }
            return null;
        }


        public string GetInputPath()
        {
            return _configPath.ContainsKey(ConfigConst.InputPath) ? _configPath[ConfigConst.InputPath] : null;
        }

        public Dictionary<string, string> LoadDefultConfig(string startUpPath)
        {
            if (File.Exists(startUpPath + "\\path.txt"))
            {

                StreamReader sr = null;
                try
                {
                    sr = File.OpenText(startUpPath + "\\path.txt");
                    string temp;
                    while ((temp = sr.ReadLine()) != null)
                    {
                        string[] p = temp.Split('=');
                        if (p.Length > 1)
                        {
                            _configPath.Add(p[0], p[1]);
                        }
                    }

                    return _configPath;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    if (sr != null)
                        sr.Close();
                }
            }
            else
            {
                SavePath(startUpPath, ConfigConst.DependPath, startUpPath + "\\BaseTable.cs");
                return _configPath;
            }
            return null;
        }


        public void RemovePath(string configPath, string flag, string item)
        {
            if (_configPathNew.ContainsKey(flag))
            {
                if (_configPathNew[flag].Count > 1)
                {
                    _configPathNew[flag].Remove(item);
                }
                else
                {
                    _configPathNew.Remove(flag);
                }
            }

            StreamWriter sw = null;
            try
            {
                sw = File.CreateText(configPath + "\\newPath.txt");
                foreach (var pList in _configPathNew)
                {
                    string pp = string.Empty;
                    foreach (var p in pList.Value)
                    {
                        pp += p + "|";
                    }
                    string ppp = pList.Key + "=" + pp.TrimEnd('|');
                    sw.WriteLine(ppp);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }

        }


        public void SavePathNew(string configPath, string flag, string path)
        {
            StreamWriter sw = null;
            if (_configPathNew.ContainsKey(flag))
            {
                _configPathNew[flag].Add(path);
            }
            else
            {
                _configPathNew.Add(flag, new List<string>() { path });
            }
            try
            {
                sw = File.CreateText(configPath + "\\newPath.txt");
                foreach (var pList in _configPathNew)
                {
                    string pp = string.Empty;
                    foreach (var p in pList.Value)
                    {
                        pp += p + "|";
                    }
                    string ppp = pList.Key + "=" + pp.TrimEnd('|');
                    sw.WriteLine(ppp);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }

        public void SavePath(string configPath, string flag, string path)
        {
            StreamWriter sw = null;

            if (_configPath.ContainsKey(flag))
            {
                _configPath[flag] = path;
            }
            else
            {
                _configPath.Add(flag, path);
            }

            try
            {
                sw = File.CreateText(configPath + "\\path.txt");
                foreach (var p in _configPath)
                {
                    sw.WriteLine(p.Key + "=" + p.Value);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }

        }

        public Dictionary<string, List<ISheet>> _pathMapping = new Dictionary<string, List<ISheet>>();

        public void LoadExecle(string folderPath, Action<List<ISheet>> completeCallback)
        {
            try
            {

                foreach (var vk in _pathMapping) vk.Value.Clear();

                var filePaths = Directory.GetFiles(folderPath, "*.xlsx", SearchOption.AllDirectories);
                _tableList.Clear();
                foreach (var path in filePaths)
                {
                    if (File.Exists(path))
                    {
                        IWorkbook wk = null;
                        FileStream fs = null;
                        var extension = Path.GetExtension(path);
                        string fileName = Path.GetFileName(path);
                        try
                        {
                            fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                            if (extension != null)
                            {
                                if (extension.Equals(".xls"))
                                {
                                    wk = new HSSFWorkbook(fs);
                                }
                                else
                                {
                                    wk = new XSSFWorkbook(fs);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            if (fs != null)
                                fs.Close();
                        }

                        if (wk != null)
                        {
                            for (var i = 0; i < wk.NumberOfSheets; i++)
                            {
                                var sheet = wk.GetSheetAt(i);
                                if (sheet.SheetName.StartsWith("Sheet", StringComparison.CurrentCultureIgnoreCase))
                                    continue;
                                _tableList.Add(sheet);
                                if (_pathMapping.ContainsKey(fileName))
                                {
                                    _pathMapping[fileName].Add(sheet);
                                }
                                else
                                {
                                    _pathMapping.Add(fileName, new List<ISheet> { sheet });
                                }
                            }
                        }
                    }
                }
                completeCallback(_tableList);
            }
            catch (Exception e)
            {
                MessageBox.Show("输入路径错误:" + e.Message);
            }

        }

        public bool CheckConfigPath()
        {

            if (!_configPath.ContainsKey(ConfigConst.InputPath))
            {
                MessageBox.Show("输入目录路径错误,请设置有效的路径.");
                return false;
            }
            if (!_configPath.ContainsKey(ConfigConst.OutPath))
            {
                MessageBox.Show("输出目录路径错误,请设置有效的路径.");
                return false;
            }

            if (!_configPath.ContainsKey(ConfigConst.DependPath))
            {
                MessageBox.Show("依赖文件路径错误,请设置有效的路径.");
                return false;
            }

            return true;
        }


        public string GenerateOutPath(string path, string dir, string dir1 = "", string append = "")
        {

            string temp = dir.Equals("1") ? "\\Client" : "\\Server";
            if (!_configPath.ContainsKey(path))
                path = ConfigConst.OutPath;

            if (!Directory.Exists(_configPath[path] + temp + dir1))
                Directory.CreateDirectory(_configPath[path] + temp + dir1);

            return _configPath[path].Trim('\\') + temp + dir1 + append;
        }

        public bool LoadBaseTable(string[] splitType)
        {

            if (!File.Exists(_configPath[ConfigConst.DependPath]))
            {
                MessageBox.Show("依赖文件 BaseTable.cs 不存在! ");
                return false;
            }

            foreach (var type in splitType)
            {
                if (_filesPath.ContainsKey(type))
                {
                    _filesPath[type].Add(_configPath[ConfigConst.DependPath]);
                }
            }
            return true;
        }

        public int GetGenerateCount()
        {
            return _generateList.Count;
        }

        public void AddGenerate(string tableName)
        {
            var s = _tableList.Find(sheet => sheet.SheetName.Equals(tableName));
            if (!_generateList.Contains(s))
            {
                _generateList.Add(s);
            }
        }

        public void RemoveGenerate(string tableName)
        {
            var s = _tableList.Find(sheet => sheet.SheetName.Equals(tableName));
            if (_generateList.Contains(s))
                _generateList.Remove(s);
        }

        public void Clear()
        {
            _generateList.Clear();
            _filesPath.Clear();
            _tableList.Clear();
        }
    }
}
