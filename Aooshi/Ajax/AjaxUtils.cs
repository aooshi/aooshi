using System;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Web.UI;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using System.Collections.Generic;

[assembly: WebResource("Aooshi.Ajax.Resources.AjaxScript.js", "application/x-javascript")]
namespace Aooshi.Ajax
{
    /// <summary>
    /// ����������
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// ����Ŀ¼�½����� AjaxAooshi.aspx &gt; <%@ Page Language="C#" Inherits="Aooshi.Ajax.CallbackPage" %>
    /// 
    /// protected void Page_Load(object sender, EventArgs e)
    /// {
    ///     //ע��ajax�ű�����ڱ�ҳ��header�ؼ���
    ///     Aooshi.Ajax.AjaxUtils.RegisterPage(this.Header);
    /// 
    ///     //�������е�Ajax�����������ҳ�У��Ա����
    ///     Aooshi.Ajax.AjaxUtils.RegisterType(typeof(_Default), this.Header);
    /// 
    ///     //ע��һ��ajax������
    ///     //ǰҳ���÷�ʽ��
    ///     //  ͬ������ <script> var value = b(); alert(value); </script>
    ///     //  �첽���� <script> b(function(value){ alert(value); });</script>
    ///     Aooshi.Ajax.AjaxUtils.RegisterGet("b", "ajaxtest.aspx");
    /// }
    /// 
    /// //ע��һ��ajax���ú���
    /// 
    /// //ǰ��ҳʹ�÷�ʽ��     
    /// //  ͬ������ <script> var value = a(); alert(value); </script>
    /// //  �첽���� <script> a(function(value){ alert(value); });</script>
    /// [Aooshi.Ajax.AjaxMethod()]
    /// public static string a()
    /// {
    ///     return DateTime.Now.Ticks.ToString();
    /// }
    /// ]]></code>
    /// </example>
    public class AjaxUtils
    {
        //internal const string eAjaxScriptName = "Script.aspx";
        //internal const string eAjaxMethod = "Method.aspx";
        internal const string ProductName = "Ajax - Asp.Net";
        /// <summary>
        /// call back page path
        /// </summary>
        public static readonly string CallbackPath;

        //internal const string eAjaxScriptDir = "Ajax";
        internal static List<string> regMet;

        private static string _AppDir;


        static AjaxUtils()
        {
            AjaxUtils.CallbackPath = AjaxUtils.GetAppDir() + "AjaxAooshi.aspx";
        }

        /// <summary>
        /// ��ȡ��������Ŀ¼
        /// </summary>
        internal static string GetAppDir()
        {
            if (_AppDir == null)
            {
                _AppDir = HttpContext.Current.Request.ApplicationPath;
                if (_AppDir != "/")
                    _AppDir = _AppDir.EndsWith("/")?_AppDir:_AppDir + "/";
            }
            return _AppDir;
        }

        /// <summary>
        /// ע��һ����ҳ���ݻ�ȡ,��ָ�������ַ�����ʽ
        /// </summary>
        /// <param name="ClientMethodName">�ͻ���js��������</param>
        /// <param name="Url">Ҫ��ȡ����ҳ���ݵ�ַ</param>
        /// <param name="callBack">�ص�����</param>
        public static void RegisterGet(string ClientMethodName, string Url, string callBack)
        {
            AjaxParame ap = new AjaxParame(Url);
            ap.callBack = callBack;
            RegisterGet(ClientMethodName, ap);
        }
        /// <summary>
        /// ע��һ����ҳ���ݻ�ȡ,��ָ�����ص�����
        /// </summary>
        /// <param name="Url">Ҫ��ȡ����ҳ���ݵ�ַ</param>
        /// <param name="ClientMethodName">�ͻ���js��������</param>
        public static void RegisterGet(string ClientMethodName, string Url)
        {
            RegisterGet(ClientMethodName, new AjaxParame(Url));
        }
        /// <summary>
        /// ע��һ����ҳ���ݻ�ȡ
        /// </summary>
        /// <param name="ClientMethodName">�ͻ���js��������</param>
        /// <param name="axpm">����ʵ��</param>
        public static void RegisterGet(string ClientMethodName, AjaxParame axpm)
        {
            if (axpm == null || string.IsNullOrEmpty(ClientMethodName)) return;
            if (string.IsNullOrEmpty(axpm.Url)) return;

            if (HttpContext.Current == null) return;
            Page page = (Page)HttpContext.Current.CurrentHandler;
            if (page.Header == null) throw new AjaxHeadNotServer();

            StringBuilder Sb = new StringBuilder();
            Sb.Append("         function ");
            Sb.Append(ClientMethodName);
            Sb.Append("(");

            for (int i = 0; i < axpm.Parames.Count; i++)
            {
                Sb.Append(axpm.Parames[i]);
            }

            Sb.AppendLine("){");
            Sb.AppendLine("         var Aj = new eAjax(); ");

            if (!string.IsNullOrEmpty(axpm.callError))
            {
                Sb.AppendFormat("         Aj.callError = {0};", axpm.callError);
                Sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(axpm.callBack))
            {
                Sb.AppendFormat("         Aj.callBack = {0};", axpm.callBack);
                Sb.AppendLine();
            }

            for (int i = 0; i < axpm.Parames.Count; i++)
            {
                Sb.AppendFormat("         Aj.Add('{0}',{0});", axpm.Parames[i]);
                Sb.AppendLine();
            }

            Dictionary<string, string>.Enumerator en = axpm.SendData.GetEnumerator();
            while (en.MoveNext())
            {
                Sb.AppendFormat("         Aj.Add('{0}','{1}');", en.Current.Key, en.Current.Value);
                Sb.AppendLine();
            }

            if (axpm.isPost)
                    Sb.AppendFormat("         return Aj.post('{0}');", axpm.Url);
            else
                    Sb.AppendFormat("         return Aj.get('{0}');", axpm.Url);

            Sb.AppendLine();
            AjaxScriptControl Sc = new AjaxScriptControl();
            Sc.Body = Sb.ToString();
            page.Header.Controls.Add(Sc);
        }

        /// <summary>
        /// ��ҳ��ע������,ҳ���뽫 Head ���ע��Ϊ: runat=server
        /// </summary>
        public static void RegisterPage()
        {
            if (HttpContext.Current == null) throw new Exception("Ajax Error Is Not Form Http Server;");
            Page page = ((Control)HttpContext.Current.CurrentHandler).Page;
            if (page == null || page.Header == null) throw new AjaxHeadNotServer();
            AjaxUtils.RegisterPage(page.Header);
        }
        /// <summary>
        /// ��ҳ��ע������,������ע�ᵽָ���Ŀؼ���
        /// </summary>
        public static void RegisterPage(Control control)
        {
            //control.Controls.Add(new AjaxScriptSrcControl(string.Format("{0}/{1}", GetAppDir() + eAjaxScriptDir, eAjaxScriptName)));
            control.Controls.Add(new AjaxScriptSrcControl(control.Page.ClientScript.GetWebResourceUrl(typeof(AjaxUtils), "Aooshi.Ajax.Resources.AjaxScript.js")));
        }
        
        /// <summary>
        /// ע��Ҫ��������ͷ�������,���ø÷���ǰ����ע��<see cref="RegisterPage()"/>or<see cref="RegisterPage(Control)"/>(ע��:�����е��������һ������Ϊ����,���򽫻����)
        /// </summary>
        /// <param name="t">Ajax���÷����ĵ���</param>
        /// <exception cref="AjaxNotFindMethodException">��������δ�ҵ�ָ�����Ƶķ���</exception>
        /// <exception cref="AjaxMethodNotPublic">�������е���������������β�Ϊpublic</exception>
        ///// <exception cref="AjaxMethodOName">������ķ�������</exception>
        public static void RegisterType(Type t)
        {
            if (HttpContext.Current == null) return;
            Page page = ((Control)HttpContext.Current.CurrentHandler).Page;
            if (page == null || page.Header == null) throw new AjaxHeadNotServer();
            RegisterType(t,page.Header);
        }
        /// <summary>
        /// ע��Ҫ��������ͷ�������,���ø÷���ǰ����ע��<see cref="RegisterPage()"/>or<see cref="RegisterPage(Control)"/>(ע��:�����е��������һ������Ϊ����,���򽫻����)
        /// </summary>
        /// <param name="t">Ajax���÷����ĵ���</param>
        /// <param name="control">Ҫ���մ����ݵĿؼ�</param>
        /// <exception cref="AjaxNotFindMethodException">��������δ�ҵ�ָ�����Ƶķ���</exception>
        /// <exception cref="AjaxMethodNotPublic">�������е���������������β�Ϊpublic</exception>
        ///// <exception cref="AjaxMethodOName">������ķ�������</exception>
        public static void RegisterType(Type t,Control control)
        {
            if (t == null) return;
            StringBuilder Sb = new StringBuilder();
            MethodInfo[] mis = t.GetMethods();////////////////
            foreach (MethodInfo mi in mis)
            {
                if (mi.GetCustomAttributes(typeof(AjaxMethod), true).Length > 0)
                {
                    Sb.AppendLine(GetMethodJScript(mi, t));
                }
            }
            control.Controls.Add(new AjaxScriptControl(Sb.ToString()));
        }
        /// <summary>
        /// ����һ��JavaScript������
        /// </summary>
        /// <param name="mi">����</param>
        /// <param name="t">�������</param>
        /// <returns>�������ɵ��ַ���ֵ</returns>
        /// <exception cref="AjaxMethodNotPublic">�������е���������������β�Ϊpublic</exception>
        ///// <exception cref="AjaxMethodOName">������ķ�������</exception>
        private static string GetMethodJScript(MethodInfo mi, Type t)
        {
            if (mi == null && t == null) return "";
            if (!mi.IsPublic) throw new AjaxMethodNotPublic(t.FullName, mi.Name);
            //if (eAjaxScriptName.StartsWith(mi.Name)) throw new AjaxMethodOName(t.FullName, mi.Name);
            //string mtdName = string.Format("{0}_{1}", t.Name, mi.Name);
            string mtdName = mi.Name;// //string.Format("{1}",mi.Name);
            if (regMet == null) regMet = new List<string>();

            StringBuilder Sb = new StringBuilder();
            ParameterInfo[] parameters = mi.GetParameters();

            Sb.Append("         function ");
            Sb.Append(mtdName);
            Sb.Append("(");

            AjaxMethod[] ajaxmethods = (AjaxMethod[])mi.GetCustomAttributes(typeof(AjaxMethod), true);
            if (ajaxmethods.Length == 0) throw new AjaxNotFindMethodException("��Ҫ���õĺ�����������ע��ΪAjaxMethod���Եĺ�����");
            if (ajaxmethods[0].MethodType != AjaxMethodType.Default)
            {
                Sb.Append("_object,");
            }
            else
            {
                foreach (ParameterInfo pi in parameters)
                {
                    Sb.Append(pi.Name);
                    Sb.Append(",");
                }
            }

            Sb.AppendLine("callBack,callError){");
            Sb.AppendLine("         var Aj = new eAjax(); ");
            //Sb.AppendFormat("         Aj.Url = eAjaxPathName + '{0}';", eAjaxMethod);eAjaxPathName
            //����Ŀ¼,Ajax����Ŀ¼,��������
            Sb.AppendLine("         Aj.callError = callError;");
            Sb.AppendLine("         Aj.callBack = callBack;");
            Sb.AppendFormat("         Aj.Add('Ajax_Aooshi_Namespace','{0},{1}');", t.FullName, t.Assembly.GetName().Name);
            Sb.AppendLine();
            Sb.AppendFormat("         Aj.Add('Ajax_Aooshi_MethodName','{0}');", mi.Name);
            Sb.AppendLine();
            if (ajaxmethods[0].MethodType != AjaxMethodType.Default)
            {
                if (ajaxmethods[0].MethodType == AjaxMethodType.Object)
                {
                    ParameterInfo[] pinfos = mi.GetParameters();
                    if (pinfos.Length != 1)
                        throw new AjaxMethodParameterException("ע��Ϊ AjaxMethodType.Object �ĺ���ֻ����һ���������,�ұ�����һ���������;");

                    Sb.AppendFormat("         Aj.Add('Ajax_Aooshi_MethodObject','{0},{1}');", pinfos[0].ParameterType.FullName, pinfos[0].ParameterType.Assembly.GetName().Name);
                    Sb.AppendLine();
                }
                else if (ajaxmethods[0].MethodType == AjaxMethodType.Array)
                {
                    Sb.AppendLine("         Aj.Add('Ajax_Aooshi_MethodObject','');");
                }
                else
                {
                    throw new AjaxMethodParameterException("�����Ϊ AjaxMethod ����ָ��һ����������;");
                }
                Sb.AppendFormat("         for(var _i in _object)");
                Sb.AppendLine();
                Sb.AppendFormat("           Aj.Add(_i,_object[_i]);");
                Sb.AppendLine();
            }
            else
            {
                Sb.AppendLine("         Aj.Add('Ajax_Aooshi_MethodObject','');");
                //������
                foreach (ParameterInfo pi in parameters)
                {
                    Sb.AppendFormat("         Aj.Add('{0}',{0});", pi.Name);
                    Sb.AppendLine();
                }
            }
            Sb.AppendLine("         return Aj.post('" + AjaxUtils.CallbackPath + "');");
            Sb.Append("         }");

            return Sb.ToString();
        }

        /// <summary>
        /// ��ҳ��ע������,eAjax�ĵ���ģʽ,���ô���ǰ�����Ƚ���ҳע�� RegisterPage ����
        /// </summary>
        public static void RegisterDebug()
        {
            if (HttpContext.Current == null) return;
            Page page = ((Control)HttpContext.Current.CurrentHandler).Page;

            if (page == null || page.Header != null)
                page.Header.Controls.Add(new AjaxScriptControl("_eAjaxDebug = true;"));
        }

        ///// <summary>
        ///// ����������Դ
        ///// </summary>
        //internal static string GetRes(string Name)
        //{
        //    string r;
        //    ResourceManager Rm = new ResourceManager("Aooshi.Ajax.AjaxRes", Assembly.GetExecutingAssembly());
        //    r = Rm.GetObject(Name) as string;
        //    Rm.ReleaseAllResources();
        //    return r;
        //}

        /// <summary>
        /// ��ȡ�ؼ��е�����
        /// </summary>
        /// <param name="ct">�ؼ�</param>
        internal static string GetControlString(Control ct)
        {
            HtmlTextWriter tw = new HtmlTextWriter(new StringWriter());
            ct.RenderControl(tw);
            StringWriter sw = (StringWriter)tw.InnerWriter;
            return sw.GetStringBuilder().ToString();
        }

        ///// <summary>
        ///// ��ȡ���ýڵ��е�����
        ///// </summary>
        ///// <returns>����ȫ�����ýڵ�</returns>
        //static GlobalizationSection GetConfigGlobalization()
        //{
        //    return (GlobalizationSection)WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath).GetSection("system.web/globalization");
        //}

        ///// <summary>
        ///// ���ط������ݵ����ñ���
        ///// </summary>
        ///// <returns>����</returns>
        //internal static Encoding GetCongfigResponseEndcoding()
        //{
        //    return GetConfigGlobalization().ResponseEncoding;
        //}


        #region �ɹٷ��������ӽ���

        /// <summary>
        /// ��ָ���ı�����Ajax���б���
        /// </summary>
        /// <param name="input">Ҫ������ļ���</param>
        public static string AjaxEncode(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            string str = input;
            string str2 = "0123456789ABCDEF";
            int length = str.Length;
            StringBuilder builder = new StringBuilder(length * 2);
            int num3 = -1;
            while (++num3 < length)
            {
                char ch = str[num3];
                int num2 = ch;
                if ((((0x41 > num2) || (num2 > 90)) && ((0x61 > num2) || (num2 > 0x7a))) && ((0x30 > num2) || (num2 > 0x39)))
                {
                    switch (ch)
                    {
                        case '@':
                        case '*':
                        case '_':
                        case '+':
                        case '-':
                        case '.':
                        case '/':
                            goto Label_0125;
                    }
                    builder.Append('%');
                    if (num2 < 0x100)
                    {
                        builder.Append(str2[num2 / 0x10]);
                        ch = str2[num2 % 0x10];
                    }
                    else
                    {
                        builder.Append('u');
                        builder.Append(str2[(num2 >> 12) % 0x10]);
                        builder.Append(str2[(num2 >> 8) % 0x10]);
                        builder.Append(str2[(num2 >> 4) % 0x10]);
                        ch = str2[num2 % 0x10];
                    }
                }
            Label_0125:
                builder.Append(ch);
            }
            return builder.ToString();


            ////byte[] arr = Encoding.Unicode.GetBytes(input);
            ////StringBuilder sb = new StringBuilder();
            ////for (int i = 0; i < arr.Length; i += 2)
            ////{
            ////    sb.Append(arr[i].ToString("X2"));
            ////    sb.Append(arr[i + 1].ToString("X2"));
            ////}

            ////return sb.ToString();
            //return Microsoft.JScript.GlobalObject.escape(input);
        }
        /// <summary>
        /// ��ָ��������Ajax�����ı����н���
        /// </summary>
        /// <param name="input">Ҫ���н�����ı���,���������ȷ�򷵻�Ϊԭ��</param>
        public static string AjaxUnEncode(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string str = input;
            int length = str.Length;
            StringBuilder builder = new StringBuilder(length);
            int num6 = -1;
            while (++num6 < length)
            {
                char ch = str[num6];
                if (ch == '%')
                {
                    int num2;
                    int num3;
                    int num4;
                    int num5;
                    if (((((num6 + 5) < length) && (str[num6 + 1] == 'u')) && (((num2 = HexDigit(str[num6 + 2])) != -1) && ((num3 = HexDigit(str[num6 + 3])) != -1))) && (((num4 = HexDigit(str[num6 + 4])) != -1) && ((num5 = HexDigit(str[num6 + 5])) != -1)))
                    {
                        ch = (char)((((num2 << 12) + (num3 << 8)) + (num4 << 4)) + num5);
                        num6 += 5;
                    }
                    else if ((((num6 + 2) < length) && ((num2 = HexDigit(str[num6 + 1])) != -1)) && ((num3 = HexDigit(str[num6 + 2])) != -1))
                    {
                        ch = (char)((num2 << 4) + num3);
                        num6 += 2;
                    }
                }
                builder.Append(ch);
            }
            return builder.ToString();

            //return Microsoft.JScript.GlobalObject.unescape(input);
            //if (input.Length < 4 || input.Length % 4 != 0) return input;
            //byte[] bs = new byte[input.Length / 2];
            //try
            //{
            //    string a, b;
            //    int len;
            //    for (int i = 1; i <= (input.Length / 4); i++)
            //    {
            //        a = string.Concat(input[4 * i - 4], input[4 * i - 3]);
            //        b = string.Concat(input[4 * i - 2], input[4 * i - 1]);
            //        len = 2 * (i - 1);
            //        bs[len] = Convert.ToByte(a, 16);
            //        bs[len + 1] = Convert.ToByte(b, 16);
            //    }
            //}
            //catch
            //{
            //    return input;
            //}
            //return Encoding.Unicode.GetString(bs);
        }

        static int HexDigit(char c)
        {
            if ((c >= '0') && (c <= '9'))
            {
                return (c - '0');
            }
            if ((c >= 'A') && (c <= 'F'))
            {
                return (('\n' + c) - 0x41);
            }
            if ((c >= 'a') && (c <= 'f'))
            {
                return (('\n' + c) - 0x61);
            }
            return -1;
        }


        #endregion

        /// <summary>
        /// ָ��ҳ�Ķ���ת��Ϊ�ͻ��˿��õĶ���,���������ֹͣҳ����
        /// </summary>
        /// <param name="obj">����</param>
        public static void AjaxToPageScript(object obj)
        {
            Type type = obj.GetType();

            //����
            if (type.IsArray)
            {
                AjaxMakeObject.RunatArrayObject(obj, type, HttpContext.Current.Response);
                return;
            }

            //������
            AjaxMakeObject.RunatClassObject(obj, type, HttpContext.Current.Response);

        }

        /// <summary>
        /// ��ҳע���쳣 AjaxResult �������
        /// </summary>
        /// <param name="Page">ҳ����</param>
        public static void AjaxErrorPageToObject(Page Page)
        {
            if (Page != null) Page.Error += new EventHandler(Page_Error);
        }

        /// <summary>
        /// д���쳣����
        /// </summary>
        internal static void Page_Error(object sender, EventArgs e)
        {
            AjaxResult result = new AjaxResult();
            Exception ex = HttpContext.Current.Server.GetLastError();
            result.Success = false;
            result.Message = ex.Message;
            result.TagA = ex.ToString();
            HttpContext.Current.Server.ClearError();
            AjaxUtils.AjaxToPageScript(result);
        }
    }

}