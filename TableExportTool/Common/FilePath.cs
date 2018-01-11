using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableExportTool
{
    [Serializable]
    public class FilePath
    {
        public string ImportPath;                                 //excel路径
        public string DependentFilePath;                          //依赖文件(tablebase)路径
        public string OutputPath;                                 //源文件输出路径
        public List<string> OutPutDll = new List<string>();       //输出dll路径
        public List<string> OutPutBin = new List<string>();       //输出bin文件路径
    }
}
