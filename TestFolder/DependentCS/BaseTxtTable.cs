using System;
using System.Collections.Generic;
using System.Reflection;

public interface ITxtTable { }

public abstract class BaseTxtTable<T> : ITxtTable where T : BaseTxtTable<T>, new()
{
    private const int titleRows = 5;
    public static List<T> list;
    public static Dictionary<int, T> dictionary;

    public static void Init(string txt)
    {
        if (string.IsNullOrEmpty(txt)) return;
        string[] lines = txt.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= titleRows) return;

        list = new List<T>();
        dictionary = new Dictionary<int, T>();
        Dictionary<FieldInfo, int> propertyInfos = GetPropertyInfos(lines[0]);

        for (int i = titleRows; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]) || string.IsNullOrWhiteSpace(lines[i])) continue;
            T t = ParseObject(lines[i], propertyInfos);
            list.Add(t);
            dictionary[t.GetID()] = t;
         }
    }

    #region 解析CSV

    private static Dictionary<FieldInfo, int> GetPropertyInfos(string memberLine)
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
            propertyInfos.Add(fieldInfo, (index == -1) ? i : index);
        }

        return propertyInfos;
    }

    private static T ParseObject(string line, Dictionary<FieldInfo, int> propertyInfos)
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
            ParsePropertyValue(obj, fieldInfo, value);
        }

        return obj;
    }

    private static void ParsePropertyValue(T obj, FieldInfo fieldInfo, string valueStr)
    {
        System.Object value = null;
        if (fieldInfo.FieldType.IsEnum)
            value = Enum.Parse(fieldInfo.FieldType, valueStr);
        else
        {
            try
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
            catch
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("table paser error:" + typeof(T).Name + "\t fieldname:" + fieldInfo.Name + "\t value:" + valueStr);
#endif
            }
        }

        if (value == null)
            return;

        fieldInfo.SetValue(obj, value);
    }

    private static int IndexOf(string[] list, string value)
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
#endregion

    public static T Get(int Id)
    {
        T t = default(T);
        dictionary.TryGetValue(Id, out t);
        return t;
    }


    public abstract int GetID();
}
