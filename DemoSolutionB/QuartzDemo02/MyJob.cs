﻿using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzDemo02
{
    public class MyJob:IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            JobKey jobKey = context.JobDetail.Key;
            return 
        }
    }
}
