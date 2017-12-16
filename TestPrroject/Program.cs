using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileUtility;
using TestProject.Data;
using System.IO;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //PersonData data = new PersonData();
            //data.ID = 12345;
            //data.Age = 10;
            //data.Job = "321";
            //data.Name = "sy";
            ////byte[] bt = BinaryUtil.ToBinary(data);
            ////for (int i = 0; i < bt.Length; i++)
            ////{
            ////    Console.Write(bt[i] + ":");
            ////}
            //string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "test.txt");
            //using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            //{
            //    //BinaryUtil.ObjectFormat(fileStream, data);
            //}


            //using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            //{
            //    //PersonData person = BinaryUtil.ToObject<PersonData>(fileStream);
            //    //Console.Write(person.ID);
            //}

            Console.Read();
        }
    }
}
