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
    /// 共用成员
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        public static Aooshi.Configuration.FrameworkSection Configuration
        {
            get
            {
                return (Aooshi.Configuration.FrameworkSection)ConfigurationManager.GetSection("Aooshi");
            }
        }

        /// <summary>
        /// 获取指定名称值的配置项
        /// </summary>
        /// <param name="name">配置项名称</param>
        /// <returns>返回字符串型式的值</returns>
        public static string GetAppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        /// <summary>
        /// 获取指定名称的配置项，如果未配置则按默认值返回
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="default">默认值</param>
        public static string GetAppSetting(string name, string @default)
        {
            return Common.GetAppSetting(name) ?? @default;
        }

        /// <summary>
        /// 获取指定的数据连接
        /// </summary>
        /// <param name="Name">要获取的数据执行串名称</param>
        public static string GetConnection(string Name)
        {
            ConnectionStringSettings Setting = ConfigurationManager.ConnectionStrings[Name];
            if (Setting == null) return "";
            return Setting.ConnectionString;
        }


        /// <summary>
        /// 获取当前模块配置
        /// </summary>
        /// <param name="module">模块名称</param>
        /// <param name="name">配置名称</param>
        public static string ModuleConfig(string module, string name)
        {
            if (string.IsNullOrEmpty(module))
            {
                throw new ArgumentNullException("module");
            }
            return ((NameValueCollection)ConfigurationManager.GetSection(module))[name];
        }

        /// <summary>
        /// 生成指定数量的html空格符号
        /// </summary>
        /// <param name="nSpaces">空格长度</param>
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
        /// 将逗号\分号\分号替换html字符
        /// </summary>
        /// <param name="strHtml">要进行编码的串</param>
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
        /// 对字符串中的大于号及小于号进行Html解码
        /// </summary>
        /// <param name="Str">要进行解辑的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public static string EasyHtmlDecode(string Str)
        {
            if (string.IsNullOrEmpty(Str)) return "";

            Str = Str.Replace("&gt;", ">");
            Str = Str.Replace("&lt;", "<");
            return Str;
        }


        /// <summary>
        /// 对字符串中的大于号及小于号进行Html编码
        /// </summary>
        /// <param name="Str">要进行编辑的字符串</param>
        /// <returns>返回加码后的字符串</returns>
        public static string EasyHtmlEncode(string Str)
        {
            if (string.IsNullOrEmpty(Str)) return "";

            Str = Str.Replace(">", "&gt;");
            Str = Str.Replace("<", "&lt;");

            return Str;
        }


        /// <summary>
        /// 将字符串进行Html简单编码，空格、大小于、换行、双引号、单引号、正反括号
        /// </summary>
        /// <param name="Str">要进行编码的字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public static string HtmlEncode(string Str)
        {
            if (string.IsNullOrEmpty(Str)) return "";

            Str = Str.Replace(" ", "&nbsp;");
            Str = Str.Replace("<", "&lt;");
            Str = Str.Replace(">", "&gt;");
            Str = Str.Replace("\n", "<BR/>");
            Str = Str.Replace("\"", "&quot;");  //双引号
            Str = Str.Replace("'", "&#39;");    //单引号
            Str = Str.Replace("(", "&#40;");
            Str = Str.Replace(")", "&#41;");

            return Str;
        }

        /// <summary>
        /// 将字符串进行Html解码，是<see cref="HtmlEncode"/>的反编辑
        /// </summary>
        /// <param name="Str">要进行解码的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public static string HtmlDecode(string Str)
        {
            if (string.IsNullOrEmpty(Str)) return "";

            Str = Str.Replace("&nbsp;", " ");
            Str = Str.Replace("&lt;", "<");
            Str = Str.Replace("&gt;", ">");
            Str = Str.Replace("<BR/>", "\n");
            Str = Str.Replace("&quot;", "\""); //双引号
            Str = Str.Replace("&#39;", "'"); //单引号
            Str = Str.Replace("&#40;", "(");
            Str = Str.Replace("&#41;", ")");

            return Str;
        }

        
        /// <summary>
        /// 创建一个文本文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="body">内容</param>
        public static void CreateFile(string path, string body)
        {
            CreateFile(path, body, Encoding.Default);
        }

        /// <summary>
        /// 创建一个文本文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="body">内容</param>
        /// <param name="encoding">编码</param>
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
        /// 打开一个文本文件并读取其内容
        /// </summary>
        /// <param name="Path">路径</param>
        public static string OpenFile(string Path)
        {
            return OpenFile(Path, null);
        }
        /// <summary>
        /// 打开一个文本文件并读取其内容
        /// </summary>
        /// <param name="Path">路径</param>
        /// <param name="enc">编码方式,当为null时默认为GB2312</param>
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
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 清除给定字符串中的回车及换行符
        /// </summary>
        /// <param name="str">要清除的字符串</param>
        /// <returns>清除后返回的字符串</returns>
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
        /// 过滤掉html数据标记
        /// </summary>
        /// <param name="input">过滤信息</param>
        public static string ReplaceHTML(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            input = Regex.Replace(input, "<.+?>", "", RegexOptions.IgnoreCase);
            //input = input.Replace("<","");
            //input = input.Replace(">", "");
            return input;
        }

        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        /// <param name="SourceString">原字符串</param>
        /// <param name="SearchString">须要搜索替换的字符串</param>
        /// <param name="ReplaceString">替换字符串</param>
        /// <param name="IsCaseInsensetive">是否区分大小写</param>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }



        /// <summary>
        /// 根据阿拉伯数字返回月份的名称
        /// </summary>	
        public static string[] Monthes
        {
            get
            {
                //(可更改为某种语言)
                return new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            }
        }

        /// <summary>
        /// 格式化字节数字符串为K/M/G
        /// </summary>
        /// <param name="bytes">字节数</param>
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
        /// 将0-9的数字字符串转换为 A-J的字母串
        /// </summary>
        /// <param name="Number">数字串，如果其中含有非数字则将转负为0</param>
        /// <returns>返回字符串</returns>
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

                b = (byte)(num + 49);   // 17 大写 a-j //49 大写 a-j

                output += (char)b;
            }

            return output.ToUpper();
        }


        /// <summary>
        /// 取得传入字符串中指定长度的字符串
        /// </summary>
        /// <param name="Str">要进行截取的字符串</param>
        /// <param name="Len">字符长度</param>
        /// <param name="LastAdd">须在其尾增加的字符串，当为Null或Empty时则不进行添加</param>
        /// <returns>返回截取后的字符串</returns>
        /// <example>一个中文按两个单位计算</example>
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
        /// 判断字符串compare 在 input字符串中出现的次数
        /// </summary>
        /// <param name="input">源字符串</param>
        /// <param name="compare">用于比较的字符串</param>
        /// <returns>字符串compare 在 input字符串中出现的次数</returns>
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
        /// 将指定的路径进行判断，如果最后未有斜杠\，如果未有则自动增加一个
        /// </summary>
        /// <param name="path">路径串</param>
        public static string PathEndSlash(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            if (path.EndsWith("\\")) return path;
            return path + "\\";
        }
        /// <summary>
        /// 验证指定的路径最后一位是否为反斜扛 / ,如果不是则自动增加一个
        /// </summary>
        /// <param name="path">路径串</param>
        public static string PathEndBackslash(string path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            if (path.EndsWith("/")) return path;
            return path + "/";
        }

        /// <summary>
        /// 将指定的字符串进行过滤，过滤掉半角下键盘上，0-9数字键上的字符、逗号、点、分号、帽号、引号字符。
        /// </summary>
        /// <param name="Str">要进行过滤的字符串</param>
        /// <returns>返回过滤后的字符串</returns>
        public static string NoGrant(string Str)
        {
            if (string.IsNullOrEmpty(Str)) return "";

            Str = Str.Replace("\"", "");//双引
            Str = Str.Replace("'", "");//单引
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
            Str = Str.Replace(".", ""); //点
            Str = Str.Replace("~", "");

            Str = Str.Replace(".", ""); //1边上那个小点

            return Str;

        }

        /// <summary>
        /// 将指定对象与目录对象对比,并输出相同时的指定值,不相同则返回为Empty
        /// </summary>
        /// <param name="CurrObject">要比较的对象</param>
        /// <param name="Cause">当前的数据对象</param>
        /// <param name="YesStr">相同时的输出</param>
        public static string IsEquals(object CurrObject, object Cause, string YesStr)
        {
            return IsEquals(CurrObject, Cause, YesStr, "");
        }

        /// <summary>
        /// 将指定对象与目录对象对比,并输出不同的值
        /// </summary>
        /// <param name="CurrObject">要比较的对象</param>
        /// <param name="Cause">当前的数据对象(此对象不得为空)</param>
        /// <param name="YesStr">相同时的输出</param>
        /// <param name="NoStr">不同时的输出</param>
        public static string IsEquals(object CurrObject, object Cause, string YesStr, string NoStr)
        {
            if (Cause == null) return NoStr;
            return Cause.Equals(CurrObject) ? YesStr : NoStr;
        }

        /// <summary>
        /// 返回内置资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="Name">资源名称</param>
        public static string GetRes(string path, string Name)
        {
            ResourceManager Rm = new ResourceManager(path, Assembly.GetExecutingAssembly());
            string r = Rm.GetString(Name);
            Rm.ReleaseAllResources();
            return r;
        }

      

        ///// <summary>
        ///// 返回配置编码
        ///// </summary>
        ///// <returns>编码</returns>
        //public static GlobalizationSection GetCongfigGlobalization()
        //{
        //    return (GlobalizationSection)ConfigurationManager.GetSection("system.web/globalization");
        //}

        /// <summary>
        /// 将指定的IP，后两位替换为 *
        /// </summary>
        /// <param name="ip">IP地址</param>
        public static string IPRelace(string ip)
        {
            return IPRelace(ip, 2);
        }

        /// <summary>
        /// 将指定的IP，除指定长度外，按*显示
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="length">显示长度</param>
        public static string IPRelace(string ip, int length)
        {
            string[] s = ip.Split('.');

            if (s.Length != 4) return "***.***.***.***";
            return string.Format("{0}.{1}.***.***",s[0],s[1]);
        }

        /// <summary>
        /// 根据指定的数字创建一组数字目录，以每目录文件数量等级数为标准
        /// </summary>
        /// <param name="number">要进行处理的数字</param>
        public static string CreateNumberPath(long number)
        {
            return Common.CreateNumberPath(number, 1000, true);
        }

        /// <summary>
        /// 根据指定的数字创建一组数字目录，以每目录文件数量等级数为标准
        /// </summary>
        /// <param name="number">要进行处理的数字</param>
        /// <param name="level">目录文件数等级,建议为1000</param>
        public static string CreateNumberPath(long number, int level)
        {
            return Common.CreateNumberPath(number, level, true);
        }

        /// <summary>
        /// 根据指定的数字创建一组数字目录，以每目录文件数量等级数为标准
        /// </summary>
        /// <param name="number">要进行处理的数字</param>
        /// <param name="level">目录文件数等级,建议为1000</param>
        /// <param name="containsbase">是否包含基数</param>
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
        /// 判断指定的数组中是否含有指定的项
        /// </summary>
        /// <param name="item">要匹配的项</param>
        /// <param name="array">数组</param>
        public static bool ContainsArray(string item, string[] array)
        {
            foreach (string i in array)
            {
                if (item == i) return true;
            }
            return false;
        }

        /// <summary>
        /// 判断指定的数组字符串中是否含有指定的项.
        /// </summary>
        /// <param name="item">要匹配的项</param>
        /// <param name="arrstr">数组字符串</param>
        /// <param name="split">进行分隔字符串的字符</param>
        public static bool ContainsArray(string item, string arrstr,char split)
        {
            return ContainsArray(item, arrstr.Split(split));
        }

        /// <summary>
        /// 获取当前模块Guid
        /// </summary>
        public static string ModuleGuid
        {
            get { return GetAppSetting("ModuleGuid"); }
        }

        /// <summary>
        /// 获取当前模块名称(非显示名称)
        /// </summary>
        public static string ModuleName
        {
            get { return GetAppSetting("ModuleName"); }
        }

        /// <summary>
        /// 获取当前模块显示名称
        /// </summary>
        public static string ModuleDisplayName
        {
            get { return GetAppSetting("ModuleDisplayName"); }
        }


        #region UnixTimestamp

        static DateTime unixtimestamp_base = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

        
        /// <summary>
        /// 根据指定的本地时间返回当前时间的Unix时间戳
        /// </summary>
        public static int DateTimeToUnixTimestamp()
        {
            return Common.DateTimeToUnixTimestamp(DateTime.Now);
        }
        /// <summary>
        /// 根据指定的本地时间返回Unix时间戳
        /// </summary>
        /// <param name="time">要返回的基础时间</param>
        public static int DateTimeToUnixTimestamp(DateTime time)
        {
            string ts = time.Subtract(Common.unixtimestamp_base).Ticks.ToString();
            return int.Parse(ts.Substring(0, ts.Length - 7));
        }

        /// <summary>
        /// 根据UNIX时间戳返回本地时间
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        public static DateTime UnixTimestampToDateTime(int timestamp)
        {
            long lTime = long.Parse(timestamp + "0000000");
            return Common.unixtimestamp_base.Add(new TimeSpan(lTime));
        }

        #endregion
    }
}
