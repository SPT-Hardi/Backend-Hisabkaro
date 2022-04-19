using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Cores.Common.Shift;
using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Branch
{
    public class BranchDetails
    {
        private readonly ContactAddress _contactAddress;
        private readonly ShiftTimes _shiftTimes;

        public BranchDetails(ContactAddress contactAddress, ShiftTimes shiftTimes)
        {
            _contactAddress = contactAddress;
            _shiftTimes = shiftTimes;
        }

        public Result Create(int URId, Models.Employer.Organization.Branch.BranchDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                    if (user == null)
                    {
                        throw new ArgumentException("User Doesn't exist");
                    }

                    var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == value.Organization.ID);
                    if (_OId is null)
                    {
                        throw new ArgumentException("Organization Does Not Exits!");
                    }

                    var _AId = _contactAddress.Create(value.Address);
                    var branch = new DevOrganisationBranch()
                    {
                        BranchName = value.BranchName,
                        ContactAddressId = _AId == null ? null : _AId.Data,
                        URId = URId,
                        Latitude = value.latitude,
                        Longitude = value.longitude,
                        OId = value.Organization.ID
                    };
                    c.DevOrganisationBranches.InsertOnSubmit(branch);
                    c.SubmitChanges();

                    if (value.status == true)
                    {
                        var shiftTime = new Models.Common.Shift.ShitTime();
                        var shift = (from x in c.DevOrganisationsShiftTimes
                                     where x.OId == _OId.OId
                                     select
                                     new Models.Common.Shift.TimeList()
                                     {
                                         StartTime = x.StartTime,
                                         EndTime = x.EndTime,
                                         MarkLate = x.MarkLate,
                                     }).ToList();
                        shiftTime.TimeLists = shift;
                        var shifttime = _shiftTimes.CreateBranchShift(shiftTime, _OId.OId, branch.BranchId);
                    }
                    else
                    {
                        var shifttime = _shiftTimes.CreateBranchShift(value.ShitTime, _OId.OId, branch.BranchId);
                    }
                    scope.Complete();
                    return new Result()
                    {
                        Message = string.Format("Branch Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = new
                        {
                            BranchId = branch.BranchId,
                            BranchName = branch.BranchName
                        }
                    };
                }
            }
        }

        public Result Update(int URId, int BId,Models.Employer.Organization.Branch.BranchDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                    if (user == null)
                    {
                        throw new ArgumentException("User Doesn't exist");
                    }

                    var branch = c.DevOrganisationBranches.SingleOrDefault(x => x.BranchId == BId && x.OId == user.OId);
                    if (branch == null)
                    {
                        throw new ArgumentException("Branch Doesn't exist");
                    }

                    var _AId = _contactAddress.Create(value.Address);
                    branch.BranchName = value.BranchName;
                    branch.ContactAddressId = _AId == null ? null : _AId.Data;
                    branch.URId = URId;
                    branch.Latitude = value.latitude;
                    branch.Longitude = value.longitude;
                    branch.OId = value.Organization.ID;
                    c.SubmitChanges();
                    scope.Complete();
                    return new Result()
                    {
                        Message = string.Format("Branch Updated Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = new
                        {
                            BranchId = branch.BranchId,
                            BranchName = branch.BranchName
                        }
                    };
                }
            }
        }

        public Result GetOrg(int URId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't exist");
                }

                var query = (from x in c.DevOrganisationBranches
                             where x.OId == user.OId
                             orderby x.OId ascending
                             select new
                             {
                                 BranchId = x.BranchId,
                                 Organization = x.DevOrganisation.OrganisationName,
                                 BranchName = x.BranchName
                             });

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Organization Branch Details!!"),
                    Data = query.ToList(),
                };
            }
        }

        public Result GetBranch(int Bid, int URId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't exist");
                }

                var branch = c.DevOrganisationBranches.SingleOrDefault(x => x.OId == user.OId && x.BranchId == Bid);
                if (branch == null)
                {
                    throw new ArgumentException("Branch Doesn't exist");
                }

                var query = (from x in c.DevOrganisationBranches
                             where x.BranchId == Bid && x.OId == user.OId
                             orderby x.OId ascending
                             select new
                             {
                                 BranchId = x.BranchId,
                                 Organization = x.DevOrganisation.OrganisationName,
                                 BranchName = x.BranchName,
                                 Address = x.CommonContactAddress.AddressLine1 + "," + x.CommonContactAddress.AddressLine2
                                           + "," + x.CommonContactAddress.City + "," + x.CommonContactAddress.State
                             }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Specific Branch Details!!"),
                    Data = query,
                };
            }
        }

    }
}
