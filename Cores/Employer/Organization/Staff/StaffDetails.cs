
using HIsabKaro.Cores.Common;
using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using HisabKaroDBContext;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff
{
    public class StaffDetails
    {
        private readonly ITokenServices _tokenService;
        
        public StaffDetails(ITokenServices tokenService)
        {
            _tokenService = tokenService;
        }

        public Result One(int OId)
        {
            using (DBContext c = new DBContext())
            {
                var _staff = (from x in c.DevOrganisationsStaffs
                              where x.OId == OId
                              select new
                              {
                                  URId=x.URId,
                                  Name = x.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                                  Profile = (from y in c.CommonFiles
                                             where y.FileId == x.SubUserOrganisation.SubUser.SubUsersDetail.FileId
                                             select y.FGUID).SingleOrDefault(),
                              }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View"),
                    Data = _staff
                };
            }
        }

        public Result Create(object URId,Models.Employer.Organization.Staff.StaffDetail value)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == value.Organization.Id);
                    if (_OId is null)
                    {
                        throw new ArgumentException("Organization Does Not Exits!");
                    }

                    var _Number = value.MobileNumber == value.AMobileNumber;
                    if (_Number is true)
                    {
                        throw new ArgumentException("MoileNumber and AlternetNumber Are Same!");
                    }

                    var _OrgRole = (from x in c.SubRoles where x.OId == value.Organization.Id && x.RoleName.ToLower() == "staff" select new { x.RId, x.RoleName }).FirstOrDefault();
                    if (_OrgRole is null)
                    {
                        var _role = new SubRole()
                        {
                            OId = _OId.OId,
                            RoleName = "staff",
                            LoginTypeId = 20,                        
                        };
                        c.SubRoles.InsertOnSubmit(_role);
                        c.SubmitChanges();
                    }

                    var _OrgRoles = (from x in c.SubRoles where x.OId == value.Organization.Id && x.RoleName.ToLower() == "staff" select new { x.RId, x.RoleName }).FirstOrDefault();
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
                                RId = _OrgRoles.RId
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
                            DefaultLoginTypeId = 20
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
                            RId = _OrgRoles.RId
                        };
                        c.SubUserOrganisations.InsertOnSubmit(_userOrg);
                        c.SubmitChanges();
                    }

                    var _users = c.SubUsers.SingleOrDefault(x => x.MobileNumber == value.MobileNumber);
                    var _URID = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _users.UId && x.OId == _OId.OId && x.RId == _OrgRoles.RId);

                    var _Sid = (from x in c.DevOrganisationsStaffs
                              where x.OId == _OId.OId
                              select x).Max(x => x.SId);
                    var i = _Sid;
                    if (_Sid == null)
                    {
                        _Sid = 1;
                    }
                    else
                    {
                        _Sid += 1;
                    }
                    var id = _Sid;

                    var staff = new DevOrganisationsStaff()
                    {
                        URId = _URID.URId,
                        OId = (int)value.Organization.Id,
                        BranchId = value.Branch.Id == 0 ? null : value.Branch.Id,
                        ShiftTimeId = value.ShiftTiming.Id,
                        Salary = value.Salary,
                        IsOpenWeek = value.IsOpenWeek,
                        SId = _Sid,
                        Status = false,
                        CreateDate = ISDT,
                    };
                    if (value.IsOpenWeek == false)
                    {
                        staff.WeekOffOneDay = value.WeekOff1.Id;
                        staff.WeekOffSecondDay = value.WeekOff2.Id;
                    }
                    c.DevOrganisationsStaffs.InsertOnSubmit(staff);
                    c.SubmitChanges();
                    
                    scope.Complete();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Staff Add Successfully!"),
                        Data = new
                        {
                            OrgCode=_OId.OrgCode,
                            OId = value.Organization.Id,
                        }
                    };
                }
            }
        }

        public Result BulkCreate(object URId,List<Models.Employer.Organization.Staff.StaffDetail> value)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    value.ForEach((v) =>
                    {
                        var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == v.Organization.Id);
                        if (_OId is null)
                        {
                            throw new ArgumentException("Organization Does Not Exits!");
                        }

                        var _Number = v.MobileNumber == v.AMobileNumber;
                        if (_Number is true)
                        {
                            throw new ArgumentException("MoileNumber and AlternetNumber Are Same!");
                        }

                        var _OrgRole = (from y in c.SubRoles where y.OId == v.Organization.Id && y.RoleName.ToLower() == "staff" select new { y.RId, y.RoleName }).FirstOrDefault();
                        if (_OrgRole is null)
                        {
                            var _role = new SubRole()
                            {
                                OId = _OId.OId,
                                RoleName = "staff",
                                LoginTypeId = 20,
                            };
                            c.SubRoles.InsertOnSubmit(_role);
                            c.SubmitChanges();
                        }

                        var _OrgRoles = (from y in c.SubRoles where y.OId == v.Organization.Id && y.RoleName.ToLower() == "staff" select new { y.RId, y.RoleName }).FirstOrDefault();
                        var _subUser = c.SubUsers.SingleOrDefault(y => y.MobileNumber == v.MobileNumber);

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
                                    RId = _OrgRoles.RId
                                };
                                c.SubUserOrganisations.InsertOnSubmit(_userOrg);
                                c.SubmitChanges();
                            }
                        }
                        else
                        {
                            var _user = new SubUser()
                            {
                                MobileNumber = v.MobileNumber,
                                DefaultLanguageId = 1,
                                DefaultLoginTypeId = 20
                            };
                            c.SubUsers.InsertOnSubmit(_user);
                            c.SubmitChanges();

                            var _userDetail = new SubUsersDetail()
                            {
                                UId = _user.UId,
                                FullName = v.Name,
                                Email = v.Email,
                                AMobileNumber = v.AMobileNumber
                            };
                            c.SubUsersDetails.InsertOnSubmit(_userDetail);
                            c.SubmitChanges();

                            var _userOrg = new SubUserOrganisation()
                            {
                                UId = _user.UId,
                                OId = _OId.OId,
                                RId = _OrgRoles.RId
                            };
                            c.SubUserOrganisations.InsertOnSubmit(_userOrg);
                            c.SubmitChanges();
                        }

                        var _users = c.SubUsers.SingleOrDefault(x => x.MobileNumber == v.MobileNumber);
                        var _URID = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _users.UId && x.OId == _OId.OId && x.RId == _OrgRoles.RId);

                        var _Sid = (from y in c.DevOrganisationsStaffs
                                    where y.OId == _OId.OId
                                    select y).Max(x => x.SId);
                        var i = _Sid;
                        if (_Sid == null)
                        {
                            _Sid = 1;
                        }
                        else
                        {
                            _Sid += 1;
                        }
                        var id = _Sid;

                        var staff = new DevOrganisationsStaff()
                        {
                            URId = _URID.URId,
                            OId = (int)v.Organization.Id,
                            BranchId = v.Branch.Id == 0 ? null : v.Branch.Id,
                            ShiftTimeId = v.ShiftTiming.Id,
                            Salary = v.Salary,
                            IsOpenWeek = v.IsOpenWeek,
                            SId = _Sid,
                            Status = false,
                            CreateDate = ISDT,
                        };
                        if (v.IsOpenWeek == false)
                        {
                            staff.WeekOffOneDay = v.WeekOff1.Id;
                            staff.WeekOffSecondDay = v.WeekOff2.Id;
                        }
                        c.DevOrganisationsStaffs.InsertOnSubmit(staff);
                        c.SubmitChanges();
                    });
                    
                    scope.Complete();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Staff Add Successfully!"),                         
                    };
                }
            }
        }

        //Example of student csv file 
        //public Result Create(int URId, Models.Employer.Organization.Staff.BulkStaffDetail value)
        //{
        //    using (DBContext c = new DBContext())
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            var _OId = c.SubUserOrganisations.SingleOrDefault(o => o.URId == URId);
        //            if (_OId is null)
        //            {
        //                throw new ArgumentException("Organization Does Not Exits!");
        //            }
        //            var _FileId = (from x in c.CommonFiles where x.FGUID == value.CSV select x).FirstOrDefault();

        //            var csvTable = new DataTable();
        //            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(_FileId.FilePath)), true))
        //            {
        //                csvTable.Load((IDataReader)csvReader);
        //            }

        //            string Column1 = csvTable.Columns[0].ToString();
        //            string Row1 = csvTable.Rows[0][0].ToString();
        //            //List<student> searchParameters = new List<student>();

        //            for (int i = 0; i < csvTable.Rows.Count; i++)
        //            {
        //                //searchParameters.Add(new student { Name = csvTable.Rows[i][0].ToString(), Age = csvTable.Rows[i][1].ToString(), MobileNumber = csvTable.Rows[i][2].ToString() });
        //                var _subUser = c.Students.SingleOrDefault(x => x.MobileNumber == csvTable.Rows[i][2].ToString());
        //                if (_subUser is not null)
        //                {
        //                    throw new ArgumentException($"{csvTable.Rows[i][0]} Alredy in This Class");
        //                }
        //                var _stud = new Student()
        //                {
        //                    Name = csvTable.Rows[i][0].ToString(),
        //                    Age = csvTable.Rows[i][1].ToString(),
        //                    MobileNumber = csvTable.Rows[i][2].ToString()

        //                };
        //                c.Students.InsertOnSubmit(_stud);
        //                c.SubmitChanges();
        //            }
        //            scope.Complete();
        //            return new Result()
        //            {
        //                Status = Result.ResultStatus.success,
        //                Message = string.Format($"Student Add Successfully"),
        //                Data = new
        //                {

        //                }
        //            };
        //        }
        //    }
        //}

        public Result JoinOrganizationCreate(Models.Employer.Organization.Staff.JoinOrganizationCreate value)   
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _User = c.SubUsers.SingleOrDefault(x =>x.MobileNumber == value.MobileNumber);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }

                    var _Org = c.DevOrganisations.SingleOrDefault(x => x.OrgCode == value.OrgCode); 
                    if (_Org is null)
                    {
                        throw new ArgumentException("Organisation Not Found!");
                    }

                    var _UserOrg = c.SubUserOrganisations.SingleOrDefault(x => x.UId==_User.UId && x.OId == _Org.OId);
                    if (_UserOrg is null)
                    {
                        throw new ArgumentException("Unauthorized!");
                    }

                    var _staff = c.DevOrganisationsStaffs.SingleOrDefault(x => x.URId == _UserOrg.URId);
                    if(_staff is null)
                    {  
                        throw new ArgumentException("Unauthorized!");
                    }

                    _staff.Status = true;
                    c.SubmitChanges();

                    var authclaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role,_UserOrg.URId.ToString()),
                        new Claim(ClaimTypes.Sid,_User.UId.ToString()),
                        new Claim(ClaimTypes.Name,_User.SubUsersDetail.FullName),
                        new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    };
                    var jwtToken = _tokenService.GenerateAccessToken(authclaims);
                    scope.Complete();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Staff Add Successfully!"),
                        Data = new
                        {
                            JWT = jwtToken,
                        }
                    };
                }
            }
        }
    }
}
