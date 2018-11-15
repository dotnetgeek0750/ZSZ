using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.App_Start
{
    //这个Attribute，可以应用在方法上，可以应用多次
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckPermissionAttribute : Attribute
    {
        public string Permission { get; set; }
        public CheckPermissionAttribute(string permission)
        {
            this.Permission = permission;
        }
    }
}