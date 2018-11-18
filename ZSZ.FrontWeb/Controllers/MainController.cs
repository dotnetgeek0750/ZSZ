using CaptchaGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.FrontWeb.Models;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
    public class MainController : Controller
    {
        public ICityService cityService { get; set; }

        public IUserService userService { get; set; }

        // GET: Main
        public ActionResult Index()
        {
            var cityId = FrontUtils.GetCityId(HttpContext);
            string cityName = cityService.GetById(cityId).Name;
            ViewBag.CityName = cityName;

            var cities = cityService.GetAll();
            return View(cities);
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult CreateVerifyCode()
        {
            var verifyCode = Common.CommonHelper.CreateVerifyCode(4);
            TempData["verifycode"] = verifyCode;

            //Session["verifycode"] = verifyCode;

            var ms = ImageFactory.GenerateImage(verifyCode, 45, 100, 20);
            return File(ms, "image/jpeg");
        }


        public ActionResult SendSmsVerifyCode(string phoneNum, string verifyCode)
        {
            string serverVerifyCode = (string)TempData["verifyCode"];
            if (serverVerifyCode != verifyCode)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = "验证码错误",
                });
            }
            string smsCode = new Random().Next(1000, 9999).ToString();
            TempData["smsCode"] = smsCode;
            //把发送验证码的手机号放在TempData，在注册的时候再次检查一下注册手机号是否为发送验证码的手机号
            //防止网站漏洞
            TempData["regPhoneNum"] = phoneNum;

            RuPengSMSSender smsSender = new RuPengSMSSender();
            smsSender.AppKey = "xxxx";
            smsSender.UserName = "rupengtest1";
            var senderResult = smsSender.SendSMS("188", smsCode, phoneNum);
            if (senderResult.code == 0)
            {
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


        [HttpPost]
        public ActionResult Register(UserRegModel model)
        {
            if (ModelState.IsValid == false)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = MVCHelper.GetValidMsg(ModelState)
                });
            }
            string serverSmsCode = (string)TempData["smsCode"];
            if (model.SmsCode != serverSmsCode)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "短信验证码错误" });
            }

            var serverPhoneNum = (string)TempData["regPhoneNum"];
            if (model.PhoneNum != serverPhoneNum)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "注册的手机号跟发送验证码手机号不一致" });
            }

            if (userService.GetByPhoneNum(model.PhoneNum) != null)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = "此手机号已经被占用",
                });
            }
            userService.AddNew(model.PhoneNum, model.Password);
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = MVCHelper.GetValidMsg(ModelState),
                });
            }
            var user = userService.GetByPhoneNum(model.Password);
            if (user != null)
            {
                if (userService.IsLocked(user.Id))
                {
                    return Json(new AjaxResult { Status = "error", ErrorMsg = "账号被锁定" });
                }
            }
            var isOk = userService.CheckLogin(model.PhoneNum, model.Password);
            if (isOk)
            {
                //一旦登陆成功，就重置登陆失败信息
                userService.ResetLoginError(user.Id);

                //把当前登录用户信息存入session
                Session["UserId"] = user.Id;
                Session["CityId"] = user.CityId;

                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                if (user != null)
                {
                    userService.IncrLoginError(user.Id);
                }
                return Json(new AjaxResult { Status = "error", ErrorMsg = "用户名或密码错误" });
            }
        }



        /// <summary>
        /// 切换当前用户的城市ID
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ActionResult SwitchCityId(long cityId)
        {
            long? userId = FrontUtils.GetUserId(HttpContext);
            if (userId == null)
            {
                Session["CityId"] = cityId;
            }
            else
            {
                userService.SetUserCityId((long)userId, cityId);
            }
            return Json(new AjaxResult { Status = "ok" });
        }

    }
}