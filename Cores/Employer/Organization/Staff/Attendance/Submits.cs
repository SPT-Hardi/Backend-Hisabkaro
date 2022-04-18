using HIsabKaro.Models.Common;
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
        //public Result Add(string UID,Models.Employer.Organization.Staff.Attendance.Submit value)
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        using (DBContext c = new DBContext())
        //        {
        //            var qs = c.OrgStaffsAttendancesDailies.Where(x => x.ChekIN.Value.Day == DateTime.Now.Day && x.RId == value.RId).SingleOrDefault();
        //            if (qs == null)
        //            {
        //               OrgStaffsAttendancesDaily attendance = new OrgStaffsAttendancesDaily();
        //                attendance.UId = int.Parse(UID);
        //                attendance.RId = value.RId;
        //                attendance.LastUpdateDate = DateTime.Now;
        //                attendance.ChekIN = value.CheckIN;
        //                c.OrgStaffsAttendancesDailies.InsertOnSubmit(attendance);
        //                c.SubmitChanges();
        //            }
        //            else
        //            {
        //                qs.CheckOUT = value.CheckIN;
        //                qs.LastUpdateDate = DateTime.Now;
        //                c.SubmitChanges();
        //            }

        //            scope.Complete();
        //            return new Result()
        //            {
        //                Status = Result.ResultStatus.success,
        //                Message = "Employer-{ROLE-NAME} Daily Attendance Add Successfully!",
        //                Data = "",
        //            };
        //        }
        //    }
        //}
    }
}
