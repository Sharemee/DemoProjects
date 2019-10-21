using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePrograms
{
    public enum HosSuportCardType
    {
        港澳通行证=2
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("当前系统时间: " + DateTime.Now.ToString() + "\n");
            Demo6();

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

        /// <summary>
        /// DateTime变量存储的时间是否是引用值
        /// </summary>
        static void Demo3()
        {
            DateTime dt = DateTime.Now;
            Console.WriteLine("记录的时间: "+dt);
            Thread.Sleep(3000);
            Console.WriteLine("3秒钟之后取变量记录的时间: "+dt);
        }

        static void Demo4()
        {
            string str = DateTime.Now.ToString("yyy_MM_dd");
            Console.WriteLine(str);
        }

        static void Demo5()
        {
            var a = Math.DivRem(10, 3, out var b);
            Console.WriteLine(a);
            Console.WriteLine(b);
        }

        static void Demo6()
        {
            Console.WriteLine((int)HosSuportCardType.港澳通行证);
            Console.WriteLine("=============================");
            var str = Enum.GetName(typeof(HosSuportCardType), 2);
            Console.WriteLine(str);
            Console.WriteLine("==================");
            HosSuportCardType hos = new HosSuportCardType();
            Console.WriteLine(hos.CustomGetType());
            Console.WriteLine(hos.GetType().Name);
            Console.WriteLine("==============");

        }

        static void Demo07()
        {

        }
    }

    public static class Extend
    {
        public static string CustomGetType(this HosSuportCardType en)
        {
            return en.GetType().Name;
        }
    }
}
