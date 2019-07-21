using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using WebMatrix.WebData;
using Erp.Domain.Crm.Entities;
using Erp.Domain.Crm.Repositories;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Repositories;
using System.Net;
using System.Data;

namespace Erp.BackOffice.Helpers
{
    public class ScheduleHelper
    {
        public static IScheduler Scheduler;
        public static void init()
        {
            //Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };
            // Grab the Scheduler instance from the Factory 
            Scheduler = StdSchedulerFactory.GetDefaultScheduler();

            // and start it off
            Scheduler.Start();

            // define the job and tie it to our Job_NotifyCustomerLiabilities class
            IJobDetail job = JobBuilder.Create<Job_NotifyCustomerLiabilities>()
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(1)
                    .RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            //Scheduler.ScheduleJob(job, trigger);
        }

        public class Job_NotifyCustomerLiabilities : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                TaskRepository taskRepository = new TaskRepository(new Domain.Crm.ErpCrmDbContext());
                var task = new Task();
                task.IsDeleted = false;
                //task.CreatedUserId = WebSecurity.CurrentUserId;
                //task.ModifiedUserId = WebSecurity.CurrentUserId;
                //task.AssignedUserId = WebSecurity.CurrentUserId;
                task.CreatedDate = DateTime.Now;
                task.ModifiedDate = DateTime.Now;

                task.Subject = "Còn 10 ngày nữa là đến ngày trả nợ của KH: ABC";

                taskRepository.InsertTask(task);
            }
        }

        //public class Job_GetDataFingerPrinter : IJob
        //{
        //    public void Execute(IJobExecutionContext context)
        //    {
        //        FPMachineRepository fPMachineRepository = new FPMachineRepository(new Domain.Sale.ErpSaleDbContext());
        //        var fPMachineList = fPMachineRepository.GetAllFPMachine().ToList();
        //        //resolve from domain to ip
        //        foreach (FPMachine item in fPMachineList)
        //        {
        //            if (item.useurl)
        //                item.Dia_chi_IP = Dns.GetHostAddresses(item.url)[0].ToString();
        //        }

        //    foreach (FPMachine item in fPMachineList)
        //        {
        //            //int autoID = item.AutoID;

        //            //lastDateUpdated = DateTime.Parse(CheckInOutAccess.GetLastDate(autoID.ToString()));

        //            //get Data from specified machine
        //            timeEntities = FingerPrinterHelper.GetAllTimeData(item.Dia_chi_IP, item.Port, item.ID_Ket_noi_IP);

        //            foreach (TimeEntity timeEntity in timeEntities)
        //            {
        //                string currUserFullCode = "";
        //                if (lastDateUpdated < DateTime.Parse(timeEntity.TimeString))
        //                {
                            

        //                    DataRow row = checkInOutToInsert.NewRow();
        //                    if (string.IsNullOrEmpty(currUserFullCode))
        //                        row["UserFullCode"] = "0";
        //                    else
        //                    {
        //                        row["UserFullCode"] = currUserFullCode;
        //                    }

        //                    row["UserEnrollNumber"] = timeEntity.EnrollNum;
        //                    row["TimeType"] = timeEntity.InOutMode == 0 ? "I" : "O";
        //                    row["MachineNo"] = timeEntity.MachineNum;
        //                    row["TimeDate"] = timeEntity.TimeString.Substring(0, 10) + " 00:00:00.000";
        //                    row["TimeStr"] = timeEntity.TimeString;
        //                    row["AutoID"] = autoID;
        //                    checkInOutToInsert.Rows.Add(row);
        //                }
        //            }
        //        //Chỗ này bắt đầu insert zô db
        //        }
        //    }
        //}
    }
}