using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using LoginServer.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LoginServer.Controllers
{
    [Route("api/[controller]")]
    public class CreateAccountController : Controller
    {
        // POST: api/createaccount/create
        [HttpPost]
        public CreationResponse Create(CreationRequest request)
        {
            return null;
        }
    }
}
