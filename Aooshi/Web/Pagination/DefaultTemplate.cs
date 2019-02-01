using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Aooshi.Web.Pagination
{
    /// <summary>
    /// 默认的输出样式
    /// </summary>
    public class DefaultTemplate : ITemplate
    {
        /// <summary>
        /// 初始化类型
        /// </summary>
        /// <param name="pagation">分页控件</param>
        public DefaultTemplate(PaginationBase pagation)
        {
            this.Pagination = pagation;
        }

        #region IViewStyle 成员
        PaginationBase _Pagination;
        /// <summary>
        /// 获取当前分页组件
        /// </summary>
        public virtual PaginationBase Pagination
        {
            get { return this._Pagination; }
            private set { this._Pagination = value; }
        }


        /// <summary>
        /// 获取Jump按钮显示文字
        /// </summary>
        public virtual string JumpText
        {
            get { return "GO"; }
        }

        /// <summary>
        /// 下拉式Jump选项显示数，默认为50
        /// </summary>
        public virtual int JumpOptionCount
        {
            get { return 50; }
        }

        /// <summary>
        /// 获取客户端单击事件
        /// </summary>
        /// <param name="index">索引</param>
        protected string GetClientClick(int index)
        {
            string c = this.Pagination.OnClientClick;
            if (string.IsNullOrEmpty(c)) return "" ;
            return string.Format(" onclick=\"return {0}({1});\"",c, index);
        }

        
        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="text">显示文字</param>
        public string CreateButton(int index, string text)
        {
            return this.CreateButton(index, text, false);
        }

        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="text">显示文字</param>
        /// <param name="nocurr">是否禁止设置为当前状态</param>
        public virtual string CreateButton(int index, string text,bool nocurr)
        {
            if (index == this.Pagination.Index && !nocurr)
                return string.Format("<a href=\"{0}\" class=\"curr\" title=\"{1}\"{2}>{1}</a>", this.Pagination.CreateLink(index), text, this.GetClientClick(index));
            else
                return string.Format("<a href=\"{0}\" title=\"{1}\"{2}>{1}</a>", this.Pagination.CreateLink(index), text, this.GetClientClick(index));
        }
        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="href">链接地址</param>
        /// <param name="text">显示文字</param>
        public virtual string CreateButton(string href, string text)
        {
            return string.Format("<a href=\"{0}\" title=\"{1}\">{1}</a>", href, text);
        }

        /// <summary>
        /// 输出数据
        /// </summary>
        /// <param name="writer">输出对象</param>
        public virtual void Render(HtmlTextWriter writer)
        {
            writer.WriteLine("<table border=\"0\" align=\"" + this.Pagination.Align.ToString().ToLower() + "\"><tr><td>");
            switch (this.Pagination.TemplateIndex)
            {
                case 1:
                    Template1(writer);
                    break;
                default:
                    Template0(writer);
                    break;
            }

            if (this.Pagination.JumpStyle > 0)
            {
                writer.Write("&nbsp;</td><td class=\"gotd\">");
                this.ReaderJump(writer);
            }

            writer.WriteLine("</td></tr></table>");
            if (this.Pagination.JumpStyle > 0)
                this.ReaderJumpScript(writer);

        }

        /// <summary>
        /// 默认的样式输出
        /// </summary>
        /// <param name="writer">输出对象</param>
        public virtual void RenderStyle(HtmlTextWriter writer)
        {
            //if (this.Pagination.BackColor == System.Drawing.Color.Empty) this.Pagination.BackColor = System.Drawing.ColorTranslator.FromHtml("#0099CC");
            //if (this.Pagination.ForeColor == System.Drawing.Color.Empty) this.Pagination.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            //if (this.Pagination.BorderColor == System.Drawing.Color.Empty) this.Pagination.BorderColor = System.Drawing.ColorTranslator.FromHtml("#336699");
            if (this.Pagination.Height == System.Web.UI.WebControls.Unit.Empty) this.Pagination.Height = System.Web.UI.WebControls.Unit.Pixel(20);

            string _height = this.Pagination.Height.ToString();
            string _align = this.Pagination.Align.ToString();
            //string _bordercolor = System.Drawing.ColorTranslator.ToHtml(this.Pagination.BorderColor);
            //string _backcolor = System.Drawing.ColorTranslator.ToHtml(this.Pagination.BackColor);
            //string _forecolor = System.Drawing.ColorTranslator.ToHtml(this.Pagination.ForeColor);
            string _bordercolor = "#336699";
            string _backcolor = "#0099CC";
            string _forecolor = "#FFFFFF";


            writer.WriteLine("<style type=\"text/css\">");
            writer.WriteLine(".Pagination,.Pagination table{text-align:" + _align.ToLower() + "; margin:0px; padding:0px;}");
            writer.WriteLine(".Pagination table{ height:" + _height + ";line-height:" + _height + ";}");
            writer.WriteLine(".Pagination table td{ border:0px;}");
            writer.WriteLine(".Pagination table td a,.Pagination table td b,.Pagination table td a:visited{ white-space:nowrap;margin-left:5px;display:block;float:left;padding:0px 8px 0px 8px;border:1px solid " + _bordercolor + ";font-size:12px; line-height:" + _height + ";}");
            writer.WriteLine(".Pagination table td input{ padding:0px;border:1px solid " + _bordercolor + "; line-height:" + _height + "; height:" + _height + "; margin:0px;}");
            writer.WriteLine(".Pagination table td a{text-decoration:none;}");
            writer.WriteLine(".Pagination table td a:hover,.Pagination table td a.curr{ background-color:" + _backcolor + "; color:" + _forecolor + "; text-decoration:none;}");
            writer.WriteLine(".Pagination table td a.next-dont,.Pagination table td a.prev-dont{ border:none; padding:0px;}");
            writer.WriteLine(".Pagination table td .go input{ width:30px; float:left; }");
            writer.WriteLine("</style>");
        }

        /// <summary>
        /// 输出Jump的脚本块
        /// </summary>
        /// <param name="writer">输出对象</param>
        protected virtual void ReaderJumpScript(HtmlTextWriter writer)
        {
            writer.WriteLine("<script language=\"javascript\" type=\"text/javascript\">");
            writer.WriteLine("function Pagination_" + this.Pagination.ClientID + "(){");
            writer.WriteLine("    var v = document.getElementById(\"pagination-goto-" + this.Pagination.ClientID + "-value\").value;");
            writer.WriteLine("    if (v == \"\") {alert(\"请输入要转向的页序！\");return;}");
            writer.WriteLine("    if (isNaN(v)){alert(\"页序只能是数字，请检查！\");return;}");

            if (this.Pagination.IsPost)
            {
                string uniqueID = this.Pagination.UniqueID;
                if ((uniqueID != null) && (uniqueID.IndexOf(':') >= 0))
                {
                    uniqueID = uniqueID.Replace(':', '$');
                }
                writer.WriteLine("    __doPostBack('" + uniqueID + "',v)");
            }
            else
                writer.WriteLine("    location.href=document.getElementById(\"pagination-url-" + this.Pagination.ClientID + "\").value.replace(\"=$PAGE$\",\"=\"+v);");

            writer.WriteLine("}");
            writer.WriteLine("</script>");

            if (!this.Pagination.IsPost)
                writer.WriteLine("<input type=\"hidden\" id=\"pagination-url-" + this.Pagination.ClientID + "\" value=\"" + this.Pagination.CreateLink(0) + "\">");
                        
        }
        /// <summary>
        /// 输出Jump样式
        /// </summary>
        /// <param name="writer">输出对象</param>
        protected virtual void ReaderJump(HtmlTextWriter writer)
        {
            switch (this.Pagination.JumpStyle)
            {
                case 1:
                    Jump1(writer);
                    break;
                case 2:
                    Jump2(writer);
                    break;
            }
        }

        /// <summary>
        ///Jump样式1
        /// </summary>
        /// <param name="writer">输出对象</param>
        protected virtual void Jump1(HtmlTextWriter writer)
        {
            writer.Write("<span class=\"go\">");
            writer.Write("<input type=\"text\" maxlength=\"8\" size=\"4\" id=\"pagination-goto-" + this.Pagination.ClientID + "-value\">");
            writer.Write(this.CreateButton("javascript:Pagination_" + this.Pagination.ClientID + "();", this.JumpText));
            //writer.WriteLine("<a href=\"javascript:Pagination_<%= {v:Viewid} %>();\" title=\"转到指定页\">GO</a>");
            writer.Write("</span>");
        }
        /// <summary>
        ///Jump样式2
        /// </summary>
        /// <param name="writer">输出对象</param>
        protected virtual void Jump2(HtmlTextWriter writer)
        {
            writer.WriteLine("<script language=\"javascript\" type=\"text/javascript\">");
            writer.WriteLine("(function(){document.write('<select id=\"pagination-goto-" + this.Pagination.ClientID + "-value\" onchange=\"Pagination_" + this.Pagination.ClientID + "();\">');");

            int start, end;
            start = this.Pagination.Index - this.JumpOptionCount;
            if (start < 1) start = 1;

            end = start + this.JumpOptionCount;
            if (end > this.Pagination.LastIndex) end = this.Pagination.LastIndex;

            writer.WriteLine("for(var i=" + start + ";i<" + end + ";i++){");
            writer.WriteLine("document.write('<option value=\"'+i+'\"');");
            writer.WriteLine("if (i == " + this.Pagination.Index + ") document.write(' selected=\"selected\"');");
            writer.WriteLine("document.write('>'+i+'</option>');");
            writer.WriteLine("}");
            writer.WriteLine("document.write('</select>');})();");
            writer.Write("</script>");
        }

        /// <summary>
        /// 样式0
        /// </summary>
        /// <param name="writer">输出对象</param>
        protected virtual void Template0(HtmlTextWriter writer)
        {
            //必需的按钮数
            if (this.Pagination.ButtonCount == 0)
                this.Pagination.ButtonCount = 10;

            int start, end;
            int b = (int)Math.Floor(((double)this.Pagination.ButtonCount / 2));

            start = this.Pagination.Index - b;
            if (start < 1) start = 1;

            end = start + this.Pagination.ButtonCount - 1;
            if (end > this.Pagination.LastIndex) end = this.Pagination.LastIndex;

            if (start > 1)
            {
                writer.WriteLine(this.CreateButton(1, "1"));
                writer.WriteLine(this.CreateButton(this.Pagination.PreIndex, "..."));
            }

            if (this.Pagination.ButtonCount > 0)
            {

                for (; start <= end; start++)
                {
                    writer.WriteLine(this.CreateButton(start, start.ToString()));
                }
            }

            if (end < this.Pagination.LastIndex)
            {
                writer.WriteLine(this.CreateButton(this.Pagination.NextIndex, "..."));
                writer.WriteLine(this.CreateButton(this.Pagination.LastIndex, this.Pagination.LastIndex.ToString()));
            }

        }

        /// <summary>
        /// 样式1
        /// </summary>
        /// <param name="writer">输出对象</param>
        protected virtual void Template1(HtmlTextWriter writer)
        {
            writer.WriteLine(this.CreateButton(1, this.Pagination.FistText,true));
            writer.WriteLine(this.CreateButton(this.Pagination.PreIndex, this.Pagination.PreText,true));

            if (this.Pagination.ButtonCount > 0)
            {
                int start, end;
                int b = (int)Math.Floor(((double)this.Pagination.ButtonCount / 2));

                start = this.Pagination.Index - b;
                if (start < 1) start = 1;

                end = start + this.Pagination.ButtonCount - 1;
                if (end > this.Pagination.LastIndex) end = this.Pagination.LastIndex;

                for (; start <= end; start++)
                {
                    writer.WriteLine(this.CreateButton(start, start.ToString()));
                }
            }
            writer.WriteLine(this.CreateButton(this.Pagination.NextIndex, this.Pagination.NextText,true));
            writer.WriteLine(this.CreateButton(this.Pagination.LastIndex, this.Pagination.LastText,true));
        }


        #endregion
    }
}
