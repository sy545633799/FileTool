using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileUtility
{
    public class ExcelConvert
    {
        /// <summary>
        /// 导出成txt
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="outputPath"></param>
        public static void ToTxt(string excelPath, string outputPath, Action onComplete = null)
        {
            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);

            Dictionary<string, List<ISheet>> dictionary = ExcelUtil.LoadExcel(excelPath);
            ToTxt(dictionary, outputPath, onComplete);
        }

        public static void ToTxt(Dictionary<string, List<ISheet>> dictionary, string outputPath, Action onComplete)
        {
            foreach (var table in dictionary.Values)
            {
                foreach (var sheet in table)
                {
                    try
                    {
                        string textPath = Path.Combine(outputPath, sheet.SheetName + ".txt");
                        for (int i = 0; i < sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            string line = "";
                            for (int j = 0; j < row.LastCellNum; j++)
                            {
                                ICell cell = row.GetCell(j);
                                line += NPOIUtil.GetCellValue(cell).ToString() + (j == row.LastCellNum - 1 ? "" : "\t");
                            }
                            TxtUtil.WriteToPath(textPath, line, true);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            onComplete?.Invoke();
        }

        /// <summary>
        /// 导出成一个Bin
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="outputPath"></param>
        public void ToSingleBin(string excelPath, string outputPath)
        {

        }

        /// <summary>
        /// 导出成多个Bin
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="outputPath"></param>
        public void ToMultipleBin(string excelPath, string outputPath)
        {

        }

    }
}
