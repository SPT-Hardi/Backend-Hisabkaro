using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIsabKaro.Services.ResumePDF
{
    public class HTMLString
    {
        public static string GetHTMLString()
        {
            using (DBContext c = new DBContext())
            {

                var employees =(from x in c.SubUsers select x).Take(5);
                var sb = new StringBuilder();
                sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                                   <img src = 'https://gemachapi.otobit.com/Upload/b79fb620-46e9-4b58-ae47-de4055289178_download1.jpg'> 
                                <table align='center'>
                                    <tr>
                                        <th>UId</th>
                                        <th>MobileNumber</th>
                                        <th>Name</th>
                                        <th>City</th>
                                    </tr>");
                foreach (var emp in employees)
                {
                    sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", emp.UId, emp.MobileNumber, emp.SubUsersDetail!=null ? emp.SubUsersDetail.FullName : null, emp.DefaultLanguageId);
                }
                sb.Append(@"
                                </table>
                            </body>
                        </html>");
                return sb.ToString();
            }
        }
    }
}
