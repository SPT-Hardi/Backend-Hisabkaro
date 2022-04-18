using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Common.Shift
{
    public class ShiftTimes
    {
        internal Result Create(int OId ,Models.Common.Shift.ShitTime value)
        {
            using (DBContext c = new DBContext())
            {
                var listShift = value.TimeLists.Select(x => new { starttime = x.StartTime, Endtime = x.EndTime, marklate = x.MarkLate }).ToList();
                if (listShift.Distinct().Count() != listShift.Count())
                {
                    throw new ArgumentException($"Duplicate Entry In Times!");
                }

                var shifttime = value.TimeLists.Select(x => new DevOrganisationsShiftTime()
                {
                    StartTime = (TimeSpan)x.StartTime,
                    EndTime = (TimeSpan)x.EndTime,
                    MarkLate = x.MarkLate,
                    OId = OId
                });
                c.DevOrganisationsShiftTimes.InsertAllOnSubmit(shifttime);
                c.SubmitChanges();

                var id = shifttime.Select(x => x.OId == OId).ToList();
                return new Models.Common.Result
                {
                    Status = Models.Common.Result.ResultStatus.success,
                    Message = string.Format("Contact Address Added Successfully!"),
                };
            }
        }

        internal Result CreateBranchShift(Models.Common.Shift.ShitTime value, int OId, int BId)
        {
            using (DBContext c = new DBContext())
            {
                var listShift = value.TimeLists.Select(x => new { starttime = x.StartTime, Endtime = x.EndTime, marklate = x.MarkLate }).ToList();
                if (listShift.Distinct().Count() != listShift.Count())
                {
                    throw new ArgumentException($"Duplicate Entry In Times!");
                }

                var shifttime = value.TimeLists.Select(x => new DevOrganisationsBranchesShiftTime()
                {
                    StartTime = (TimeSpan)x.StartTime,
                    EndTime = (TimeSpan)x.EndTime,
                    MarkLate = x.MarkLate,
                    OId = OId,
                    BranchId = BId
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
