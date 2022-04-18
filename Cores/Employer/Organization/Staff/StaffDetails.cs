using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff
{
    public class StaffDetails
    {
        public Result One(int OId)
        {
            using (DBContext c = new DBContext())
            {
                var _staff = (from x in c.DevOrganisationsStaffs
                              where x.OId == OId
                              select new
                              {
                                  Name = x.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                                  Profile = (from y in c.CommonFiles
                                             where y.FileId == x.SubUserOrganisation.SubUser.SubUsersDetail.FileId
                                             select y.FilePath).SingleOrDefault(),
                              }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View"),
                    Data = _staff
                };
            }
        }
        public Result Create(Models.Employer.Organization.Staff.StaffDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == value.Organization.ID);
                    if (_OId is null)
                    {
                        throw new ArgumentException("Organization Does Not Exits!");
                    }
                    //var _BId = c.DevOrganisationBranches.SingleOrDefault(o => o.OId == value.Organization.ID && o.BranchId == value.Branch.ID);
                    //if (_BId is null)
                    //{
                    //    throw new ArgumentException($"Branch Does Not Exits For {_OId.OrganisationName}!");
                    //}
                    var _Number = value.MobileNumber == value.AMobileNumber;
                    if (_Number is true)
                    {
                        throw new ArgumentException("MoileNumber and AlternerNumaber Are Same!");
                    }
                    var _OrgRole = c.SubRoles.Where(x => x.RoleName == "Staff").SingleOrDefault(x => x.OId == _OId.OId);

                    var _subUser = c.SubUsers.SingleOrDefault(x => x.MobileNumber == value.MobileNumber);
                    if (_subUser is not null)
                    {
                        var _subUserOrg = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _subUser.UId && x.OId == _OId.OId && x.RId == _OrgRole.RId);
                        if (_subUserOrg is not null)
                        {
                            throw new ArgumentException($"Staff Member Are Alredy Exits In {_subUserOrg.DevOrganisation.OrganisationName}!");
                        }
                        else
                        {
                            var _userOrg = new SubUserOrganisation()
                            {
                                UId = _subUser.UId,
                                OId = _OId.OId,
                                RId = _OrgRole.RId
                            };
                            c.SubUserOrganisations.InsertOnSubmit(_userOrg);
                            c.SubmitChanges();
                        }
                    }
                    else
                    {
                        var _user = new SubUser()
                        {
                            MobileNumber = value.MobileNumber,
                            DefaultLanguageId = 1,
                            LoginTypeId = 20
                        };
                        c.SubUsers.InsertOnSubmit(_user);
                        c.SubmitChanges();

                        var _userDetail = new SubUsersDetail()
                        {
                            UId = _user.UId,
                            FullName = value.Name,
                            Email = value.Email,
                            AMobileNumber = value.AMobileNumber
                        };
                        c.SubUsersDetails.InsertOnSubmit(_userDetail);
                        c.SubmitChanges();

                        var _userOrg = new SubUserOrganisation()
                        {
                            UId = _user.UId,
                            OId = _OId.OId,
                            RId = _OrgRole.RId
                        };
                        c.SubUserOrganisations.InsertOnSubmit(_userOrg);
                        c.SubmitChanges();
                    }
                    var _OrgRol = c.SubRoles.Where(x => x.RoleName == "Staff").SingleOrDefault(x => x.OId == _OId.OId);
                    var _users=c.SubUsers.SingleOrDefault(x => x.MobileNumber == value.MobileNumber);
                    var _URID = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _users.UId && x.OId == _OId.OId && x.RId == _OrgRol.RId);

                    var staff = new DevOrganisationsStaff()
                    {
                        URId = _URID.URId,
                        OId = (int)value.Organization.ID,
                        BranchId = value.Branch.ID == 0 ? null : value.Branch.ID,
                        ShiftTimeId = value.ShiftTiming.ID,
                        Salary = value.Salary,
                        IsOpenWeek = value.IsOpenWeek,
                    };
                    if (value.IsOpenWeek == false)
                    {
                        staff.WeekOffOneDay = value.WeekOff1.ID;
                        staff.WeekOffSecondDay = value.WeekOff2.ID;
                    }
                    c.DevOrganisationsStaffs.InsertOnSubmit(staff);
                    c.SubmitChanges();
                    scope.Complete();
                }
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Staff Add Successfully!"),
                };
            }
        }
    }
}
