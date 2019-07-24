using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePrograms
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("当前系统时间: " + DateTime.Now.ToString() + "\n");
            Demo3();

            Console.ReadKey();
        }

        /// <summary>
        /// 异或运算_不使用第三个变量交换两值
        /// </summary>
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

        /// <summary>
        /// 获取本月月初时间
        /// </summary>
        static void Demo2()
        {
            DateTime dt = DateTime.Today;
            Console.WriteLine("当前时间: "+dt.ToString());
            Console.WriteLine("--: "+(1-dt.Day));
            Console.WriteLine("本月月初: "+dt.AddDays(1-dt.Day));
        }

        static void Demo3()
        {
            DateTime dt = DateTime.Now;
            Console.WriteLine("记录的时间: "+dt);
            Thread.Sleep(3000);
            Console.WriteLine("3秒钟之后取变量记录的时间: "+dt);
        }
    }
}
