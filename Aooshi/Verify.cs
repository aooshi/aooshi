using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Aooshi
{
    /// <summary>
    /// �������ݼ��
    /// </summary>
    public class Verify
    {
        /// <summary>
        /// ��ָ�����ַ���������ָ����������䣬��������ʽ�ж��Ƿ���������
        /// </summary>
        /// <param name="Str">Ҫ�жϵ��ַ���������ֵΪNull��Emptyʱ������ֵ��ԶΪfalse</param>
        /// <param name="RegEx">Ҫ�����жϵ�������ʽ����</param>
        /// <returns>����ֵ��ʾ��֤�Ƿ���������</returns>
        public static bool IsDefine(string Str, string @RegEx)
        {
            if (string.IsNullOrEmpty(Str)) return false;

            return Regex.IsMatch(Str, RegEx);
        }

        /// <summary>
        /// �ж�ָ�����ַ����Ƿ��������µĵ㡢���š��������š��������е������κ�һ���ַ�
        /// </summary>
        /// <param name="Str">Ҫ�����жϵ��ַ���</param>
        /// <returns>����ֵ��ʾ�Ƿ�����������</returns>
        public static bool IsInc(string Str)
        {
            return Verify.IsDefine(Str, @"[.\,\(\)\']+");
        }

        /// <summary>
        /// �ж��Ƿ����̷�ƥ��,��:c:\��E:\��
        /// </summary>
        /// <param name="Str">Ҫ��֤������</param>
        public static bool IsHard(string Str)
        {
            return Verify.IsDefine(Str, @"^[A-Za-z]{1}:\\");
        }

        /// <summary>
        /// �ж�ָ���ַ����Ƿ�Ϊ��Ч��Email��ַ
        /// </summary>
        /// <param name="Str">Ҫ�����жϵ��ַ���</param>
        /// <returns>����ֵ��ʾ�Ƿ�Ϊ��ȷ���ʼ���ַ</returns>
        public static bool IsMail(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsEmail);
        }

        /// <summary>
        /// ��֤�Ƿ�Ϊ��ȷ�Ĳ�Ϊ����������λС���Ľ��ֵ
        /// </summary>
        /// <param name="Str">Ҫ��֤���ַ���</param>
        /// <returns>����ֵ��ʾ��֤�Ƿ���������</returns>
        public static bool IsMoney(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsJustMoney);
        }

        /// <summary>
        /// �ж������ַ����Ƿ�����������ĸ�����
        /// </summary>
        /// <param name="Str">Ҫ������֤���ַ���</param>
        /// <returns>��ʾ�Ƿ�Ϊ��������ĸ�����</returns>
        public static bool IsLetterAndNumber(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsLetterAndNumber);
        }

        /// <summary>
        /// �ж������ַ����Ƿ���ָ��λ������������ĸ�����,
        /// </summary>
        /// <param name="Str">Ҫ������֤���ַ���</param>
        /// <param name="Size">�ַ�����</param>
        /// <returns>��ʾ�Ƿ�Ϊָ�����ȵ���������ĸ�����</returns>
        public static bool IsLetterAndNumber(string Str, int Size)
        {
            return Verify.IsDefine(Str, "^[A-Za-z0-9]{" + Size.ToString() + "}$");
        }

        /// <summary>
        /// �ж������ַ����Ƿ���ָ��λ������������ĸ�����,
        /// </summary>
        /// <param name="Str">Ҫ������֤���ַ���</param>
        /// <param name="Max">��󳤶�</param>
        /// <param name="Min">��С����</param>
        /// <returns>��ʾ�Ƿ�Ϊָ�����ȵ���������ĸ�����</returns>
        public static bool IsLetterAndNumber(string Str, int Min, int Max)
        {
            return Verify.IsDefine(Str, "^[A-Za-z0-9]{" + Min.ToString() + "," + Max.ToString() + "}$");
        }

        /// <summary>
        /// �ж��ַ����Ƿ�ȫΪ����
        /// </summary>
        /// <param name="Str">Ҫ�����жϵ��ַ���</param>
        /// <returns>����ֵ��ʾ��֤�Ƿ�ɹ�</returns>
        public static bool IsNumber(string Str)
        {
            return Verify.IsDefine(Str, "^[0-9]+$");
        }

        /// <summary>
        /// �ж��ַ����Ƿ�ָ��λ��������
        /// </summary>
        /// <param name="Str">Ҫ�����жϵ��ַ���</param>
        /// <param name="Size">����</param>
        /// <returns>����ֵ��ʾ��֤�Ƿ�ɹ�</returns>
        public static bool IsNumeric(string Str, int Size)
        {
            return Verify.IsDefine(Str, "^[0-9]{" + Size.ToString() + "}$");
        }

        /// <summary>
        /// �ж��ַ����Ƿ�ָ��λ��������
        /// </summary>
        /// <param name="Str">Ҫ�����жϵ��ַ���</param>
        /// <param name="Min">���ٳ���</param>
        /// <param name="Max">��󳤶�</param>
        /// <returns>����ֵ��ʾ��֤�Ƿ�ɹ�</returns>
        public static bool IsNumeric(string Str, int Min, int Max)
        {
            return Verify.IsDefine(Str, "^[0-9]{" + Min.ToString() + "," + Max.ToString() + "}$");
        }

        /// <summary>
        /// �ж��Ƿ�Ϊ��ȷ��ҳ��, 0 - 999999 һ����λ����ҳ��
        /// </summary>
        /// <param name="Str">Ҫ�жϵ��ַ���</param>
        /// <returns>����ֵ��ʾ�Ƿ�ɹ�</returns>
        public static bool IsPage(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsPage);
        }

        /// <summary>
        /// ��ʾ�Ƿ�Ϊ<seealso cref="RandomString.CreateOnlyID()"/>�������ɵ���Ч��2000����2700��֮���10λ�����
        /// </summary>
        /// <param name="input">Ҫ�жϵ�����</param>
        public static bool IsOnlyID(string input)
        {
            return Verify.IsDefine(input, RegexString.IsOnlyID);
        }

        /// <summary>
        /// ��ʾ�Ƿ�Ϊ<seealso cref="RandomString.CreateCourseID()"/>��������������Ч�ַ���
        /// </summary>
        /// <param name="input">Ҫ��֤���ַ���</param>
        public static bool IsCourseID(string input)
        {
            return Verify.IsDefine(input, RegexString.IsCourseID);
        }

        /// <summary>
        /// ��֤�Ƿ�Ϊ��ȷ��32λMD5���ܴ�
        /// </summary>
        /// <param name="Str">Ҫ������֤���ַ���</param>
        /// <returns>����ֵ��ʾ��֤�Ƿ�ɹ�</returns>
        public static bool IsMD5(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsMD5);
        }

        /// <summary>
        /// �ж�������Ƿ�Ϊ����
        /// </summary>
        /// <param name="Str">Ҫ������֤���ַ���</param>
        /// <returns>����ֵ��ʾ��֤�Ƿ�ͨ��</returns>
        public static bool IsChinese(string Str)
        {
            return Verify.IsDefine(Str, RegexString.IsChinese);
        }

        /// <summary>
        /// �ж�������ַ����Ƿ��ǺϷ���IPV6 ��ַ
        /// </summary>
        /// <param name="input">��Ҫ�жϵ��ַ���</param>
        /// <returns>��IP��ַ��ΪTrue</returns>
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
        /// �ж�ָ���Ķ����Ƿ�Ϊ����ֵ��Trueֵ
        /// </summary>
        /// <param name="obj">Ҫ���д���Ķ���</param>
        /// <returns>����ֵ��ʾ�Ƿ�Ϊ����ֵTrueֵ</returns>
        public static bool IsTrue(object obj)
        {
            if (obj == null) return false;
            return obj.ToString().ToLower() == "true";
        }


        /// <summary>
        /// �Ƿ�Ϊ����
        /// </summary>
        /// <param name="input">Ҫ�жϵĴ�</param>
        public static bool IsSpell(string input)
        {
            return Regex.IsMatch(input, @"^[\u4E00-\u9FA5]+$");
        }


        /// <summary>
        /// �ж��Ƿ���ʱ���ʽ,ֻ��֤��ʽ,������ת����֤
        /// </summary>
        /// <param name="timeval">Ҫ�жϵ�ֵ</param>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }
        /// <summary>
        /// �Ƿ�ΪIPv4
        /// </summary>
        /// <param name="ip">IP</param>
        public static bool IsIP(string ip)
        {
            //^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }

    }
