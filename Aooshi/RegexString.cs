using System;

namespace Aooshi
{
    /// <summary>
    /// 常规正常表达式
    /// </summary>
    public class RegexString
    {
        /// <summary>
        /// 表示是否为日期时间的正则表达式 格式为: 2000-01-01 00:00:00
        /// </summary>
        public const string IsDateTime = @"^[0-9]{4}\-[0-9]{1,2}\-[0-9]{1,2} [0-9]{2}\:[0-9]{2}\:[0-9]{2}$";

        /// <summary>
        /// 表示是否为正确的日期格式正则表达式 格式为: 2000-01-01
        /// </summary>
        public const string IsDate = @"^[0-9]{4}\-[0-9]{1,2}\-[0-9]{1,2}$";

        /// <summary>
        /// 表示是否为正确的时间格式正则表达式 格式为: 00:00:00
        /// </summary>
        public const string IsTime = @"^[0-9]{2}\:[0-9]{2}\:[0-9]{2}$";

        /// <summary>
        /// 表示是否为邮件的正则表达式
        /// </summary>
        public const string IsEmail = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// 表示是否为金额的正则表达式,不带小数位数
        /// </summary>
        public const string IsMoney = @"^[123456789]{1}\d{0,9}$";

        /// <summary>
        /// 表示是否为不可为负数,最多带两数小数的正金额值
        /// </summary>
        public const string IsJustMoney = @"^\d+(\.\d{1,2})?$";

        /// <summary>
        /// 表示是否为最多两位小数的只负金额值
        /// </summary>
        public const string IsMinusMoney = @"^(\-){1}\d+{1}(\.\d{1,2})?$";

        /// <summary>
        /// 表示是否为可正可负且可带两位小数的金额数值
        /// </summary>
        public const string IsJMMoney = @"^(\-)?\d+{1}(\.\d{1,2})?$";

        /// <summary>
        /// 判断输入字符串是否是数字与字母的组合
        /// </summary>
        public const string IsLetterAndNumber = "^[A-Za-z0-9]+$";

        /// <summary>
        /// 判断输入字符串是否为中文
        /// </summary>
        public const string IsChinese = @"[\u4e00-\u9fa5]+";

        /// <summary>
        /// 判断是否为正确的页码, 0 - 999999 一至六位数的页码的正则表达式
        /// </summary>
        public const string IsPage = @"^\d{1,6}$";
        ///// <summary>
        ///// 表示是否为<seealso cref="MyRandom.CreateRanID()"/>方法生成的随机数
        ///// </summary>
        //public const string IsID = @"^\d{15}$";

        /// <summary>
        /// 表示是否为<seealso cref="RandomString.CreateOnlyID()"/>方法生成的有效的2000年至2700年之间的10位随机数
        /// </summary>
        public const string IsOnlyID = @"^[0-9A-Z]{10}$";

        /// <summary>
        /// 表示是否为<seealso cref="RandomString.CreateCourseID()"/>方法所产生的有效字符串
        /// </summary>
        public const string IsCourseID = @"^[0-9A-Z]+$";

        /// <summary>
        /// 验证是否为正确的32位MD5加密串
        /// </summary>
        public const string IsMD5 = "^[0-9a-zA-Z]{32}$";
    }
}
