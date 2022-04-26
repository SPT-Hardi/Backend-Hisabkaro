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
                    //remove after contoller added
                    if (findrole.SubRole.RoleName.ToLower() != "admin")
                    {
                        throw new ArgumentException("You are not authorize!");
                    }
                    var staffexist = c.SubUserOrganisations.Where(x => x.OId == findrole.OId && x.URId == value.URId).SingleOrDefault();
                    if (staffexist == null) 
                    {
                        throw new ArgumentException("Staff not exist in current organiztion!,(enter valid URId)");
                    }
                    var staffattendance = c.OrgStaffsAttendancesDailies.Where(x => x.URId == value.URId && x.ChekIN.Value.Date == value.AttendanceDate.Date).SingleOrDefault();
                    //33-present
                    //34-absent
                    //35-HalfDay
                    var checkindate = value.AttendanceDate.ToString("yyyy/MM/dd");
                    var org = c.DevOrganisationsStaffs.Where(x => x.URId == value.URId).SingleOrDefault();
                    if (org==null) 
                    {
                        throw new ArgumentException("Staff not exist in any organization!");
                    }
                    
                    if (staffattendance == null)
                    {
                        if (value.Status.Id ==34) 
                        {
                            throw new ArgumentException($"Staff already absent at {value.AttendanceDate.ToString("dd/MM/yyyy")} day!");
                        }
                        else if (value.Status.Id == 33)
                        {
                            OrgStaffsAttendancesDaily attendance = new OrgStaffsAttendancesDaily();
                            attendance.ChekIN = Convert.ToDateTime($"{checkindate} {org.DevOrganisationsShiftTime.StartTime}");
                            attendance.CheckOUT = Convert.ToDateTime($"{checkindate} {org.DevOrganisationsShiftTime.EndTime}");
                            attendance.URId = value.URId;
                            attendance.LastUpdateDate = DateTime.Now.ToLocalTime();
                            attendance.IsAccessible = false;
                            c.OrgStaffsAttendancesDailies.InsertOnSubmit(attendance);
                            c.SubmitChanges();
                        }
                        else if (value.Status.Id == 35) 
                        {
                            OrgStaffsAttendancesDaily attendance = new OrgStaffsAttendancesDaily();
                            var total_org_runnigtime = (org.DevOrganisationsShiftTime.EndTime-org.DevOrganisationsShiftTime.StartTime)/2;
                            var checkouttime= org.DevOrganisationsShiftTime.EndTime-total_org_runnigtime;
                            attendance.ChekIN = Convert.ToDateTime($"{checkindate} {org.DevOrganisationsShiftTime.StartTime}");
                            attendance.CheckOUT = Convert.ToDateTime($"{checkindate} {checkouttime}");
                            attendance.URId = value.URId;
                            attendance.LastUpdateDate = DateTime.Now.ToLocalTime();
                            attendance.IsAccessible = false;
                            c.OrgStaffsAttendancesDailies.InsertOnSubmit(attendance);
                            c.SubmitChanges();

                        }

                    }
                    else 
                    {
                        if (value.Status.Id == 33)
                        {
                            throw new ArgumentException($"Staff already present at {value.AttendanceDate.ToString("dd/MM/yyyy")} day!");
                        }
                        else if (value.Status.Id == 34)
                        {
                            c.OrgStaffsAttendancesDailies.DeleteOnSubmit(staffattendance);
                            c.SubmitChanges();
                        }
                        else if (value.Status.Id == 35)
                        {
                            
                            
                            var total_org_runnigtime = (org.DevOrganisationsShiftTime.EndTime - org.DevOrganisationsShiftTime.StartTime) / 2;
                            var checkouttime = org.DevOrganisationsShiftTime.EndTime - total_org_runnigtime;
                            staffattendance.ChekIN = Convert.ToDateTime($"{checkindate} {org.DevOrganisationsShiftTime.StartTime}");
                            staffattendance.CheckOUT = Convert.ToDateTime($"{checkindate} {checkouttime}");
                            staffattendance.URId = value.URId;
                            staffattendance.LastUpdateDate = DateTime.Now.ToLocalTime();
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
                            Status =value.Status.Id,
                        }
                    };
                }
            }
        }
    }
}
