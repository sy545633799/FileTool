using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class PathHelper
{
        //获取包含清单的已加载文件的路径或 UNC 位置。
        public static string sApplicationPath = Assembly.GetExecutingAssembly().Location;

    //result: X:\xxx\xxx\xxx.dll (.dll文件所在的目录+.dll文件名)



    //获取当前进程的完整路径，包含文件名(进程名)。

    //string str1 = this.GetType().Assembly.Location;

    //result: X:\xxx\xxx\xxx.exe (.exe文件所在的目录+.exe文件名)



    //获取新的 Process 组件并将其与当前活动的进程关联的主模块的完整路径，包含文件名(进程名)。

    string str2 = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

    //result: X:\xxx\xxx\xxx.exe (.exe文件所在的目录+.exe文件名)



    //获取和设置当前目录（即该进程从中启动的目录）的完全限定路径。

    string str3 = System.Environment.CurrentDirectory;

    //result: X:\xxx\xxx (.exe文件所在的目录)



    //获取当前 Thread 的当前应用程序域的基目录，它由程序集冲突解决程序用来探测程序集。

    string str4 = System.AppDomain.CurrentDomain.BaseDirectory;

    //result: X:\xxx\xxx\ (.exe文件所在的目录+"\")



    //获取和设置包含该应用程序的目录的名称。

    string str5 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

    //result: X:\xxx\xxx\ (.exe文件所在的目录+"\")



    ////获取启动了应用程序的可执行文件的路径，不包括可执行文件的名称。

    //string str6 = System.Windows.Forms.Application.StartupPath;

    ////result: X:\xxx\xxx (.exe文件所在的目录)



    ////获取启动了应用程序的可执行文件的路径，包括可执行文件的名称。

    //string str7 = System.Windows.Forms.Application.ExecutablePath;

    ////result: X:\xxx\xxx\xxx.exe (.exe文件所在的目录+.exe文件名)



    //获取应用程序的当前工作目录(不可靠)。

    string str8 = System.IO.Directory.GetCurrentDirectory();

    //result: X:\xxx\xxx (.exe文件所在的目录)
}
