using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Common.Shift
{
    public class ShiftTimes
    {
        internal Result Create(int OId ,List<Models.Common.Shift.ShitTime> value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var listShift = value.Select(x => new { starttime = x.StartTime, Endtime = x.EndTime, marklate = x.MarkLate }).ToList();
                    if (listShift.Distinct().Count() != listShift.Count())
                    {
                        throw new ArgumentException($"Duplicate Entry In Times!");
                    }

                    value.ForEach((x) =>
                    {
                        if (x.ShiftTimeId is null)
                        {
                            var shifttime = new DevOrganisationsShiftTime()
                            {
                                StartTime = (TimeSpan)x.StartTime,
                                EndTime = (TimeSpan)x.EndTime,
                                MarkLate = x.MarkLate,
                                OId = OId
                            };
                            c.DevOrganisationsShiftTimes.InsertOnSubmit(shifttime);
                            c.SubmitChanges();
                        }
                        else
                        {
                            var _Shift = c.DevOrganisationsShiftTimes.SingleOrDefault(y => y.ShiftTimeId == x.ShiftTimeId);
                            _Shift.StartTime = (TimeSpan)x.StartTime;
                            _Shift.EndTime = (TimeSpan)x.EndTime;
                            _Shift.MarkLate = x.MarkLate;
                            _Shift.OId = OId;
                            c.SubmitChanges();
                        }
                    });

                    scope.Complete();
                    return new Models.Common.Result
                    {
                        Status = Models.Common.Result.ResultStatus.success,
                        Message = string.Format("Shift Time Added Successfully!"),
                    };
                }
            }
        }

        internal Result CreateBranchShift(int OId, int BId, List<Models.Common.Shift.ShitTime> value)
        {
            using (DBContext c = new DBContext())
            {
                var listShift = value.Select(x => new { starttime = x.StartTime, Endtime = x.EndTime, marklate = x.MarkLate }).ToList();
                if (listShift.Distinct().Count() != listShift.Count())
                {
                    throw new ArgumentException($"Duplicate Entry In Times!");
                }

               
                var shifttime = value.Select(x => new DevOrganisationsBranchesShiftTime()
                {
                    StartTime = (TimeSpan)x.StartTime,
                    EndTime = (TimeSpan)x.EndTime,
                    MarkLate = x.MarkLate,
                    OId = OId,
                    BranchId = BId,
                    IsSameAsOrg = x.IsSameAsOrg == true ? true : false
                });
                c.DevOrganisationsBranchesShiftTimes.InsertAllOnSubmit(shifttime);
                c.SubmitChanges();

                return new Models.Common.Result
                {
                    Status = Models.Common.Result.ResultStatus.success,
                    Message = string.Format("shift Added Successfully!"),
                };
            }
        }
    }
}
