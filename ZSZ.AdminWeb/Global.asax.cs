using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZSZ.AdminWeb.App_Start;
using ZSZ.CommonMVC;

namespace ZSZ.AdminWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            log4net.Config.XmlConfigurator.Configure();
            GlobalFilters.Filters.Add(new ZSZExceptionFilter());

            ModelBinders.Binders.Add(typeof(string), new TrimToDBCModelBinder());
        }
    }
}
