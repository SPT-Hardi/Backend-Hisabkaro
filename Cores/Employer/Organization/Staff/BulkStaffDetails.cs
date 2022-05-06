using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff;
using HIsabKaro.Models.Employer.Organization.Staff.Salary;
using HisabKaroDBContext;
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
        public Result Create(int URId,Models.Employer.Organization.Staff.BulkStaffDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _OId = c.SubUserOrganisations.SingleOrDefault(o => o.URId == URId);
                    if (_OId is null)
                    {
                        throw new ArgumentException("Organization Does Not Exits!");
                    }
                    var _FileId = (from x in c.CommonFiles where x.FGUID == value.CSV select x).FirstOrDefault();

                    var csvTable = new DataTable();
                    using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(_FileId.FilePath)), true))
                    {
                        csvTable.Load((IDataReader)csvReader);
                    }

                    string Column1 = csvTable.Columns[0].ToString();
                    string Row1 = csvTable.Rows[0][0].ToString();
                    //List<student> searchParameters = new List<student>();

                    for (int i = 0; i < csvTable.Rows.Count; i++)
                    {
                        //searchParameters.Add(new student { Name = csvTable.Rows[i][0].ToString(), Age = csvTable.Rows[i][1].ToString(), MobileNumber = csvTable.Rows[i][2].ToString() });
                        var _subUser = c.Students.SingleOrDefault(x => x.MobileNumber == csvTable.Rows[i][2]);
                        if (_subUser is not null)
                        {
                            throw new ArgumentException($"{csvTable.Rows[i][0]} Alredy in This Class");
                        }
                        var _stud = new Student()
                        {
                            Name = csvTable.Rows[i][0].ToString(),
                            Gender = csvTable.Rows[i][1].ToString(),
                            MobileNumber = csvTable.Rows[i][2].ToString()

                        };
                        c.Students.InsertOnSubmit(_stud);
                        c.SubmitChanges();
                    }
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Student Add Successfully"),
                        Data = new
                        {
                            
                        }
                    };
                }
            }
        }

        //public Result Create(Models.Employer.Organization.Staff.SClass value)
        //{
        //    using (DBContext c = new DBContext())
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            //var _OId = c.SubUserOrganisations.SingleOrDefault(o => o.URId == URId);
        //            //if (_OId is null)
        //            //{
        //            //    throw new ArgumentException("Organization Does Not Exits!");
        //            //}
        //            var _FileId = (from x in c.CommonFiles where x.FGUID == value.CSV select x).FirstOrDefault();

        //            var csvTable = new DataTable();
        //            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(_FileId.FilePath)), true))
        //            {
        //                csvTable.Load(csvReader);
        //            }

        //            string Column1 = csvTable.Columns[0].ToString();
        //            string Row1 = csvTable.Rows[0][0].ToString();
        //            List<student> searchParameters = new List<student>();

        //            for (int i = 0; i < csvTable.Rows.Count; i++)
        //            {
        //                searchParameters.Add(new student { Name = csvTable.Rows[i][0].ToString(), Age = csvTable.Rows[i][1].ToString(), MobileNumber = csvTable.Rows[i][2].ToString() });
        //                var _subUser = c.Students.SingleOrDefault(x => x.MobileNumber == csvTable.Rows[i][2]);
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
        //                    Student = searchParameters,
        //                }
        //            };
        //        }
        //    }
        //}

        //public Result Create(Models.Employer.Organization.Staff.SClass value)
        //{
        //    using (DBContext c = new DBContext())
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            var _FileId = (from x in c.CommonFiles where x.FGUID == value.CSV select x).FirstOrDefault();

        //            var csvTable = new DataTable();
        //            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(_FileId.FilePath)), true))
        //            {
        //                csvTable.Load(csvReader);
        //            }

        //            string Column1 = csvTable.Columns[0].ToString();
        //            string Row1 = csvTable.Rows[0][0].ToString();
        //            List<student> searchParameters = new List<student>();

        //            for (int i = 0; i < csvTable.Rows.Count; i++)
        //            {
        //                searchParameters.Add(new student { Name = csvTable.Rows[i][0].ToString(), Age = csvTable.Rows[i][1].ToString(), MobileNumber = csvTable.Rows[i][2].ToString() });
        //            }


        //            scope.Complete();
        //            return new Result()
        //            {
        //                Status = Result.ResultStatus.success,
        //                Message = string.Format($"Organisation Add Successfully"),
        //                Data = new
        //                {
        //                    Student=searchParameters,
        //                }
        //            };
        //        }
        //    }
        //}


    }    
}
