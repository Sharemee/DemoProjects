﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt = DateTime.Now;
            Console.WriteLine(dt.AddHours(5).ToString());
            Console.WriteLine(dt.AddDays(-10));
            Console.WriteLine(dt.AddDays(-1).Date);

            Console.WriteLine(dt.AddMinutes(1)-dt);
            Console.WriteLine("------------------");
            Console.WriteLine((dt.AddSeconds(1)-dt).Ticks);

            string _DIRPath = @"E:\Work\药品导入\DRUG\7.26";
            string relativePath = @"\Image\A.Jpg";
            while (relativePath.StartsWith(@"\") || relativePath.StartsWith(@"/"))
            {
                relativePath = relativePath.Substring(1, relativePath.Length - 1);
            }
            Console.WriteLine(Path.Combine(_DIRPath, relativePath));
            Console.ReadKey();
        }
    }
}