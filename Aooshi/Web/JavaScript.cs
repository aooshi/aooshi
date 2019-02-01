using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace Aooshi.Web
{
    /// <summary>
    /// JavaScript Web 控件
    /// </summary>
    [Description("Aooshi.Org 发布的用于ASP.Net Web应用程序的JavaScript呈现控件")]
    [ToolboxData("<{0}:JavaScript runat=\"server\"></{0}:JavaScript>")]
    public class JavaScript : WebControl
    {
        StringBuilder body;

        /// <summary>
        /// 获取或设置函数名称,注:此属性设置后将使所有脚本均包含在这个函数中,如须在内容中增加函数,请使用AppendFunction与AppendEndFunction
        /// </summary>
        public string Function
        {
            get
            {
                object obj = ViewState["Function"];
                if (obj == null) return string.Empty;
                return (string)obj;
            }
            set
            {
                ViewState["Function"] = value;
            }
        }

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
                ViewState["CharSet"] = value;
            }
        }



        /// <summary>
        /// 获取Script的内容
        /// </summary>
        public string Body
        {
            get
            {
                return body.ToString();
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
                ViewState["Type"] = value;
            }
        }

        //转义
        string TryString(string msg)
        {
            return msg.Replace(@"\", @"\\").Replace("'", @"\'").Replace(Environment.NewLine,"\\n");

            ////防止\n被替换
            //string[] temp = Regex.Split(msg, @"\\n");
            //if (temp.Length <= 1)
            //    return msg.Replace(@"\", @"\\").Replace("'", @"\'");

            //StringBuilder sb = new StringBuilder();
            //int i = -1;
            //while (++i < temp.Length)
            //{
            //    msg = temp[i].Replace(@"\", @"\\");
            //    msg = msg.Replace("'", @"\'");
            //    sb.Append(msg);
            //    sb.Append(@"\n");
            //}

            //return sb.ToString();
        }

        /// <summary>
        /// 清除所增加的所有数据
        /// </summary>
        public void Clear()
        {
            body = new StringBuilder();
        }

        /// <summary>
        /// 增加控件新内容
        /// </summary>
        /// <param name="script">添加脚本内容</param>
        public void Append(string script)
        {
            body.Append(script);
        }
        /// <summary>
        /// 增加控件新内容并自动增加换行符
        /// </summary>
        /// <param name="script">添加脚本内容</param>
        public void AppendLine(string script)
        {
            body.AppendLine(script);
        }

        /// <summary>
        /// 向控件增加提示消息 alert();
        /// </summary>
        /// <param name="msg">要写入的消息</param>
        public void AppendAlert(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;

            AppendLine(string.Format("  alert('{0}');", TryString(msg)));
        }

        /// <summary>
        /// 向控件增加一个窗体关闭信息 window.close();
        /// </summary>
        public void AppendWindowClose()
        {
            AppendLine("window.close();");
        }

        /// <summary>
        /// 向控件增加一组提示消息,消息每组一行
        /// </summary>
        /// <param name="msg">消息组</param>
        public void AppendAlert(string[] msg)
        {
            int index = msg.Length - 1;
            StringBuilder Sb = new StringBuilder();
            for (int i = 0; i < index; i++)
            {
                if (string.IsNullOrEmpty(msg[i])) continue;
                Sb.Append(TryString(msg[i]));
                Sb.Append(@"\n");
            }
            //增加最后一个
            Sb.Append(TryString(msg[index]));

            //AppendAlert(Sb.ToString());
            AppendLine(string.Format("  alert('{0}');", Sb.ToString()));
        }

        /// <summary>
        /// 向控件增加一个转向地址,location.href
        /// </summary>
        /// <param name="url">要转向的URL</param>
        public void AppendGoUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return;

            AppendLine(string.Format("  location.href='{0}';", TryString(url)));
        }

        /// <summary>
        /// 向控件增加一个历史返回页 history.back()
        /// </summary>
        /// <param name="num">返回的页数</param>
        [CLSCompliant(false)]
        public void AppendBack(UInt32 num)
        {
            AppendLine(string.Format("  history.go(-{0});", num));
        }

        /// <summary>
        /// 向控件增加一个历史前进页 history.go()
        /// </summary>
        /// <param name="num">前进的页数</param>
        [CLSCompliant(false)]
        public void AppendNext(UInt32 num)
        {
            AppendLine(string.Format("  history.go({0});", num));
        }

        /// <summary>
        /// 向控件增加一个输出document.write();
        /// </summary>
        /// <param name="input">要输出的数据</param>
        public void AppendWrite(string input)
        {
            if (string.IsNullOrEmpty(input)) return;
            input = TryString(input);
            AppendLine(string.Format("  document.write('{0}');"));
        }
        /// <summary>
        /// 向控件增加一个询问窗,当客户端用户确定时执行,否则取消执行
        /// </summary>
        /// <param name="msg">询问内容</param>
        public void AppendConfirm(string msg)
        {
            AppendConfirm(msg, null, null);
        }
        /// <summary>
        /// 向控件增加一个询问窗
        /// </summary>
        /// <param name="msg">询问内容</param>
        /// <param name="True">当点击确定时执行的操作,当为null时,执行return true;</param>
        /// <param name="False">当点击取消时执行的操作,当为null时,执行return false;</param>
        public void AppendConfirm(string msg, string True, string False)
        {
            if (string.IsNullOrEmpty(msg)) return;
            if (string.IsNullOrEmpty(True)) True = "return true;";
            if (string.IsNullOrEmpty(False)) False = "return false;";

            //msg = msg.Replace("'", "\'");
            AppendLine(string.Format("  if (confirm('{0}')){" + True + "} else {" + False + "}", TryString(msg)));
        }
        /// <summary>
        /// 向内容增加一个函数开始
        /// </summary>
        /// <param name="FunName">函数名称</param>
        public void AppendFunction(string FunName)
        {
            AppendLine(string.Format("function {0}{"));
        }
        /// <summary>
        /// 向内容增加一个函数结束
        /// </summary>
        public void AppendEndFunction()
        {
            AppendLine("}");
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

            writer.AddAttribute("language", "javascript");

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public JavaScript()
            : base(HtmlTextWriterTag.Script)
        {
            this.body = new StringBuilder();

            if (ViewState["Body"] != null)
            {
                this.body.Append(ViewState["Body"] as string);
            }
        }

        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="writer">对象</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);
            writer.Indent = 0;

            string bo = Body;

            if (!string.IsNullOrEmpty(bo))
            {
                bool isFun = !string.IsNullOrEmpty(Function);
                writer.WriteLine("// <!CDATA[");

                if (isFun)
                    writer.WriteLine(string.Format("function {0}{", Function));

                writer.WriteLine(bo);
                ViewState["Body"] = bo;


                if (isFun)
                    writer.WriteLine("}");

                writer.Write("// ]]>");
            }
            writer.Write("//  (Aooshi.Net Framework) http://www.aooshi.org/");

        }

        /// <summary>
        /// 获取控件输出数据
        /// </summary>
        public string GetString()
        {
            HtmlTextWriter tw = new HtmlTextWriter(new StringWriter());
            base.Render(tw);
            return ((StringWriter)tw.InnerWriter).GetStringBuilder().ToString();
        }
    }
}
