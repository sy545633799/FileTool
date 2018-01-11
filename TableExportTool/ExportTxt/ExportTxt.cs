using FileUtility;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TableExportTool;

public partial class ExportUtil
{
    /// <summary>
    /// 导出成txt
    /// </summary>
    /// <param name="excelPath"></param>
    /// <param name="outputPath"></param>
    public static void ExportTxt(string excelPath, string outputPath, Action<bool, string> onProgress = null)
    {
        Dictionary<string, List<ISheet>> dictionary = ExcelHelper.LoadExcel(excelPath);
        ExportTxt(dictionary, outputPath);
    }

    public static void ExportTxt(Dictionary<string, List<ISheet>> dictionary, string outputPath)
    {
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);
        DirectoryHelper.DelectContent(outputPath);

        foreach (var table in dictionary)
        {
            foreach (var sheet in table.Value)
            {
                try
                {
                    string textPath = Path.Combine(outputPath, sheet.SheetName + ".txt");
                    for (int i = 0; i < sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null)
                            Form1.Log(false, table.Key + "\t" + sheet.SheetName + "\t" + string.Format("第{0}行出错", i + 1) + "\n");
                        else
                        {
                            string line = "";
                            for (int j = 0; j < row.LastCellNum; j++)
                            {
                                ICell cell = row.GetCell(j);
                                try
                                {
                                    cell.GetCellValue();
                                    line += cell.GetCellValue().ToString().Replace("\n", "\\n").Replace("\n\r", "\\n") + (j == row.LastCellNum - 1 ? "" : "\t");
                                }
                                catch (Exception e)
                                {
                                    Form1.Log(false, table.Key + "\t" + sheet.SheetName + "\t" + string.Format("第{0}行第{1}列", i + 1, j + 1) + "\t" + e.Message + "\n");
                                    break;
                                }
                            }
                            TxtUtil.WriteToPath(textPath, line, true);
                        }
                    }
                    Form1.Log(true, sheet.SheetName + "\t表文件生成---------------->\tOK\n");
                }
                catch (Exception e)
                {
                    Form1.Log(false, sheet.SheetName + "\t表文件生成---------------->\tFlase\t" + e.Message + "\n");
                }
            }
        }
    }

}