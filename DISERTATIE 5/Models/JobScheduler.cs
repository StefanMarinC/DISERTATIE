using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISERTATIE_5.Models
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = (IScheduler)StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<JobCurrency>().Build();

            ITrigger trigger = TriggerBuilder.Create().WithCronSchedule("0 0 14 1/1 * ? *").Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}