using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzDemo01
{
    public class MyJob : IJob
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MyJob));

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() => log.Info("Hello World!"));
        }
    }
}
