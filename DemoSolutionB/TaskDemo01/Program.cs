using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo01
{
    class Program
    {
        static void Main(string[] args)
        {
            Task01.Function1("Sunshine");
            Task01.Function2();
            Console.WriteLine($"{DateTime.Now:T}");
            Console.WriteLine(string.Format("{0}", DateTime.Now.ToString("T")));
            Console.ReadKey();
        }
    }
}
