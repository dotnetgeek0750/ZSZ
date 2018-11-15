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
    public class AdminUserController : Controller
    {

        public IAdminUserService userService { get; set; }
        public ICityService cityService { get; set; }
        public IRoleService roleService { get; set; }

        // GET: AdminUser
        public ActionResult List()
        {
            var users = userService.GetAll();
            return View(users);
        }

        public ActionResult Delete(long id)
        {
            userService.MarkDeleted(id);
            return Json(new AjaxResult { Status = "ok" });
        }

        public ActionResult Add()
        {
            var cities = cityService.GetAll().ToList();
            cities.Insert(0, new DTO.CityDTO { Id = 0, Name = "总部" });
            var roles = roleService.GetAll();
            AdminUserAddViewModel model = new AdminUserAddViewModel
            {
                Cities = cities.ToArray(),
                Roles = roles,
            };
            return View(model);
        }
    }
}