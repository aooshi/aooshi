using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using Aooshi.Configuration;
using System.Reflection;

namespace Aooshi.Web
{
    /// <summary>
    /// 网页共公成员
    /// </summary>
    public static class WebCommon
    {
        static int isDebug = -1;
        /// <summary>
        /// 是否为调试状态
        /// </summary>
        /// <remarks>
        /// <code>
        ///         <add key="Aooshi:Debug" value="true" />
        /// </code>
        /// </remarks>
        public static bool IsDebug
        {
            get
            {
                if (isDebug == -1) isDebug = (ConfigurationManager.AppSettings["Aooshi:Debug"] == "true") ? 1 : 0;
                return isDebug == 1;
            }
        }

        /// <summary>
        /// 获取通过代理IP访问时的访问IP
        /// </summary>
        /// <returns>所获取的值</returns>
        public static string UserAgentIp()
        {
            HttpRequest hr = HttpContext.Current.Request;
            if (hr == null) return "";

            string test = hr.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (test == null || test == "")
                test = hr.ServerVariables["REMOTE_ADDR"];
            return test;
        }

        /// <summary>
        /// 客户端系统版本
        /// </summary>
        /// <returns>返回客户端系统版本</returns>
        public static string UserOS()
        {
            string text1 = HttpContext.Current.Request.UserAgent;
            //if (text1.IndexOf("Windows NT 6.0") != -1)
            //{
            //    return "Windows Vista";
            //}
            if (text1.IndexOf("Windows NT 5.1") != -1)
            {
                return "Windows XP";
            }
            if (text1.IndexOf("Windows NT 5.2") != -1)
            {
                return "Windows Server 2003";
            }
            if (text1.IndexOf("Windows NT 6") != -1)
            {
                return "Windows Longhorn";
            }
            if (text1.IndexOf("Windows NT 5") != -1)
            {
                return "Windows 2000";
            }
            if (text1.IndexOf("9x") != -1)
            {
                return "Windows ME";
            }
            if (text1.IndexOf("98") != -1)
            {
                return "Windows 98";
            }
            if (text1.IndexOf("95") != -1)
            {
                return "Windows 95";
            }
            if (text1.IndexOf("NT 4") != -1)
            {
                return "Windows NT 4";
            }
            if (text1.IndexOf("Linux") != -1)
            {
                return "Linux";
            }
            if (text1.IndexOf("SunOS") != -1)
            {
                return "SunOS";
            }
            if (text1.IndexOf("Mac") != -1)
            {
                return "Mac";
            }
            return HttpContext.Current.Request.Browser.Platform.Replace("WinCE", "Windows CE");
        }

        /// <summary>
        /// 用户浏览器
        /// </summary>
        /// <returns>返回客户端浏览器版本</returns>
        public static string UserBrowser()
        {
            HttpBrowserCapabilities capabilities1 = HttpContext.Current.Request.Browser;
            string text1 = capabilities1.Browser;
            if (text1 == "IE")
            {
                text1 = "Internet Explorer";
            }
            text1 = text1 + " " + capabilities1.Version;
            if (capabilities1.Beta)
            {
                text1 = text1 + "(Beta)";
            }
            return text1;
        }

        /// <summary>
        /// 判断是否有上一页返回,如果存在则返回为上一页绝对URL地址,不存在则返回所输入的新URL
        /// </summary>
        /// <param name="newUrl">新的URL地址</param>
        /// <param name="format">格式化串</param>
        /// <returns>返回URL地址</returns>
        public static string Refurl(string newUrl,params object[] format)
        {
            if (format.Length > 0)
            {
                newUrl = string.Format(newUrl, format);
            }
            HttpContext context = HttpContext.Current;
            return (context.Request.UrlReferrer == null) ? newUrl : context.Request.UrlReferrer.AbsoluteUri;
        }

        /// <summary>
        /// 进行URL移动(HTTP 301 处理)，并结束页运行
        /// </summary>
        /// <param name="newUrl">新URL地址</param>
        /// <param name="format">格式化串</param>
        public static void MoveURL(string newUrl,params object[] format)
        {
            if (format.Length > 0)
            {
                newUrl = string.Format(newUrl, format);
            }

            HttpResponse response = HttpContext.Current.Response;
            response.StatusCode = 301;
            response.Status = "301 Moved Permanently";
            response.RedirectLocation = newUrl;
            response.End();
        }

        /// <summary>
        /// 返回完整的URL路径地址(不含文件名)
        /// </summary>
        public static string GetUrlPath()
        {
            HttpContext context = HttpContext.Current;
            Uri u = context.Request.Url;
            string result = context.Request.IsSecureConnection ? "https://" : "http://";
            result += (u.Port == 80) ? u.Host : u.Authority;
            result += VirtualPathUtility.GetDirectory( u.AbsolutePath );
            return result;
        }

        /// <summary>
        /// 得到虚拟目录根路径
        /// </summary>
        public static string GetVirtualPath()
        {
            return VirtualPathUtility.ToAbsolute("~/");
        }
               

        /// <summary>
        /// 处理一个URL，如果该URL为~/程序路径或/根路径则进行转换处理，否则直接输出原值
        /// </summary>
        /// <param name="path">路径或Url地址</param>
        /// <param name="format">格式化串</param>
        public static string ProcessUrl(string path,params object[] format)
        {
            return WebCommon.ProcessUrl(path, false,format);
        }

        /// <summary>
        /// 处理一个URL，如果该URL为~/程序路径或/根路径则进行转换处理，否则直接输出原值
        /// </summary>
        /// <param name="path">路径或Url地址</param>
        /// <param name="appendSlash">如果地址末尾不存在/正斜杠则自动添加</param>
        /// <param name="format">格式化串</param>
        public static string ProcessUrl(string path,bool appendSlash,params object[] format)
        {
            if (string.IsNullOrEmpty(path)) return path;
            
            if (format.Length > 0)
            {
                path = string.Format(path, format);
            }
            
            switch (path[0])
            {
                case '~':
                case '/':
                    path = System.Web.VirtualPathUtility.ToAbsolute(path);
                    break;
            }

            if (appendSlash)
            {
                //path = System.Web.VirtualPathUtility.AppendTrailingSlash(path);
                if (!path.EndsWith("/")) path += "/";
            }

            return path;
        }

        /// <summary>
        /// 写出一段字符串,是Response.Write()的引用
        /// </summary>
        /// <param name="content">要进行写出的字符串</param>
        /// <param name="format">格式化串</param>
        public static void Write(string content,params object[] format)
        {
            if (format.Length > 0)
            {
                content = string.Format(content, format);
            }

            HttpContext.Current.Response.Write(content);
        }

        /// <summary>
        /// 写出一段字符串,并在其属加上Br换行标记
        /// </summary>
        /// <param name="content">要写出的字段串值</param>
        /// <param name="format">格式化串</param>
        public static void WriteBr(string content,params object[] format)
        {
            if (format.Length > 0)
            {
                content = string.Format(content, format);
            }
            HttpContext.Current.Response.Write(content + "<br />");
        }

        /// <summary>
        /// 写出一段字符串,是Response.Write()的简写,并在其后增加一个换行符\n
        /// </summary>
        /// <param name="content">要进行写出的字符串</param>
        /// <param name="format">格式化串</param>
        public static void WriteLine(string content,params object[] format)
        {
            if (format.Length > 0)
            {
                content = string.Format(content, format);
            }

            HttpContext.Current.Response.Write(content);
            HttpContext.Current.Response.Write(Environment.NewLine);
        }

        /// <summary>
        /// 写出一段字符串,是Response.Write()的简写,并在写出后结束页执行
        /// </summary>
        /// <param name="content">要进行写出的字符串</param>
        /// <param name="format">格式化串</param>
        public static void WriteEnd(string content,params object[] format)
        {
            if (format.Length > 0)
            {
                content = string.Format(content, format);
            }

            HttpContext.Current.Response.Write(content);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 向服务器写入一个 error 行,返回为null成功,否则失败
        /// </summary>
        /// <param name="error">异常消息</param>
        /// <param name="format">格式化串</param>
        public static Exception WriteError(string error,params object[] format)
        {
            if (format.Length > 0)
            {
                error = string.Format(error, format);
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\error.txt", true))
                {
                    sw.WriteLine(error);
                    sw.WriteLine();
                }
                return null;
            }
            catch (Exception err) { return err; }
        }

        /// <summary>
        /// 获取通过Get方法传输的一个参数值，如果未找到则返回为Empty，所返回的值均通过Trim()方法进行了前后空格消除
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>如果未找到则返回为Empty，所返回的值均通过Trim()方法进行了前后空格消除</returns>
        public static string Query(string name)//与,BGForm(string Name)
        {
            string qs = HttpContext.Current.Request.QueryString[name];

            if (qs == null) return string.Empty;

            return qs.Trim();
        }

        /// <summary>
        /// 获取通过Get方法传输的一个参数值，如果未找到则以给定数据返回
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="newValue">未找到的代替数据</param>
        public static string Query(string name, string newValue)
        {
            string qs = HttpContext.Current.Request.QueryString[name];
            if (string.IsNullOrEmpty(qs)) return newValue;
            return qs.Trim();
        }


        /// <summary>
        /// 获取通过Form表单所传送的一个参数值，如果未找到则返回为Empty，所返回的值均通过Trim()方法进行了前后空格消除
        /// </summary>
        /// <param name="name">参数名称</param>
        public static string Form(string name)
        {
            string box = HttpContext.Current.Request.Form[name];
            if (box == null) return string.Empty;
            return box.Trim();
        }
        /// <summary>
        /// 获取通过Form表单所传送的一个参数值，如果未找到则返回指定新数据
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="newValue">未找到时的新值</param>
        public static string Form(string name, string newValue)
        {
            string box = HttpContext.Current.Request.Form[name];
            if (string.IsNullOrEmpty(box)) return newValue;
            return box.Trim();
        }

        /// <summary>
        /// 将指定的表单中 TextBox 控值全部 Trim 一次
        /// </summary>
        /// <param name="form">表单</param>
        public static void FromTrim(HtmlForm form)
        {
            int i = 0;
            Type type = typeof(TextBox);
            while (form.Controls[i].GetType() == type)
                ((TextBox)form.Controls[i]).Text = ((TextBox)form.Controls[i]).Text.Trim();
        }

        /// <summary>
        /// 获取通过QueryString或Form传递的参数
        /// </summary>
        /// <param name="name">参数名称</param>
        public static string GetPageParame(string name)
        {
            return GetPageParame(name, "");
        }

        /// <summary>
        /// 获取通过QueryString或Form传递的参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="newValue">未找到时的新值</param>
        public static string GetPageParame(string name, string newValue)
        {
            if (newValue == null) newValue = string.Empty;
            string tmp = Query(name);
            if (tmp.Equals(string.Empty))
            {
                tmp = Form(name);
            }
            return tmp.Equals(string.Empty) ? newValue : tmp;
        }
        ///// <summary>
        ///// 将指定的文本增加至网页Head之间
        ///// </summary>
        ///// <param name="text">文本信息</param>
        //public static void AppTextToHead(string text)
        //{
        //    Literal literal = new Literal();
        //    literal.Text = text;
        //    if (CurrentPage().Header == null) throw new Exception("Head Not Set Runat=Server");
        //    CurrentPage().Header.Controls.Add(literal);
        //}

        /// <summary>
        /// 获取当前正在运行的页
        /// </summary>
        public static Page CurrentPage()
        {
            var context = HttpContext.Current;
            if (context == null) return null;

            return context.Handler as Page;
        }

        /// <summary>
        /// 将数据源绑定到指定的控件
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="data">数据源</param>
        public static void DataBindToContrl(object data,Control control)
        {
            if (control == null) throw new ArgumentNullException("control", "control is null");
            Type type = control.GetType();
            PropertyInfo pi = type.GetProperty("DataSource");
            if (pi == null)
            {
                throw new AooshiException("not find DataSource property.");
            }
            pi.SetValue(control, data, null);
            control.DataBind();
        }

        /// <summary>
        /// 将数据源绑定到指定的控件
        /// </summary>
        /// <param name="theControl">要进行查找的控件</param>
        /// <param name="controlname">控件名称</param>
        /// <param name="data">数据源</param>
        /// <param name="nofindisthrow">未找到时是否抛出异常</param>
        public static void DataBindToFindControl(Control theControl,string controlname, object data, bool nofindisthrow)
        {
            Control control = theControl.FindControl(controlname);
            if (control != null)
            {
                WebCommon.DataBindToContrl(data, control);
            }
            else
            {
                if (nofindisthrow)
                {
                    throw new AooshiException(string.Format("{0} not find {1}", theControl.ClientID, controlname));
                }
            }
        }


        /// <summary>
        /// 将数据源绑定到指定的控件
        /// </summary>
        /// <param name="theControl">要查找控件的容器</param>
        /// <param name="controlname">控件名称</param>
        /// <param name="data">数据源</param>
        public static void DataBindToFindControl(Control theControl, string controlname, object data)
        {
            WebCommon.DataBindToFindControl(theControl,controlname, data, true);
        }


          /// <summary>
        /// 获取Web控件中的内容
        /// </summary>
        /// <param name="ct">控件</param>
        public static string GetWebControlString(Control ct)
        {
            HtmlTextWriter tw = new HtmlTextWriter(new StringWriter());
            ct.RenderControl(tw);
            StringWriter sw = (StringWriter)tw.InnerWriter;
            return sw.GetStringBuilder().ToString();
        }

          /// <summary>
        /// 获取Web用户控件中的内容
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <param name="format">格式化串</param>
        public static string GetUserControlString(string virtualPath,params object[] format)
        {
            return WebCommon.GetUserControlString<Page>(virtualPath,format);
        }

        /// <summary>
        /// 获取Web用户控件中的内容
        /// </summary>
        /// <typeparam name="T">控件所基于的基础网页类型，该类必需是由<see cref="System.Web.UI.Page"/>继承而来</typeparam>
        /// <param name="virtualPath">虚拟地址</param>
        /// <param name="format">格式化串</param>
        public static string GetUserControlString<T>(string virtualPath,params object[] format)
        {

            if (format.Length > 0)
            {
                virtualPath = string.Format(virtualPath, format);
            }

            System.Web.UI.Page p = Activator.CreateInstance<T>() as Page;
            p.LoadControl(virtualPath);
            StringWriter output = new StringWriter();
            HttpContext.Current.Server.Execute(p, output, false);
            return output.ToString();

            //System.Web.UI.HtmlTextWriter tw = new HtmlTextWriter(new System.IO.StringWriter());
            //p.Controls.Add(((System.Web.UI.Control)System.Web.Compilation.BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(System.Web.UI.Control))));
            //p.ProcessRequest(HttpContext.Current);
            //p.RenderControl(tw);
            //System.IO.StringWriter sw = (System.IO.StringWriter)tw.InnerWriter;
            //return sw.GetStringBuilder().ToString();
        }
    }
}