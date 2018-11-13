using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class PermissionController : Controller
    {
        public IPermissionService PermSvc { get; set; }

        // GET: Permission
        public ActionResult List()
        {
            var perms = PermSvc.GetAll();
            return View(perms);
        }

        public ActionResult Delete(long id)
        {
            PermSvc.MarkDeleted(id);
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}