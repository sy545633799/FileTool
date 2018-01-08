using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TableExportTool
{
    public class ExcelManager
    {
        public static List<ISheet> _tableList = new List<ISheet>();

        public static List<ISheet> LoadExcel()
        {
            var filePaths = Directory.GetFiles(PathManager.filePath.ImportPath, "*.xlsx", SearchOption.AllDirectories);
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
                        }
                    }
                }
            }

            return _tableList;
        }
    }
}
