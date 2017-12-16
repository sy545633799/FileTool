using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.CodeDom;
using System.CodeDom.Compiler;

using Microsoft.CSharp;


public class ExcelConvter 
{
    public static object GetCellValue(ICell cell)
    {
        if (cell == null)
        {
            //Debug.Log("cell 为空");
            return "";
        }
        object value = "";
        try
        {
            if (cell.CellType != CellType.Blank)
            {
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        // Date comes here
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            value = cell.DateCellValue;
                        }
                        else
                        {
                            // Numeric type
                            value = cell.NumericCellValue;
                        }
                        break;
                    case CellType.Boolean:
                        // Boolean type
                        value = cell.BooleanCellValue;
                        break;
                    case CellType.Formula:
                        value = cell.NumericCellValue;
                        break;
                    default:
                        // String type
                        value = cell.StringCellValue;
                        break;
                }
            }
        }
        catch (Exception)
        {
            value = "";
            throw;
        }

        return value;
    }


    public static byte[] WriteObject(string type, string value)
    {
        MemoryStream ms = new MemoryStream();
        BinaryWriter bw = new BinaryWriter(ms);
        if (type == "int")
        {
            bw.Write((byte)1);
            bw.Write(int.Parse(value));
        }
        else if (type == "float")
        {
            bw.Write((byte)2);
            bw.Write(float.Parse(value));
        }
        else if (type == "table")
        {
            bw.Write((byte)3);
            string[] temp = value.Split(new string[] { "{", "}" }, System.StringSplitOptions.RemoveEmptyEntries);
            List<float[]> tempList = new List<float[]>();
            foreach (var item in temp)
            {
                string[] tempListV = item.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (tempListV.Length > 0)
                {
                    List<float> tempListFloat = new List<float>();
                    for (int i = 0; i < tempListV.Length; i++)
                    {
                        tempListFloat.Add(float.Parse(tempListV[i]));
                    }
                    tempList.Add(tempListFloat.ToArray());
                }
            }
            bw.Write(tempList.Count);
            foreach (var item in tempList)
            {
                bw.Write(item.Length);
                foreach (var item1 in item)
                {
                    bw.Write(item1);
                }
            }
        }
        else if (type == "float[]")
        {
            bw.Write((byte)4);
            string[] temp = value.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
            List<float> tempList = new List<float>();
            for (int i = 0; i < temp.Length; i++)
            {
                tempList.Add(float.Parse(temp[i]));
            }
            bw.Write(tempList.Count);
            foreach (var item in tempList)
            {
                bw.Write(item);
            }
        }
        else if (type == "int[]")
        {
            bw.Write((byte)5);
            string[] temp = value.Replace("，", ",").Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
            List<int> tempList = new List<int>();
            for (int i = 0; i < temp.Length; i++)
            {
                tempList.Add(int.Parse(temp[i]));
            }
            bw.Write(tempList.Count);
            foreach (var item in tempList)
            {
                bw.Write(item);
            }
        }
        else if (type == "string")
        {
            bw.Write((byte)6);
            bw.Write(value);
        }
        return ms.ToArray();
    }

    static Type ConvterToType(string type)
    {

        if (type.Contains("int[]"))
        {
            return typeof(int[]);
        }
        else if (type.Contains("float[]"))
        {
            return typeof(float[]);
        }
        else if (type.Contains("double[]"))
        {
            return typeof(double[]);
        }
        else if (type.Contains("string[]"))
        {
            return typeof(string[]);
        }

        else if (type.Contains("table"))
        {
            return typeof(float[][]);
        }

        else if (type.Contains("int"))
        {
            return typeof(int);
        }

        else if (type.Contains("float"))
        {
            return typeof(float);
        }

        else if (type.Contains("double"))
        {
            return typeof(double);
        }

        else if (type.Contains("string"))
        {
            return typeof(string);
        }
        return null;
    }




    public static void ReadExcelToCsharp(string execlePath, string generatePath,Action<string> serializableProgress)
    {

        foreach (var item in Directory.GetFiles(execlePath, "*.xlsx", SearchOption.AllDirectories))
        {
            string filePath = item;
            if (!File.Exists(filePath))
            {
                serializableProgress(filePath + "  文件不存在");
                return;
            }

            IWorkbook wk = null;
            string extension = Path.GetExtension(filePath);

            try
            {
                FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                if (extension.Equals(".xls"))
                {
                    //把xls文件中的数据写入wk中
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    wk = new XSSFWorkbook(fs);
                }
                fs.Close();
            }
            catch (Exception ex)
            {
                serializableProgress(ex.Message);
            }
            //读取当前表数据
            if (wk == null)
                continue;
            for (int i = 0; i < wk.NumberOfSheets; i++)
            {
                ISheet sheet = wk.GetSheetAt(i);
                if (sheet.SheetName.StartsWith("Sheet", StringComparison.CurrentCultureIgnoreCase))
                    continue;
                int cellFircount = sheet.GetRow(0).LastCellNum;
                int count = sheet.LastRowNum >= sheet.PhysicalNumberOfRows ? sheet.LastRowNum : sheet.PhysicalNumberOfRows;
                int classCount = 0;
                MemoryStream ms1 = new MemoryStream();
                BinaryWriter bw1 = new BinaryWriter(ms1);
                List<byte> tempDataList = new List<byte>();
                for (int j = 5; j < count; j++)
                {
                    if (sheet.GetRow(j) == null)
                        continue;
                    object objCell = GetCellValue(sheet.GetRow(j).GetCell(0));
                    if (objCell == null)
                            continue;
                    string id = objCell.ToString();
                    int number;
                    if(!int.TryParse(id, out number))
                    {
                        continue;
                    }
                    int cells = 0;
                    MemoryStream ms2 = new MemoryStream();
                    BinaryWriter bw2 = new BinaryWriter(ms2);
                    for (int m = 0; m < cellFircount; m++)
                    {
                        IRow row = sheet.GetRow(j);
                        if (row == null)
                        {
                            continue;
                        }
                        ICell cell = row.GetCell(m);
                        if (cell == null)
                        {
                            serializableProgress(sheet.SheetName + " 中  行：" + j + " 列：" + m + "  " + cell + " is null");
                            continue;
                        }
                        string fprototyType = GetCellValue(sheet.GetRow(4).GetCell(m)).ToString();
                        if(fprototyType.Contains("0"))
                        {
                            continue;
                        }
                        string cellValue = GetCellValue(sheet.GetRow(j).GetCell(m)).ToString();
                        string prototyName = GetCellValue(sheet.GetRow(0).GetCell(m)).ToString();
                        string prototyDesc = GetCellValue(sheet.GetRow(1).GetCell(m)).ToString();
                        string prototyType = GetCellValue(sheet.GetRow(2).GetCell(m)).ToString();
                        try
                        {
                            byte[] data = WriteObject(prototyType, cellValue);
                            if (data.Length > 0)
                            {
                                bw2.Write(prototyName);
                                bw2.Write(data);
                                cells++;
                            }
                        }
                        catch (Exception ex)
                        {

                            serializableProgress(sheet.SheetName + " 中  行：" + j + " 列：" + m + "  " + cellValue + "  |  " + prototyName + " | " + prototyDesc + " | " + prototyType + "" + ex.Message);
                        }

                    }
                    bw1.Write(cells);
                    bw1.Write(ms2.ToArray());
                    classCount++;
                }
                tempDataList.AddRange(BitConverter.GetBytes(classCount));
                tempDataList.AddRange(ms1.ToArray());
                File.WriteAllBytes(generatePath + "/" + sheet.SheetName + ".bin", tempDataList.ToArray());
            }
        }
      
    }

    public static void ConvterAllFileCsharp(string execlePath,string generatePath,string externalPath,Action<string> serializableProgress)
    {
        CSharpCodeProvider p1 = new CSharpCodeProvider();
        CodeNamespace sampleNamespace1 = new CodeNamespace();
        sampleNamespace1.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
        sampleNamespace1.Imports.Add(new CodeNamespaceImport("System.IO"));
        CodeTypeDeclaration Customerclass1 = new CodeTypeDeclaration("TableEntity");
        sampleNamespace1.Types.Add(Customerclass1);

        
        CodeMemberField GetTableListByKey = new CodeMemberField();
        GetTableListByKey.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
        GetTableListByKey.Name = "TableList";
        GetTableListByKey.Type = new CodeTypeReference("List<string>");
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        Customerclass1.Members.Add(GetTableListByKey);
        CodeMemberMethod ReadData = new CodeMemberMethod();
        ReadData.Name = "ReadData";
        ReadData.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
        ReadData.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "path"));
        ReadData.Statements.Add(new CodeSnippetStatement(File.ReadAllText(externalPath + "\\ReadData.txt")));
        Customerclass1.Members.Add(ReadData);
        Directory.Delete(generatePath, true);
        Directory.CreateDirectory(generatePath);
        string[] paths = Directory.GetFiles(execlePath);
        foreach(var filePath in paths)
        {
            if (!File.Exists(filePath))
            {
                serializableProgress(filePath + "  文件不存在");
                return;
            }
            IWorkbook wk = null;
            string extension = Path.GetExtension(filePath);

            try
            {
                FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                if (extension.Equals(".xls"))
                {
                    //把xls文件中的数据写入wk中
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    wk = new XSSFWorkbook(fs);
                }
                fs.Close();
                //读取当前表数据

                for (int i = 0; i < wk.NumberOfSheets; i++)
                {
                    ISheet sheet = wk.GetSheetAt(i);
                    if (sheet.SheetName.StartsWith("Sheet", StringComparison.CurrentCultureIgnoreCase))
                        continue;
                    int cellFircount = sheet.GetRow(0).LastCellNum;
                    int count = sheet.LastRowNum >= sheet.PhysicalNumberOfRows ? sheet.LastRowNum : sheet.PhysicalNumberOfRows;
                    CodeCompileUnit unit = new CodeCompileUnit();
                    CodeNamespace sampleNamespace = new CodeNamespace();
                    sampleNamespace.Imports.Add(new CodeNamespaceImport("System.IO"));
                    CSharpCodeProvider p = new CSharpCodeProvider();
                    CodeTypeDeclaration Customerclass = new CodeTypeDeclaration(sheet.SheetName);
                    Customerclass.BaseTypes.Add(new CodeTypeReference("BaseTable<" + sheet.SheetName + ">"));
                    CodeMemberMethod DecodeList = new CodeMemberMethod();
                    DecodeList.Name = "DecodeList";
                    DecodeList.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                    DecodeList.Parameters.Add(new CodeParameterDeclarationExpression("BinaryReader", "br"));
                    DecodeList.Statements.Add(new CodeSnippetStatement("int dataCount = br.ReadInt32();"));
                    DecodeList.Statements.Add(new CodeSnippetStatement("for (int i = 0; i < dataCount; i++){"));
                    DecodeList.Statements.Add(new CodeSnippetStatement(sheet.SheetName + " _" + sheet.SheetName.ToLower() + " = new " + sheet.SheetName + "();"));
                    DecodeList.Statements.Add(new CodeSnippetStatement("      _" + sheet.SheetName.ToLower() + ".ReadObject(br);"));
                    DecodeList.Statements.Add(new CodeSnippetStatement("     list.Add(_" + sheet.SheetName.ToLower() + ");"));
                    DecodeList.Statements.Add(new CodeSnippetStatement("     dictionary[_" + sheet.SheetName.ToLower() + ".GetID()] = _" + sheet.SheetName.ToLower() + ";}"));
                    Customerclass.Members.Add(DecodeList);

                    sb.Append("\""+sheet.SheetName + "\",");
                    sampleNamespace.Types.Add(Customerclass);
                    unit.Namespaces.Add(sampleNamespace);

                    for (int m = 0; m < cellFircount; m++)
                    {
                        string prototyName = GetCellValue(sheet.GetRow(0).GetCell(m)).ToString();
                        string prototyDesc = GetCellValue(sheet.GetRow(1).GetCell(m)).ToString();
                        string prototyType = GetCellValue(sheet.GetRow(2).GetCell(m)).ToString();
                        string filterPrototy = GetCellValue(sheet.GetRow(4).GetCell(m)).ToString();
                        if (string.IsNullOrEmpty(prototyName) || string.IsNullOrEmpty(prototyDesc) || string.IsNullOrEmpty(prototyType))
                        {
                            //Debug.Log(sheet.SheetName + " 列 " + i + " " + prototyName + " " + prototyDesc + " " + prototyType);
                            continue;
                        }
                        if (ConvterToType(prototyType) == null)
                            continue;

                        if (filterPrototy.Contains("0"))
                        {
                            continue;
                        }

                        if (m == 0)
                        {
                            CodeMemberMethod GetID = new CodeMemberMethod();
                            GetID.Name = "GetID";
                            GetID.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                            GetID.ReturnType = new CodeTypeReference(typeof(int));
                            GetID.Statements.Add(new CodeSnippetStatement("return " + GetCellValue(sheet.GetRow(0).GetCell(0)) + " ;"));
                            Customerclass.Members.Add(GetID);
                        }
                        CodeMemberProperty property = new CodeMemberProperty();
                        property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                        property.Name = prototyName;
                        property.HasGet = true;
                        property.Type = new CodeTypeReference(ConvterToType(prototyType));
                        property.Comments.Add(new CodeCommentStatement("<summary>", true));
                        property.Comments.Add(new CodeCommentStatement(prototyDesc, true));
                        property.Comments.Add(new CodeCommentStatement("</summary>", true));
                        property.GetStatements.Add(new CodeSnippetExpression("return (" + ConvterToType(prototyType).FullName + ")valueDic[\"" + prototyName + "\"]; "));
                        Customerclass.Members.Add(property);
                    }
                    CodeGeneratorOptions options = new CodeGeneratorOptions();
                    options.BracingStyle = "C";
                    options.BlankLinesBetweenMembers = true;
                    using (System.IO.StreamWriter sw = new StreamWriter(generatePath+"\\"+ sheet.SheetName + ".cs"))
                    {
                        p.GenerateCodeFromCompileUnit(unit, sw, options);
                    }
                }


            }
            catch (Exception ex)
            {
                //Debug.Log(ex.Message + "<>" + ex.StackTrace);
            }
        }
        sb.Length = sb.Length - 1;
        GetTableListByKey.InitExpression = new CodeSnippetExpression("new List<string>(){" + sb.ToString() + "}");
        CodeGeneratorOptions options1 = new CodeGeneratorOptions();
        options1.BracingStyle = "C";
        options1.BlankLinesBetweenMembers = true;
        using (System.IO.StreamWriter sw = new StreamWriter(generatePath + "\\TableEntity.cs"))
        {
            p1.GenerateCodeFromNamespace(sampleNamespace1, sw, options1);
        }
    }
}

