using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Common
{
    /// <summary>
    /// 公共类库
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// 根据字符串生成MD5
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>MD5值</returns>
        public static string CalcMD5(this string str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            return CalcMD5(bytes);
        }

        /// <summary>
        /// 根据Byte数组生成MD5值
        /// </summary>
        /// <param name="bytes">Byte数组</param>
        /// <returns>MD5值</returns>
        public static string CalcMD5(byte[] bytes)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeBytes = md5.ComputeHash(bytes);
                string result = "";
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    result += computeBytes[i].ToString("X").Length == 1 ? "0" + computeBytes[i].ToString("X") : computeBytes[i].ToString("X");
                }
                return result;
            }
        }

        /// <summary>
        /// 根据流生成MD5值
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>MD5值</returns>
        public static string CalcMD5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeBytes = md5.ComputeHash(stream);
                string result = "";
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    result += computeBytes[i].ToString("X").Length == 1 ? "0" + computeBytes[i].ToString("X") : computeBytes[i].ToString("X");
                }
                return result;
            }
        }

        /// <summary>
        /// 生成验证码字符串
        /// </summary>
        /// <param name="len">要生成的验证码长度</param>
        /// <returns>验证码</returns>
        public static string CreateVerifyCode(int len)
        {
            char[] data = { 'a', 'c', 'd', 'e', 'f', 'g', 'k', 'm', 'p', 'r', 's', 't', 'w', 'x', 'y', '3', '4', '5', '7', '8' };
            StringBuilder sbCode = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < len; i++)
            {
                char ch = data[rand.Next(data.Length)];
                sbCode.Append(ch);
            }
            return sbCode.ToString();
        }
    }
}
