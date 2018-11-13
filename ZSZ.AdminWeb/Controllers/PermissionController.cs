using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.Models;
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

        public ActionResult GetDelete(long id)
        {
            PermSvc.MarkDeleted(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Add(PermissionAddNewModel model)
        {
            PermSvc.AddPermission(model.Name, model.Description);
            return Json(new AjaxResult() { Status = "ok" });
        }


        [HttpGet]
        public ActionResult Edit(long id)
        {
            var perm = PermSvc.GetById(id);
            return View(perm);
        }

        [HttpPost]
        public ActionResult Edit(PermissionEditModel model)
        {
            PermSvc.UpdatePermission(model.Id, model.Name, model.Description);
            return Json(new AjaxResult() { Status = "ok" });
        }
    }
}