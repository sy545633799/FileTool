
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableExportTool;

public static class NPOIHelper
{
    public static object GetCellValue(this ICell cell)
    {
        if (cell == null) return "";

        object value = "";
        if (cell.CellType != CellType.Blank)
        {

            switch (cell.CellType)
            {
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                        value = cell.DateCellValue;
                    else
                        value = cell.NumericCellValue;
                    break;
                //case CellType.Boolean:
                //    // Boolean type
                //    value = cell.BooleanCellValue;
                //    break;
                //case CellType.Formula:
                //    value = cell.NumericCellValue;
                //    break;
                default:
                    value = cell.StringCellValue;
                    break;
            }

        }
        return value;
    }
}
