using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb
{
    public class AdminHelper
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
    }
}