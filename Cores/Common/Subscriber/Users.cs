using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace HIsabKaro.Cores.Common.Subscriber.Users
{
    public class User
    {
       /* internal Result Create(Models.Common.Subscriber.User value, DBContext c, int OId = 0)
        {

            // Check for valid role
            // Check for unique username and username format

            var DateNow = DateTime.UtcNow;

            var Ids = Contact.Current.Ids;

            if (OId == 0)
            {
                OId = Ids.OId;
            }

            var Role = (from x in c.sub_Roles
                        where x.OId == OId & x.RId == value.Role.Id & x.RoleName == value.Role.Text
                        select x).SingleOrDefault();

            if (Role is null)
            {
                throw new ArgumentException("Invalid Role!");
            }

            if (Ids.LId != 1 && Role.LoginTypeId == 1)
            {
                throw new ArgumentException("Invalid Role!");
            }

            var cn = new Contact.ContactNumber().Create(value.ContactNumber, c);

            var passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
            var Password = BCrypt.Net.BCrypt.HashPassword((value.Password == null ? value.UserName : value.Password), passwordSalt);

            var uT = new Entities.sub_User()
            {
                OId = OId,
                GUID = Guid.NewGuid(),
                UserName = value.UserName,
                RId = Role.RId,
                ContactNumberId = cn.Data.Id,
                EnableConnection = value.EnableConnection,
                EnableChat = value.EnableChat,
                EnableLiveFeed = value.EnableLiveFeed,
                EnableMyNotifications = value.EnableMyNotifications,
                EnableSystemNotifications = value.EnableSystemNotifications,
                IsAnonymous = false,
                LastActivityDate = DateNow,
                IsLockedOut = value.IsLocked,
                PasswordSalt = passwordSalt,
                Password = Password,
                PasswordFormat = 2,
                Comment = value.Comment,
                FailedPasswordAttemptCount = 0,
                LastLockoutDate = DateNow,
                LastLoginDate = DateNow,
                LastPasswordChangedDate = DateNow,
                EMailId = value.EMail
            };
            c.sub_Users.InsertOnSubmit(uT);
            c.SubmitChanges();

            return new Result()
            {
                Message = string.Format("User: {0} created successfully!", uT.UserName),
                Status = Result.ResultStatus.success,
                Data = new 
                {
                    Id = uT.GUID,
                    Text = uT.UserName,
                    UId = uT.UId,
                }
            };
        }

        internal Result Update(int id, Models.Common.Subscriber.User value, DBContext c)
        {
            var Ids = Context.Current.Ids;

            //Pending: Check for unique username and username format

            var uT = (from x in c.sub_Users
                      where x.OId == Ids.OId & x.UId == id
                      select x).SingleOrDefault();
            if (uT == null)
            {
                throw new ArgumentException("Invalid User!");
            }

            var Role = (from x in c.sub_Roles
                        where x.OId == Ids.OId && x.RId == value.Role.Id && x.RoleName == value.Role.Text
                        select x).SingleOrDefault();

            if (Role is null)
            {
                throw new ArgumentException("Invalid Role!");
            }

            if (Ids.LId != 1 && Role.LoginTypeId == 1)
            {
                throw new ArgumentException("Invalid Role!");
            }

            uT.OId = Ids.OId;
            uT.UserName = value.UserName;
            uT.RId = Role.RId;

            var cn = new Result();
            if (uT.ContactNumberId is null)
            {
                cn = new Contact.ContactNumber().Create(value.ContactNumber, c);
            }
            else
            {
                cn = new Contact.ContactNumber().Update((int)uT.ContactNumberId, value.ContactNumber, c);
            }
            if (value.ResetPassword)
            {
                var passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
                var Password = BCrypt.Net.BCrypt.HashPassword(value.UserName , passwordSalt);
                uT.PasswordSalt = passwordSalt;
                uT.Password = Password;
            }

            uT.ContactNumberId = cn.Data.Id;
            uT.IsLockedOut = value.IsLocked;
            uT.EnableConnection = value.EnableConnection;
            uT.EnableChat = value.EnableChat;
            uT.EnableLiveFeed = value.EnableLiveFeed;
            uT.EnableMyNotifications = value.EnableMyNotifications;
            uT.EnableSystemNotifications = value.EnableSystemNotifications;
            uT.EMailId = value.EMail;

            c.SubmitChanges();

            return new Result()
            {
                Message = string.Format("User: {0} updated successfully!", uT.UserName),
                Status = Result.ResultStatus.success,
                Data = new Models.Common.Pair.ObjectObject()
                {
                    Id = uT.GUID,
                    Text = uT.UserName
                }
            };
        }

        //public Common.Result Connected(SignalR.Connection connection)
        //{
        //    Common.Result ConnectedRet = default;
        //    ConnectedRet = new Common.Result();
        //    using (var c = new DBDataContext())
        //    {
        //        var UId = Core.Current.Contact.UId;
        //        var uc = (from x in c.Users_Connections
        //                  where x.ConnectionId == connection.ConnectionId & x.UId == UId
        //                  select x).FirstOrDefault;
        //        if (uc is null)
        //        {
        //            uc = new Core.LogisticsIN.Users_Connection();
        //            uc.UId = UId;
        //            uc.ConnectionId = connection.ConnectionId;
        //            uc.UserAgent = connection.UserAgent;
        //            uc.IsConnected = true;
        //            c.Users_Connections.InsertOnSubmit(uc);
        //        }
        //        else
        //        {
        //            uc.IsConnected = true;
        //            uc.UserAgent = connection.UserAgent;
        //        }

        //        c.SubmitChanges();
        //    }

        //    return ConnectedRet;
        //}

        //public Common.Result Disconnected(Model.SignalR.Connection connection)
        //{
        //    Common.Result DisconnectedRet = default;
        //    DisconnectedRet = new Common.Result();
        //    using (var c = new Core.DBDataContext())
        //    {
        //        var uc = (from x in c.Users_Connections
        //                  where x.UId == Core.Current.Contact.UId & x.ConnectionId == connection.ConnectionId & x.IsConnected == true
        //                  select x).FirstOrDefault;
        //        if (uc is object)
        //        {
        //            uc.IsConnected = false;
        //            c.SubmitChanges();
        //        }
        //    }

        //    return DisconnectedRet;
        //}

        //public Common.Result Reconnected(Model.SignalR.Connection connection)
        //{
        //    Common.Result ReconnectedRet = default;
        //    ReconnectedRet = new Common.Result();
        //    using (var c = new Core.DBDataContext())
        //    {
        //        var uc = (from x in c.Users_Connections
        //                  where x.ConnectionId == connection.ConnectionId & x.IsConnected == false
        //                  select x).FirstOrDefault;
        //        if (uc is object)
        //        {
        //            uc.IsConnected = true;
        //            c.SubmitChanges();
        //        }
        //    }

        //    return ReconnectedRet;
        //}
*/
        public Models.Common.Ids ForURIdIds(int URId)
        {
            using (DBContext c = new DBContext())
            {
                var Ids = (from x in c.SubUserOrganisations
                           where x.URId==URId
                           select new Models.Common.Ids
                           {
                               URId= x.URId,
                               UId = x.UId,
                               RId = x.RId,
                               OId = x.OId,
                               LId = (int)x.SubRole.LoginTypeId
                           }).FirstOrDefault();
                return Ids;
            }
        }
        public Models.Common.Ids ForUIdIds(int UId)
        {
            using (DBContext c = new DBContext())
            {
                var Ids = (from x in c.SubUsers
                           where x.UId == UId
                           select new Models.Common.Ids
                           {
                               UId = x.UId,
                               LId = (int)x.DefaultLoginTypeId
                           }).FirstOrDefault();
                return Ids;
            }
        }

        /* public Result CreateFirst(Models.Office.Subscriber.User value)
         {
             using (DBContext c = new DBContext())
             {
                 // Check for valid role
                 // Check for unique username and username format

                 var DateUTCNow = DateTime.UtcNow;

                 var passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
                 var Password = BCrypt.Net.BCrypt.HashPassword((value.Password == null ? value.UserName : value.Password), passwordSalt);

                 var uT = new Entities.sub_User()
                 {
                     OId = 30000001,
                     GUID = Guid.NewGuid(),
                     UserName = value.UserName,
                     RId = 40000001,
                     ContactNumberId = null,
                     EnableConnection = true,
                     EnableChat = true,
                     EnableLiveFeed = true,
                     EnableMyNotifications = true,
                     EnableSystemNotifications = true,
                     IsAnonymous = false,
                     LastActivityDate = DateUTCNow,
                     IsLockedOut = false,
                     PasswordSalt = passwordSalt,
                     Password = Password,
                     PasswordFormat = 2,
                     Comment = "Create first",
                     FailedPasswordAttemptCount = 0,
                     LastLockoutDate = DateUTCNow,
                     LastLoginDate = DateUTCNow,
                     LastPasswordChangedDate = DateUTCNow,
                     EMailId = "prasanth@teamin.in"
                 };
                 c.sub_Users.InsertOnSubmit(uT);
                 c.SubmitChanges();

                 return new Result()
                 {
                     Message = string.Format("User: {0} created successfully!", uT.UserName),
                     Status = Result.ResultStatus.success,
                     Data = new Models.Common.Pair.ObjectObject()
                     {
                         Id = uT.GUID,
                         Text = uT.UserName
                     }
                 };
             }
         }*/
    }

   /* public class ChangePassword
    {
        public Models.Common.Result Update(Models.Common.Subscriber.Authentication.ChangePassword value)
        {
            using (var c = new DBContext())
            {
                var Ids = Common.Contact.Current.Ids;
                var q = (from x in c.sub_Users
                         where x.UId == Ids.UId & x.OId == Ids.OId
                         select x).Single();

                if (value.NewPassword != value.ConfirmPassword)
                {
                    throw new ArgumentException("New Password and Confirm Password should match");
                }

                if (BCrypt.Net.BCrypt.Verify(value.CurrentPassword, q.Password) == false)
                {
                    throw new ArgumentException("Invalid Current Password!");
                }

                q.PasswordSalt = BCrypt.Net.BCrypt.GenerateSalt();
                q.Password = BCrypt.Net.BCrypt.HashPassword(value.NewPassword, q.PasswordSalt);
                q.PasswordFormat = 2;

                c.SubmitChanges();

                return new Models.Common.Result()
                {
                    Message = string.Format("Password changed successfully!"),
                    Status = Models.Common.Result.ResultStatus.success,
                    Data = new Models.Common.Pair.ObjectObject()
                    {
                        Id = q.GUID,
                        Text = q.UserName
                    }
                };
            }
        }
    }*/
}