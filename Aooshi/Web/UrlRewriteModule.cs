using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Aooshi.Web
{
    /// <summary>
    /// URL重写模块
    /// </summary>
    /// <remarks>使用此模块时请注意路径问题，当进行URL重写后可能一些原路径数据将不能获取，例: cookie</remarks>
    public class UrlRewriteModule : IHttpModule
    {
        #region IHttpModule 成员
        /// <summary>
        /// 资料释放
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context">上下文</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        /// <summary>
        /// 发生请求时执行
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void context_BeginRequest(object sender, EventArgs e)
        {
            UrlRewrite.Rewrite(((HttpApplication)sender).Context,false);
        }

        #endregion
    }
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
         <add verb="*" path="*.ascx" type="Aooshi.Web.UrlRewriteHandler,Aooshi"/>
      </httpHandlers>
		<httpModules>
			<add name="UrlRewrite" type="Aooshi.Web.UrlRewriteModule,Aooshi" />
		</httpModules>
   </system.web>
</configuration>
*/
