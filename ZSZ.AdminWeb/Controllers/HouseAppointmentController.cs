using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class HouseAppointmentController : Controller
    {
        public IHouseAppointmentService appService { get; set; }
        public IAdminUserService userService { get; set; }

        public ActionResult List()
        {
            long? userId = AdminHelper.GetUserId(HttpContext);
            long? cityId = userService.GetById(userId.Value).CityId;
            if (cityId == null)
            {
                return View("Error", (object)"总部的人不能进行房源抢单");
            }
            var app = appService.GetPagedData(cityId.Value, "未处理", 10, 1);
            return View(app);
        }

        public ActionResult Follow(long appId)
        {
            long? userId = AdminHelper.GetUserId(HttpContext);
            bool isOK = appService.Follow(userId.Value, appId);
            if (isOK)
            {
                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult { Status = "fail" });
            }

        }
    }
}