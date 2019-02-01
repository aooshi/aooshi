using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;

namespace Aooshi.Ajax
{
    /// <summary>
    /// JavaScript Web 控件
    /// </summary>
    [ToolboxItem(false)]
    internal class AjaxScriptControl : WebControl
    {
        string src;
        string charset;
        string body;
        string type;

        /// <summary>
        /// 获取或设置连接串
        /// </summary>
        public string Src
        {
            get
            {
                object obj = ViewState["Src"];
                if (obj == null) return string.Empty;
                return (string)obj;
            }
            set
            {
                src = value;
                ViewState["Src"] = value;
            }
        }

        /// <summary>
        /// 获取或设置连接字符集
        /// </summary>
        public string CharSet
        {
            get
            {
                object obj = ViewState["CharSet"];
                if (obj == null) return string.Empty;
                return (string)obj;
            }
            set
            {
                charset = value;
                ViewState["CharSet"] = value;
            }
        }

        /// <summary>
        /// 获取或设置Script内容
        /// </summary>
        public string Body
        {
            get
            {
                object obj = ViewState["Body"];
                if (obj == null) return string.Empty;
                return (string)body;
            }
            set
            {
                body = value;
                ViewState["Body"] = value;
            }
        }

        /// <summary>
        /// 获取或设置类别
        /// </summary>
        public string Type
        {
            get
            {
                object obj = ViewState["Type"];
                if (obj == null) return "text/javascript";
                return (string)obj;
            }
            set
            {
                type = value;
                ViewState["Type"] = value;
            }
        }

        /// <summary>
        /// 重写属性写入
        /// </summary>
        /// <param name="writer">值</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.WriteLine();
            base.AddAttributesToRender(writer);

            if (!string.IsNullOrEmpty(Src))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, Src);
            }

            if (!string.IsNullOrEmpty(CharSet))
                writer.AddAttribute("charset", CharSet);

            if (!string.IsNullOrEmpty(Type))
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");

            writer.AddAttribute("language","javascript");

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public AjaxScriptControl(): base(HtmlTextWriterTag.Script)
        {
        }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="body">内容</param>
        public AjaxScriptControl(string body):this()
        {
            this.Body = body;
        }

        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="writer">流对象</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);
            writer.Indent = 0;

            if (!string.IsNullOrEmpty(Body))
            {
                writer.WriteLine("// <!CDATA[");
                writer.WriteLine(Body);
                writer.Write("// ]]>");
            }
            else
            {
                writer.Write("<!--ASP.NET SCRIPT CONTROL(HTTP://www.aooshi.org/dotnet/ajax)-->");
            }

        }
    }
}
