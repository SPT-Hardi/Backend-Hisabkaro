﻿using HIsabKaro.Cores.Helpers;
using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Common
{
    public class Drops
    {
        public Models.Common.Response.Data General(string Con, List<SqlParameter> @params)
        {
            var selectQuery = new Developer.Schema.Properties.Property().Value(Con, "SqlCommand");
            var Ids = Cores.Common.Contact.Current.Ids;
           // var Ids = (Ids)id;
            using (var sqlCon = new SqlConnection(DatabaseFunctions.ConnectionString()))
            {
                var sqlCom = new System.Data.SqlClient.SqlCommand("", sqlCon);
                var OId = Ids.OId is null ? null : Ids.OId;
                var RId = Ids.RId is null ? null : Ids.RId;
                
                sqlCom.Parameters.AddWithValue("OId", OId);
                sqlCom.Parameters.AddWithValue("RId", RId);
                sqlCom.Parameters.AddWithValue("UId", Ids.UId);
                sqlCom.Parameters.AddWithValue("LId", Ids.LId);
                var sqlComTotal = new System.Data.SqlClient.SqlCommand("", sqlCon);
                sqlComTotal.Parameters.AddWithValue("OId", OId);
                sqlComTotal.Parameters.AddWithValue("RId", RId);
               sqlComTotal.Parameters.AddWithValue("UId", Ids.UId);
                sqlComTotal.Parameters.AddWithValue("LId", Ids.LId);
                if (@params.Count != 0)
                {
                    var TotalWhereSqlParameters = new List<SqlParameter>();
                    var WhereSqlParameters = new List<SqlParameter>();
                    foreach (var x in @params)
                    {
                        TotalWhereSqlParameters.Add(new SqlParameter(x.SourceColumn, x.Value));
                        WhereSqlParameters.Add(new SqlParameter(x.SourceColumn, x.Value));
                    }

                    sqlCom.Parameters.AddRange(WhereSqlParameters.ToArray());
                    sqlComTotal.Parameters.AddRange(TotalWhereSqlParameters.ToArray());
                }

                var sq = "select * from(" + selectQuery + ") as temp2 ";
                sqlCom.CommandText = sq;
                sqlComTotal.CommandText = "select count(*) from (" + selectQuery + ") as temp";
                sqlCon.Open();
                var dt = new DataTable();
                var da = new System.Data.SqlClient.SqlDataAdapter(sqlCom);
                da.Fill(dt);
                var columns = new List<Models.Common.Columns>();
                foreach (DataColumn col in dt.Columns)
                    columns.Add(new Models.Common.Columns() { Name = col.ColumnName, Type = col.DataType.Name });
                var records = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                        row.Add(col.ColumnName, dr[col]);
                    records.Add(row);
                }

                int Total = (int)sqlComTotal.ExecuteScalar();
                var currentTotal = dt.Rows.Count;
                var data = new Models.Common.Response.Data()
                {
                    Total = Total,
                    CurrentTotal = currentTotal,
                    Records = records
                };
                return data;
            }
        }
        public Result ProfileDrop(object UId,int Id,object URId)
        {
            using (DBContext c = new DBContext())
            {
                //var org = (from x in c.SubUserOrganisations where x.UId == (int)UId select x).ToList();
                var orglist = (from x in c.SubUserOrganisations
                           where x.UId == (int)UId
                           select x.OId).ToList().Distinct();
                Models.Common.ProfileDrop profile = new ProfileDrop();
                foreach (var item in orglist)
                {
                    var rolelist = (from x in c.SubUserOrganisations where x.OId==item && x.UId==(int)UId select x).ToList();
                    var Orgrolelist=new List<IntegerNullString>();
                    foreach (var x in rolelist)
                    {

                        Orgrolelist.Add(new IntegerNullString()
                        {
                            Id =x.URId,
                            Text = x.SubRole.RoleName,
                        }); 
                    }
                    profile.Profiles.Add(new ProfileDropOrg()
                    {
                        OrgName =new IntegerNullString() { Id=item,Text= c.DevOrganisations.Where(x => x.OId == item).SingleOrDefault().OrganisationName},
                        Orgrolelist=Orgrolelist,
                    });
                }
                if (URId == null)
                {
                    if (orglist.Count() == 0)
                    {
                        var data = (from x in c.SubUsers where x.UId == (int)UId select x.UId).FirstOrDefault();
                        return new Result()
                        {
                            Status = Result.ResultStatus.success,
                            Message = "ProfileDrop for UId get successfully!",
                            Data = data,
                        };
                    }
                }
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "ProfileDrop for URId get successfully!",
                    Data = profile,
                };
            }
        }
    }
}
