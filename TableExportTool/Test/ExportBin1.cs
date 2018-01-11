using Microsoft.CSharp;
using NPOI.SS.UserModel;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TableExportTool.Common;
using UnityEngine;
using OfficeOpenXml;

namespace TableExportTool.ExportBin
{
    class ExportBin1
    {
        public void Generate(Action<ISheet> progressCalback, GenerateType generateType, Action completeCallback, Action<string> serializableProgress)
        {
            if (!PathManager01.Instance.CheckConfigPath())
            {
                completeCallback();
                return;
            }

            var loopCount = 1;
            var fieldType = string.Empty;
            if ((int)generateType == 3)
            {
                loopCount = 2;
                fieldType = "1&2";
                PathManager01.Instance._filesPath.Add("1", new List<string>());
                PathManager01.Instance._filesPath.Add("2", new List<string>());
            }
            else if ((int)generateType == 2)
            {
                fieldType = "2";
                PathManager01.Instance._filesPath.Add("2", new List<string>());
            }
            else if ((int)generateType == 1)
            {
                fieldType = "1";
                PathManager01.Instance._filesPath.Add("1", new List<string>());
            }


            var spiltFied = fieldType.Split('&');

            if (!PathManager01.Instance.LoadBaseTable(spiltFied))
            {
                PathManager01.Instance._filesPath.Clear();
                completeCallback();
                return;
            }

            for (var i = 0; i < loopCount; i++)
            {
                if (spiltFied[i].Equals("1"))//客户端  
                {
                    //生成cs文件
                    GenerateTableStruct(progressCalback, spiltFied[i]);
                    //生成TableEntity文件
                    GenerateRunTimeTable(spiltFied[i]);
                    //编译
                    string dllOutPath = CompileCode(spiltFied[i], serializableProgress);//, Serializeble);

                    //客户端结构文件
                    if (PathManager01.Instance._configPathNew.ContainsKey(ConfigConst.ClientDllOutPath))
                    {
                        foreach (var s in PathManager01.Instance._configPathNew[ConfigConst.ClientDllOutPath])
                        {
                            File.Copy(dllOutPath, s + "\\DMNTableData.dll", true);
                        }
                    }
                    //客户端Bin
                    if (PathManager01.Instance._configPathNew.ContainsKey(ConfigConst.ClientBinOutPath))
                    {
                        Assembly ab = Assembly.Load(File.ReadAllBytes(dllOutPath));
                        foreach (var s in PathManager01.Instance._configPathNew[ConfigConst.ClientBinOutPath])
                        {
                            Serializeble(ab, s, serializableProgress);
                        }
                    }


                }
                else if (spiltFied[i].Equals("2"))//战斗服务器
                {
                    ////生成结构文件
                    //if (PathManager.Instance._configPathNew.ContainsKey(ConfigConst.BattleConfigFileOutPath))
                    //{
                    //    foreach (var s in PathManager.Instance._configPathNew[ConfigConst.BattleConfigFileOutPath])
                    //    {
                    //        ExcelConvter.ConvterAllFileCsharp(PathManager.Instance._configPath[ConfigConst.InputPath], s, System.Windows.Forms.Application.StartupPath + "\\ServerDepend", serializableProgress);
                    //    }
                    //}
                    ////生成Bin文件
                    //if (PathManager.Instance._configPathNew.ContainsKey(ConfigConst.BattleBinOutPath))
                    //{
                    //    foreach (var s in PathManager.Instance._configPathNew[ConfigConst.BattleBinOutPath])
                    //    {
                    //        ExcelConvter.ReadExcelToCsharp(PathManager.Instance._configPath[ConfigConst.InputPath], s, serializableProgress);
                    //    }
                    //}
                }
            }


            if (PathManager01.Instance.GenerateProtobuf)
            {
                if (PathManager01.Instance._configPath.ContainsKey(ConfigConst.ProtoBuffPath))
                {
                    string batPath = PathManager01.Instance._configPath[ConfigConst.ProtoBuffPath];
                    string eBatPath = Path.GetDirectoryName(batPath);
                    Environment.CurrentDirectory = eBatPath;
                    Process.Start(batPath);
                    Environment.CurrentDirectory = System.Windows.Forms.Application.StartupPath;
                }
            }

            completeCallback();
            PathManager01.Instance._filesPath.Clear();
        }


        private string CompileCode(string spiltFied, Action<string> serializableProgress)//, Action<Assembly,string, Action<string>> serializeble)
        {
            CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.GenerateExecutable = false;
            compilerParameters.IncludeDebugInformation = true;
            compilerParameters.ReferencedAssemblies.Add("UnityEngine.dll");
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("EPPlus.dll");
            compilerParameters.GenerateInMemory = false;
            string filePath = PathManager01.Instance.GenerateOutPath(ConfigConst.OutPath, spiltFied, string.Empty, "\\DMNTableData.dll");
            compilerParameters.CompilerOptions = "/doc:" + PathManager01.Instance.GenerateOutPath(ConfigConst.OutPath, spiltFied, string.Empty, "\\DMNTableDataDoc.xml");
            compilerParameters.OutputAssembly = filePath;
            CompilerResults cr = cSharpCodeProvider.CompileAssemblyFromFile(compilerParameters, PathManager01.Instance._filesPath[spiltFied].ToArray());
            for (int i = 0; i < cr.Errors.Count; i++)
            {
                if (!cr.Errors[i].IsWarning)
                {
                    MessageBox.Show(string.Format("表结构生成错误  {0} into {1}", cr.Errors[i], cr.PathToAssembly));
                    break;
                }
            }
            //Assembly ab = Assembly.Load(File.ReadAllBytes(filePath));
            //serializeble(ab,spiltFied, serializableProgress);
            return filePath;
        }

        /// <summary>
        /// 序列化 策划表中的 值
        /// </summary>
        /// <param name="ab"></param>
        private void Serializeble(Assembly ab, string exprotType, Action<string> serializableProgress)
        {

            StringBuilder sb = new StringBuilder();
            Dictionary<string, IList> dictionary = new Dictionary<string, IList>();

            foreach (var sheet in PathManager01.Instance._tableList)
            {

                Type t = ab.GetType(sheet.SheetName);
                if (null == t) return;
                Type list = typeof(List<>).MakeGenericType(t);
                IList objList = (IList)Activator.CreateInstance(list);
                dictionary[t.Name] = objList;

                int cellFircount = sheet.GetRow(0).LastCellNum;

                int count = sheet.LastRowNum >= sheet.PhysicalNumberOfRows
                    ? sheet.LastRowNum
                    : sheet.PhysicalNumberOfRows;

                for (int j = 5; j < count; j++)
                {
                    if (sheet.GetRow(j) == null)
                        continue;
                    object objCell = GetCellValue(sheet.GetRow(j).GetCell(0));
                    if (objCell == null)
                        continue;
                    string id = objCell.ToString();
                    int number;
                    if (!int.TryParse(id, out number))
                    {
                        continue;
                    }
                    object obj = Activator.CreateInstance(t);
                    for (int m = 0; m < cellFircount; m++)
                    {

                        IRow row = sheet.GetRow(j);
                        if (row == null)
                        {
                            continue;
                        }

                        try
                        {
                            string prototyType = GetCellValue(sheet.GetRow(4).GetCell(m)).ToString();//检测表头
                            if (prototyType.Contains("0")) continue;//略过策划备用字段
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(sheet.SheetName + "表[表头类型错误]第" + j + "行 第" + m + "列" + ex.StackTrace);
                        }

                        //取消字段值生成限制
                        //if (!(prototyType.Contains(exprotType) || prototyType.Contains("3")))
                        //{
                        //    continue;
                        //}

                        ICell cell = row.GetCell(m);
                        if (cell == null)
                        {
                            sb.AppendLine(sheet.SheetName + " 中  行：" + j + " 列：" + m + "  " + cell + " 空值");
                            serializableProgress(sheet.SheetName + " 中  行：" + j + " 列：" + m + "  " + cell + " 空值.");
                            continue;
                        }

                        string cellValue = string.Empty;

                        try
                        {
                            cellValue = GetCellValue(sheet.GetRow(j).GetCell(m)).ToString();

                            if (cellValue.Contains("\\n"))
                                cellValue = cellValue.Replace("\\n", "\n");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(sheet.SheetName + "出现值类型错误" + j + "行 第" + m + "列" + ex.StackTrace);
                        }

                        FieldInfo fieldInfo = t.GetField(GetCellValue(sheet.GetRow(0).GetCell(m)).ToString());
                        try
                        {
                            if (fieldInfo == null)
                                continue;
                            if (fieldInfo.FieldType.IsArray)
                            {

                                if (fieldInfo.FieldType.Name.Contains("[][]"))
                                {
                                    Type multi = typeof(List<>).MakeGenericType(typeof(float[]));
                                    IList multiList = (IList)Activator.CreateInstance(multi);
                                    StringBuilder sb1 = new StringBuilder();
                                    List<string> cacheList = new List<string>();
                                    bool cache = false;
                                    for (int k = 0; k < cellValue.Length; k++)
                                    {
                                        if (cellValue[k] == '{')
                                        {
                                            cache = true;
                                        }
                                        if (cellValue[k] == '}')
                                        {
                                            sb1.Append(cellValue[k]);
                                            cache = false;
                                            cacheList.Add(sb1.ToString());
                                            sb1.Remove(0, sb1.Length);
                                        }

                                        if (cache)
                                        {
                                            sb1.Append(cellValue[k]);
                                        }
                                    }

                                    for (int k = 0; k < cacheList.Count; k++)
                                    {
                                        string[] rank2 = cacheList[k].Trim('{', '}').Split(',');
                                        List<float> temp = new List<float>();
                                        for (int l = 0; l < rank2.Length; l++)
                                        {
                                            float v;
                                            if (float.TryParse(rank2[l], out v))
                                                temp.Add(v);
                                        }
                                        multiList.Add(temp.ToArray());
                                    }
                                    object objTemp = multi.GetMethod("ToArray").Invoke(multiList, new object[] { });
                                    fieldInfo.SetValue(obj, objTemp);
                                }
                                else
                                {
                                    Type lis1t = typeof(List<>).MakeGenericType(fieldInfo.FieldType.GetElementType());
                                    IList objList1 = (IList)Activator.CreateInstance(lis1t);
                                    string[] cellListValue = null;
                                    if (cellValue.Contains("，"))
                                    {
                                        cellListValue = cellValue.Split('，');
                                    }
                                    else
                                    {
                                        cellListValue = cellValue.Split(',');
                                    }
                                    if (null != cellListValue)
                                    {
                                        foreach (var itemsss in cellListValue)
                                        {
                                            objList1.Add(Convert.ChangeType(itemsss, fieldInfo.FieldType.GetElementType()));
                                        }
                                        object objTemp = lis1t.GetMethod("ToArray").Invoke(objList1, new object[] { });
                                        fieldInfo.SetValue(obj, objTemp);
                                    }
                                }
                            }
                            else
                            {
                                fieldInfo.SetValue(obj, Convert.ChangeType(cellValue, fieldInfo.FieldType));
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(ex.Message + "   " + sheet.SheetName + " 中  行：" + j + " 列：" + m + " 值:" + cellValue);
                            serializableProgress(ex.Message + "   " + sheet.SheetName + " 中  行：" + j + " 列：" + m + " 值:" + cellValue);
                        }
                    }
                    objList.Add(obj);
                }

            }

            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(dictionary.Count);
            foreach (var item in dictionary)
            {
                bw.Write(item.Key);
                sb.AppendLine(item.Key + "  " + item.Value.Count);
                bw.Write(item.Value.Count);
                foreach (object o in item.Value)
                {
                    Type tableType = ab.GetType(o.ToString());
                    MethodInfo tableMethod = tableType.GetMethod("Serialize");
                    tableMethod.Invoke(o, new[] { bw });
                }
            }
            bw.Close();


            File.WriteAllBytes(exprotType + "\\TableBin.bin", ms.ToArray());
            File.WriteAllText(exprotType + "\\log.txt", sb.ToString());

            //File.WriteAllBytes(GenerateOutPath(ConfigConst.OutPath,exprotType, string.Empty, "\\TableBin.bin"), ms.ToArray());
            //File.WriteAllText(GenerateOutPath(ConfigConst.OutPath,exprotType, string.Empty, "\\log.txt"), sb.ToString());

        }

        

        

        private void GenerateTableStruct(Action<ISheet> progressCalback, string generateType)
        {
            foreach (var sheet in PathManager01.Instance._generateList)
            {

                string xlsxFileName = string.Empty;
                foreach (var vk in PathManager01.Instance._pathMapping)
                {
                    if (vk.Value.Contains(sheet))
                    {
                        xlsxFileName = vk.Key;
                        break;
                    }
                }

                int cellFircount = sheet.GetRow(0).LastCellNum;
                
                var p = new CSharpCodeProvider();

                var Customerclass = new CodeTypeDeclaration(sheet.SheetName);
                Customerclass.CustomAttributes.Add(
                    new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializableAttribute))));
                Customerclass.BaseTypes.Add(new CodeTypeReference("BaseTable<" + sheet.SheetName + ">"));

                var xlsxMapping = new CodeMemberField();
                xlsxMapping.Attributes  = MemberAttributes.Public | MemberAttributes.Final;
                xlsxMapping.Name = "XlsxMapping";
                xlsxMapping.Type = new CodeTypeReference(typeof(string));
                xlsxMapping.InitExpression = new CodeSnippetExpression("\"" + xlsxFileName + "\"");
                Customerclass.Members.Add(xlsxMapping);

                var Serialize = new CodeMemberMethod();
                Serialize.Name = "Serialize";
                Serialize.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                Serialize.Parameters.Add(new CodeParameterDeclarationExpression("BinaryWriter", "bw"));
                var Deserialize = new CodeMemberMethod();
                Deserialize.Name = "Deserialize";
                Deserialize.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                Deserialize.Parameters.Add(new CodeParameterDeclarationExpression("BinaryReader", "br"));

                var DecodeList = new CodeMemberMethod();
                DecodeList.Name = "DecodeList";
                DecodeList.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                DecodeList.Parameters.Add(new CodeParameterDeclarationExpression("BinaryReader", "br"));
                DecodeList.Statements.Add(new CodeSnippetStatement("int dataCount = br.ReadInt32();"));
                DecodeList.Statements.Add(new CodeSnippetStatement("for (int i = 0; i < dataCount; i++){"));
                DecodeList.Statements.Add(
                    new CodeSnippetStatement(sheet.SheetName + " _" + sheet.SheetName.ToLower() + " = new " +
                                             sheet.SheetName + "();"));
                DecodeList.Statements.Add(
                    new CodeSnippetStatement("      _" + sheet.SheetName.ToLower() + ".Deserialize(br);"));
                DecodeList.Statements.Add(new CodeSnippetStatement("     list.Add(_" + sheet.SheetName.ToLower() + ");"));
                DecodeList.Statements.Add(
                    new CodeSnippetStatement("     dictionary[_" + sheet.SheetName.ToLower() + ".GetID()] = _" +
                                             sheet.SheetName.ToLower() + ";}"));
                DecodeList.Statements.Add(
                    new CodeSnippetStatement("TableEntity.TableListByKey[\"" + sheet.SheetName + "\"] = list;"));
                DecodeList.Statements.Add(
                    new CodeSnippetStatement("TableEntity.TableDicByKey[\"" + sheet.SheetName + "\"] =dictionary;"));
                Customerclass.Members.Add(DecodeList);

                var getKey = new CodeMemberMethod();
                getKey.Name = "GetKey";
                getKey.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                getKey.ReturnType = new CodeTypeReference(typeof(object));
                getKey.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "key"));
                getKey.Statements.Add(new CodeSnippetStatement(" switch(key){ "));

                var write = new CodeMemberMethod();
                write.Name = "Write";
                write.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                write.Parameters.AddRange(new[] { new CodeParameterDeclarationExpression(typeof(ExcelWorksheet), "ew"), new CodeParameterDeclarationExpression(typeof(int), "row") });

                var unit = new CodeCompileUnit();
                var sampleNamespace = new CodeNamespace();
                sampleNamespace.Imports.Add(new CodeNamespaceImport("System.IO"));
                sampleNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
                sampleNamespace.Imports.Add(new CodeNamespaceImport("System.Collections"));
                sampleNamespace.Types.Add(Customerclass);
                unit.Namespaces.Add(sampleNamespace);

                Customerclass.Members.Add(getKey);
                Customerclass.Members.Add(write);

                for (var m = 0; m < cellFircount; m++)
                {
                    var prototyName = GetCellValue(sheet.GetRow(0).GetCell(m)).ToString();
                    var prototyDesc = GetCellValue(sheet.GetRow(1).GetCell(m)).ToString();
                    var prototyType = GetCellValue(sheet.GetRow(2).GetCell(m)).ToString();
                    var filterPrototy = GetCellValue(sheet.GetRow(4).GetCell(m)).ToString();



                    if (string.IsNullOrEmpty(prototyName) || string.IsNullOrEmpty(prototyDesc) ||
                        string.IsNullOrEmpty(prototyType))
                    {
                        continue;
                    }
                    if (ConvterToType(prototyType) == null)
                        continue;

                    //略过策划备用字段
                    if (filterPrototy.Contains("0")) continue;

                    //取消字段结构生成限制
                    //if (!(filterPrototy.Contains(generateType) || filterPrototy.Contains("3")))
                    //{
                    //    continue;
                    //}

                    if (m == 0)
                    {
                        var GetID = new CodeMemberMethod();
                        GetID.Name = "GetID";
                        GetID.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                        GetID.ReturnType = new CodeTypeReference(typeof(int));
                        GetID.Statements.Add(
                            new CodeSnippetStatement("return " + GetCellValue(sheet.GetRow(0).GetCell(0)) + " ;"));
                        Customerclass.Members.Add(GetID);
                    }




                    var property = new CodeMemberField();
                    property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    string modifyName = prototyName;
                    if (prototyName.Contains(" ")) { modifyName = prototyName.Replace(" ", ""); }
                    property.Name = modifyName;
                    property.Type = new CodeTypeReference(ConvterToType(prototyType));
                    property.Comments.Add(new CodeCommentStatement("<summary>", true));
                    property.Comments.Add(new CodeCommentStatement(prototyDesc, true));
                    property.Comments.Add(new CodeCommentStatement("</summary>", true));
                    getKey.Statements.Add(new CodeSnippetStatement("case \"" + modifyName + "\":"));
                    getKey.Statements.Add(new CodeSnippetStatement(" return " + modifyName + ";"));
                    if (prototyType.Contains("[]"))
                    {
                        Serialize.Statements.Add(new CodeSnippetStatement(" bw.Write(" + property.Name + ".Length); "));
                        Serialize.Statements.Add(
                            new CodeSnippetStatement(" for(int i = 0;i<" + property.Name + ".Length;i++){ "));
                        Serialize.Statements.Add(new CodeSnippetStatement(" bw.Write(" + property.Name + "[i]); "));
                        Serialize.Statements.Add(new CodeSnippetStatement(" }"));
                        var newTypeName = ConvterToType(prototyType.Replace("[]", "")).Name;
                        Deserialize.Statements.Add(
                            new CodeSnippetStatement(property.Name + " = new " + prototyType.Replace("[]", "") +
                                                     "[br.ReadInt32()];"));
                        Deserialize.Statements.Add(
                            new CodeSnippetStatement(" for(int i = 0;i<" + property.Name + ".Length;i++){ "));
                        Deserialize.Statements.Add(
                            new CodeSnippetStatement(property.Name + "[i] = br.Read" + newTypeName + "(); "));
                        Deserialize.Statements.Add(new CodeSnippetStatement(" }"));
                        property.InitExpression = new CodeSnippetExpression("new " + prototyType.Replace("[]", "[0]"));

                        write.Statements.Add(new CodeSnippetStatement("string tempStr" + property.Name + " = string.Empty;"));
                        write.Statements.Add(new CodeSnippetStatement("for(int i = 0; i <" + property.Name + ".Length;++i)"));
                        write.Statements.Add(new CodeSnippetStatement("{"));
                        write.Statements.Add(new CodeSnippetStatement("tempStr" + property.Name + "+=" + property.Name + "[i].ToString()+\",\";"));
                        write.Statements.Add(new CodeSnippetStatement("}"));
                        write.Statements.Add(new CodeSnippetStatement("if(string.IsNullOrEmpty(tempStr" + property.Name + "))"));
                        write.Statements.Add(new CodeSnippetStatement("tempStr" + property.Name + "= \"-1\";"));
                        write.Statements.Add(new CodeSnippetStatement("ew.SetValue(row," + (m + 1) + "," + "tempStr" + property.Name + ".TrimEnd(','));"));

                    }
                    else
                    {
                        if (prototyType.Contains("table"))
                        {
                            Serialize.Statements.Add(new CodeSnippetStatement("bw.Write(" + property.Name + ".Length);"));
                            Serialize.Statements.Add(
                                new CodeSnippetStatement("for(int i = 0 ; i <" + property.Name + ".Length;i++){"));
                            Serialize.Statements.Add(
                                new CodeSnippetStatement("bw.Write(" + property.Name + "[i].Length);"));
                            Serialize.Statements.Add(
                                new CodeSnippetStatement("for(int j = 0 ; j <" + property.Name + "[i].Length;j++){"));
                            Serialize.Statements.Add(new CodeSnippetStatement("bw.Write(" + property.Name + "[i][j]); "));
                            Serialize.Statements.Add(new CodeSnippetStatement(" }"));
                            Serialize.Statements.Add(new CodeSnippetStatement(" }"));

                            Deserialize.Statements.Add(
                                new CodeSnippetStatement(property.Name + " = new float[br.ReadInt32()][];"));
                            Deserialize.Statements.Add(
                                new CodeSnippetStatement(" for(int i = 0;i<" + property.Name + ".Length;i++){ "));
                            Deserialize.Statements.Add(
                                new CodeSnippetStatement(property.Name + "[i] = new float[br.ReadInt32()];"));
                            Deserialize.Statements.Add(
                                new CodeSnippetStatement(" for(int j = 0;j<" + property.Name + "[i].Length;j++){ "));
                            Deserialize.Statements.Add(
                                new CodeSnippetStatement(property.Name + "[i][j] = br.ReadSingle(); "));
                            Deserialize.Statements.Add(new CodeSnippetStatement(" }"));
                            Deserialize.Statements.Add(new CodeSnippetStatement(" }"));

                            property.InitExpression = new CodeSnippetExpression("new float[0][]");



                            write.Statements.Add(new CodeSnippetStatement("string tempStr" + property.Name + " = \"-1\";"));
                            write.Statements.Add(new CodeSnippetStatement("for(int i = 0; i <" + property.Name + ".Length;++i)"));
                            write.Statements.Add(new CodeSnippetStatement("{"));
                            write.Statements.Add(new CodeSnippetStatement("tempStr" + property.Name + " = string.Empty;"));
                            write.Statements.Add(new CodeSnippetStatement("string str" + property.Name + " = string.Empty;"));
                            write.Statements.Add(new CodeSnippetStatement("tempStr" + property.Name + "+=\"{\";"));
                            write.Statements.Add(new CodeSnippetStatement("for(int j = 0; j <" + property.Name + "[i].Length;++j)"));
                            write.Statements.Add(new CodeSnippetStatement("{"));
                            write.Statements.Add(new CodeSnippetStatement("str" + property.Name + "+=" + property.Name + "[i][j].ToString()+\",\";"));
                            write.Statements.Add(new CodeSnippetStatement("}"));
                            write.Statements.Add(new CodeSnippetStatement("tempStr" + property.Name + "+=" + "str" + property.Name + ".TrimEnd(',')+" + "\"},\";"));
                            write.Statements.Add(new CodeSnippetStatement("}"));
                            write.Statements.Add(new CodeSnippetStatement("ew.SetValue(row," + (m + 1) + "," + "tempStr" + property.Name + ".TrimEnd(','));"));

                        }
                        else
                        {
                            if (prototyType == "string")
                            {
                                property.InitExpression = new CodeSnippetExpression("string.Empty");
                            }

                            write.Statements.Add(new CodeSnippetStatement("ew.SetValue(row," + (m + 1) + "," + property.Name + ");"));

                            Serialize.Statements.Add(new CodeSnippetStatement(" bw.Write(" + property.Name + "); "));
                            Deserialize.Statements.Add(
                                new CodeSnippetStatement(property.Name + " = br.Read" + ConvterToType(prototyType).Name +
                                                         "(); "));
                        }
                    }

                    Customerclass.Members.Add(property);
                }
                getKey.Statements.Add(new CodeSnippetStatement(" } "));
                getKey.Statements.Add(new CodeSnippetStatement(" return null;"));
                Customerclass.Members.Add(Serialize);
                Customerclass.Members.Add(Deserialize);
                var options = new CodeGeneratorOptions();
                options.BracingStyle = "C";
                options.BlankLinesBetweenMembers = true;
                string outPath = PathManager01.Instance.GenerateOutPath(ConfigConst.OutPathSources, generateType, "\\Table\\", sheet.SheetName + ".cs");
                using (var sw = new StreamWriter(outPath))
                {
                    p.GenerateCodeFromCompileUnit(unit, sw, options);
                    PathManager01.Instance._filesPath[generateType].Add(outPath);
                    progressCalback(sheet);
                }
            }
        }

        private void GenerateRunTimeTable(string spiltFied)
        {
            var p1 = new CSharpCodeProvider();

            var sampleNamespace1 = new CodeNamespace();
            sampleNamespace1.Imports.Add(new CodeNamespaceImport("System.IO"));
            sampleNamespace1.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            sampleNamespace1.Imports.Add(new CodeNamespaceImport("System.Collections"));

            var Customerclass1 = new CodeTypeDeclaration("TableEntity");
            Customerclass1.BaseTypes.Add(new CodeTypeReference(typeof(MonoBehaviour)));
            sampleNamespace1.Types.Add(Customerclass1);
            var GetClassMethod = new CodeMemberMethod();
            GetClassMethod.Name = "DeCodeObject";


            var writeData = new CodeMemberMethod();
            writeData.Name = "WriteDataToExecel";
            writeData.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            writeData.Parameters.AddRange(new[] { new CodeParameterDeclarationExpression(typeof(string), "execelPath"), new CodeParameterDeclarationExpression(typeof(string), "tableName") });
            writeData.Statements.Add(new CodeSnippetStatement(File.ReadAllText(System.Windows.Forms.Application.StartupPath + "\\WriteData.txt")));
            Customerclass1.Members.Add(writeData);

            var GetTableListByKey = new CodeMemberField();
            GetTableListByKey.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            GetTableListByKey.Name = "TableListByKey";
            GetTableListByKey.Type = new CodeTypeReference("Dictionary<string,IList>");
            GetTableListByKey.InitExpression = new CodeObjectCreateExpression(GetTableListByKey.Type);
            Customerclass1.Members.Add(GetTableListByKey);

            var GetTableDicByKey = new CodeMemberField();
            GetTableDicByKey.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            GetTableDicByKey.Name = "TableDicByKey";
            GetTableDicByKey.Type = new CodeTypeReference("Dictionary<string,IDictionary>");
            GetTableDicByKey.InitExpression = new CodeObjectCreateExpression(GetTableDicByKey.Type);
            Customerclass1.Members.Add(GetTableDicByKey);

            GetClassMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            GetClassMethod.Parameters.Add(new CodeParameterDeclarationExpression("BinaryReader", "br"));
            GetClassMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "name"));
            GetClassMethod.Statements.Add(new CodeSnippetStatement(" switch(name){ "));
            Customerclass1.Members.Add(GetClassMethod);


            var ReadData = new CodeMemberMethod();
            ReadData.Name = "ReadData";
            ReadData.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            ReadData.Parameters.Add(new CodeParameterDeclarationExpression(typeof(byte[]), "dataList"));
            ReadData.Statements.Add(new CodeSnippetStatement(File.ReadAllText(System.Windows.Forms.Application.StartupPath + "\\ReadData.txt")));
            Customerclass1.Members.Add(ReadData);


            foreach (var sheet in PathManager01.Instance._tableList)
            {
                GetClassMethod.Statements.Add(new CodeSnippetStatement(" case \"" + sheet.SheetName + "\": "));
                GetClassMethod.Statements.Add(new CodeSnippetStatement(sheet.SheetName + ".DecodeList(br); break; "));
            }

            GetClassMethod.Statements.Add(new CodeSnippetStatement(" } "));

            var options1 = new CodeGeneratorOptions();
            options1.BracingStyle = "C";
            options1.BlankLinesBetweenMembers = true;
            string outPath = PathManager01.Instance.GenerateOutPath(ConfigConst.OutPathSources, spiltFied, string.Empty, "\\TableEntity.cs");
            if (PathManager01.Instance._configPath.ContainsKey(ConfigConst.OutPathSources))
            {
                File.Copy(PathManager01.Instance._configPath[ConfigConst.DependPath], PathManager01.Instance._configPath[ConfigConst.OutPathSources] + "\\BaseTable.cs", true);
            }
            using (var sw = new StreamWriter(outPath))
            {
                p1.GenerateCodeFromNamespace(sampleNamespace1, sw, options1);
                PathManager01.Instance._filesPath[spiltFied].Add(outPath);
            }

        }


        private static Type ConvterToType(string type)
        {
            if (type.Contains("[]"))
            {
                if (type.Contains("int"))
                {
                    return typeof(int[]);
                }
                if (type.Contains("float"))
                {
                    return typeof(float[]);
                }
                if (type.Contains("double"))
                {
                    return typeof(double[]);
                }
                if (type.Contains("string"))
                {
                    return typeof(string[]);
                }
            }
            else
            {
                if (type.Contains("table"))
                {
                    return typeof(float[][]);
                }

                if (type.Contains("int"))
                {
                    return typeof(int);
                }

                if (type.Contains("float"))
                {
                    return typeof(float);
                }

                if (type.Contains("double"))
                {
                    return typeof(double);
                }

                if (type.Contains("string"))
                {
                    return typeof(string);
                }
            }
            return null;
        }

        private static object GetCellValue(ICell cell)
        {
            if (cell == null)
            {
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
                            if (DateUtil.IsCellDateFormatted(cell))
                            {
                                value = cell.DateCellValue;
                            }
                            else
                            {
                                value = cell.NumericCellValue;
                            }
                            break;
                        case CellType.Boolean:
                            value = cell.BooleanCellValue;
                            break;
                        case CellType.Formula:
                            value = cell.NumericCellValue;
                            break;
                        default:
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

        
    }
}
