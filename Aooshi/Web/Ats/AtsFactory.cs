using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Web.UI;
using Aooshi.Configuration;
using System.Web.Compilation;

namespace Aooshi.Web.Ats
{
    /// <summary>
    /// Atsģ�廯������
    /// </summary>
    public class AtsFactory
    {
        static Regex reg_template = new Regex("<ats:template path=\"(.+?)\" />", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static Regex reg_templatem = new Regex("<ats:template path=\"(.+?)\" model=\"(.+?)\" />", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        static Regex reg_get = new Regex("<ats:get exp=\"(.+?)\" />", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static Regex reg_getx = new Regex(@"<\%=(.+?)\%>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        static Regex reg_set = new Regex("<ats:set name=\"(.+?)\" value=\"(.+?)\" />", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static Regex reg_method = new Regex("<ats:method name=\"(.+?)\" parames=\"(.+?)\" />", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        static Regex reg_loop = new Regex("<ats:loop id=\"(.+?)\" source=\"(.+?)\">(((?!</?ats:loop[^>]*>)[\\s\\S])+)</ats:loop>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static Regex reg_for = new Regex("<ats:for id=\"(.+?)\" begin=\"(.+?)\" end=\"(.+?)\">(((?!</?ats:for[^>]*>)[\\s\\S])+)</ats:for>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        static Regex reg_if = new Regex("<ats:if exp=\"(.+)\">(((?!</?ats:if[^>]*>)[\\s\\S])+)</ats:if>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static Regex reg_elseif = new Regex("<ats:elseif exp=\"(.+?)\">", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static Regex reg_else = new Regex("<ats:else />", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        static Regex reg_expression = new Regex(@"\{([a-z]+?):([^\{\}]+)\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        static Regex reg_expression_b = new Regex(@"\{b:(.+?)\}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        static Regex reg_expression_bid = new Regex(@"\{b:\[(.+?),(.+?)\]\}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        static Regex reg_expression_iid = new Regex(@"\{i:(.+?)\}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        static Regex reg_dotnetclass = new Regex("<ats:custom dotnetclass=\"(.+?)\" />", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);


        string ViewRootPath, PhysicalViewRootPath, ViewCachePath, PhysicalViewCachePath, Suffix;

        /// <summary>
        /// initialize
        /// </summary>
        /// <param name="ViewRootPath"></param>
        /// <param name="PhysicalViewRootPath"></param>
        /// <param name="ViewCachePath"></param>
        /// <param name="PhysicalViewCachePath"></param>
        /// <param name="AtsSuffix"></param>
        internal AtsFactory(string ViewRootPath,string PhysicalViewRootPath,string ViewCachePath,string PhysicalViewCachePath,string AtsSuffix)
        {
            this.ViewCachePath = ViewCachePath;
            this.ViewRootPath = ViewRootPath;
            this.PhysicalViewCachePath = PhysicalViewCachePath;
            this.PhysicalViewRootPath = PhysicalViewRootPath;
            this.Suffix = AtsSuffix;
        }


        Encoding _encoding = null;
        /// <summary>
        /// ��ȡϵͳ���뷽ʽ
        /// </summary>
        protected virtual Encoding GetEncoding
        {
            get
            {
                if (_encoding == null)
                {
                    _encoding = ((System.Web.Configuration.GlobalizationSection)HttpContext.Current.GetSection("system.web/globalization")).FileEncoding;
                }
                return _encoding;
            }
        }


        string _inheritsclass = "Aooshi.Web.Ats.AtsView";
        /// <summary>
        /// ��ͼ����Ĭ�ϻ�����
        /// </summary>
        public virtual string InheritsClass
        {
            get { return _inheritsclass; }
            set { this._inheritsclass = value; }
        }

        bool _entitymodel = true;
        /// <summary>
        /// ��ͼʹ�õ�Model�Ƿ�Ϊ����ʵ�塣��ֵǿ�ҽ���ʹ��ʵ�巺��
        /// </summary>
        public virtual bool EntityModel
        {
            get { return _entitymodel; }
            set { _entitymodel = value; }
        }

        /// <summary>
        /// ��������ģ��
        /// </summary>
        public virtual void MakeTemplate()
        {
            MakeTemplate(null);
        }

        /// <summary>
        /// ����ָ��ģ�������ģ��ҳ
        /// </summary>
        /// <param name="groupname">�������ƣ���Ҫ�������������ô�ֵΪnull</param>
        public virtual void MakeTemplate(string groupname)
        {
            string viewcachepath, viewpath;

            if (!string.IsNullOrEmpty(groupname))
            {
                viewpath = Path.Combine(this.PhysicalViewRootPath, groupname);
                viewcachepath = Path.Combine(this.PhysicalViewCachePath, groupname);
            }
            else
            {
                viewpath = this.PhysicalViewRootPath;
                viewcachepath = this.PhysicalViewCachePath;
            }

            if (!Directory.Exists(viewcachepath))
                Directory.CreateDirectory(viewcachepath);

            string objpath;
            foreach (DirectoryInfo dir in new DirectoryInfo(viewpath).GetDirectories("*", SearchOption.AllDirectories))
            {
                objpath = Path.Combine(viewcachepath, dir.FullName.Replace(viewpath, ""));
                if (!Directory.Exists(objpath)) Directory.CreateDirectory(objpath);

                foreach (string p in Directory.GetFiles(dir.FullName, "*" + this.Suffix))
                {
                    Make( p, Path.Combine(objpath, Path.GetFileNameWithoutExtension(p)));
                }
            }

            ////current dir file
            //foreach (string p in Directory.GetFiles(viewpath, "*" + _suffix))
            //{
            //    Make("",p, Path.Combine(viewcachepath, Path.GetFileNameWithoutExtension(p)));
            //}
        }


        /// <summary>
        /// ����ָ��ģ���ָ��ģ��ҳ
        /// </summary>
        /// <param name="groupname">��������</param>
        /// <param name="template">ģ��ҳ,�磺  register �� accounts/register</param>
        public virtual void MakeTemplate(string groupname, string template)
        {
            string viewpath = Path.Combine(this.PhysicalViewRootPath, groupname);
            string viewcachepath = Path.Combine(this.PhysicalViewCachePath, groupname);

            if (!Directory.Exists(viewcachepath))
                Directory.CreateDirectory(viewcachepath);

            viewcachepath = Path.Combine(viewcachepath, template);
            viewpath = Path.Combine(viewpath, template + this.Suffix);

            Make(viewpath, viewcachepath);

        }

        /// <summary>
        /// ����ָ��ģ��
        /// </summary>
        /// <param name="source_path">Դ·��</param>
        /// <param name="cache_path">�����ļ���չ����Ŀ��·��</param>
        /// <returns>���ش����������</returns>
        protected virtual string Make(string source_path, string cache_path)
        {
            cache_path += ".ascx";


            if (File.Exists(cache_path)) File.Delete(cache_path);
            string content = File.ReadAllText(source_path, this.GetEncoding);

            //ѭ�� �����һ�����п�����get�ཻ����ڲ����������쳣
            while (reg_loop.IsMatch(content))
                content = reg_loop.Replace(content, new MatchEvaluator(CapLoop));

            while (reg_for.IsMatch(content))
                content = reg_for.Replace(content, new MatchEvaluator(CapFor));

            while (reg_if.IsMatch(content))
                content = reg_if.Replace(content, new MatchEvaluator(CapIf));


            content = reg_templatem.Replace(content, new MatchEvaluator(CapTemplateM));
            content = reg_template.Replace(content, new MatchEvaluator(CapTemplate));
            content = reg_get.Replace(content, new MatchEvaluator(CapGet));
            content = reg_getx.Replace(content, new MatchEvaluator(CapGetx));
            content = reg_set.Replace(content, new MatchEvaluator(CapSet));
            content = reg_method.Replace(content, new MatchEvaluator(CapMethod));

            string inheritsname = "";

            Match match = reg_dotnetclass.Match(content);
            if (match != null && match.Success)
            {
                inheritsname = match.Groups[1].Value;
            }

            if (string.IsNullOrEmpty(inheritsname))
            {
                inheritsname = this.InheritsClass;
            }

            content = reg_dotnetclass.Replace(content, "");
            content = "<%@ Control Language=\"C#\" AutoEventWireup=\"true\" Inherits=\"" + inheritsname + "\" %>" + content;


            File.WriteAllText(cache_path, content, this.GetEncoding);

            return content;
        }


        /// <summary>
        /// ��鲢����ָ����ģ��
        /// </summary>
        /// <param name="groupname">��������</param>
        /// <param name="template">ģ��ҳ,�磺  register �� accounts/register</param>
        public virtual void UpdateTemplate(string groupname, string template)
        {
            string op = Path.Combine(this.PhysicalViewRootPath, groupname + "\\" + template + this.Suffix);
            string cp = Path.Combine(this.PhysicalViewCachePath, groupname + "\\" + template + ".ascx");
            if (File.GetLastWriteTime(op).Ticks > File.GetLastWriteTime(cp).Ticks)
            {
                this.MakeTemplate(groupname,template);
            }
        }

        /// <summary>
        /// ���ʽ
        /// </summary>
        /// <param name="str">Ҫ������ַ���</param>
        protected virtual string Expression(string str)
        {
            if (!reg_expression.IsMatch(str)) return str.Replace('\'', '"');
            return Expression(reg_expression.Replace(str, new MatchEvaluator(CapExpression)));
        }

        /// <summary>
        /// ���ʽƥ��
        /// </summary>
        /// <param name="m">ƥ����</param>
        /// <returns>���ش�����ɵ�����</returns>
        protected virtual string CapExpression(Match m)
        {
            if (m.Success)
            {
                string v = m.Groups[2].Value;
                switch (m.Groups[1].Value)
                {
                    case "v": return (this.EntityModel) ? string.Format("base.Model.{0}", v) : string.Format("base.ViewDatas[\"{0}\"]", v);
                    //case "t": return string.Format("this.ViewDatas[\"{0}\"]", v);
                    //case "d": return string.Format("this.model.{0}", v);
                    //case "x": return string.Format("Page.GetVariable<{0}>(\"{1}\")",m.Groups[3].Value.ToLower(), v);
                    //case "c": return string.Format("this.{0}", v);
                    case "m": return string.Format("this.{0}", v);
                    case "b": throw new Exception(" {b:" + m.Groups[2].Value + "} is not included in loop ");

                    case "int": return string.Format("this.Integer({0})", v);
                    case "string": return string.Format("this.String({0})", v);
                    case "double": return string.Format("this.Double({0})", v);
                    //err
                    default: return m.Groups[2].Value;
                }
            }
            return "";
        }

        /// <summary>
        /// else if ƥ��������
        /// </summary>
        /// <param name="m">ƥ����</param>
        protected virtual string CapElseIf(Match m)
        {
            if (m.Success)
            {
                return "<% } else if (" + Expression(m.Groups[1].Value) + ") { %>";
            }

            return "";
        }

        /// <summary>
        /// if ƥ��������
        /// </summary>
        /// <param name="m">ƥ����</param>
        protected virtual string CapIf(Match m)
        {
            if (m.Success)
            {
                string content = m.Groups[2].Value;
                string expression = Expression(m.Groups[1].Value);

                content = reg_else.Replace(content, "<% }else{ %>");
                content = reg_elseif.Replace(content, new MatchEvaluator(CapElseIf));

                return "<% { if (" + expression + "){ %>" + content + "<% } } %>";
            }

            return "";
        }

        /// <summary>
        /// for ƥ��������
        /// </summary>
        /// <param name="m">ƥ����</param>
        protected virtual string CapFor(Match m)
        {
            //����ǰ׺��ʶ����ֹ�����ѭ�����±���������
            if (m.Success)
            {
                string index = "_i_" + m.Groups[1].Value;
                string begin = Expression(reg_expression_bid.Replace(m.Groups[2].Value, "_i_$1"));
                string end = Expression(reg_expression_bid.Replace(m.Groups[3].Value, "_i_$1"));
                string content = reg_expression_bid.Replace(m.Groups[4].Value, "_i_$1");
                content = reg_expression_b.Replace(content, index);

                StringBuilder builder = new StringBuilder();
                builder.Append("<% { ");
                builder.AppendLine(string.Format("System.Int32 {0} = {1};", index, begin));
                builder.AppendLine(string.Format("while({0} <= {1})", index, end));
                builder.Append("{ %>");
                builder.Append(content);
                builder.Append("<% " + index + " ++; }} %>");
                return builder.ToString();
            }
            return "";
        }

        /// <summary>
        /// loop �б�ѭ��
        /// </summary>
        /// <param name="m">ƥ����</param>
        protected virtual string CapLoop(Match m)
        {
            //����ǰ׺��ʶ����ֹ�����ѭ�����±���������
            if (m.Success)
            {
                string n = "_" + m.Groups[1].Value;
                string dr = n + "_dr";
                string ix = n + "_index";

                string dd = reg_expression_bid.Replace(m.Groups[2].Value, "_$1_dr[\"$2\"]");
                dd = reg_expression_iid.Replace(dd, "_$1_index");
                dd = Expression(dd);
                string content = reg_expression_bid.Replace(m.Groups[3].Value, "_$1_dr[\"$2\"]");
                content = reg_expression_iid.Replace(content, "_$1_index");
                content = reg_expression_b.Replace(content, dr + "[\"$1\"]");

                StringBuilder builder = new StringBuilder();
                builder.Append("<% { ");
                builder.AppendLine(string.Format("System.Int32 {0} = 0;", ix));
                builder.Append("foreach (System.Data.DataRow " + dr + " in (((System.Data.DataTable)" + dd + ") ?? new System.Data.DataTable()).Rows) { %>");
                builder.Append(content);
                builder.Append("<% " + ix + " ++; }} %>");
                return builder.ToString();
            }
            return "";
        }

        /// <summary>
        /// ƥ�亯������
        /// </summary>
        /// <param name="m">ƥ����</param>
        protected virtual string CapMethod(Match m)
        {
            if (m.Success)
            {
                return string.Format("<% {0}({1}); %>", m.Groups[1].Value, Expression(m.Groups[2].Value));
            }

            return "";
        }

        /// <summary>
        /// ƥ�����ô���
        /// </summary>
        /// <param name="m">ƥ����</param>
        protected virtual string CapSet(Match m)
        {
            if (m.Success)
            {
                //return string.Format("<% Page.SetVariable(\"{0}\",{1}) %>", m.Groups[1].Value, Expression(m.Groups[2].Value));
                
                if (this.EntityModel)
                    return string.Format("<% base.Model.{0}={1}; %>", m.Groups[1].Value, Expression(m.Groups[2].Value));
                else
                    return string.Format("<% base.ViewDatas.SetVariable(\"{0}\",{1}); %>", m.Groups[1].Value, Expression(m.Groups[2].Value));
            }

            return "";
        }

        /// <summary>
        /// ƥ���ȡ����
        /// </summary>
        /// <param name="m">ƥ����</param>
        protected virtual string CapGet(Match m)
        {
            if (m.Success)
            {
                return "<%= " + Expression(m.Groups[1].Value) + " %>";
            }

            return "";
        }

        /// <summary>
        /// ƥ���ȡ����
        /// </summary>
        /// <param name="m">ƥ����</param>
        protected virtual string CapGetx(Match m)
        {
            if (m.Success)
            {
                return "<%= " + Expression(m.Groups[1].Value.Trim()) + " %>";
            }

            return "";
        }

        /// <summary>
        /// ƥ��ģ��
        /// </summary>
        /// <param name="m">ƥ����</param>
        protected virtual string CapTemplate(Match m)
        {
            if (m.Success)
            {
                //return "<!-- #include file=\"" + m.Groups[1].Value + ".view\" -->";
                string id = "a_" + Guid.NewGuid().ToString("N");
                return "<asp:placeholder OnLoad=\"" + id + "_load\" id=\"" + id + "\" runat=\"server\"></asp:placeholder><script language=\"c#\" runat=\"server\">void " + id + "_load(object sender, EventArgs e) { ((System.Web.UI.WebControls.PlaceHolder)sender).Controls.Add(base.ViewPage.CreateView(\"" + Expression(m.Groups[1].Value.Trim()) + "\",null)); }</script> ";

                //return "<% ViewWrite(\"" + Expression(m.Groups[1].Value.Trim()) + "\"); %>";
            }

            return "";
        }

        /// <summary>
        /// ƥ��ģ��
        /// </summary>
        /// <param name="m">ƥ����</param>
        protected virtual string CapTemplateM(Match m)
        {
            if (m.Success)
            {
                //return "<!-- #include file=\"" + m.Groups[1].Value + ".view\" -->";
                string id = "a_" + Guid.NewGuid().ToString("N");
                return "<asp:placeholder OnLoad=\"" + id + "_load\" id=\"" + id + "\" runat=\"server\"></asp:placeholder><script language=\"c#\" runat=\"server\">void " + id + "_load(object sender, EventArgs e) { ((System.Web.UI.WebControls.PlaceHolder)sender).Controls.Add(base.ViewPage.CreateView(\"" + Expression(m.Groups[1].Value.Trim()) + "\"," + Expression(m.Groups[2].Value.Trim()) + ")); }</script> ";

                //return "<% ViewWrite(\"" + Expression(m.Groups[1].Value.Trim()) + "\"," + Expression(m.Groups[2].Value.Trim()) + "); %>";
            }

            return "";
        }
    }
}
