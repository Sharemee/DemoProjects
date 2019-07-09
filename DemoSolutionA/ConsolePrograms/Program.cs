using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePrograms
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("当前系统时间: " + DateTime.Now.ToString() + "\n");
            Demo1();

            Console.ReadKey();
        }

        static void Demo1()
        {
            int num1 = 2;
            int num2 = 3;
            Console.WriteLine("原始结果:");
            Console.WriteLine("num1=" + num1 + "\n" + "num2=" + num2);
            Cryptology.异或运算_不使用第三个变量交换两值(num1, num2, out num1, out num2);
            Console.WriteLine("运算后结果:");
            Console.WriteLine("num1=" + num1 + "\n" + "num2=" + num2);
        }

    }
}
