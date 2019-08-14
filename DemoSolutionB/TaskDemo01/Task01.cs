using System;
using System.Threading.Tasks;

namespace TaskDemo01
{
    public class Task01
    {

        public async static void Function1(string str)
        {
            long num = 0;
            await Task.Run(() =>
            {
                for (long i = 0; i < 1000000000; i++)
                {
                    num += i;
                }
            });
            Console.WriteLine(str + " >> " + DateTime.Now.ToString() + "\n" + num);
            Console.ReadKey();
        }

        public static void Function2()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i + " Function2: " + DateTime.Now);
            }
        }
    }
}
