using System.IO;
using System.Collections.Generic;
using System;

public interface IBaseTable
{
   void Serialize(BinaryWriter bw);
   void Deserialize(BinaryReader br);

   void Write(OfficeOpenXml.ExcelWorksheet ew,int row);

   float[][] GetTwoRankTable(string key);

    /// <summary>
    /// 取int类型的值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    int GetInt(string key);

    /// <summary>
    /// 取float类型的值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    float Getfloat(string key);

    /// <summary>
    /// 取string 类型的值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    string GetString(string key);

    object GetOneRankTable(string key);

    
}

public abstract class BaseBinTable<T>:IBaseTable
{
    public static List<T> list = new List<T>();
    public static Dictionary<int, T> dictionary = new Dictionary<int, T>();

    public static T Get(int key)
    {
        T t = default(T);
        dictionary.TryGetValue(key, out t);
        return t;
    }

    public virtual int GetID() {
        return -1;
    }

    public virtual object GetKey(string key)
    {
        return null;
    }

    public abstract void Serialize(BinaryWriter bw);
    public abstract void Deserialize(BinaryReader br);

    /// <summary>
    /// 取2唯数组类型的值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public float[][] GetTwoRankTable(string key)
    {
        return (float[][])GetKey(key);
    }

    /// <summary>
    /// 取int类型的值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int GetInt(string key)
    {
        return  (int)GetKey(key);
    }

    /// <summary>
    /// 取float类型的值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public float Getfloat(string key)
    {
        return (float)GetKey(key);
    }

    /// <summary>
    /// 取string 类型的值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetString(string key)
    {
        return (string)GetKey(key);
    }

    /// <summary>
    /// 取1维数类型的值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object GetOneRankTable(string key)
    {
        return GetKey(key);
    }

    public virtual void Write(OfficeOpenXml.ExcelWorksheet ew,int row)
    {
        
    }
}
