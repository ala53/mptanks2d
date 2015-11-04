using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Apis.Login.Database;
using ZSB.Infrastructure.Apis.Login.Database.Contexts;
using ZSB.Infrastructure.Apis.Login.Models;

namespace ZSB.Infrastructure.Apis.Login.Controllers
{
    public class CleanupController : Controller
    {
        LoginDB ldb;
        public CleanupController(LoginDatabaseContext context)
        {
            ldb = new LoginDB(context);
        }

        /// <summary>
        /// Does database cleanup...meaning:
        /// Removes all expired session keys and
        /// Removes all expired server tokens
        /// </summary>
        [HttpGet, Route("/Private/Internal/Cleanup")]
        public async Task<ResponseModelBase> DoDBCleanup()
        {
            try
            {
                //Clean up sessions and tokens
                var oldSessions = ldb.DBContext.Sessions.Where(a => DateTime.UtcNow > a.ExpiryDate);
                var oldTokens = ldb.DBContext.ServerTokens.Where(a => DateTime.UtcNow > a.ExpiryDate);

                int sessCt = 0, tknCt = 0;
                foreach (var sess in oldSessions)
                {
                    sessCt++;
                    //remove index from user object
                    sess?.Owner?.RemoveSession(sess);
                }
                await ldb.Save();
                foreach (var tkn in oldTokens)
                {
                    tknCt = 0;
                    //remove index from user object
                    tkn?.Owner?.RemoveToken(tkn);
                }
                await ldb.Save();
                //Remove accounts more than 7 days old that are not verified
                var oldUsers = ldb.DBContext.Users.Where(a => !a.IsEmailConfirmed)
                    .Where(a => (DateTime.UtcNow - a.EmailConfirmationSent).TotalDays > 7).ToList();

                var usrCt = oldUsers.Count();
                oldUsers.ForEach(async a => await ldb.DeleteUser(a));
                await ldb.Save();

                return OkModel.Of($"{tknCt} tokens removed, {sessCt} sessions removed, {usrCt} users removed");
            }
            catch (Exception e)
            {
                return ErrorModel.Of(e);
            }
        }
    }
}
