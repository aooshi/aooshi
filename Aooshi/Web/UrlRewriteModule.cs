using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Aooshi.Web
{
    /// <summary>
    /// URL��дģ��
    /// </summary>
    /// <remarks>ʹ�ô�ģ��ʱ��ע��·�����⣬������URL��д�����һЩԭ·�����ݽ����ܻ�ȡ����: cookie</remarks>
    public class UrlRewriteModule : IHttpModule
    {
        #region IHttpModule ��Ա
        /// <summary>
        /// �����ͷ�
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="context">������</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        /// <summary>
        /// ��������ʱִ��
        /// </summary>
        /// <param name="sender">����</param>
        /// <param name="e">�¼�</param>
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
