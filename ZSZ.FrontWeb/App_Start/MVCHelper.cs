using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ZSZ.FrontWeb
{
    public static class MVCHelper
    {
        // using System.Web.Mvc;
        //有两个ModelStateDictionary类，别弄混乱了。要使用System.Web.Mvc下的
        public static string GetValidMsg(ModelStateDictionary modelState)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in modelState.Keys)
            {
                if (modelState[key].Errors.Count <= 0)
                {
                    continue;
                }
                sb.Append("属性【").Append(key).Append("】错误：");
                foreach (var modelError in modelState[key].Errors)
                {
                    sb.AppendLine(modelError.ErrorMessage);
                }
            }
            return sb.ToString();
        }



        public static string ToQueryString(this NameValueCollection queryString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < queryString.Keys.Count; i++)
            {
                string key = queryString.Keys[i];
                sb.Append(key).Append("=").Append(Uri.EscapeDataString(queryString[key]));
                if (i != queryString.Keys.Count - 1)
                {
                    sb.Append("&");
                }
            }
            return sb.ToString();
        }


        /// 从QueryString中移除name的值
        public static string RemoveQueryString(NameValueCollection queryString, string name)
        {
            NameValueCollection newNVC = new NameValueCollection(queryString);
            newNVC.Remove(name);
            return newNVC.ToQueryString();
        }

        /// 修改QueryString中name的值为value，如果不存在，则添加
        // NameValueCollection相当于Dictionary，存放的是QueryString中的键值对
        public static string UpdateQueryString(NameValueCollection queryString, string name, object value)
        {
            //拷贝一份，不影响本来的QueryString
            NameValueCollection newNVC = new NameValueCollection(queryString);
            if (newNVC.AllKeys.Contains(name))
            {
                newNVC[name] = Convert.ToString(value);
            }
            else
            {
                newNVC.Add(name, Convert.ToString(value));
            }
            return newNVC.ToQueryString();
        }
    }
}
