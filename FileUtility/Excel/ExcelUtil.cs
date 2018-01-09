using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileUtility
{
    public class ExcelUtil
    {
        /// <summary>
        /// 读取文件夹下的所有Excel文件
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static Dictionary<string, List<ISheet>> LoadExcel(string paths)
        {
            var filePaths = Directory.GetFiles(paths, "*.xlsx", SearchOption.AllDirectories);
            Dictionary<string, List<ISheet>> dictionary = new Dictionary<string, List<ISheet>>();
            foreach (var path in filePaths)
            {
                if (File.Exists(path))
                {
                    IWorkbook wk = null;
                    var extension = Path.GetExtension(path);
                    string fileName = Path.GetFileName(path);

                    using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        try
                        {
                            if (extension != null)
                            {
                                if (extension.Equals(".xls"))
                                    wk = new HSSFWorkbook(fs);
                                else
                                    wk = new XSSFWorkbook(fs);
                            }
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    if (wk != null)
                    {
                        for (var i = 0; i < wk.NumberOfSheets; i++)
                        {
                            var sheet = wk.GetSheetAt(i);
                            if (sheet.SheetName.StartsWith("Sheet", StringComparison.CurrentCultureIgnoreCase))
                                continue;
                            if (dictionary.ContainsKey(fileName))
                                dictionary[fileName].Add(sheet);
                            else
                                dictionary[fileName] = new List<ISheet>() { sheet };
                        }
                    }
                }
            }
            return dictionary;
        }

    }
}
