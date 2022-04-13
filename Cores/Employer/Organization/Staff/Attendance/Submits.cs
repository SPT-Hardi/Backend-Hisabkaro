using HisaabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HisaabKaro.Cores.Employer.Organization.Staff.Attendance
{
    public class Submits
    {
        public Result Add(Models.Employer.Organization.Staff.Attendance.Submit value, string UID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var qs = context.OrgStaffsAttendancesDailies.Where(x => x.ChekIN.Value.Day == DateTime.Now.Day && x.RId == value.RId).SingleOrDefault();
                    if (qs == null)
                    {
                        HisabKaroDBContext.OrgStaffsAttendancesDaily attendance = new HisabKaroDBContext.OrgStaffsAttendancesDaily();
                        attendance.UId = int.Parse(UID);
                        attendance.RId = value.RId;
                        attendance.LastUpdateDate = DateTime.Now;
                        attendance.ChekIN = value.CheckIN;
                        context.OrgStaffsAttendancesDailies.InsertOnSubmit(attendance);
                        context.SubmitChanges();
                    }
                    else
                    {
                        qs.CheckOUT = value.CheckIN;
                        qs.LastUpdateDate = DateTime.Now;
                        context.SubmitChanges();
                    }

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employer-{ROLE-NAME} Daily Attendance Add Successfully!",
                        Data = "",
                    };
                }
            }
        }
    }
}
