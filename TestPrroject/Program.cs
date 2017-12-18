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
        private static string filepath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "test6.txt");

        static void Main(string[] args)
        {


            Console.Read();
        }
    }
}
