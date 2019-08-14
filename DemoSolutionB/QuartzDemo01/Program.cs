using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzDemo01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Args(args));

            //默认使用App.config里的配置
            //XmlConfigurator.Configure();

            //使用自定义配置文件里的配置
            //XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));

            //获取一个日志记录器
            ILog _log = LogManager.GetLogger(typeof(Program));
            _log.Info("test");
            Console.ReadKey();
        }

        static string Args(string[] str)
        {
            if (str.Length == 0)
            {
                return "0";
            }

            string res = null;
            foreach (var item in str)
            {
                res += item;
            }
            return res;
        }
    }
}
