using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class TableManager
{
    /// <summary>
    /// 初始化所有表格
    /// </summary>
    /// <param name="GetTxt">根据表名返回Txt</param>
    public static void InitTxtTable(Dictionary<string,string> dictionary)
    {
        Type baseType = typeof(ITable);
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0, len = assemblies.Length; i < len; i++)
        {
            Type[] types = assemblies[i].GetTypes();
            for (int typeIdx = 0, typeLen = types.Length; typeIdx < typeLen; typeIdx++)
            {
                Type type = types[typeIdx];
                if (!baseType.IsAssignableFrom(type) || type.IsAbstract || type.IsInterface || type.IsGenericType) continue;
                string param = string.Empty;
                if (dictionary.ContainsKey(type.Name)) param = dictionary[type.Name];
                type.BaseType.InvokeMember("Init", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static,
                                  null, null, new object[] { param });
            }
        }
    }

}
