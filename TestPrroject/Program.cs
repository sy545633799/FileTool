using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileUtility;
using TestProject.Data;
using System.IO;
using UnityEngine;

namespace TestProject
{
    class Program
    {
        

        static void Main(string[] args)
        {

            PersonData data = new PersonData();
            data.Age = 13;
            data.ID = 123;
            data.Name = "test";
            data.Job = "programmer";

            
            
            Console.Read();
        }
    }
}
