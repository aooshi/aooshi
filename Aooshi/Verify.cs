using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Aooshi
{
    /// <summary>
    /// 进行数据检查
    /// </summary>
    public class Verify
    {
        /// <summary>
        /// 将指定的字符串，根据指定的条件语句，用正则表达式判断是否满足条件
        /// </summary>
        /// <param name="Str">要判断的字符串，当该值为Null或Empty时，返回值永远为false</param>
        /// <param name="RegEx">要进行判断的正则表达式条件</param>
        /// <returns>返回值表示验证是否满足条件</returns>
        public static bool IsDefine(string Str, string @RegEx)
        {
            if (string.IsNullOrEmpty(Str)) return false;

            return Regex.IsMatch(Str, RegEx);
        }

        /// <summary>
        /// 判断指定的字符串是否包含半角下的点、逗号、正返括号、单引号中的其中任何一个字符
        /// </summary>
        /// <param name="Str">要进行判断的字符串</param>
        /// <returns>返回值表示是否包含该类符号</returns>
        public static bool IsInc(string Str)
        {
            return Verify.IsDefine(Str, @"[.\,\(\)\']+");
        }

        /// <summary>
        /// 判断是否与盘符匹配,如:c:\或E:\等
        /// </summary>
        /// <param name="Str">要验证的数据</param>
        public static bool IsHard(string Str)
        {
            return Verify.IsDefine(Str, @"^[A-Za-z]{1}:\\");
        }

        /// <summary>
        /// 判断指定字符串是否为有效的Email地址
        /// </summary>
        /// <param name="Str">要进行判断的字符串</param>
        /// <returns>返回值表示是否为正确的邮件地址</returns>
        public static bool IsMail(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsEmail);
        }

        /// <summary>
        /// 验证是否为正确的不为负数最多带两位小数的金额值
        /// </summary>
        /// <param name="Str">要验证的字符串</param>
        /// <returns>返回值表示验证是否满足条件</returns>
        public static bool IsMoney(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsJustMoney);
        }

        /// <summary>
        /// 判断输入字符串是否是数字与字母的组合
        /// </summary>
        /// <param name="Str">要进行验证的字符串</param>
        /// <returns>表示是否为数字与字母的组合</returns>
        public static bool IsLetterAndNumber(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsLetterAndNumber);
        }

        /// <summary>
        /// 判断输入字符串是否是指定位数的数字与字母的组合,
        /// </summary>
        /// <param name="Str">要进行验证的字符串</param>
        /// <param name="Size">字符长度</param>
        /// <returns>表示是否为指定长度的数字与字母的组合</returns>
        public static bool IsLetterAndNumber(string Str, int Size)
        {
            return Verify.IsDefine(Str, "^[A-Za-z0-9]{" + Size.ToString() + "}$");
        }

        /// <summary>
        /// 判断输入字符串是否是指定位数的数字与字母的组合,
        /// </summary>
        /// <param name="Str">要进行验证的字符串</param>
        /// <param name="Max">最大长度</param>
        /// <param name="Min">最小长度</param>
        /// <returns>表示是否为指定长度的数字与字母的组合</returns>
        public static bool IsLetterAndNumber(string Str, int Min, int Max)
        {
            return Verify.IsDefine(Str, "^[A-Za-z0-9]{" + Min.ToString() + "," + Max.ToString() + "}$");
        }

        /// <summary>
        /// 判断字符串是否全为数字
        /// </summary>
        /// <param name="Str">要进行判断的字符串</param>
        /// <returns>返回值表示验证是否成功</returns>
        public static bool IsNumber(string Str)
        {
            return Verify.IsDefine(Str, "^[0-9]+$");
        }

        /// <summary>
        /// 判断字符串是否指定位数的数字
        /// </summary>
        /// <param name="Str">要进行判断的字符串</param>
        /// <param name="Size">长度</param>
        /// <returns>返回值表示验证是否成功</returns>
        public static bool IsNumeric(string Str, int Size)
        {
            return Verify.IsDefine(Str, "^[0-9]{" + Size.ToString() + "}$");
        }

        /// <summary>
        /// 判断字符串是否指定位数的数字
        /// </summary>
        /// <param name="Str">要进行判断的字符串</param>
        /// <param name="Min">最少长度</param>
        /// <param name="Max">最大长度</param>
        /// <returns>返回值表示验证是否成功</returns>
        public static bool IsNumeric(string Str, int Min, int Max)
        {
            return Verify.IsDefine(Str, "^[0-9]{" + Min.ToString() + "," + Max.ToString() + "}$");
        }

        /// <summary>
        /// 判断是否为正确的页码, 0 - 999999 一至六位数的页码
        /// </summary>
        /// <param name="Str">要判断的字符串</param>
        /// <returns>返回值表示是否成功</returns>
        public static bool IsPage(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsPage);
        }

        /// <summary>
        /// 表示是否为<seealso cref="RandomString.CreateOnlyID()"/>方法生成的有效的2000年至2700年之间的10位随机数
        /// </summary>
        /// <param name="input">要判断的数据</param>
        public static bool IsOnlyID(string input)
        {
            return Verify.IsDefine(input, RegexString.IsOnlyID);
        }

        /// <summary>
        /// 表示是否为<seealso cref="RandomString.CreateCourseID()"/>方法所产生的有效字符串
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        public static bool IsCourseID(string input)
        {
            return Verify.IsDefine(input, RegexString.IsCourseID);
        }

        /// <summary>
        /// 验证是否为正确的32位MD5加密串
        /// </summary>
        /// <param name="Str">要进行验证的字符串</param>
        /// <returns>返回值表示验证是否成功</returns>
        public static bool IsMD5(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsMD5);
        }

        /// <summary>
        /// 判断输入的是否为中文
        /// </summary>
        /// <param name="Str">要进行验证的字符串</param>
        /// <returns>返回值表示验证是否通过</returns>
        public static bool IsChinese(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsChinese);
        }

        /// <summary>
        /// 判断输入的字符串是否是合法的IPV6 地址
        /// </summary>
        /// <param name="input">须要判断的字符串</param>
        /// <returns>是IP地址则为True</returns>
        public static bool IsIPv6(string input)
        {
            if (!string.IsNullOrEmpty(input)) return false;

            string pattern = "";
            string temp = input;
            string[] strs = temp.Split(':');
            if (strs.Length > 8)
            {
                return false;
            }

            int count = Common.GetStringCount(input, "::");
            if (count > 1)
            {
                return false;
            }
            else if (count == 0)
            {
                pattern = @"^([\da-f]{1,4}:){7}[\da-f]{1,4}$";

                Regex regex = new Regex(pattern);
                return regex.IsMatch(input);
            }
            else
            {
                pattern = @"^([\da-f]{1,4}:){0,5}::([\da-f]{1,4}:){0,5}[\da-f]{1,4}$";
                Regex regex1 = new Regex(pattern);
                return regex1.IsMatch(input);
            }
        }
        /// <summary>
        /// 判断指定的对象是否为布尔值的True值
        /// </summary>
        /// <param name="obj">要进行处理的对象</param>
        /// <returns>返回值表示是否为布尔值True值</returns>
        public static bool IsTrue(object obj)
        {
            if (obj == null) return false;
            return obj.ToString().ToLower() == "true";
        }


        /// <summary>
        /// 是否为中文
        /// </summary>
        /// <param name="input">要判断的串</param>
        public static bool IsSpell(string input)
        {
            return Regex.IsMatch(input, @"^[\u4E00-\u9FA5]+$");
        }


        /// <summary>
        /// 判断是否是时间格式,只验证格式,不进行转换验证
        /// </summary>
        /// <param name="timeval">要判断的值</param>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }
        /// <summary>
        /// 是否为IPv4
        /// </summary>
        /// <param name="ip">IP</param>
        public static bool IsIP(string ip)
        {
            //^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }

    }
