using FileUtility;
using Microsoft.CSharp;
using NPOI.SS.UserModel;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using TableExportTool;

public partial class ExportUtil
{
    /// <summary>
    /// 导出cs文件
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="outFolderPath"></param>
    public static void ExportCSAccordingMultipleTxt(Dictionary<string, List<ISheet>> dictionary, string outFolderPath)
    {
        if (!Directory.Exists(outFolderPath))
            Directory.CreateDirectory(outFolderPath);
        DirectoryHelper.DelectContent(outFolderPath);

        foreach (var sheets in dictionary)
        {
            foreach (var sheet in sheets.Value)
            {
                var customerclass = new CodeTypeDeclaration(sheet.SheetName);
                //给类添加属性
                //customerclass.CustomAttributes.Add(
                //    new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializableAttribute))));
                //添加父类
                customerclass.BaseTypes.Add(new CodeTypeReference("BaseTxtTable<" + sheet.SheetName + ">"));

                var xlsxMapping = new CodeMemberField();
                xlsxMapping.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                xlsxMapping.Name = "XlsxMapping";
                xlsxMapping.Type = new CodeTypeReference(typeof(string));
                //默认值 public string XlsxMapping = "sheetsName.xlsx";
                xlsxMapping.InitExpression = new CodeSnippetExpression("\"" + sheets.Key + "\"");
                customerclass.Members.Add(xlsxMapping);

                //根据Excel成员生成类
                for (int i = 0; i < sheet.GetRow(0).LastCellNum; i++)
                {
                    string prototyName = sheet.GetRow(0).GetCell(i).GetCellValue().ToString();
                    string prototyDesc = sheet.GetRow(1).GetCell(i).GetCellValue().ToString();
                    string prototyType = sheet.GetRow(2).GetCell(i).GetCellValue().ToString();
                    string filterPrototy = sheet.GetRow(4).GetCell(i).GetCellValue().ToString();

                    //略过策划备用字段
                    if (filterPrototy.Contains("0")) continue;

                    //生成GetId方法
                    if (i == 0)
                    {
                        var GetID = new CodeMemberMethod();
                        GetID.Name = "GetID";
                        GetID.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                        GetID.ReturnType = new CodeTypeReference(typeof(int));
                        GetID.Statements.Add(
                            new CodeSnippetStatement("return " + sheet.GetRow(0).GetCell(0).GetCellValue() + " ;")); //第一行第一列（ID）
                        customerclass.Members.Add(GetID);
                    }
                    //生成字段
                    var property = new CodeMemberField();
                    property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    string modifyName = prototyName;
                    if (prototyName.Contains(" ")) { modifyName = prototyName.Replace(" ", ""); }
                    property.Name = modifyName;
                    property.Type = new CodeTypeReference(ConvterToType(prototyType));
                    property.Comments.Add(new CodeCommentStatement("<summary>", true));
                    property.Comments.Add(new CodeCommentStatement(prototyDesc, true));
                    property.Comments.Add(new CodeCommentStatement("</summary>", true));
                    customerclass.Members.Add(property);
                }

                //自定义命名空间
                CodeNamespace sampleNamespace = new CodeNamespace();
                sampleNamespace.Imports.Add(new CodeNamespaceImport("System.IO"));
                sampleNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
                sampleNamespace.Imports.Add(new CodeNamespaceImport("System.Collections"));
                //将类加入命名空间
                sampleNamespace.Types.Add(customerclass);
                //将命名空间加入容器
                CodeCompileUnit unit = new CodeCompileUnit();
                unit.Namespaces.Add(sampleNamespace);

                //设置代码样式
                var options = new CodeGeneratorOptions();
                options.BracingStyle = "C";
                options.BlankLinesBetweenMembers = true;

                try
                {
                    using (var sw = new StreamWriter(Path.Combine(outFolderPath, sheet.SheetName + ".cs")))
                    {
                        CSharpCodeProvider p = new CSharpCodeProvider();
                        p.GenerateCodeFromCompileUnit(unit, sw, options);
                    }
                    Form1.Log(true, sheet.SheetName + "\t表结构生成---------------->\tOK\n");
                }
                catch
                {
                    Form1.Log(false, sheet.SheetName + "\t表结构生成---------------->\tFalse\n");
                }

            }
        }
    }

    private static Type ConvterToType(string prototyType)
    {
        if(prototyType.Contains(",") || prototyType.Contains("{") || prototyType.Contains("["))
            return typeof(string);
        if (prototyType.Contains("int"))
            return typeof(int);
        else if (prototyType.Contains("float"))
            return typeof(float);
        else
            return typeof(string);
    }
}
