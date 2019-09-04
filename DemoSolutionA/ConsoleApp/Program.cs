using System;
using System.Collections.Generic;
using System.Configuration;
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
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var config = ConfigurationManager.AppSettings["RejectType"].TrimEnd('|');
            string[] s = config.Split('|');
            foreach (var item in s)
            {
                int index = item.IndexOf(":");
                int key = Convert.ToInt32(item.Substring(0, index));
                string value = item.Substring(index+1);
                dic.Add(key, value);
            }

            foreach (var item in dic)
            {
                Console.WriteLine($"Key={item.Key}\tValue={item.Value}");
            }

            Console.WriteLine(dic[3]);

            Console.ReadKey();
        }
    }
}
