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
    /// Pageҳ��URL��д
    /// </summary>
    /// <remarks>ע�⣺ʹ�ô�ӳ����������URL��Ŀ�ĵر�����.aspxҳ�����򽫿��ܲ�����ȷִ��</remarks>
    public class PageUrlRewriteHandler : IHttpHandlerFactory, IRequiresSessionState
    {
        #region IHttpHandlerFactory ��Ա
        /// <summary>
        /// �õ��������
        /// </summary>
        /// <param name="context">������</param>
        /// <param name="requestType">��������</param>
        /// <param name="url">�����ַ</param>
        /// <param name="pathTranslated">�����ַ</param>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            //UrlRewriteCollection urlrewrite = Common.Configuration.UrlRewrite;
            string opath;
            string path = opath = context.Request.Path;
            string source;


            //�������ļ�����ʱ��������Mvc����
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
                        context.ApplicationInstance.CompleteRequest(); //ֱ������
                        return null;
                    }
                    else
                    {
                        context.Items.Add("_urlrewrite_rewrite_path", path); //��д·��
                        context.Items.Add("_urlrewrite_source_path", opath); //Դ·��
                        
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
        /// ִ�в���
        /// </summary>
        /// <param name="handler">�������</param>
        public void ReleaseHandler(IHttpHandler handler)
        {

        }

        ///// <summary>
        ///// ����·��
        ///// </summary>
        ///// <param name="virtualPath">·��</param>
        //private IHttpHandler CreateHandler(string virtualPath)
        //{
        //    return System.Web.Compilation.BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(Page)) as IHttpHandler;
        //}

        #endregion
    }

    ///// <summary>
    ///// UrlRewrite ������
    ///// </summary>
    //public class UrlRewriteHandler : IHttpHandler,IRequiresSessionState
    //{
    //    #region IHttpHandler ��Ա
    //    /// <summary>
    //    /// �Ƿ�֧������
    //    /// </summary>
    //    public bool IsReusable
    //    {
    //        get { return false; }
    //    }

    //    /// <summary>
    //    /// ִ������
    //    /// </summary>
    //    /// <param name="context">����������</param>
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
