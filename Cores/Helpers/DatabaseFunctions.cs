using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Helpers
{
    internal partial class DatabaseFunctions
    {
        public int GetNextId(string ColumnName, DBContext cT = default)
        {
            int GetNextIdRet = default;
            DBContext c = cT is null ? new DBContext() : cT;
            GetNextIdRet = c.ExecuteQuery<int>("exec sp_common_Next_Id {0}", ColumnName).Single();
            if (cT is null)
                c.Dispose();
            return GetNextIdRet;
        }

        // Public Shared Function ExecuteScalar(Query As String) As String
        // Try
        // Query = Query.ToString.Replace("@OId", TeamIN.Core.Current.Organisation.OId.ToString)
        // Dim sqlCon As New SqlClient.SqlConnection(ConnectionString())
        // Dim sqlCom As New SqlClient.SqlCommand(Query, sqlCon)
        // sqlCon.Open()
        // ExecuteScalar = sqlCom.ExecuteScalar()
        // sqlCon.Close()
        // Catch ex As Exception
        // ExecuteScalar = ""
        // End Try
        // End Function

        internal static string ConnectionString()
        {
            using (DBContext c = new DBContext())
            {
                return c.Connection.ConnectionString.ToString();
            }
        }

        //internal static string HangFireConnectionString()
        //{
        //    return My.Settings.HangFireConnectionString;
        //}

        //internal static string StorageConnectionString()
        //{
        //    return My.Settings.StorageConnectionString;
        //}
    }
}
