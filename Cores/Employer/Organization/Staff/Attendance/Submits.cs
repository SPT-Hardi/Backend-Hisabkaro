﻿using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff.Attendance;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Attendance
{
    public class Submits
    {
        public Result Add(int URId,SubmitDaily value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var qs = c.OrgStaffsAttendancesDailies.Where(x => x.ChekIN.Value.Day == DateTime.Now.Day && x.URId == URId).SingleOrDefault();
                    var OrgStaffAttendanceDailyId = 0;
                     if (qs == null)
                     {
         
                        OrgStaffsAttendancesDaily attendance = new OrgStaffsAttendancesDaily();
                        attendance.URId = URId;
                        attendance.LastUpdateDate = value.CheckIN;
                        attendance.ChekIN = value.CheckIN;
                        c.OrgStaffsAttendancesDailies.InsertOnSubmit(attendance);
                        c.SubmitChanges();
                        OrgStaffAttendanceDailyId = attendance.OrgStaffAttendanceDailyId;
                    
                     }
                    else
                    {
                        qs.CheckOUT = value.CheckIN;
                        qs.LastUpdateDate = value.CheckIN;
                        c.SubmitChanges();
                        OrgStaffAttendanceDailyId = qs.OrgStaffAttendanceDailyId;
                    }

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staff Daily Attendance Add Successfully!",
                        Data = new
                        {
                            OrgStaffsAttendancesDailyId = OrgStaffAttendanceDailyId,
                            LastUpdate = DateTime.Now,
                        },
                    };
                }
            }
        }
    }
}
