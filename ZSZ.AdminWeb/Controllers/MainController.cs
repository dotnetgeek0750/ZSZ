using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.Models;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class MainController : Controller
    {
        public IAdminUserService userService { get; set; }

        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (model.VerifyCode != (string)Session["verifycode"])
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
            bool result = userService.CheckLogin(model.PhoneNum, model.Password);
            if (result)
            {
                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "用户名或者密码错误" });
            }
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateVerifyCode()
        {
            var verifyCode = Common.CommonHelper.CreateVerifyCode(4);
            TempData["verifycode"] = verifyCode;

            Session["verifycode"] = verifyCode;



            var ms = ImageFactory.GenerateImage(verifyCode, 60, 100, 20);
            return File(ms, "image/jpeg");
        }
    }
}