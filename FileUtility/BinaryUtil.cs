using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileUtility
{
    public  class BinaryUtil
    {
        public static byte[] IntToByte(int data)
        {
            return BitConverter.GetBytes(data);
        }

        public static int ByteToInt(byte[] bt)
        {
            //必读4位
            return BitConverter.ToInt32(bt, 0);
        }

        public static byte[] StringToByte(string data)
        {
            return System.Text.Encoding.UTF8.GetBytes(data);
        }

        public static string ByteToString(byte[] bt)
        {
            return System.Text.Encoding.UTF8.GetString(bt);
        }

        public static byte[] ObjectToBinary(object obj)
        {
            byte[] data;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                data = stream.ToArray();
                stream.Close();
            }
            return data;
        }

        public static T ByteToObject<T>(byte[] data) where T : class
        {
            T obj;
            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(stream) as T;
            }
            return obj;
        }
    }
}
