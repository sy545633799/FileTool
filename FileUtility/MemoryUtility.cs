using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility
{
    public class MemoryUtility
    {
        public static void ReadFromMemory(byte[] bt, Action<object> callback)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(bt))
            {
                while (stream.CanRead)
                {
                    try
                    {
                        if (stream.Position == stream.Length) break;
                        callback(formatter.Deserialize(stream));
                    }
                    catch
                    {
                        Console.Write("解析错误");
                    }
                }
            }
        }
    }
}
