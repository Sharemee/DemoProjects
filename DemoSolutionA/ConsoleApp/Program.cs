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
            int timeOut = 10;
            DateTime startDay = DateTime.Now.AddDays(-1 * timeOut).Date;
            DateTime endDay = DateTime.Now.AddDays(-1).Date;//最后时间是昨天

            while (startDay < endDay)
            {
                Console.WriteLine("计算: "+startDay);
                startDay=startDay.AddDays(1);
            }

            Console.ReadKey();
        }
    }
}
