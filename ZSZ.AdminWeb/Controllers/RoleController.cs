using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
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


        public ActionResult BatchDelete(long[] selectIds)
        {
            foreach (var id in selectIds)
            {
                roleService.MarkDeleted(id);
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        [HttpGet]
        public ActionResult Add()
        {
            //检查Model验证是否通过
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = MVCHelper.GetValidMsg(ModelState),
                });
            }
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
            var allPerms = perService.GetAll();
            RoleEditGetModel model = new RoleEditGetModel
            {
                AllPerms = allPerms,
                RolePerms = rolePerms,
                Role = role,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RoleEditModel model)
        {
            roleService.Update(model.Id, model.Name);
            perService.UpdatePermIds(model.Id, model.PermissionIds);
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}