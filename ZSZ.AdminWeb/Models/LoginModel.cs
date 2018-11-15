using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.Models
{
    public class LoginModel
    {
        public string PhoneNum { get; set; }
        public string Password { get; set; }
        public string VerifyCode { get; set; }
    }
}