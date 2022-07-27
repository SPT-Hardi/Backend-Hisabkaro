  using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores
{
    public class CustomeDrops
    {
        public class RoleDrop
        {
            public Result Get()
            {
                using (DBContext c = new DBContext())
                {
                    var fixedlookup = (from x in c.SubFixedLookups where x.FixedLookupType.ToLower() == "logintype" && x.FixedLookupId != 23 select new { x.FixedLookupId, x.FixedLookup }).ToList();
                    List<IntegerNullString> logintype = new List<IntegerNullString>();
                    foreach (var item in fixedlookup)
                    {
                        logintype.Add(new IntegerNullString()
                        {
                            Id = item.FixedLookupId,
                            Text = item.FixedLookup,
                        });
                    }
                    if (logintype == null)
                    {
                        throw new ArgumentException("Not logintype");
                    }
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Role drop get successfully!",
                        Data = logintype,
                    };
                }
            }
        }
        public class Org_Drop
        {
            public Result Get(object UId)
            {
                using (DBContext c = new DBContext())
                {
                    if (UId == null)
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var orgList = (from x in c.DevOrganisations where x.UId == (int)UId select new IntegerNullString() { Id = x.OId, Text = x.OrganisationName, }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "organization drop get successfully!",
                        Data = orgList,
                    };
                }
            }
        }
        public class Branches_Drop
        {
            public Result Get(object UId, int Id)
            {
                using (DBContext c = new DBContext())
                {
                    if (UId == null)
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var org = (from x in c.DevOrganisations where x.UId == (int)UId && x.OId == (int)Id select x).FirstOrDefault();
                    var res = org.DevOrganisations_ParentOrgId.ToList().Select(z => new IntegerNullString() { Id = z.OId, Text = z.OrganisationName, }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "organization's branches drop get successfully!",
                        Data = res
                    };
                }
            }
        }
        public class Orgs_Branches_Drop
        {
            public Result Get(object UId, int? Id)
            {
                using (DBContext c = new DBContext())
                {
                    if (UId == null)
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    if (Id == null)
                    {
                        List<Models.Common.CustomeDrop.Org_Branch_Drop> org_Branch_Drop = new List<CustomeDrop.Org_Branch_Drop>();
                        var orgList = (from x in c.DevOrganisations where x.UId == (int)UId select x).ToList();
                        foreach (var x in orgList)
                        {
                            var res = new Models.Common.CustomeDrop.Org_Branch_Drop()
                            {
                                Organization = new IntegerNullString() { Id = x.OId, Text = x.OrganisationName },
                                Branches = x.DevOrganisations_ParentOrgId.ToList().Select(z => new IntegerNullString() { Id = z.OId, Text = z.OrganisationName }).ToList(),
                            };
                            org_Branch_Drop.Add(res);
                        }
                        return new Result()
                        {
                            Status = Result.ResultStatus.success,
                            Message = "organization's branches drop get successfully!",
                            Data = org_Branch_Drop,
                        };
                    }
                    else
                    {
                        var org = (from x in c.DevOrganisations where x.UId == (int)UId && x.OId == Id select x).FirstOrDefault();
                        var res = new Models.Common.CustomeDrop.Org_Branch_Drop()
                        {
                            Organization = new IntegerNullString() { Id = org.OId, Text = org.OrganisationName },
                            Branches = org.DevOrganisations_ParentOrgId.ToList().Select(z => new IntegerNullString() { Id = z.OId, Text = z.OrganisationName }).ToList(),
                        };
                        return new Result()
                        {
                            Status = Result.ResultStatus.success,
                            Message = "organization's branches drop get successfully!",
                            Data = res,
                        };
                    }
                }
            }
        }
        public class Salary_Type_Drop
        {
            public Result Get()
            {
                using (DBContext c = new DBContext())
                {
                    var salaryTypeList = (from x in c.SubFixedLookups where x.FixedLookupType == "SalaryType" select new IntegerNullString() { Id = x.FixedLookupId, Text = x.FixedLookup }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Salary type list get successfully!",
                        Data = salaryTypeList,
                    };
                }
            }
        }
        public class Incentive_Type_Drop
        {
            public Result Get()
            {
                using (DBContext c = new DBContext())
                {
                    var incentiveTypeList = (from x in c.SubFixedLookups where x.FixedLookupId == 48 || x.FixedLookupId == 49 select new IntegerNullString() { Id = x.FixedLookupId, Text = x.FixedLookup }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Incentive type list get successfully!",
                        Data = incentiveTypeList,
                    };
                }
            }
        }
        public class Org_Shift_Type
        {
            public Result Get(object UId,int Id) 
            {
                using (DBContext c = new DBContext())
                {
                    if (UId == null) 
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var shiftTimeList = (from x in c.DevOrganisations where x.OId == Id select x).FirstOrDefault().DevOrganisationsShiftTimes.ToList().Select(z => new Models.Common.CustomeDrop.Shit_Time()
                    {
                        ShiftTimeId=z.ShiftTimeId,
                        StartTime=z.StartTime,
                        EndTime=z.EndTime,
                        MarkLate=z.MarkLate
                    }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "shift time list get successfully!",
                        Data = shiftTimeList
                    };
                }
            }
        }
       /* public class Branch_Shift_Type
        {
            public Result Get(object UId,int Id)
            {
                using (DBContext c = new DBContext())
                {
                    if (UId == null) 
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var shiftTimeList = (from x in c.DevOrganisationBranches where x.BranchId == Id select x).FirstOrDefault().DevOrganisationsBranchesShiftTimes.ToList().Select(z => new Models.Common.CustomeDrop.Shit_Time()
                    {
                        ShiftTimeId=z.ShiftTimeId,
                        StartTime=z.StartTime,
                        EndTime=z.EndTime,
                        MarkLate=z.MarkLate
                    }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "shift time list get successfully!",
                        Data = shiftTimeList
                    };
                }
            }
        }*/
        public class Skill_Search 
        {
            public Result Get(string keyword) 
            {
                using (DBContext c = new DBContext())
                {
                    var skills = (from x in c.SubLookups where x.SubFixedLookup.FixedLookupType == "Skills" && x.Lookup.Contains(keyword) orderby x.Lookup.IndexOf(keyword) ascending select new IntegerNullString() { Id =x.LookupId, Text =x.Lookup, }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "skills searched successfully!",
                        Data = skills
                    };
                }
            }
        }
        public class Language_Search
        {
            public Result Get(string keyword) 
            {
                using (DBContext c = new DBContext())
                {
                    var languageList = (from x in c.SubFixedLookups where x.FixedLookupType == "Language" && x.FixedLookup.Contains(keyword) orderby x.FixedLookup.IndexOf(keyword) ascending select new IntegerNullString() { Id = x.FixedLookupId, Text = x.FixedLookup, }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "language list get successfully!",
                        Data = languageList
                    };
                }
            }
        }
        public class EnglishLevel_Drop 
        {
            public Result Get()
            {
                using (DBContext c = new DBContext())
                {
                    var englishLevelList = (from x in c.SubFixedLookups where x.FixedLookupType == "EnglishLevel" select new IntegerNullString() { Id = x.FixedLookupId, Text = x.FixedLookup }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "english level list get successfully!",
                        Data = englishLevelList,
                    };
                }
            }
        }
        public class Job_Type 
        {
            public Result Get() 
            {
                using (DBContext c = new DBContext())
                {
                    var jobTypes = (from x in c.SubFixedLookups where x.FixedLookupType == "JobType" select new IntegerNullString() { Id = x.FixedLookupId, Text = x.FixedLookup, }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Job type list get successfully!",
                        Data = jobTypes
                    };
                }
            }
        }
        public class LoginType_Drop 
        {
            public Result Get() 
            {
                using (DBContext c = new DBContext())
                {
                    var loginTypes = (from x in c.SubFixedLookups where x.FixedLookupId == 20 || x.FixedLookupId == 21 select new IntegerNullString() { Id = x.FixedLookupId, Text = x.FixedLookup }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "loginType drop get successfully!",
                        Data = loginTypes
                    };
                }
            }
        }
        public class Languages 
        {
            public Result Get()
            {
                using (DBContext c = new DBContext())
                {
                    var languages = (from x in c.SubFixedLookups where x.FixedLookupType=="Language" select new IntegerNullString() { Id = x.FixedLookupId, Text = x.FixedLookup }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "loginType drop get successfully!",
                        Data = languages,
                    };
                }
            }
        }

        public class Industry_Sector_Drop
        {
            public Result Get()
            {
                using (DBContext c = new DBContext())
                {
                    var IndustrySector = (from x in c.SubFixedLookups where x.FixedLookupType == "IndustrySector" select new IntegerNullString() { Id = x.FixedLookupId, Text = x.FixedLookup }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "IndustrySector drop get successfully!",
                        Data = IndustrySector,
                    };
                }
            }
        }

        public class HighestEducation_Drop
        {
            public Result Get()
            {
                using (DBContext c = new DBContext())
                {
                    var HighestEducation = (from x in c.SubFixedLookups where x.FixedLookupType == "HighestEducation" select new IntegerNullString() { Id = x.FixedLookupId, Text = x.FixedLookup }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "HighestEducation drop get successfully!",
                        Data = HighestEducation,
                    };
                }
            }
        }
    }
}
