using System;
using System.Configuration;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Resources;
using System.Reflection;
using Aooshi.Smtp;
using System.Collections.Specialized;

namespace Aooshi
{
    /// <summary>
    /// ���ó�Ա
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// ��ȡ������Ϣ
        /// </summary>
        public static Aooshi.Configuration.FrameworkSection Configuration
        {
            get
            {
                return (Aooshi.Configuration.FrameworkSection)ConfigurationManager.GetSection("Aooshi");
            }
        }

        /// <summary>
        /// ��ȡָ������ֵ��������
        /// </summary>
        /// <param name="name">����������</param>
        /// <returns>�����ַ�����ʽ��ֵ</returns>
        public static string GetAppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        /// <summary>
        /// ��ȡָ�����Ƶ���������δ������Ĭ��ֵ����
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="default">Ĭ��ֵ</param>
        public static string GetAppSetting(string name, string @default)
        {
            return Common.GetAppSetting(name) ?? @default;
        }

        /// <summary>
        /// ��ȡָ������������
        /// </summary>
        /// <param name="Name">Ҫ��ȡ������ִ�д�����</param>
        public static string GetConnection(string Name)
        {
            ConnectionStringSettings Setting = ConfigurationManager.ConnectionStrings[Name];
            if (Setting == null) return "";
            return Setting.ConnectionString;
        }


        /// <summary>
        /// ��ȡ��ǰģ������
        /// </summary>
        /// <param name="module">ģ������</param>
        /// <param name="name">��������</param>
        public static string ModuleConfig(string module, string name)
        {
            if (string.IsNullOrEmpty(module))
            {
                throw new ArgumentNullException("module");
            }
            return ((NameValueCollection)ConfigurationManager.GetSection(module))[name];
        }

        /// <summary>
        /// ����ָ��������html�ո����
        /// </summary>
        /// <param name="nSpaces">�ո񳤶�</param>
        public static string Spaces(int nSpaces)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nSpaces; i++)
            {
                sb.Append(" &nbsp;&nbsp;");
            }
            return sb.ToString();
        }

        /// <summary>
        /// ������\�ֺ�\�ֺ��滻html�ַ�
        /// </summary>
        /// <param name="strHtml">Ҫ���б���Ĵ�</param>
        public static string EncodeHtml(string strHtml)
        {
            if (!string.IsNullOrEmpty(strHtml))
            {
                strHtml = strHtml.Replace(",", "&def");
                strHtml = strHtml.Replace("'", "&dot");
                strHtml = strHtml.Replace(";", "&dec");
                return strHtml;
            }
            return "";
        }


        /// <summary>
        /// ���ַ����еĴ��ںż�С�ںŽ���Html����
        /// </summary>
        /// <param name="Str">Ҫ���н⼭���ַ���</param>
        /// <returns>���ؽ������ַ���</returns>
        public static string EasyHtmlDecode(string Str)
        {
            if (string.IsNullOrEmpty(Str)) return "";

            Str = Str.Replace("&gt;", ">");
            Str = Str.Replace("&lt;", "<");
            return Str;
        }


        /// <summary>
        /// ���ַ����еĴ��ںż�С�ںŽ���Html����
        /// </summary>
        /// <param name="Str">Ҫ���б༭���ַ���</param>
        /// <returns>���ؼ������ַ���</returns>
        public static string EasyHtmlEncode(string Str)
        {
            if (string.IsNullOrEmpty(Str)) return "";

            Str = Str.Replace(">", "&gt;");
            Str = Str.Replace("<", "&lt;");

            return Str;
        }


        /// <summary>
        /// ���ַ�������Html�򵥱��룬�ո񡢴�С�ڡ����С�˫���š������š���������
        /// </summary>
        /// <param name="Str">Ҫ���б�����ַ���</param>
        /// <returns>���ر������ַ���</returns>
        public static string HtmlEncode(string Str)
        {
            if (string.IsNullOrEmpty(Str)) return "";

            Str = Str.Replace(" ", "&nbsp;");
            Str = Str.Replace("<", "&lt;");
            Str = Str.Replace(">", "&gt;");
            Str = Str.Replace("\n", "<BR/>");
            Str = Str.Replace("\"", "&quot;");  //˫����
            Str = Str.Replace("'", "&#39;");    //������
            Str = Str.Replace("(", "&#40;");
            Str = Str.Replace(")", "&#41;");

            return Str;
        }

        /// <summary>
        /// ���ַ�������Html���룬��<see cref="HtmlEncode"/>�ķ��༭
        /// </summary>
        /// <param name="Str">Ҫ���н�����ַ���</param>
        /// <returns>���ؽ������ַ���</returns>
        public static string HtmlDecode(string Str)
        {
            if (string.IsNullOrEmpty(Str)) return "";

            Str = Str.Replace("&nbsp;", " ");
            Str = Str.Replace("&lt;", "<");
            Str = Str.Replace("&gt;", ">");
            Str = Str.Replace("<BR/>", "\n");
            Str = Str.Replace("&quot;", "\""); //˫����
            Str = Str.Replace("&#39;", "'"); //������
            Str = Str.Replace("&#40;", "(");
            Str = Str.Replace("&#41;", ")");

            return Str;
        }

        
        /// <summary>
        /// ����һ���ı��ļ�
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="body">����</param>
        public static void CreateFile(string path, string body)
        {
            CreateFile(path, body, Encoding.Default);
        }

        /// <summary>
        /// ����һ���ı��ļ�
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="body">����</param>
        /// <param name="encoding">����</param>
        public static void CreateFile(string path, string body,Encoding encoding)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path,false,encoding))
                {
                    sw.Write(body);
                    sw.Flush();
                }
            }
            catch (Exception e)
            {
                throw new IOException(e.Message, e);
            }
        }


        /// <summary>
        /// ��һ���ı��ļ�����ȡ������
        /// </summary>
        /// <param name="Path">·��</param>
        public static string OpenFile(string Path)
        {
            return OpenFile(Path, null);
        }
        /// <summary>
        /// ��һ���ı��ļ�����ȡ������
        /// </summary>
        /// <param name="Path">·��</param>
        /// <param name="enc">���뷽ʽ,��ΪnullʱĬ��ΪGB2312</param>
        public static string OpenFile(string Path, Encoding enc)
        {
            string str = "";
            if (enc == null) enc = Encoding.GetEncoding("gb2312");
            try
            {
                using (StreamReader sw = new StreamReader(Path, enc))
                {
                    str = sw.ReadToEnd();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                throw new IOException(e.Message, e);
            }
            return str;
        }


        /// <summary>
        /// �����ַ�����ʵ����, 1�����ֳ���Ϊ2
        /// </summary>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// ��������ַ����еĻس������з�
        /// </summary>
        /// <param name="str">Ҫ������ַ���</param>
        /// <returns>����󷵻ص��ַ���</returns>
        public static string ClearCrLf(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            Regex r = null;
            Match m = null;

            r = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);
            for (m = r.Match(str); m.Success; m = m.NextMatch())
            {
                str = str.Replace(m.Groups[0].ToString(), "");
            }


            return str;
        }

        /// <summary>
        /// ���˵�html���ݱ��
        /// </summary>
        /// <param name="input">������Ϣ</param>
        public static string ReplaceHTML(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            input = Regex.Replace(input, "<.+?>", "", RegexOptions.IgnoreCase);
            //input = input.Replace("<","");
            //input = input.Replace(">", "");
            return input;
        }

        /// <summary>
        /// �Զ�����滻�ַ�������
        /// </summary>
        /// <param name="SourceString">ԭ�ַ���</param>
        /// <param name="SearchString">��Ҫ�����滻���ַ���</param>
        /// <param name="ReplaceString">�滻�ַ���</param>
        /// <param name="IsCaseInsensetive">�Ƿ����ִ�Сд</param>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }



        /// <summary>
        /// ���ݰ��������ַ����·ݵ�����
        /// </summary>	
        public static string[] Monthes
        {
            get
            {
                //(�ɸ���Ϊĳ������)
                return new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            }
        }

        /// <summary>
        /// ��ʽ���ֽ����ַ���ΪK/M/G
        /// </summary>
        /// <param name="bytes">�ֽ���</param>
        public static string FormatBytesStr(int bytes)
        {
            if (bytes > 1073741824)
            {
                return ((double)(bytes / 1073741824)).ToString("0") + "G";
            }
            if (bytes > 1048576)
            {
                return ((double)(bytes / 1048576)).ToString("0") + "M";
            }
            if (bytes > 1024)
            {
                return ((double)(bytes / 1024)).ToString("0") + "K";
            }
            return bytes.ToString() + "Bytes";
        }


        /// <summary>
        /// ��0-9�������ַ���ת��Ϊ A-J����ĸ��
        /// </summary>
        /// <param name="Number">���ִ���������к��з�������ת��Ϊ0</param>
        /// <returns>�����ַ���</returns>
        public static string NumberToLatter(string Number)
        {
            string output = "";
            byte b;
            foreach (byte num in Number)
            {
                if (num < 48 || num > 57)
                {
                    output += "0";
                    continue;
                }

                b = (byte)(num + 49);   // 17 ��д a-j //49 ��д a-j

                output += (char)b;
            }

            return output.ToUpper();
        }


        /// <summary>
        /// ȡ�ô����ַ�����ָ�����ȵ��ַ���
        /// </summary>
        /// <param name="Str">Ҫ���н�ȡ���ַ���</param>
        /// <param name="Len">�ַ�����</param>
        /// <param name="LastAdd">������β���ӵ��ַ�������ΪNull��Emptyʱ�򲻽������</param>
        /// <returns>���ؽ�ȡ����ַ���</returns>
        /// <example>һ�����İ�������λ����</example>
        public static string CutString(string Str, int Len, string LastAdd)
        {
            if (string.IsNullOrEmpty(Str)) return "";
            int i = 0, j = 0;

            foreach (char Char in Str)
            {
                if ((int)Char > 127)
                    i += 2;
                else
                    i++;
                if (i > Len)
                {
                    Str = Str.Substring(0, j);
                    if (!string.IsNullOrEmpty(LastAdd))
                        Str += LastAdd;
                    break;
                }
                j++;
            }

            return Str;
        }


        /// <summary>
        /// �ж��ַ���compare �� input�ַ����г��ֵĴ���
        /// </summary>
        /// <param name="input">Դ�ַ���</param>
        /// <param name="compare">���ڱȽϵ��ַ���</param>
        /// <returns>�ַ���compare �� input�ַ����г��ֵĴ���</returns>
        public static int GetStringCount(string input, string compare)
        {
            int index = input.IndexOf(compare);
            if (index != -1)
            {
                return 1 + GetStringCount(input.Substring(index + compare.Length), compare);
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// ��ָ����·�������жϣ�������δ��б��\�����δ�����Զ�����һ��
        /// </summary>
        /// <param name="path">·����</param>
        public static string PathEndSlash(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            if (path.EndsWith("\\")) return path;
            return path + "\\";
        }
        /// <summary>
        /// ��ָ֤����·�����һλ�Ƿ�Ϊ��б�� / ,����������Զ�����һ��
        /// </summary>
        /// <param name="path">·����</param>
        public static string PathEndBackslash(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            if (path.EndsWith("/")) return path;
            return path + "/";
        }

        /// <summary>
        /// ��ָ�����ַ������й��ˣ����˵�����¼����ϣ�0-9���ּ��ϵ��ַ������š��㡢�ֺš�ñ�š������ַ���
        /// </summary>
        /// <param name="Str">Ҫ���й��˵��ַ���</param>
        /// <returns>���ع��˺���ַ���</returns>
        public static string NoGrant(string Str)
        {
            if (string.IsNullOrEmpty(Str)) return "";

            Str = Str.Replace("\"", "");//˫��
            Str = Str.Replace("'", "");//����
            Str = Str.Replace("!", "");
            Str = Str.Replace("@", "");
            Str = Str.Replace("#", "");
            Str = Str.Replace("$", "");
            Str = Str.Replace("%", "");
            Str = Str.Replace("^", "");
            Str = Str.Replace("&", "");
            Str = Str.Replace("*", "");
            Str = Str.Replace("(", "");
            Str = Str.Replace(")", "");
            Str = Str.Replace("?", "");
            Str = Str.Replace(",", "");
            Str = Str.Replace(".", ""); //��
            Str = Str.Replace("~", "");

            Str = Str.Replace(".", ""); //1�����Ǹ�С��

            return Str;

        }

        /// <summary>
        /// ��ָ��������Ŀ¼����Ա�,�������ͬʱ��ָ��ֵ,����ͬ�򷵻�ΪEmpty
        /// </summary>
        /// <param name="CurrObject">Ҫ�ȽϵĶ���</param>
        /// <param name="Cause">��ǰ�����ݶ���</param>
        /// <param name="YesStr">��ͬʱ�����</param>
        public static string IsEquals(object CurrObject, object Cause, string YesStr)
        {
            return IsEquals(CurrObject, Cause, YesStr, "");
        }

        /// <summary>
        /// ��ָ��������Ŀ¼����Ա�,�������ͬ��ֵ
        /// </summary>
        /// <param name="CurrObject">Ҫ�ȽϵĶ���</param>
        /// <param name="Cause">��ǰ�����ݶ���(�˶��󲻵�Ϊ��)</param>
        /// <param name="YesStr">��ͬʱ�����</param>
        /// <param name="NoStr">��ͬʱ�����</param>
        public static string IsEquals(object CurrObject, object Cause, string YesStr, string NoStr)
        {
            if (Cause == null) return NoStr;
            return Cause.Equals(CurrObject) ? YesStr : NoStr;
        }

        /// <summary>
        /// ����������Դ
        /// </summary>
        /// <param name="path">��Դ·��</param>
        /// <param name="Name">��Դ����</param>
        public static string GetRes(string path, string Name)
        {
            ResourceManager Rm = new ResourceManager(path, Assembly.GetExecutingAssembly());
            string r = Rm.GetString(Name);
            Rm.ReleaseAllResources();
            return r;
        }

      

        ///// <summary>
        ///// �������ñ���
        ///// </summary>
        ///// <returns>����</returns>
        //public static GlobalizationSection GetCongfigGlobalization()
        //{
        //    return (GlobalizationSection)ConfigurationManager.GetSection("system.web/globalization");
        //}

        /// <summary>
        /// ��ָ����IP������λ�滻Ϊ *
        /// </summary>
        /// <param name="ip">IP��ַ</param>
        public static string IPRelace(string ip)
        {
            return IPRelace(ip, 2);
        }

        /// <summary>
        /// ��ָ����IP����ָ�������⣬��*��ʾ
        /// </summary>
        /// <param name="ip">IP��ַ</param>
        /// <param name="length">��ʾ����</param>
        public static string IPRelace(string ip, int length)
        {
            string[] s = ip.Split('.');

            if (s.Length != 4) return "***.***.***.***";
            return string.Format("{0}.{1}.***.***",s[0],s[1]);
        }

        /// <summary>
        /// ����ָ�������ִ���һ������Ŀ¼����ÿĿ¼�ļ������ȼ���Ϊ��׼
        /// </summary>
        /// <param name="number">Ҫ���д��������</param>
        public static string CreateNumberPath(long number)
        {
            return Common.CreateNumberPath(number, 1000, true);
        }

        /// <summary>
        /// ����ָ�������ִ���һ������Ŀ¼����ÿĿ¼�ļ������ȼ���Ϊ��׼
        /// </summary>
        /// <param name="number">Ҫ���д��������</param>
        /// <param name="level">Ŀ¼�ļ����ȼ�,����Ϊ1000</param>
        public static string CreateNumberPath(long number, int level)
        {
            return Common.CreateNumberPath(number, level, true);
        }

        /// <summary>
        /// ����ָ�������ִ���һ������Ŀ¼����ÿĿ¼�ļ������ȼ���Ϊ��׼
        /// </summary>
        /// <param name="number">Ҫ���д��������</param>
        /// <param name="level">Ŀ¼�ļ����ȼ�,����Ϊ1000</param>
        /// <param name="containsbase">�Ƿ��������</param>
        public static string CreateNumberPath(long number, int level, bool containsbase)
        {
            string result = "";
            if (containsbase) result += number.ToString();

            while (number > level)
            {
                number /= level;
                result = number.ToString() + "/" + result;
            }

            return result;
        }

        /// <summary>
        /// �ж�ָ�����������Ƿ���ָ������
        /// </summary>
        /// <param name="item">Ҫƥ�����</param>
        /// <param name="array">����</param>
        public static bool ContainsArray(string item, string[] array)
        {
            foreach (string i in array)
            {
                if (item == i) return true;
            }
            return false;
        }

        /// <summary>
        /// �ж�ָ���������ַ������Ƿ���ָ������.
        /// </summary>
        /// <param name="item">Ҫƥ�����</param>
        /// <param name="arrstr">�����ַ���</param>
        /// <param name="split">���зָ��ַ������ַ�</param>
        public static bool ContainsArray(string item, string arrstr,char split)
        {
            return ContainsArray(item, arrstr.Split(split));
        }

        /// <summary>
        /// ��ȡ��ǰģ��Guid
        /// </summary>
        public static string ModuleGuid
        {
            get { return GetAppSetting("ModuleGuid"); }
        }

        /// <summary>
        /// ��ȡ��ǰģ������(����ʾ����)
        /// </summary>
        public static string ModuleName
        {
            get { return GetAppSetting("ModuleName"); }
        }

        /// <summary>
        /// ��ȡ��ǰģ����ʾ����
        /// </summary>
        public static string ModuleDisplayName
        {
            get { return GetAppSetting("ModuleDisplayName"); }
        }


        #region UnixTimestamp

        static DateTime unixtimestamp_base = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

        
        /// <summary>
        /// ����ָ���ı���ʱ�䷵�ص�ǰʱ���Unixʱ���
        /// </summary>
        public static int DateTimeToUnixTimestamp()
        {
            return Common.DateTimeToUnixTimestamp(DateTime.Now);
        }
        /// <summary>
        /// ����ָ���ı���ʱ�䷵��Unixʱ���
        /// </summary>
        /// <param name="time">Ҫ���صĻ���ʱ��</param>
        public static int DateTimeToUnixTimestamp(DateTime time)
        {
            string ts = time.Subtract(Common.unixtimestamp_base).Ticks.ToString();
            return int.Parse(ts.Substring(0, ts.Length - 7));
        }

        /// <summary>
        /// ����UNIXʱ������ر���ʱ��
        /// </summary>
        /// <param name="timestamp">ʱ���</param>
        public static DateTime UnixTimestampToDateTime(int timestamp)
        {
            long lTime = long.Parse(timestamp + "0000000");
            return Common.unixtimestamp_base.Add(new TimeSpan(lTime));
        }

        #endregion
    }
}
