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
        public Task Execute(IJobExecutionContext context)
        {
            DateTime dt = new DateTime();
            Task task = new Task(() =>
            {
                dt = DateTime.Now;
                Console.WriteLine(dt);
            });
            return task;
        }
    }
}
