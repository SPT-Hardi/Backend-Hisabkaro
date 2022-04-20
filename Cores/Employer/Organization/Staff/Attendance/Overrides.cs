using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Attendance
{
    public class Overrides
    {
        public Result Add(int URId,Models.Employer.Organization.Staff.Attendance.Override value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var findrole = c.SubUserOrganisations.Where(x => x.URId == URId).SingleOrDefault();
                    var staffexist = c.SubUserOrganisations.Where(x => x.OId == findrole.OId && x.URId == value.URId).SingleOrDefault();
                    if (staffexist == null) 
                    {
                        throw new ArgumentException("Staff not exist in current organiztion!,(enter valid URId)");
                    }
                    var staffattendance = c.OrgStaffsAttendancesDailies.Where(x => x.URId == value.URId && x.ChekIN.Value.Date == value.AttendanceDate.Date).SingleOrDefault();
                    //33-present
                    //34-absent
                    //35-HalfDay
                    var checkindate = value.AttendanceDate.ToString("dd/MM/yyyy");
                    var org = c.DevOrganisationsStaffs.Where(x => x.URId == value.URId).SingleOrDefault();
                    
                    if (staffattendance == null)
                    {
                        if (value.Status.ID ==34) 
                        {
                            throw new ArgumentException($"Staff already absent at {value.AttendanceDate.ToString("dd/MM/yyyy")} day!");
                        }
                        else if (value.Status.ID == 33)
                        {
                            OrgStaffsAttendancesDaily attendance = new OrgStaffsAttendancesDaily();
                            attendance.ChekIN = DateTime.Parse($"{checkindate} {org.DevOrganisationsShiftTime.StartTime}");
                            attendance.CheckOUT = DateTime.Parse($"{checkindate} {org.DevOrganisationsShiftTime.EndTime}");
                            attendance.URId = value.URId;
                            attendance.LastUpdateDate = DateTime.Now;
                            attendance.IsAccessible = false;
                            c.OrgStaffsAttendancesDailies.InsertOnSubmit(attendance);
                            c.SubmitChanges();
                        }
                        else if (value.Status.ID == 35) 
                        {
                            OrgStaffsAttendancesDaily attendance = new OrgStaffsAttendancesDaily();
                            var total_org_runnigtime = (org.DevOrganisationsShiftTime.EndTime-org.DevOrganisationsShiftTime.StartTime)/2;
                            var checkouttime= org.DevOrganisationsShiftTime.EndTime-total_org_runnigtime;
                            attendance.ChekIN = DateTime.Parse($"{checkindate} {org.DevOrganisationsShiftTime.StartTime}");
                            attendance.CheckOUT = DateTime.Parse($"{checkindate} {checkouttime}");
                            attendance.URId = value.URId;
                            attendance.LastUpdateDate = DateTime.Now;
                            attendance.IsAccessible = false;
                            c.OrgStaffsAttendancesDailies.InsertOnSubmit(attendance);
                            c.SubmitChanges();

                        }

                    }
                    else 
                    {
                        if (value.Status.ID == 33)
                        {
                            throw new ArgumentException($"Staff already present at {value.AttendanceDate.ToString("dd/MM/yyyy")} day!");
                        }
                        else if (value.Status.ID == 34)
                        {
                            c.OrgStaffsAttendancesDailies.DeleteOnSubmit(staffattendance);
                            c.SubmitChanges();
                        }
                        else if (value.Status.ID == 35)
                        {
                            
                            
                            var total_org_runnigtime = (org.DevOrganisationsShiftTime.EndTime - org.DevOrganisationsShiftTime.StartTime) / 2;
                            var checkouttime = org.DevOrganisationsShiftTime.EndTime - total_org_runnigtime;
                            staffattendance.ChekIN = DateTime.Parse($"{checkindate} {org.DevOrganisationsShiftTime.StartTime}");
                            staffattendance.CheckOUT = DateTime.Parse($"{checkindate} {checkouttime}");
                            staffattendance.URId = value.URId;
                            staffattendance.LastUpdateDate = DateTime.Now;
                            staffattendance.IsAccessible = false;
                            
                            c.SubmitChanges();

                        }
                    }
                    var name = c.SubUsersDetails.Where(x=>x.UId==staffexist.UId).SingleOrDefault().FullName;
                    
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staff attendance override successful!",
                        Data = new
                        {
                            StaffName =name,
                            Status =value.Status.ID,
                        }
                    };
                }
            }
        }
    }
}
