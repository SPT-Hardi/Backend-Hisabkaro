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
            using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {
                var _staff = (from x in context.DevOrganisationsStaffs
                              where x.OId == OId
                              select new
                              {
                                  Name = x.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                                  Profile = (from y in context.CommonFiles
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
            using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _OId = context.DevOrganisations.SingleOrDefault(o => o.OId == value.Organization.ID);
                    if (_OId is null)
                    {
                        throw new ArgumentException("Organization Does Not Exits!");
                    }
                    //var _BId = context.DevOrganisationBranches.SingleOrDefault(o => o.OId == value.Organization.ID && o.BranchId == value.Branch.ID);
                    //if (_BId is null)
                    //{
                    //    throw new ArgumentException($"Branch Does Not Exits For {_OId.OrganisationName}!");
                    //}
                    var _Number = value.MobileNumber == value.AMobileNumber;
                    if (_Number is true)
                    {
                        throw new ArgumentException("MoileNumber and AlternerNumaber Are Same!");
                    }
                    var _OrgRole = context.SubRoles.Where(x => x.RoleName == "Staff").SingleOrDefault(x => x.OId == _OId.OId);

                    var _subUser = context.SubUsers.SingleOrDefault(x => x.MobileNumber == value.MobileNumber);
                    if (_subUser is not null)
                    {
                        var _subUserOrg = context.SubUserOrganisations.SingleOrDefault(x => x.UId == _subUser.UId && x.OId == _OId.OId && x.RId == _OrgRole.RId);
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
                            context.SubUserOrganisations.InsertOnSubmit(_userOrg);
                            context.SubmitChanges();
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
                        context.SubUsers.InsertOnSubmit(_user);
                        context.SubmitChanges();

                        var _userDetail = new SubUsersDetail()
                        {
                            UId = _user.UId,
                            FullName = value.Name,
                            Email = value.Email,
                            AMobileNumber = value.AMobileNumber
                        };
                        context.SubUsersDetails.InsertOnSubmit(_userDetail);
                        context.SubmitChanges();

                        var _userOrg = new SubUserOrganisation()
                        {
                            UId = _user.UId,
                            OId = _OId.OId,
                            RId = _OrgRole.RId
                        };
                        context.SubUserOrganisations.InsertOnSubmit(_userOrg);
                        context.SubmitChanges();
                    }
                    var _OrgRol = context.SubRoles.Where(x => x.RoleName == "Staff").SingleOrDefault(x => x.OId == _OId.OId);
                    var _users=context.SubUsers.SingleOrDefault(x => x.MobileNumber == value.MobileNumber);
                    var _URID = context.SubUserOrganisations.SingleOrDefault(x => x.UId == _users.UId && x.OId == _OId.OId && x.RId == _OrgRol.RId);

                    var staff = new DevOrganisationsStaff()
                    {
                        URId = _URID.URId,
                        OId = (int)value.Organization.ID,
                        BranchId = value.Branch.ID == 0 ? null : value.Branch.ID,
                        ShiftTimeId = value.ShiftTiming.ID,
                        Salary = value.Salary,
                        IsOnenWeek = value.IsOpenWeek,
                    };
                    if (value.IsOpenWeek == false)
                    {
                        staff.WeekOffOneDay = value.WeekOff1.ID;
                        staff.WeekOffSecondDay = value.WeekOff2.ID;
                    }
                    context.DevOrganisationsStaffs.InsertOnSubmit(staff);
                    context.SubmitChanges();
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
