using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.App_Start
{
    public class ZSZAuthorizationFilter : IAuthorizationFilter
    {
        //由于此类不归AutoFac管治，因此无法注入
        // public IAdminUserService userService { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //获得当前要执行的Action上标注的CheckPermissionAttribute实例对象
            CheckPermissionAttribute[] permAttrs = (CheckPermissionAttribute[])filterContext.ActionDescriptor
                     .GetCustomAttributes(typeof(CheckPermissionAttribute), false);
            if (permAttrs.Length == 0)
            {
                //没有标注任何的CheckPermissionAttribute
                return;
            }
            //得到当前登录用户ID
            long? userId = (long?)filterContext.HttpContext.Session["LoginUserId"];
            if (userId == null)
            {
                //修改filterContext.Result，真正的Action方法就不会执行了
                filterContext.Result = new ContentResult() { Content = "没有登录" };
                return;
            }
            else
            {
                //由于ZSZAuthorizationFilter不是被AutoFac创建，因此不会自动进行属性的注入
                //需要手动获取Service对象
                IAdminUserService userService = DependencyResolver.Current.GetService<IAdminUserService>();

                //检查权限，用Action上的Attribute跟数据库中用户的权限对比
                foreach (var permAttr in permAttrs)
                {
                    if (!userService.HasPermission(userId.Value, permAttr.Permission))
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            filterContext.Result = new JsonNetResult
                            {
                                Data = new AjaxResult
                                {
                                    Status = "redirect",
                                    ErrorMsg = "没有权限",
                                    Data = "/Main/Login",
                                },
                            };
                        }
                        else
                        {
                            filterContext.Result = new ContentResult
                            {
                                Content = $"没有{permAttr.Permission}权限",
                            };
                        }
                        return;
                    }
                }
            }
        }
    }
}