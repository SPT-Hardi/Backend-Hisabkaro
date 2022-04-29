using HIsabKaro.Models.Common;
using HisabKaroDBContext;


namespace HIsabKaro.Cores.Employer.Organization.Staff
{
    public class StaffOrganizations
    {
        public Result Get(int URId) 
        {
            using (DBContext c = new DBContext())
            {
                
                        
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Staff organization details get successfully!",
                    Data = new IntegerNullString()
                    {
                        
                    }
                };
            }
        }
    }
}
