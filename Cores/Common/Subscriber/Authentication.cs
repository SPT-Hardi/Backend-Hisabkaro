using System;
using System.Dynamic;
using System.Linq;


namespace HIsabKaro.Cores.Common.Subscriber
{
    public partial class Authentication
    {/*
        public Models.Common.Result Validate(string Username, string Password, string OrganisationIdentifier)
        {
            Models.Common.Result ValidateRet = new Models.Common.Result();

            var now = DateTime.Now;

            var guid = new Guid(OrganisationIdentifier);
            if (string.IsNullOrEmpty(Username.Trim()) | string.IsNullOrEmpty(Password.Trim()))
            {
                ValidateRet.Message = "Username and Password is required!";
                ValidateRet.Status = Models.Common.Result.ResultStatus.info;
            }
            else
            {
                using (var c = new DBContext())
                {
                    var q = (from x in c.sub_Users
                             where x.UserName == Username & x.dev_Organisation.GUID == guid
                             select x).FirstOrDefault();
                    if (q is null)
                    {
                        ValidateRet.Message = "Invalid username or password!";
                        ValidateRet.Status = Models.Common.Result.ResultStatus.info;
                    }
                    else if (q.IsLockedOut == true)
                    {
                        ValidateRet.Message = "User is currently locked / not approved!";
                        ValidateRet.Status = Models.Common.Result.ResultStatus.danger;
                    }
                    else if (q.sub_Role.Status == false)
                    {
                        ValidateRet.Message = "Currently your default role has been disabled!";
                        ValidateRet.Status = Models.Common.Result.ResultStatus.info;
                    }
                    else if (BCrypt.Net.BCrypt.Verify(Password, q.Password))
                    {
                        q.LastLoginDate = now;
                        q.LastActivityDate = now;
                        ValidateRet.Message = "Validation was successfull!";
                        dynamic d = new { q.UId, q.UserName, q.sub_Role.dev_Fixed_Login_Type.LoginType, UGUID = q.GUID }.ToExpandoObject();
                        ValidateRet.Data = d;
                        ValidateRet.Status = Models.Common.Result.ResultStatus.success;
                        q.FailedPasswordAttemptCount = 0;
                        c.SubmitChanges();
                    }
                    else
                    {
                        ValidateRet.Message = "Invalid username or password!";
                        ValidateRet.Status = Models.Common.Result.ResultStatus.warning;
                        q.FailedPasswordAttemptCount = q.FailedPasswordAttemptCount + 1;
                        if (q.FailedPasswordAttemptCount >= 20)
                        {
                            q.IsLockedOut = true;
                        }

                        c.SubmitChanges();
                    }
                }
            }

            return ValidateRet;
        }*/

        //public Models.Common.Result RefreshToken(Guid UGUID, string Client, Guid Token, string Ticket)
        //{
        //    Models.Common.Result RefreshTokenRet = default;
        //    RefreshTokenRet = new Models.Common.Result();
        //    using (var scope = new TransactionScope())
        //    {
        //        using (var c = new DBContext())
        //        {
        //            var user = (from x in c.sub_Users
        //                        where x.GUID == UGUID
        //                        select x).SingleOrDefault();
        //            if (user is null)
        //            {
        //                throw new ArgumentException("Invalid UGUID!");
        //            }

        //            var Exists = (from x in c.sub_Users_RefreshTokens
        //                          where x.sub_User.GUID == UGUID & x.Client == Client
        //                          select x).SingleOrDefault();
        //            if (Exists is object)
        //            {
        //                c.sub_Users_RefreshTokens.DeleteOnSubmit(Exists);
        //                c.SubmitChanges();
        //            }

        //            var t = new Entities.sub_Users_RefreshToken()
        //            {
        //                UId = user.UId,
        //                Client = Client,
        //                DeviceData = "",
        //                Issued = DateTime.Now,
        //                Expires = DateTime.Now.AddDays(7d),
        //                RefreshToken = Token,
        //                ProtectedTicket = Ticket
        //            };
        //            c.sub_Users_RefreshTokens.InsertOnSubmit(t);
        //            c.SubmitChanges();
        //            scope.Complete();
        //        }
        //    }

        //    RefreshTokenRet.Message = "Refresh token updated successfully!";
        //    RefreshTokenRet.Status = Models.Common.Result.ResultStatus.success;
        //    return RefreshTokenRet;
        //}

        //public string GetRefreshToken(Guid Token)
        //{
        //    using (var c = new DBContext())
        //    {
        //        var Exists = (from x in c.sub_Users_RefreshTokens
        //                      where x.Token.ToString() == Token.ToString()
        //                      select x).SingleOrDefault();
        //        if (Exists is null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return Exists.Token;
        //        }
        //    }
        //}

        //public string RemoveRefreshToken(Guid Token)
        //{
        //    string Ticket = "";
        //    using (var c = new DBContext())
        //    {
        //        var exists = (from x in c.sub_Users_RefreshTokens
        //                      where x.Token.ToString() == Token.ToString()
        //                      select x).SingleOrDefault();
        //        if (exists is object)
        //        {
        //            Ticket = exists.ProtectedTicket;
        //            c.sub_Users_RefreshTokens.DeleteOnSubmit(exists);
        //            c.SubmitChanges();
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    return Ticket;
        //}

        /*public Models.Common.Ids GetUserByRefreshToken(string token)
        {
            using (DBContext c = new DBContext())
            {
                var Ids = (from x in c.sub_Users_RefreshTokens
                           where x.Token == token
                           select new Models.Common.Ids
                           {
                               UId = x.UId,
                               RId = x.sub_User.RId,
                               LId = x.sub_User.sub_Role.LoginTypeId,
                               OId = x.sub_User.OId,
                               UGUId = x.sub_User.GUID
                           }).FirstOrDefault();
                if (Ids == null)
                {
                    throw new ArgumentException("Invalid token");
                }
                return Ids;
            }
        }*/

    }
}