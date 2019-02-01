using System;
using System.Web;
using System.Web.SessionState;
using Aooshi.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.UI;
using System.Web.Compilation;

namespace Aooshi.Web
{
    /// <summary>
    /// 虚拟Mvc HttpHandler 处理类，注意：使用此类不得使用Page.ResolveUrl 或 Page.ResolveClientUrl 方法，请使用 ViewResource
    /// </summary>
    public class VMvcHandler : IHttpHandler,IRequiresSessionState
    {
        #region IHttpHandler 成员

        /// <summary>
        /// 是否可重用
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="context">当前上下文</param>
        public void ProcessRequest(HttpContext context)
        {
            //有实体文件则以实体文件为准
            if (File.Exists(context.Request.PhysicalPath))
            {
                IHttpHandler handler = (IHttpHandler)BuildManager.CreateInstanceFromVirtualPath(context.Request.Path, typeof(Page));
                handler.ProcessRequest(context);
                return;
            }


            VMvcElementCollection vm = Common.Configuration.VirtualMvc;
            string path = context.Request.Path;
            string source;

            string typestring;
            Type type;

            string T,N,query;
            string[] rules;
            int rindex;


            foreach (VMvcElement rule in vm)
            {
                source = rule.Path;
                if (source[0] == '~')
                {
                    string ap = context.Request.ApplicationPath;
                    if (!ap.EndsWith("/")) ap += "/";
                    source = ap + source.Substring(2);
                }

                Match m = Regex.Match(path, source, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    typestring = rule.NameSpace;
                    query = T = N = "";
                    rindex = 0;
                    rules = rule.Rule.Split(',');

                    //匹配个数有差异时
                    if (rules.Length >= m.Groups.Count)
                        throw new AooshiException(string.Format("{0} count > {1} count",rule.Rule,rule.Path));
                    
                    foreach (string n in rules)
                    {
                        //因为匹配中索引需要加1，所以此处采用先加方式
                        rindex++;
                        switch (n)
                        {
                            case "T":
                                T = m.Groups[rindex].Value;
                                break;

                            case "N":
                                N = m.Groups[rindex].Value;
                                break;
                            default:
                                query += string.Format("&{0}={1}",n,m.Groups[rindex].Value);
                                break;
                        }
                    }

                    if (query != "")
                        context.RewritePath(path, "", query.Substring(1));

                    if (N != "")
                        typestring = string.Format("{0}.{1}.{2},{3}", rule.NameSpace,N, T, rule.Assembly); //Aooshi.Web.JavaScripts,Aooshi
                    else
                        typestring = string.Format("{0}.{1},{2}", rule.NameSpace, T, rule.Assembly); //Aooshi.Web.JavaScripts,Aooshi*/

                    type = Type.GetType(typestring, false, true);

                    if (type == null)
                    {
                        //context.Response.StatusCode = 404;
                        //context.Response.StatusDescription = "Not Found";
                        //context.Response.Write("Not Found Type To " + typestring.Split(',')[0]);
                        //context.ApplicationInstance.CompleteRequest();
                        throw new HttpException(404, "Not Found");
                        //return;
                    }

                    IHttpHandler handler = (IHttpHandler)System.Activator.CreateInstance(type);
                    handler.ProcessRequest(context);
                    return;
                    //context.RemapHandler(handler);
                }
            }
        }

        #endregion
    }
}


/*

<configuration>

	<configSections>
		<section name="Aooshi" type="Aooshi.Configuration.FrameworkSection,Aooshi"/>
	</configSections>


	<Aooshi>

		<VirtualMvc>
			<!-- 演示带功能作用的URL映射 -->
			<set path="~/(.+)/(.+).aspx" assembly="VMvcExample" namespace="VMvcExample.Control"></set>
			<!-- 演示不带后缀的URL映射 -->
			<set path="~/(.+)/(.+)" assembly="VMvcExample" namespace="VMvcExample.Control"></set>
			<!-- 演示单页映射 -->
			<set path="~/(.+).aspx" assembly="VMvcExample" namespace="VMvcExample.Control"></set>
		</VirtualMvc>

	</Aooshi>

	<system.web>

		<httpHandlers>
			<add path="*.aspx"  verb="POST,GET" type="Aooshi.Web.VMvcHandler,Aooshi"/>
			<add path="NoSuffix/*"  verb="POST,GET" type="Aooshi.Web.VMvcHandler,Aooshi"/>
		</httpHandlers>

	</system.web>
</configuration>


*/


