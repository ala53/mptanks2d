using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Login.Models;

namespace ZSB.Infrastructure.Apis.Login.Database
{
    public class LoginDB
    {
        public LoginDB(Contexts.LoginDatabaseContext ctx)
        {
            DBContext = ctx;
        }
        public Contexts.LoginDatabaseContext DBContext { get; set; }
        public const int MaxActiveLoginCount = 25;
        public const int MaxActiveServerTokenCount = 25;
        public readonly TimeSpan LoginLength = TimeSpan.FromDays(15);
        internal UserModel GetUser(string email)
        {
            return DBContext.Users
                .Where(a => a.Username.Equals(email, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

        internal bool IsAuthorized(string sessionKey, out UserModel usr)
        {
            var sess = FindBySessionKey(sessionKey);
            usr = null;
            if (sess == null) return false;
            if (DateTime.UtcNow > sess.ExpiryDate)
            {
                //Remove session
                RemoveSession(sess);
                return false;
            }
            usr = sess.Owner;
            return true;
        }

        internal bool ValidateAccount(string email, string password)
        {
            var usr = GetUser(email);
            return usr == null ? false : Backend.PasswordHasher.CompareHash(password, usr.PasswordHashes);
        }

        internal UserActiveSessionModel DoLogin(string email, string password)
        {
            if (!ValidateAccount(email, password))
                throw new ArgumentException("username_or_password_incorrect");

            var usr = GetUser(email);
            if (!usr.IsEmailConfirmed)
                throw new ArgumentException("email_not_confirmed");
            //Clean up all of the sessions that have expired
            var removable = new List<UserActiveSessionModel>();
            foreach (var mdl in usr.ActiveSessions)
                if (DateTime.UtcNow > mdl.ExpiryDate)
                    removable.Add(mdl);
            foreach (var mdl in removable)
                usr.ActiveSessions.Remove(mdl);
            //Check if we are over the limit
            if (usr.ActiveSessions.Count >= MaxActiveLoginCount) //Remove the first one
                usr.ActiveSessions.Remove(usr.ActiveSessions.First());
            //And create the login key
            var sess = new UserActiveSessionModel();
            sess.Owner = usr;
            sess.ExpiryDate = DateTime.Now + LoginLength;
            sess.SessionKey = Guid.NewGuid().ToString("N");
            usr.ActiveSessions.Add(sess);
            DBContext.Sessions.Add(sess);

            DBContext.SaveChanges();
            return sess;
        }

        internal void UpdateUser(UserModel user)
        {
            DBContext.Users.Update(user);
            DBContext.SaveChanges();
        }

        internal UserActiveSessionModel FindBySessionKey(string sessionKey)
        {
            var sk = DBContext.Sessions
                .Where(a => a.SessionKey == sessionKey)
                .FirstOrDefault();

            if (sk == null) return null;

            //Check if expired
            if (DateTime.UtcNow > sk.ExpiryDate)
            {
                RemoveSession(sk);
                return null;
            }

            return sk;
        }

        internal void RemoveSession(UserActiveSessionModel session)
        {
            DBContext.Sessions.Remove(session);
            session.Owner.ActiveSessions.Remove(session);
            DBContext.Users.Update(session.Owner);
            Save();
        }

        internal UserServerTokenModel ValidateServerToken(string token)
        {
            var tk = DBContext.ServerTokens
                .Where(a => a.ServerToken == token)
                .FirstOrDefault();
            return tk;
        }

        internal void Save()
        {
            DBContext.SaveChanges();
        }
    }
}
