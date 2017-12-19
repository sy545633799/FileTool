using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Data
{
    public class LoadTable
    {
        // 加载表格
        public void Load()
        {
            Type baseType = typeof(ITable);
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0, len = assemblies.Length; i < len; i++)
            {
                Type[] types = assemblies[i].GetTypes();
                for (int typeIdx = 0, typeLen = types.Length; typeIdx < typeLen; typeIdx++)
                {
                    Type type = types[typeIdx];
                    if (!baseType.IsAssignableFrom(type) || type.IsAbstract || type.IsInterface) continue;

                    Activator.CreateInstance(type);
                }
            }
        }
    }
}
