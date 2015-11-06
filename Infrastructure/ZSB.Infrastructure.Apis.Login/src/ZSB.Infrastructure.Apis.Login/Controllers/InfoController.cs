using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ZSB.Infrastructure.Apis.Account.Models;
using ZSB.Infrastructure.Apis.Account.Database;
using ZSB.Infrastructure.Apis.Account.Database.Contexts;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ZSB.Infrastructure.Apis.Account.Controllers
{
    public class InfoController : Controller
    {
        private LoginDB ldb;
        public InfoController(LoginDatabaseContext ctx)
        {
            ldb = new LoginDB(ctx);
        }

        [HttpGet, Route("/Info/{userId}")]
        public async Task<ResponseModelBase> GetUserInfo(Guid userId)
        {
            var usr = await ldb.FindByUniqueId(userId);

            if (usr == null)
                return ErrorModel.Of("user_not_found");

            return OkModel.Of(new UserInfoResponseModel(usr));
        }
    }
}
