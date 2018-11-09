using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
    public class DefaultController : Controller
    {
        public static ICityService ICityService { get; set; }

        // GET: Default
        public ActionResult Index()
        {
            ICityService.AddNew("北京");

            return Content("OK");
        }
    }
}