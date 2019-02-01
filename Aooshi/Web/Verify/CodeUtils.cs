using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Aooshi.Web.Verify
{
    /// <summary>
    /// ������֤����
    /// </summary>
    public class CodeUtils
    {
        /// <summary>
        /// Session����
        /// </summary>
        public const string CacheName = "Aooshi::vCode";
        /// <summary>
        /// ����ֵ
        /// </summary>
        private static string CacheSeed = Aooshi.RandomString.CreateOnlyID();
        /// <summary>
        /// �ж�ָ������֤���Ƿ���ȷ
        /// </summary>
        /// <param name="Code">��֤��</param>
        public static bool CheckCode(string Code)
        {
            if (string.IsNullOrEmpty(Code)) return false;

            HttpCookie cook = HttpContext.Current. Request.Cookies[CodeUtils.CacheName];
            if (cook == null || cook.Value == "")  return false;
            return Code.Equals(HttpContext.Current.Cache[cook.Value] as string, StringComparison.OrdinalIgnoreCase);
        }


        static int _Seed = 1;
        /// <summary>
        /// ��ȡ��ǰ������
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
