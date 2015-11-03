﻿using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZSB.Infrastructure.Web.Home.Models;

namespace ZSB.Infrastructure.Web.Home.Controllers
{
    [Route("/Login/ForgotPassword")]
    public class ForgotPasswordController : Controller
    {
        [HttpGet, Route("Request")]
        public IActionResult RequestSendEmail()
        {
            return View("SendEmail/Request");
        }

        [HttpPost, Route("Request")]
        public async Task<IActionResult> ConfirmSendEmail(string EmailAddress)
        {
            var result = await Rest.RestHelper.DoPostDynamic(
                Startup.LoginServerAddress + "account/password/forgot/request",
                new
                {
                    EmailAddress = EmailAddress
                });

            //Error handling
            ViewBag.Error = result == null || result.Error;
            if (ViewBag.Error)
            {
                if (result == null)
                    ViewBag.Message = Rest.ResponseHelper.Get("unknown_error");
                else ViewBag.Message = Rest.ResponseHelper.Get(result.Message);
            }
            else ViewBag.Message = Rest.ResponseHelper.Get(result.Message);

            return View("SendEmail/Confirm");
        }

        [HttpGet, Route("Change/{accountId}/{emailConfirmCode}")]
        public IActionResult ChangePassword(Guid accountId, Guid emailConfirmCode)
        {
            ViewBag.Error = false;
            return View("Change/Change");
        }

        [HttpPost, Route("Change/{accountId}/{emailConfirmCode}")]
        public async Task<IActionResult> ConfirmPasswordChanged(
            Guid accountId, Guid emailConfirmCode, string Password, string ConfirmPassword)
        {
            ViewBag.Error = false;
            ViewBag.Password = Password;
            ViewBag.ConfirmPassword = ConfirmPassword;
            //Check that they match
            if (Password != ConfirmPassword)
            {
                ViewBag.Error = true;
                ViewBag.Message = Rest.ResponseHelper.Get("password_do_not_match");
                return View("Change/Change");
            }

            var resp = await Rest.RestHelper.DoPostDynamic(Startup.LoginServerAddress + "account/password/forgot/change",
                new
                {
                    ConfirmationCode = emailConfirmCode,
                    UserId = accountId,
                    NewPassword = Password
                });

            ViewBag.Error = resp == null || resp.Error;

            if (resp != null)
                ViewBag.Message = Rest.ResponseHelper.Get(resp.Message);

            if (ViewBag.Error)
                return View("Change/Change");
            else
                return View("Change/Confirm");
        }
    }
}
