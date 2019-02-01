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
    /// ��ҳ������Ա
    /// </summary>
    public static class WebCommon
    {
        static int isDebug = -1;
        /// <summary>
        /// �Ƿ�Ϊ����״̬
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
        /// ��ȡͨ������IP����ʱ�ķ���IP
        /// </summary>
        /// <returns>����ȡ��ֵ</returns>
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
        /// �ͻ���ϵͳ�汾
        /// </summary>
        /// <returns>���ؿͻ���ϵͳ�汾</returns>
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
        /// �û������
        /// </summary>
        /// <returns>���ؿͻ���������汾</returns>
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
        /// �ж��Ƿ�����һҳ����,��������򷵻�Ϊ��һҳ����URL��ַ,�������򷵻����������URL
        /// </summary>
        /// <param name="newUrl">�µ�URL��ַ</param>
        /// <param name="format">��ʽ����</param>
        /// <returns>����URL��ַ</returns>
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
        /// ����URL�ƶ�(HTTP 301 ����)��������ҳ����
        /// </summary>
        /// <param name="newUrl">��URL��ַ</param>
        /// <param name="format">��ʽ����</param>
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
        /// ����������URL·����ַ(�����ļ���)
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
        /// �õ�����Ŀ¼��·��
        /// </summary>
        public static string GetVirtualPath()
        {
            return VirtualPathUtility.ToAbsolute("~/");
        }
               

        /// <summary>
        /// ����һ��URL�������URLΪ~/����·����/��·�������ת����������ֱ�����ԭֵ
        /// </summary>
        /// <param name="path">·����Url��ַ</param>
        /// <param name="format">��ʽ����</param>
        public static string ProcessUrl(string path,params object[] format)
        {
            return WebCommon.ProcessUrl(path, false,format);
        }

        /// <summary>
        /// ����һ��URL�������URLΪ~/����·����/��·�������ת����������ֱ�����ԭֵ
        /// </summary>
        /// <param name="path">·����Url��ַ</param>
        /// <param name="appendSlash">�����ַĩβ������/��б�����Զ����</param>
        /// <param name="format">��ʽ����</param>
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
        /// д��һ���ַ���,��Response.Write()������
        /// </summary>
        /// <param name="content">Ҫ����д�����ַ���</param>
        /// <param name="format">��ʽ����</param>
        public static void Write(string content,params object[] format)
        {
            if (format.Length > 0)
            {
                content = string.Format(content, format);
            }

            HttpContext.Current.Response.Write(content);
        }

        /// <summary>
        /// д��һ���ַ���,������������Br���б��
        /// </summary>
        /// <param name="content">Ҫд�����ֶδ�ֵ</param>
        /// <param name="format">��ʽ����</param>
        public static void WriteBr(string content,params object[] format)
        {
            if (format.Length > 0)
            {
                content = string.Format(content, format);
            }
            HttpContext.Current.Response.Write(content + "<br />");
        }

        /// <summary>
        /// д��һ���ַ���,��Response.Write()�ļ�д,�����������һ�����з�\n
        /// </summary>
        /// <param name="content">Ҫ����д�����ַ���</param>
        /// <param name="format">��ʽ����</param>
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
        /// д��һ���ַ���,��Response.Write()�ļ�д,����д�������ҳִ��
        /// </summary>
        /// <param name="content">Ҫ����д�����ַ���</param>
        /// <param name="format">��ʽ����</param>
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
        /// �������д��һ�� error ��,����Ϊnull�ɹ�,����ʧ��
        /// </summary>
        /// <param name="error">�쳣��Ϣ</param>
        /// <param name="format">��ʽ����</param>
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
        /// ��ȡͨ��Get���������һ������ֵ�����δ�ҵ��򷵻�ΪEmpty�������ص�ֵ��ͨ��Trim()����������ǰ��ո�����
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns>���δ�ҵ��򷵻�ΪEmpty�������ص�ֵ��ͨ��Trim()����������ǰ��ո�����</returns>
        public static string Query(string name)//��,BGForm(string Name)
        {
            string qs = HttpContext.Current.Request.QueryString[name];

            if (qs == null) return string.Empty;

            return qs.Trim();
        }

        /// <summary>
        /// ��ȡͨ��Get���������һ������ֵ�����δ�ҵ����Ը������ݷ���
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="newValue">δ�ҵ��Ĵ�������</param>
        public static string Query(string name, string newValue)
        {
            string qs = HttpContext.Current.Request.QueryString[name];
            if (string.IsNullOrEmpty(qs)) return newValue;
            return qs.Trim();
        }


        /// <summary>
        /// ��ȡͨ��Form�������͵�һ������ֵ�����δ�ҵ��򷵻�ΪEmpty�������ص�ֵ��ͨ��Trim()����������ǰ��ո�����
        /// </summary>
        /// <param name="name">��������</param>
        public static string Form(string name)
        {
            string box = HttpContext.Current.Request.Form[name];
            if (box == null) return string.Empty;
            return box.Trim();
        }
        /// <summary>
        /// ��ȡͨ��Form�������͵�һ������ֵ�����δ�ҵ��򷵻�ָ��������
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="newValue">δ�ҵ�ʱ����ֵ</param>
        public static string Form(string name, string newValue)
        {
            string box = HttpContext.Current.Request.Form[name];
            if (string.IsNullOrEmpty(box)) return newValue;
            return box.Trim();
        }

        /// <summary>
        /// ��ָ���ı��� TextBox ��ֵȫ�� Trim һ��
        /// </summary>
        /// <param name="form">��</param>
        public static void FromTrim(HtmlForm form)
        {
            int i = 0;
            Type type = typeof(TextBox);
            while (form.Controls[i].GetType() == type)
                ((TextBox)form.Controls[i]).Text = ((TextBox)form.Controls[i]).Text.Trim();
        }

        /// <summary>
        /// ��ȡͨ��QueryString��Form���ݵĲ���
        /// </summary>
        /// <param name="name">��������</param>
        public static string GetPageParame(string name)
        {
            return GetPageParame(name, "");
        }

        /// <summary>
        /// ��ȡͨ��QueryString��Form���ݵĲ���
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="newValue">δ�ҵ�ʱ����ֵ</param>
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
        ///// ��ָ�����ı���������ҳHead֮��
        ///// </summary>
        ///// <param name="text">�ı���Ϣ</param>
        //public static void AppTextToHead(string text)
        //{
        //    Literal literal = new Literal();
        //    literal.Text = text;
        //    if (CurrentPage().Header == null) throw new Exception("Head Not Set Runat=Server");
        //    CurrentPage().Header.Controls.Add(literal);
        //}

        /// <summary>
        /// ��ȡ��ǰ�������е�ҳ
        /// </summary>
        public static Page CurrentPage()
        {
            var context = HttpContext.Current;
            if (context == null) return null;

            return context.Handler as Page;
        }

        /// <summary>
        /// ������Դ�󶨵�ָ���Ŀؼ�
        /// </summary>
        /// <param name="control">�ؼ�</param>
        /// <param name="data">����Դ</param>
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
        /// ������Դ�󶨵�ָ���Ŀؼ�
        /// </summary>
        /// <param name="theControl">Ҫ���в��ҵĿؼ�</param>
        /// <param name="controlname">�ؼ�����</param>
        /// <param name="data">����Դ</param>
        /// <param name="nofindisthrow">δ�ҵ�ʱ�Ƿ��׳��쳣</param>
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
        /// ������Դ�󶨵�ָ���Ŀؼ�
        /// </summary>
        /// <param name="theControl">Ҫ���ҿؼ�������</param>
        /// <param name="controlname">�ؼ�����</param>
        /// <param name="data">����Դ</param>
        public static void DataBindToFindControl(Control theControl, string controlname, object data)
        {
            WebCommon.DataBindToFindControl(theControl,controlname, data, true);
        }


          /// <summary>
        /// ��ȡWeb�ؼ��е�����
        /// </summary>
        /// <param name="ct">�ؼ�</param>
        public static string GetWebControlString(Control ct)
        {
            HtmlTextWriter tw = new HtmlTextWriter(new StringWriter());
            ct.RenderControl(tw);
            StringWriter sw = (StringWriter)tw.InnerWriter;
            return sw.GetStringBuilder().ToString();
        }

          /// <summary>
        /// ��ȡWeb�û��ؼ��е�����
        /// </summary>
        /// <param name="virtualPath">����·��</param>
        /// <param name="format">��ʽ����</param>
        public static string GetUserControlString(string virtualPath,params object[] format)
        {
            return WebCommon.GetUserControlString<Page>(virtualPath,format);
        }

        /// <summary>
        /// ��ȡWeb�û��ؼ��е�����
        /// </summary>
        /// <typeparam name="T">�ؼ������ڵĻ�����ҳ���ͣ������������<see cref="System.Web.UI.Page"/>�̳ж���</typeparam>
        /// <param name="virtualPath">�����ַ</param>
        /// <param name="format">��ʽ����</param>
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