﻿using HIsabKaro.Cores.Common;
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff;
using HIsabKaro.Models.Employer.Organization.Salary;
using HisabKaroContext;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff
{
    public class BulkStaffDetails
    {
        //public Result Create(object URId, Models.Employer.Organization.Staff.BulkStaffDetail value)
        //{
        //    using (DBContext c = new DBContext())
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
        //            var _OId = c.SubUserOrganisations.SingleOrDefault(o => o.URId == (int)URId);
        //            if (_OId is null)
        //            {
        //                throw new ArgumentException("Organization Does Not Exits!");
        //            }
        //            var _FileId = (from x in c.CommonFiles where x.FGUID == value.CSV select x).FirstOrDefault();
        //            if (_FileId == null)
        //            {
        //                throw new ArgumentException("File Does Not Exits!");
        //            }
        //            var csvTable = new DataTable();
        //            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(_FileId.FilePath)), true))
        //            {
        //                csvTable.Load((IDataReader)csvReader);
        //            }

        //            string Column1 = csvTable.Columns[0].ToString();
        //            string Row1 = csvTable.Rows[0][0].ToString();

        //            for (int i = 0; i < csvTable.Rows.Count; i++)
        //            {
        //                var _OrgRole = (from x in c.SubRoles where x.OId == _OId.OId && x.RoleName.ToLower() == "staff" select new { x.RId, x.RoleName }).FirstOrDefault();
        //                if (_OrgRole is null)
        //                {
        //                    var _role = new SubRole()
        //                    {
        //                        OId = _OId.OId,
        //                        RoleName = "staff",
        //                        LoginTypeId = 20,
        //                    };
        //                    c.SubRoles.InsertOnSubmit(_role);
        //                    c.SubmitChanges();
        //                }

        //                var _OrgRoles = (from x in c.SubRoles where x.OId == _OId.OId && x.RoleName.ToLower() == "staff" select new { x.RId, x.RoleName }).FirstOrDefault();
        //                var _subUser = c.SubUsers.SingleOrDefault(x => x.MobileNumber == csvTable.Rows[i][2].ToString());
        //                if (_subUser is not null)
        //                {
        //                    var _subUserOrg = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _subUser.UId && x.OId == _OId.OId && x.RId == _OrgRole.RId);
        //                    if (_subUserOrg is not null)
        //                    {
        //                        throw new ArgumentException($"Staff Already Exits with this {_subUserOrg.SubUser.MobileNumber}!");
        //                    }
        //                    else
        //                    {
        //                        var _userOrg = new SubUserOrganisation()
        //                        {
        //                            UId = _subUser.UId,
        //                            OId = _OId.OId,
        //                            RId = _OrgRoles.RId
        //                        };
        //                        c.SubUserOrganisations.InsertOnSubmit(_userOrg);
        //                        c.SubmitChanges();
        //                    }
        //                }
        //                else
        //                {
        //                    var _user = new SubUser()
        //                    {
        //                        MobileNumber = (string)csvTable.Rows[i][2],
        //                        DefaultLanguageId = 1,
        //                        DefaultLoginTypeId = 20
        //                    };
        //                    c.SubUsers.InsertOnSubmit(_user);
        //                    c.SubmitChanges();

        //                    var _userDetail = new SubUsersDetail()
        //                    {
        //                        UId = _user.UId,
        //                        FullName = (string)csvTable.Rows[i][0],
        //                        Email = (string)csvTable.Rows[i][1],
        //                        AMobileNumber = (string)csvTable.Rows[i][3]
        //                    };
        //                    c.SubUsersDetails.InsertOnSubmit(_userDetail);
        //                    c.SubmitChanges();

        //                    var _userOrg = new SubUserOrganisation()
        //                    {
        //                        UId = _user.UId,
        //                        OId = _OId.OId,
        //                        RId = _OrgRoles.RId
        //                    };
        //                    c.SubUserOrganisations.InsertOnSubmit(_userOrg);
        //                    c.SubmitChanges();
        //                }

        //                var _users = c.SubUsers.SingleOrDefault(x => x.MobileNumber == csvTable.Rows[i][2].ToString());
        //                var _URID = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _users.UId && x.OId == _OId.OId && x.RId == _OrgRoles.RId);

        //                var _Sid = (from x in c.DevOrganisationsStaffs
        //                            where x.OId == _OId.OId
        //                            select x).Max(x => x.SId);

        //                if (_Sid == null)
        //                {
        //                    _Sid = 1;
        //                }
        //                else
        //                {
        //                    _Sid += 1;
        //                }

        //                var staff = new DevOrganisationsStaff()
        //                {
        //                    URId = _URID.URId,
        //                    OId = _OId.OId,
        //                    //Salary = float.Parse(csvTable.Rows[i][4].ToString()),
        //                    IsOpenWeek = bool.Parse(csvTable.Rows[i][5].ToString()),
        //                    SId = (int)_Sid,
        //                    Status = false,
        //                    CreateDate = ISDT,
        //                };
        //                c.DevOrganisationsStaffs.InsertOnSubmit(staff);
        //                c.SubmitChanges();
        //            }
        //            scope.Complete();
        //            return new Result()
        //            {
        //                Status = Result.ResultStatus.success,
        //                Message = string.Format($"Staffs Added Successfully"),
        //            };
        //        }
        //    }
        //}
    }    
}
