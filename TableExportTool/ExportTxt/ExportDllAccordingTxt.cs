using FileUtility;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableExportTool;

public partial class ExportUtil
{
    /// <summary>
    /// 根据
    /// </summary>
    public static string ExportDllAccordingTxt(string sourcePath, string outputPath)
    {
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);
        DirectoryHelper.DelectContent(outputPath);

        CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
        CompilerParameters compilerParameters = new CompilerParameters();
        compilerParameters.GenerateExecutable = false;
        compilerParameters.IncludeDebugInformation = true;
        compilerParameters.ReferencedAssemblies.Add("UnityEngine.dll");
        compilerParameters.ReferencedAssemblies.Add("System.dll");
        compilerParameters.ReferencedAssemblies.Add("EPPlus.dll");
        compilerParameters.GenerateInMemory = false;

        compilerParameters.CompilerOptions = "/doc:" + Path.Combine(outputPath, "TableDataDoc.xml");
        compilerParameters.OutputAssembly = Path.Combine(outputPath, "TableTool.dll");
        string[] sources = Directory.GetFiles(sourcePath);
        CompilerResults cr = cSharpCodeProvider.CompileAssemblyFromFile(compilerParameters, sources);
        for (int i = 0; i < cr.Errors.Count; i++)
        {
            if (!cr.Errors[i].IsWarning)
            {
                Form1.Log(false, string.Format("表结构生成错误  {0} into {1}", cr.Errors[i], cr.PathToAssembly));
                break;
            }
        }

        return compilerParameters.OutputAssembly;
    }
}
