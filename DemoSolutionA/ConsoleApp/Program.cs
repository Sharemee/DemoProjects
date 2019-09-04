using System;
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
            FileStream fs=new FileStream("D:\a.txt",FileMode.OpenOrCreate,FileAccess.Write,FileShare.Read)
            Console.ReadKey();
        }
    }
}
