using FileUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Data;

namespace TestProject.Test
{
    public class MemeryUtil
    {
        //测试读取文件后再序列化成类
        public void Read()
        {
            //方式一
            byte[] bt = File.ReadAllBytes(PathHelper.filepath);
            //方式二
            string str = File.ReadAllText(PathHelper.filepath);
            bt = Encoding.Default.GetBytes(str);
            //方式三
            //TextAsset text = Resources.Load<TextAsset>(filepath);
            //bt = text.bytes;
            

            MemoryUtility.ReadFromMemory(bt, obj =>
            {
            
            });
        }
    }
}
