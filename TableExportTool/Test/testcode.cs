using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System;

namespace CodeDomDemo1
{
    class Program
    {
        public static void Test()
        {
            Program pro = new Program();
            CodeNamespace nspace = pro.CreateCodeDomHelloDemo();
            Console.WriteLine(pro.GengerCode(nspace));
            string filename = "HelloWorld.exe";
            CompilerResults result = pro.Execute(nspace, filename);
            if (result.Errors.HasErrors)//是否存在错误；
            {
                for (int i = 0; i < result.Output.Count; i++)

                    Console.WriteLine(result.Output[i]);

                for (int i = 0; i < result.Errors.Count; i++)

                    Console.WriteLine(i.ToString() + ": " + result.Errors[i].ToString());
            }
            else
            {
                System.Diagnostics.Process.Start(filename);//这里比较懒，不想动手去自己打开，呵呵；
            }
            Console.Read();
        }

        public CodeNamespace CreateCodeDomHelloDemo()
        {
            CodeMemberMethod method = new CodeMemberMethod();//方法声明； 
            method.Name = "SayHello";//  方法名
            method.Attributes = MemberAttributes.Public | MemberAttributes.Final;//属性
            method.ReturnType = new CodeTypeReference(typeof(string));//返回类型
            method.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression("Hello  from code!")));//方法体，只有一句返回语句return "Hello  from code!";

            CodeEntryPointMethod main = new CodeEntryPointMethod();//主方法Main
            main.Statements.Add(new CodeVariableDeclarationStatement("HelloWord", "hw",
                new CodeObjectCreateExpression("HelloWord", new CodeExpression[] { })));//变量声明：HelloWord hw = new HelloWord();

            CodeMethodInvokeExpression methodinvoke = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("hw"), "SayHello", new CodeExpression[] { });
            main.Statements.Add(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("System.Console"), "WriteLine", methodinvoke));
            main.Statements.Add(new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("System.Console"), "Read"));//两个方法调用：System.Console.WriteLine(hw.SayHello());

            CodeTypeDeclaration helloword = new CodeTypeDeclaration("HelloWord");//类型Class声明
            helloword.Attributes = MemberAttributes.Public;
            helloword.Members.AddRange(new CodeTypeMember[] { method, main });//添加方法到clss

            CodeNamespace nspace = new CodeNamespace("HelloDemo1");//命名空间声明
            nspace.Imports.Add(new CodeNamespaceImport("System"));//引入程序命名空间：using System；
            nspace.Types.Add(helloword);//
            return nspace;
        }

        public string GengerCode(CodeNamespace nspace)
        {
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            CodeGeneratorOptions geneOptions = new CodeGeneratorOptions();//代码生成选项

            geneOptions.BlankLinesBetweenMembers = false;

            geneOptions.BracingStyle = "C";

            geneOptions.ElseOnClosing = true;

            geneOptions.IndentString = "    ";
            CodeDomProvider.GetCompilerInfo("c#").CreateProvider().GenerateCodeFromNamespace(nspace, sw, geneOptions);//代码生成
            sw.Close();
            return sb.ToString();

        }

        public CompilerResults Execute(CodeNamespace nspace, string filename)
        {
            CodeCompileUnit unit = new CodeCompileUnit();//code编译单元
            unit.Namespaces.Add(nspace);
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
            CompilerParameters options = new CompilerParameters();//

            options.GenerateInMemory = false;//是否在内存中生成；
            options.IncludeDebugInformation = true;// 包含调试信息；
            options.ReferencedAssemblies.Add("System.dll");
            options.OutputAssembly = filename;
            if (System.IO.Path.GetExtension(filename).ToLower() == ".exe")
            {
                options.GenerateExecutable = true;//true为可执行exe，false：dll
            }
            else
            {
                options.GenerateExecutable = false;//true为可执行exe，false：dll
            }
            return provider.CompileAssemblyFromDom(options, unit);//编译程序集
        }

    }
}