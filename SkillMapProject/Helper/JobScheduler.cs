using Quartz;
using Quartz.Impl;
using SkillMapProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SkillMapProject.Helper
{
    public class JobScheduler
    {
        public static void Start()

        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.Start();
            IJobDetail job = JobBuilder.Create<UpdateJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                  )
                .Build();
            scheduler.ScheduleJob(job, trigger);

        }
        public class UpdateJob : IJob

        {
            public void Execute(IJobExecutionContext context)

            {
                try
                {
                    using (var db = new UMC_SKILLEntities())
                    {

                        GA_UMCEntities gaDB = new GA_UMCEntities();
                        object[] param =
                            {
                            new SqlParameter() { ParameterName = "@deptCode", Value = DBNull.Value, SqlDbType = SqlDbType.NVarChar},
                            new SqlParameter("@Out_Parameter", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            }
                        };
                        var reports = gaDB.sp_Get_All_Staff_2().ToList();
                        var list = db.Members.ToList();
                        foreach (var mem in list)
                        {
                            //Nếu không có trên ds Ga
                            if (reports.Where(m => m.StaffCode == mem.Code).FirstOrDefault() == null)
                            {
                                var memInDb = db.Members.Where(m => m.Code == mem.Code).FirstOrDefault();
                                memInDb.Removed = 1;
                                db.SaveChanges();
                            }
                        }
                        foreach (var employee in reports)
                        {
                            // Nếu không có trên ds member thì thêm vào
                            var eInSkillMap = db.Members.Where(m => m.Code == employee.StaffCode).FirstOrDefault();
                            if (eInSkillMap == null)
                            {
                                var mem = new Member()
                                {
                                    Code = employee.StaffCode,
                                    Name = employee.FullName,
                                    Dept = employee.DeptCode,
                                    RoleID = 1,
                                    Removed = 0,
                                    Pass = "umcvn",
                                    Pos = employee.PosName,
                                    Customer = employee.Customer
                                };
                                if (employee.EntryDate is DateTime date)
                                {
                                    mem.DateEnter = date;
                                }
                                db.Members.Add(mem);
                                db.SaveChanges();
                            }
                            else
                            {
                                bool isChanged = false;
                                if (eInSkillMap.Dept != employee.DeptCode)
                                {
                                    eInSkillMap.Dept = employee.DeptCode;
                                    isChanged = true;
                                }
                                if (eInSkillMap.Pos != employee.PosName)
                                {
                                    eInSkillMap.Pos = employee.PosName;
                                    isChanged = true;
                                }
                                if (eInSkillMap.Customer != employee.Customer)
                                {
                                    eInSkillMap.Customer = employee.Customer;
                                    isChanged = true;
                                }
                                if (isChanged)
                                    db.SaveChanges();
                            }
                        }
                        
                    }

                }
                catch (Exception e)

                {
                    Console.WriteLine(e.ToString());
                }

            }

        }
    }
}