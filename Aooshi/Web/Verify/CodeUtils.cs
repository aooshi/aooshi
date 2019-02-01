using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Aooshi.Web.Verify
{
    /// <summary>
    /// 基础验证方法
    /// </summary>
    public class CodeUtils
    {
        /// <summary>
        /// Session名称
        /// </summary>
        public const string CacheName = "Aooshi::vCode";
        /// <summary>
        /// 种子值
        /// </summary>
        private static string CacheSeed = Aooshi.RandomString.CreateOnlyID();
        /// <summary>
        /// 判断指定的验证码是否正确
        /// </summary>
        /// <param name="Code">验证码</param>
        public static bool CheckCode(string Code)
        {
            if (string.IsNullOrEmpty(Code)) return false;

            HttpCookie cook = HttpContext.Current. Request.Cookies[CodeUtils.CacheName];
            if (cook == null || cook.Value == "")  return false;
            return Code.Equals(HttpContext.Current.Cache[cook.Value] as string, StringComparison.OrdinalIgnoreCase);
        }


        static int _Seed = 1;
        /// <summary>
        /// 获取当前排序项
        /// </summary>
        internal static string Seed
        {
            get
            {
                _Seed++;
                if (_Seed == 99999999) _Seed = 1;
                return "A-" + _Seed;
            }
        }
    }
}
