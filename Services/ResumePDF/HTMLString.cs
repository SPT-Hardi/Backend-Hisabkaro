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
        public static string GetHTMLString(int UId)
        {
            using (DBContext c = new DBContext())
            {
               
                if ((int)UId == 0) 
                {
                    throw new ArgumentException("Not authorized!");
                }
                var MobileNumber = (from x in c.SubUsers where x.UId == (int)UId 
                                    select x.MobileNumber).FirstOrDefault();
                var UserDetails = (from x in c.SubUsersDetails where x.UId == (int)UId select new
                {
                    Name=x.FullName,
                    Email=x.Email,
                    City=x.CommonContactAddress==null ? null : x.CommonContactAddress.City,
                    State=x.CommonContactAddress==null ?null : x.CommonContactAddress.State,
                }).FirstOrDefault();
                var About = (from x in c.EmpResumeAbouts where x.UId == (int)UId select x.About).FirstOrDefault();
                var WorkExperience = (from x in c.EmpResumeWorkExperiences
                                      where x.UId == (int)UId
                                      select new
                                      {
                                          Designation=x.JobTitle,
                                          CompanyName=x.OrganizationName,
                                          Duration = new Cores.Common.StringFormators().CountDuration(x.StartDate,x.EndDate),
                                      }).ToList();
                var Education = (from x in c.EmpResumeEducations where x.UId == (int)UId select new
                {
                    StreamName=x.SubFixedLookup.FixedLookup,
                    InstituteName=x.InstituteName,
                    Duration= new Cores.Common.StringFormators().CountDuration(x.StartDate, x.EndDate),
                }).ToList();
                var Skills = (from x in c.EmpResumeSkills where x.UId == (int)UId select new
                {
                    SkillName=x.SkillName
                }).ToList();
                var OtherCertificate = (from x in c.EmpResumeOtherCertificates
                                        where x.UId == (int)UId
                                        select new
                                        {
                                            CertificateName=x.CertificateName,
                                            Duration= new Cores.Common.StringFormators().CountDuration(x.StartDate, x.EndDate),
                                        }).ToList();

                var sb = new StringBuilder();
                sb.Append($@"
                              <html>
                              <head> 
                              </head>
                              <body>
                             <div class='left'></div>
                              <div class='stuff'>
                              <br><br>
                             <h1>{UserDetails.Name}</h1>
                              <br/>
                              <p class='head'>{UserDetails.Email}</p>
                              <p class='head'>{MobileNumber}</p>
                              <p class='head'>{UserDetails.City},{UserDetails.State}</p><br/>
                              <h2>About</h2>
                                <hr/>
                                <p class='head'>{About}</p>
                                <br>
                                <h2> Work Experience </h2>
                                <hr/>

                          ");
                foreach (var x in WorkExperience)
                {
                    sb.Append($@"
                                 <h3>{x.Designation}</h3>
                                 <p class='title'>{x.CompanyName}</p>
                                 <p class='head'>{x.Duration}</p>
                                 <br>  
                              ");
                }
                sb.Append(@"
                          <h2>Education</h2>
                          <hr/>
                          ");
                foreach (var x in Education)
                {
                    sb.Append($@"
                               <h3>{x.InstituteName}</h3>
                               <p class='title'>{x.StreamName}</p>
                               <p class='head'>{x.Duration}</p>
                               <br>
                               ");
                }
                sb.Append(@"
                           <h2>Skills</h2>
                             <hr/>
                             <p class='head'> 
                         ");
                foreach (var x in Skills) 
                {
                    sb.Append($@"
                                 <li>{x.SkillName}</li>
                               ");
                }
                sb.Append(@"
                             </p>
                             <br>
                             <h2>Certificate</h2>
                             <hr/>
                           ");
                foreach (var x in OtherCertificate) 
                {
                    sb.Append($@"
                               <h3>{x.CertificateName}</h3>
                                <p class='head'>{x.Duration}</p>
                                ");
                }
                sb.Append(@"
                               </div>
                               <div class='right'></div>
                                </body>
                                </html>
                          ");
                return sb.ToString();
            }
        }
    }
}
//< html >
//< head >
//< link rel = "stylesheet" type = "text/css" href = "C:\Users\RajMashruwala\Desktop\ResumeCss.css" > </ link >
//     </ head >
//     < body >
//       < div class= "left" ></ div >
//        < div class= "stuff" >
//           < br >< br >
//           < h1 > Patel Vijal </ h1 >
//              < br />
//              < p class= "head" > Address </ p >
//                 < p class= "head" > email Id </ p >
//                     < p class= "head" > Conatact Number </ p >< br />
//                       < h2 > About </ h2 >
//                       < hr />
//                       < h3 > email Id </ h3 >                   
//                          < br >                        
//                          < h2 > Work Experience </ h2 >                         
//                            < hr />                          
//                             < h3 > Designation Name </ h3 >                              
//                                < p class= "title" > Company Name </ p >                                  
//                                    < p class= "head" > Duration </ p >                                   
//                                     < br >                                   
//                                     < h2 > Education </ h2 >                                   
//                                     < hr />                                   
//                                     < h3 > Institute Name </ h3 >                                      
//                                        < p class= "title" > Stream </ p >                                      
//                                         < p class= "head" > Duration </ p >                                        
//                                          < br >                                        
//                                          < h2 > Skills </ h2 >                                        
//                                          < hr />                                       
//                                          < p class= "head" >                                         
//                                             < li > Web Design with HTML & CSS</li>
//    <li>Web Design with HTML & CSS</li>     
//  </p>
//  <br>
//<h2>Certificte</h2>
//  <hr />
// <h3>Name</h3>
//  <p class= "head" > Duration </ p >
//</ div >
//< div class= "right" ></ div >   
//          </ body >        
//          </ html >