using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility
{
    public class TxtUtil
    {
        public static void Clear(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Seek(0, SeekOrigin.Begin);
                fs.SetLength(0);
            }
        }

        #region string
        public static void WriteToPath(string path, string data, bool append = false)
        {
            #region 其他方法
            //File类写入
            //方式一
            //File.AppendAllLines(path, new string[] { data });
            //方式二
            //File.AppendAllText(path, data + "\r\n"); 

            //FileStream写入    
            //FileMode mode = File.Exists(path) ? FileMode.Append : FileMode.Create;
            //using (FileStream fsWrite = new FileStream(path, mode, FileAccess.Write))
            //{
            //    data = mode == FileMode.Append ? "\r\n" + data : data;
            //    byte[] buffer = Encoding.UTF8.GetBytes(data);
            //    fsWrite.Write(buffer, 0, buffer.Length);
            //}
#endregion
            //StreamWrite写入文件 (bool 参数表示是否append) 
            using (StreamWriter writer = new StreamWriter(path, append))
            {
                writer.WriteLine(data);
            }
        }

        public static string ReadFromPath(string filepath)
        {
            string data = "";
            #region 替他方法
            //读取  
            //string data = File.ReadAllText(filepath);
            //string[] datas = File.ReadAllLines(filepath);
            ////写入   
            //string[] ss = new string[] { "arr1", "arr2" };
            //File.WriteAllLines(filepath, ss);
            //File.WriteAllText(filepath, "\r\n换行\r\n");
            ////附加
            //File.AppendAllLines(filepath, ss);
            //string str = "apend";
            //File.AppendAllText(filepath, str);
#endregion
            //SreamRead读取文件  
            using (StreamReader reader = new StreamReader(filepath))
            {
                while (!reader.EndOfStream)
                    data = reader.ReadLine();
            }
            return data;
        }

        public static Dictionary<string, string> ReadAllFile(string directorypath)
        {
            string[] filePaths = Directory.GetFiles(directorypath, "*.txt", SearchOption.AllDirectories);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < filePaths.Length; i++)
                dictionary[Path.GetFileNameWithoutExtension(filePaths[i])] = File.ReadAllText(filePaths[i]);
            return dictionary;
        }

        #endregion

        public static void SaveToPath(string filePath, object obj, bool append)
        {
            FileMode mode = append ? FileMode.Append : FileMode.OpenOrCreate;
            using (FileStream fileStream = new FileStream(filePath, mode))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, obj);
            }
        }

        public static T ReadFromPath<T>(string filePath) where T : class
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(fileStream) as T;
            }
        }

        public static void ReadFromPath(string filePath, Action<object> callback)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                while (fileStream.CanRead)
                {
                    if (fileStream.Position == fileStream.Length) break;
                    try
                    {
                        callback(formatter.Deserialize(fileStream));
                    }
                    catch
                    {
                        Console.WriteLine("解析错误");
                        break;
                    }
                }
            }
        }

    }
}
