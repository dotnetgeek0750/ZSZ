using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.FrontWeb
{
    public class FrontUtils
    {
        /// <summary>
        /// 获得当前登录用户ID，如果没有登录则返回NULL
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static long? GetUserId(HttpContextBase ctx)
        {
            return (long?)ctx.Session["UserId"];
        }

        /// <summary>
        /// 获得当前城市ID
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static long GetCityId(HttpContextBase ctx)
        {
            long? userId = GetUserId(ctx);
            if (userId == null)
            {
                long? cityId = (long?)ctx.Session["CityId"];
                if (cityId != null)
                {
                    return cityId.Value;
                }
                else
                {
                    var citySvc = DependencyResolver.Current.GetService<ICityService>();
                    return citySvc.GetAll()[0].Id;
                }
            }
            else
            {
                var userSvc = DependencyResolver.Current.GetService<IUserService>();
                long? cityId = userSvc.GetById(userId.Value).CityId;
                if (cityId == null)
                {
                    var citySvc = DependencyResolver.Current.GetService<ICityService>();
                    return citySvc.GetAll()[0].Id;
                }
                else
                {
                    return cityId.Value;
                }
            }

        }
    }
}