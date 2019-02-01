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
    /// JavaScript Web �ؼ�
    /// </summary>
    [Description("Aooshi.Org ����������ASP.Net WebӦ�ó����JavaScript���ֿؼ�")]
    [ToolboxData("<{0}:JavaScript runat=\"server\"></{0}:JavaScript>")]
    public class JavaScript : WebControl
    {
        StringBuilder body;

        /// <summary>
        /// ��ȡ�����ú�������,ע:���������ú�ʹ���нű������������������,���������������Ӻ���,��ʹ��AppendFunction��AppendEndFunction
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
        /// ��ȡ���������Ӵ�
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
        /// ��ȡ�����������ַ���
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
        /// ��ȡScript������
        /// </summary>
        public string Body
        {
            get
            {
                return body.ToString();
            }
        }

        /// <summary>
        /// ��ȡ���������
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

        //ת��
        string TryString(string msg)
        {
            return msg.Replace(@"\", @"\\").Replace("'", @"\'").Replace(Environment.NewLine,"\\n");

            ////��ֹ\n���滻
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
        /// ��������ӵ���������
        /// </summary>
        public void Clear()
        {
            body = new StringBuilder();
        }

        /// <summary>
        /// ���ӿؼ�������
        /// </summary>
        /// <param name="script">��ӽű�����</param>
        public void Append(string script)
        {
            body.Append(script);
        }
        /// <summary>
        /// ���ӿؼ������ݲ��Զ����ӻ��з�
        /// </summary>
        /// <param name="script">��ӽű�����</param>
        public void AppendLine(string script)
        {
            body.AppendLine(script);
        }

        /// <summary>
        /// ��ؼ�������ʾ��Ϣ alert();
        /// </summary>
        /// <param name="msg">Ҫд�����Ϣ</param>
        public void AppendAlert(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;

            AppendLine(string.Format("  alert('{0}');", TryString(msg)));
        }

        /// <summary>
        /// ��ؼ�����һ������ر���Ϣ window.close();
        /// </summary>
        public void AppendWindowClose()
        {
            AppendLine("window.close();");
        }

        /// <summary>
        /// ��ؼ�����һ����ʾ��Ϣ,��Ϣÿ��һ��
        /// </summary>
        /// <param name="msg">��Ϣ��</param>
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
            //�������һ��
            Sb.Append(TryString(msg[index]));

            //AppendAlert(Sb.ToString());
            AppendLine(string.Format("  alert('{0}');", Sb.ToString()));
        }

        /// <summary>
        /// ��ؼ�����һ��ת���ַ,location.href
        /// </summary>
        /// <param name="url">Ҫת���URL</param>
        public void AppendGoUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return;

            AppendLine(string.Format("  location.href='{0}';", TryString(url)));
        }

        /// <summary>
        /// ��ؼ�����һ����ʷ����ҳ history.back()
        /// </summary>
        /// <param name="num">���ص�ҳ��</param>
        [CLSCompliant(false)]
        public void AppendBack(UInt32 num)
        {
            AppendLine(string.Format("  history.go(-{0});", num));
        }

        /// <summary>
        /// ��ؼ�����һ����ʷǰ��ҳ history.go()
        /// </summary>
        /// <param name="num">ǰ����ҳ��</param>
        [CLSCompliant(false)]
        public void AppendNext(UInt32 num)
        {
            AppendLine(string.Format("  history.go({0});", num));
        }

        /// <summary>
        /// ��ؼ�����һ�����document.write();
        /// </summary>
        /// <param name="input">Ҫ���������</param>
        public void AppendWrite(string input)
        {
            if (string.IsNullOrEmpty(input)) return;
            input = TryString(input);
            AppendLine(string.Format("  document.write('{0}');"));
        }
        /// <summary>
        /// ��ؼ�����һ��ѯ�ʴ�,���ͻ����û�ȷ��ʱִ��,����ȡ��ִ��
        /// </summary>
        /// <param name="msg">ѯ������</param>
        public void AppendConfirm(string msg)
        {
            AppendConfirm(msg, null, null);
        }
        /// <summary>
        /// ��ؼ�����һ��ѯ�ʴ�
        /// </summary>
        /// <param name="msg">ѯ������</param>
        /// <param name="True">�����ȷ��ʱִ�еĲ���,��Ϊnullʱ,ִ��return true;</param>
        /// <param name="False">�����ȡ��ʱִ�еĲ���,��Ϊnullʱ,ִ��return false;</param>
        public void AppendConfirm(string msg, string True, string False)
        {
            if (string.IsNullOrEmpty(msg)) return;
            if (string.IsNullOrEmpty(True)) True = "return true;";
            if (string.IsNullOrEmpty(False)) False = "return false;";

            //msg = msg.Replace("'", "\'");
            AppendLine(string.Format("  if (confirm('{0}')){" + True + "} else {" + False + "}", TryString(msg)));
        }
        /// <summary>
        /// ����������һ��������ʼ
        /// </summary>
        /// <param name="FunName">��������</param>
        public void AppendFunction(string FunName)
        {
            AppendLine(string.Format("function {0}{"));
        }
        /// <summary>
        /// ����������һ����������
        /// </summary>
        public void AppendEndFunction()
        {
            AppendLine("}");
        }
        /// <summary>
        /// ��д����д��
        /// </summary>
        /// <param name="writer">ֵ</param>
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
        /// ��ʼ��
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
        /// ����
        /// </summary>
        /// <param name="writer">����</param>
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
        /// ��ȡ�ؼ��������
        /// </summary>
        public string GetString()
        {
            HtmlTextWriter tw = new HtmlTextWriter(new StringWriter());
            base.Render(tw);
            return ((StringWriter)tw.InnerWriter).GetStringBuilder().ToString();
        }
    }
}
