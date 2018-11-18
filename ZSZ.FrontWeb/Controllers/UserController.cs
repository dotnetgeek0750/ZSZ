using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
    public class UserController : Controller
    {
        public IUserService userService { get; set; }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string phoneNum, string verifyCode)
        {
            var serverVerifyCode = (string)TempData["verifyCode"];
            if (serverVerifyCode != verifyCode)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = "验证码错误"
                });
            }
            var user = userService.GetByPhoneNum(phoneNum);
            if (user == null)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = "没有这个手机号"
                });
            }

            string smsCode = new Random().Next(1000, 9999).ToString();
            RuPengSMSSender smsSender = new RuPengSMSSender();
            smsSender.AppKey = "xxxx";
            smsSender.UserName = "rupengtest1";
            var senderResult = smsSender.SendSMS("12", smsCode, phoneNum);
            if (senderResult.code == 0)
            {
                TempData["smsCode"] = smsCode;
                TempData["ForgotPasswordPhoneNum"] = phoneNum;

                return Json(new AjaxResult
                {
                    Status = "ok",
                });
            }
            else
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = senderResult.msg,
                });
            }
        }



        [HttpGet]
        public ActionResult ForgotPassword2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword2(string verifyCode)
        {
            string serverSmsCode = (string)TempData["smsCode"];
            if (verifyCode != serverSmsCode)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = "短信验证码错误"
                });
            }
            else
            {
                //告诉第三步，短信验证码验证通过，防止恶意用户修改URL跳过第二步
                TempData["IsForgotPassword2_OK"] = true;
                return Json(new AjaxResult
                {
                    Status = "ok",
                });
            }
        }



        [HttpGet]
        public ActionResult ForgotPassword3()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword3(string password)
        {
            bool? is2OK = (bool?)TempData["IsForgotPassword2_OK"];
            if (is2OK != true)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = "你没有通过短信验证码的验证",
                });
            }
            string phoneNum = (string)TempData["ForgotPasswordPhoneNum"];
            var user = userService.GetByPhoneNum(phoneNum);
            userService.UpdatePwd(user.Id, password);
            return Json(new AjaxResult
            {
                Status = "ok",
            });
        }


        [HttpGet]
        public ActionResult ForgotPassword4()
        {
            return View();
        }
    }
}