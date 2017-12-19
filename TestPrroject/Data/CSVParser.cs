using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestProject.Data
{
    class CSVParser
    {
        static AssetBundle tabelBundle = null;

        static void ParsePropertyValue<T>(T obj, FieldInfo fieldInfo, string valueStr)
        {
            System.Object value = null;
            if (fieldInfo.FieldType.IsEnum)
                value = Enum.Parse(fieldInfo.FieldType, valueStr);
            else
            {
                if (fieldInfo.FieldType == typeof(int))
                    value = int.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(float))
                    value = float.Parse(valueStr);
                else if (fieldInfo.FieldType == typeof(double))
                    value = double.Parse(valueStr);
                else
                    value = valueStr.Replace("\\n", "\n");
            }

            if (value == null)
                return;

            fieldInfo.SetValue(obj, value);
        }

        static T ParseObject<T>(string line, Dictionary<FieldInfo, int> propertyInfos)
        {
            T obj = Activator.CreateInstance<T>();
            string[] values = line.Split('\t');

            List<FieldInfo> fieldInfos = new List<FieldInfo>(propertyInfos.Keys);
            for (int j = 0; j < fieldInfos.Count; j++)
            {
                FieldInfo fieldInfo = fieldInfos[j];
                string value = values[propertyInfos[fieldInfo]];
                if (string.IsNullOrEmpty(value))
                    continue;

                try
                {
                    ParsePropertyValue(obj, fieldInfo, value);
                }
                catch (Exception ex)
                {
                    Debug.LogError("line:" + line + " for: " + fieldInfo.Name);
                    throw ex;
                }
            }

            return obj;
        }

        static int IndexOf(string[] list, string value)
        {
            int index = -1;

            for (int i = 0, len = list.Length; i < len; i++)
            {
                if (list[i] == value)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        static Dictionary<FieldInfo, int> GetPropertyInfos<T>(string memberLine)
        {
            Dictionary<FieldInfo, int> propertyInfos = new Dictionary<FieldInfo, int>();
            Type objType = typeof(T);

            string[] members = memberLine.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            FieldInfo[] fieldInfos = new FieldInfo[members.Length];
            for (int i = 0; i < members.Length; i++)
            {
                FieldInfo fieldInfo = objType.GetField(members[i]);
                int index = -1;
                if (fieldInfo == null)
                    continue;

                //if (fieldInfo.FieldType == typeof(string))
                //{
                //    string langName = fieldInfo.Name + "_" + Global.Language.ToString();
                //    index = IndexOf(members, langName);
                //}

#if UNITY_EDITOR
            if (propertyInfos.ContainsKey(fieldInfo))
            {
                Debug.LogError("Table Error MemberLine: " + memberLine);
                continue;
            }
#endif
                propertyInfos.Add(fieldInfo, (index == -1) ? i : index);
            }

            return propertyInfos;
        }

        // parse a data array from the table data.   
        static public T[] Parse<T>(string name)
        {
            TextAsset textAsset = new TextAsset();
            if (textAsset == null)
            {
                Debug.LogError("无法加载表格文件：" + name);
                return null;
            }
            string text = textAsset.text;

            // try parse the table lines.
            string[] lines = text.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 3)
            {
                Debug.LogError("表格文件行数错误，【1】属性名称【2】变量名称【3-...】值：" + name);
                return null;
            }

            Dictionary<FieldInfo, int> propertyInfos = GetPropertyInfos<T>(lines[1]);

            T[] array = new T[lines.Length - 2];
            for (int i = 0; i < lines.Length - 2; i++)
                array[i] = ParseObject<T>(lines[i + 2], propertyInfos);

            return array;
        }
    }
}
