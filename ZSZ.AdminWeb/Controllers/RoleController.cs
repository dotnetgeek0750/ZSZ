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
    public class RoleController : Controller
    {
        public IRoleService roleService { get; set; }
        public IPermissionService perService { get; set; }

        // GET: Role
        public ActionResult List()
        {
            var roles = roleService.GetAll();
            return View(roles);
        }

        public ActionResult Delete(long id)
        {
            roleService.MarkDeleted(id);
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        public ActionResult Add()
        {
            var perms = perService.GetAll();
            return View(perms);
        }

        [HttpPost]
        public ActionResult Add(RoleAddModel model)
        {
            long roleId = roleService.AddNew(model.Name);
            perService.AddPermIds(roleId, model.PermissionIds);
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var role = roleService.GetById(id);
            var rolePerms = perService.GetByRoleId(id);
            return View();
        }
    }
}