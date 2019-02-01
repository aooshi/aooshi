using System;
using System.Collections.Generic;
using System.Text;
using Aooshi.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;

namespace Aooshi.Web
{
    /// <summary>
    /// Url Rewrite 转换数据
    /// </summary>
    public class UrlRewrite
    {
        const string REWRITE_PATH_NAME = "_urlrewrite_rewrite_path";
        const string SOURCE_PATH_NAME = "_urlrewrite_source_path";

        /// <summary>
        /// 处理URL转向
        /// </summary>
        /// <param name="context">引用</param>
        /// <param name="isTransfer">是否使用换页执行</param>
        internal static bool Rewrite(HttpContext context,bool isTransfer)
        {
            UrlRewriteCollection urlrewrite = Common.Configuration.UrlRewrite;
            string opath;
            string path = opath = context.Request.Path;
            string source;

            foreach (UrlRewirteRule rule in urlrewrite)
            {
                source = rule.Source;
                if (source[0] == '~')
                {
                    string ap = context.Request.ApplicationPath;
                    if (!ap.EndsWith("/")) ap += "/"; 
                    source = ap + source.Substring(2);
                }

                Regex re = new Regex(source, RegexOptions.IgnoreCase);
                if (re.IsMatch(path))
                {
                    path = re.Replace(path, rule.Object);

                    string query = context.Request.QueryString.ToString();
                    if (!string.IsNullOrEmpty(query))
                    {
                        if (path.IndexOf('?') != -1) path = path + "&" + query;
                        else path += "?" + query;
                    }

                    //if Redirect
                    if (rule.Redirect)
                    {
                        context.Response.StatusCode = 301;
                        context.Response.Status = "301 Moved Permanently";
                        context.Response.AddHeader("Location", path);
                        context.ApplicationInstance.CompleteRequest(); //直接跳出
                        return true;
                    }
                    else
                    {
                        context.Items.Add(REWRITE_PATH_NAME, path); //重写路径
                        context.Items.Add(SOURCE_PATH_NAME, opath); //源路径
                        if (isTransfer)
                        {
                            context.Server.Transfer(path, true);
                        }
                        else
                        {
                            context.RewritePath(path);//,false);// true);
                        }
                        return true;
                    }
                }
            }

            return false;
        }

        ///// <summary>
        ///// 处理URL转向
        ///// </summary>
        ///// <param name="application">引用</param>
        //internal static bool MvcRewrite(HttpApplication application)
        //{
        //    string opath;
        //    string path = opath = application.Request.Path;

        //    //当物理文件存在时，不进行Mvc处理
        //    //if (File.Exists(application.Request.MapPath(path))) return false; 

        //    MvcTemplate template = new MvcTemplate(MvcCommon.GetTemplateName());
        //    HttpContext context = application.Context;

        //    path = opath.Replace(application.Request.ApplicationPath, "");
        //    path = path.Substring(0, path.LastIndexOf('.'));
        //    if (path.StartsWith("/")) path = path.Substring(1);
        //    path = template.GetTemplatePath(path);

        //    System.Web.UI.Page page = new System.Web.UI.Page();

        //    page.Controls.Add(page.LoadControl("~/WebUserControl.ascx"));
        //    page.Controls.Add(page.LoadControl("~/WebUserControl.ascx"));
        //    page.Controls.Add(page.LoadControl("~/WebUserControl.ascx"));
        //    page.Controls.Add(page.LoadControl("~/WebUserControl.ascx"));
        


        //    context.Handler = page;
        //    page.ProcessRequest(context);

        //    application.CompleteRequest();

        //    //context.Response.Write(context.GetType().ToString());

        //    //context.Response.End();


        //    //page.ProcessRequest(context);

        //    //context.RemapHandler(page);

        //    //context.Items.Add("_urlrewrite_rewrite_path", path); //重写路径
        //    //context.Items.Add("_urlrewrite_source_path", opath); //源路径
        //    //context.RewritePath(path,false);

        //    return true;
        //}

        #region old url rewrite
        ///// <summary>
        ///// 处理URL转向
        ///// </summary>
        ///// <param name="context">引用</param>
        ///// <param name="istre">istre</param>
        //internal static void UrlRewrite(HttpContext context, bool istre)
        //{
        //    UrlRewriteCollection urlrewrite = AooshiConfiguration.Configuration.UrlRewrite;
        //    string template = MvcCommon.GetTemplate();
        //    string opath;
        //    string path = opath = context.Request.Path;
        //    string source;
        //    int indexof;

        //    foreach (UrlRewirteRule rule in urlrewrite)
        //    {
        //        source = rule.Source;
        //        if (source[0] == '~')
        //        {
        //            source = context.Request.ApplicationPath + source.Substring(1);
        //        }

        //        Regex re = new Regex("^" + source + "$", RegexOptions.IgnoreCase);
        //        if (re.IsMatch(path))
        //        {
        //            path = re.Replace(path, rule.Object);

        //            string query = context.Request.QueryString.ToString();
        //            if (!string.IsNullOrEmpty(query))
        //            {
        //                if (path.IndexOf('?') != -1) path = path + "&" + query;
        //                else path += "?" + query;
        //            }

        //            indexof = path.IndexOf("{template}");

        //            if (!string.IsNullOrEmpty(rule.Template))
        //            {
        //                path = path.Replace("{template}", rule.Template);
        //                template = rule.Template;
        //            }
        //            else if (!string.IsNullOrEmpty(template))
        //            {
        //                path = path.Replace("{template}", template);
        //            }



        //            //if Redirect
        //            if (rule.Redirect)
        //            {
        //                context.Response.StatusCode = 301;
        //                context.Response.Status = "301 Moved Permanently";
        //                context.Response.AddHeader("Location", path);
        //                return;
        //            }
        //            else
        //            {
        //                if (indexof > -1)
        //                {
        //                    context.Items.Add("__template_path", path.Substring(0, indexof) + template + "/");
        //                    context.Items.Add("__source_path", opath.Replace(Path.GetFileName(opath), ""));
        //                }
        //                if (istre)
        //                    context.Server.Transfer(path);
        //                else
        //                    context.RewritePath(path);
        //                return;
        //            }
        //        }
        //    }
        //}

        #endregion

        /// <summary>
        /// 当URL重写时获取重写路径
        /// </summary>
        public static string GetRewritePath
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Items[REWRITE_PATH_NAME]);
            }
        }

        /// <summary>
        /// 当Url重写时获取源请求路径
        /// </summary>
        public static string GetSourcePath
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Items[SOURCE_PATH_NAME]);
            }
        }
    }
}
