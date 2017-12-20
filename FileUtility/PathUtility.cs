using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility
{
    //File即可以读取文本 也可以写入文本  
    //StreamRead,StreamWrite,File只能操作文本文件 
    //FileStream可以操作所有格式 包括文本，文件，图片，视频  
    public class PathUtility
    {
        public static void WriteToPath(string path, string data)
        {
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

            //StreamWrite写入文件 (bool 参数表示是否append) 
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                //方式一
                //writer.Write(data + "\r\n");
                //方式二
                writer.WriteLine(data);
            }
        }

        public static void ReadFromPath(string filepath, Action<string> callback)
        {
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
            //SreamRead读取文件  
            using (StreamReader reader = new StreamReader(filepath))
            {
                while (!reader.EndOfStream)
                {
                    callback(reader.ReadLine());
                }
            }
        }


        public static void AppendToPath(string filePath, object obj)
        {
            
            using (FileStream fileStream = new FileStream(filePath, FileMode.Append))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, obj);
            }
        }

        public static T ReadFromPath<T>(string filePath) where T: class
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
