using log4net;
using log4net.Config;
using Quartz;
using Quartz.Impl;
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
        static async void Main()
        {
            //默认使用App.config里的配置
            //XmlConfigurator.Configure();

            //使用自定义配置文件里的配置
            //XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));

            //获取一个日志记录器
            ILog log = LogManager.GetLogger(typeof(Program));
            log.Info("log4net configure complete!");

            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = await sf.GetScheduler();
            log.Info("Create Scheduler!");

            IJobDetail job = JobBuilder.Create<MyJob>().WithIdentity("job1", "group1").Build();
            log.Info("Create JobDetail!");

            DateTime dt = DateTime.Now.AddMinutes(1);
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger1", "group1").StartAt(dt).Build();
            log.Info("Create Trigger!");

            await sched.ScheduleJob(job, trigger);
            log.Info($"{job.Key} will run at: {dt:T}");

            await sched.Start();
            log.Info("Started Schedule!");

            await sched.Shutdown();
            log.Info("Schedule was shutdown!");

            Console.ReadKey();
        }
    }
}
