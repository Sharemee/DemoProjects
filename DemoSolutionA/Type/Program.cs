using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Types
{
    class Program
    {
        struct St1
        {
            public string name;
            public int age;
        }

        static void Main(string[] args)
        {
            Student stu = new Student();
            Type type = stu.GetType();
            var p = type.GetProperties();

            foreach (var i in p)
            {
                Console.WriteLine(i.Name);
            }

            Console.WriteLine();

            St1 n = new St1();
            n.name = "SunTaicheng";
            n.age = 20;

            int mun = 0;
            foreach (var i in p)
            {
                if (mun == 0)
                {
                    i.SetValue(stu, n.name, null);
                }
                else
                {
                    i.SetValue(stu, n.age, null);
                }
                mun++;
            }

            Console.WriteLine(stu.StuName);
            Console.WriteLine(stu.StuAge);

            Console.WriteLine("==================================");
            string a = "[Config]=90";
            int index = a.IndexOf('=');
            Console.WriteLine(index);
            Console.WriteLine("pause");
            Console.ReadLine();
            if (index <= 0) return;
            Console.WriteLine("Key=" + a.Substring(0, index));
            Console.WriteLine("Value=" + a.Substring(index + 1));
            Console.WriteLine();
            Console.WriteLine("==================================");
            FileStream fs = new FileStream("a.dll", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            string ass = "Hello";
            //byte[] bytes = Encoding.Unicode.GetBytes("hello");
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(ass);
            sw.Flush();
            sw.Close();

            Console.ReadKey();
        }
    }

    public class Student
    {
        public string StuName { get; set; }
        public int StuAge { get; set; }

    }
}
