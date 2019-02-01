using System;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using Aooshi.Configuration;
using System.Text.RegularExpressions;
using System.IO;

namespace Aooshi.Web
{
    /// <summary>
    /// Page页用URL重写
    /// </summary>
    /// <remarks>注意：使用此映射所操作的URL其目的地必需是.aspx页，否则将可能不可正确执行</remarks>
    public class PageUrlRewriteHandler : IHttpHandlerFactory, IRequiresSessionState
    {
        #region IHttpHandlerFactory 成员
        /// <summary>
        /// 得到处理程序
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="requestType">请求类型</param>
        /// <param name="url">请求地址</param>
        /// <param name="pathTranslated">物理地址</param>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            //UrlRewriteCollection urlrewrite = Common.Configuration.UrlRewrite;
            string opath;
            string path = opath = context.Request.Path;
            string source;


            //当物理文件存在时，不进行Mvc处理
            //if (File.Exists(pathTranslated)) return PageParser.GetCompiledPageInstance(url, pathTranslated, context);

            foreach (UrlRewirteRule rule in Common.Configuration.UrlRewrite)
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
                        return null;
                    }
                    else
                    {
                        context.Items.Add("_urlrewrite_rewrite_path", path); //重写路径
                        context.Items.Add("_urlrewrite_source_path", opath); //源路径
                        
                        string[] ps = path.Split('?');
                        string fp = context.Request.MapPath(ps[0]);

                        if (ps.Length > 1)
                            context.RewritePath(ps[0], "", ps[1]);
                        return PageParser.GetCompiledPageInstance(ps[0], fp , context);
                    }
                }
            }

            return PageParser.GetCompiledPageInstance(url, pathTranslated, context);
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="handler">处理程序</param>
        public void ReleaseHandler(IHttpHandler handler)
        {

        }

        ///// <summary>
        ///// 虚拟路径
        ///// </summary>
        ///// <param name="virtualPath">路径</param>
        //private IHttpHandler CreateHandler(string virtualPath)
        //{
        //    return System.Web.Compilation.BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(Page)) as IHttpHandler;
        //}

        #endregion
    }

    ///// <summary>
    ///// UrlRewrite 处理类
    ///// </summary>
    //public class UrlRewriteHandler : IHttpHandler,IRequiresSessionState
    //{
    //    #region IHttpHandler 成员
    //    /// <summary>
    //    /// 是否支持重用
    //    /// </summary>
    //    public bool IsReusable
    //    {
    //        get { return false; }
    //    }

    //    /// <summary>
    //    /// 执行请求
    //    /// </summary>
    //    /// <param name="context">请求上下文</param>
    //    public  void ProcessRequest(HttpContext context)
    //    {
    //        if (!UrlRewrite.Rewrite(context, true))

    //            throw new HttpException(404, "Not Found"); 
    //    }

    //    #endregion
    //}
}



/*
______________________________________________________________

To use this handler, include the following lines in a Web.config file.
 
<configuration>
 
 * 
 * 
	<configSections>
		<section name="Aooshi" type="Aooshi.Configuration.FrameworkSection,Aooshi"/>
	</configSections>
 
	<Aooshi>
		<UrlRewrite>
			<add source="~/(affiche|dynamic|news)/(\d+).ascx" object="~/article/Detail.aspx?aid=$2"/>
			<add source="~/book/bookinfo_(\d+).ascx" object="~/book/Detail.aspx?bid=$1"/>
		</UrlRewrite>
	</Aooshi>
 * 
 * 
   <system.web>
      <httpHandlers>
         <add verb="*" path="*.ascx" type="Aooshi.Web.PageUrlRewriteHandler,Aooshi"/>
      </httpHandlers>
   </system.web>
</configuration>
*/
