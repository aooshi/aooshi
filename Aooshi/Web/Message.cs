using System;
using System.Text;
using System.Web;
using System.Collections;

namespace Aooshi.Web
{
    /// <summary>
    /// 封装一个可对Web信息写出提示消息的字符串版本
    /// </summary>
    public class Message
    {
        private StringBuilder Li = null;
        private bool _Back = false, _Close = false;
        private string _Jump = null;
        //bool isChinese;
        Hashtable NButton;
        int count;

        /// <summary>
        /// 创建新默认实例,只显示返回按钮
        /// </summary>
        public Message()
            : this(null, true, false, null)
        {
        }

        /// <summary>
        /// 创建新的实例,并显示返回上一页按钮
        /// </summary>
        /// <param name="Message">要进行显示的信息</param>
        public Message(string Message)
            : this(Message, true, false, null)
        {
        }


        /// <summary>
        /// 创建新的实例,显示返回按钮,并指示是否显示关闭按钮
        /// </summary>
        /// <param name="Message">要进行显示的信息</param>
        /// <param name="Close">是否显示关闭按钮</param>
        public Message(string Message, bool Close)
            : this(Message, true, Close, null)
        {
        }

        /// <summary>
        /// 创建新的实例，显示一个转向URL按钮
        /// </summary>
        /// <param name="Message">要进行显示的信息</param>
        /// <param name="Jump">要进行转向的URL地地</param>
        public Message(string Message, string Jump)
            : this(Message, false, false, Jump)
        {
        }

        /// <summary>
        /// 创建新的实例
        /// </summary>
        /// <param name="Message">要进行显示的信息</param>
        /// <param name="Back">是否显示返回按钮</param>
        /// <param name="Close">是否显示关闭按钮</param>
        /// <param name="Jump">要跳转的URL,如果不为Null或Empty则将显示一个跳转该URL的按钮</param>
        public Message(string Message, bool Back, bool Close, string Jump)
        {
            this.Li = new StringBuilder();
            this.Jump = Jump;
            this.Back = Back;
            this.Close = Close;
            //this.isChinese   = true;
            this.NButton = null;
            this.count = 0;

            this.Li = new StringBuilder();

            //加入消息
            if (!string.IsNullOrEmpty(Message))
                this.Add(Message);
        }


        /// <summary>
        /// 增加新的提示信息行
        /// </summary>
        /// <param name="Message">提示信息</param>
        public virtual void Add(string Message)
        {
            this.count++;
            this.Li.AppendFormat("<li>{0}</li>", Message);
        }

        /// <summary>
        /// 附加一个新的按钮
        /// </summary>
        /// <param name="Text">按钮文字</param>
        /// <param name="OnClick">当按下时须要处理的JavaScript数据</param>
        public virtual void AddButton(string Text, string OnClick)
        {
            if (Text == null || OnClick == null) return;

            if (this.NButton == null) this.NButton = new Hashtable();

            this.NButton.Add(Text, OnClick);
        }

        /*/// <summary>
        /// 获取或设置一个值，该值表示须要显示的按钮上所显示的文字是否为中文，默认值为true
        /// </summary>
        public virtual bool IsChinese
        {
            get
            {
                return this.isChinese;
            }
            set
            {
                this.isChinese = value;
            }
        }*/

        /// <summary>
        /// 获取一个值，该值示表是否有须要写出的数据
        /// </summary>
        public virtual bool IsAdd
        {
            get
            {
                return this.count > 0;
            }
        }

        /// <summary>
        /// 获取一个值，该值表示有多少条数据项
        /// </summary>
        public virtual int Count
        {
            get
            {
                return this.count;
            }
        }

        /// <summary>
        /// 获取或设置一个值.该值表示是否显示返回上一页按钮
        /// </summary>
        public virtual bool Back
        {
            get
            {
                return this._Back;
            }
            set
            {
                this._Back = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示是否显示关闭按钮
        /// </summary>
        public virtual bool Close
        {
            get
            {
                return this._Close;
            }
            set
            {
                this._Close = value;
            }
        }

        /// <summary>
        /// 获取或设置页跳转时的URL，当为Null或Empty时，将不显示该按钮
        /// </summary>
        public virtual string Jump
        {
            get
            {
                return this._Jump;
            }
            set
            {
                this._Jump = value;
            }
        }

        /// <summary>
        /// 写出样式表
        /// </summary>
        /// <returns>返回字符符串的表达形式</returns>
        protected virtual string WriteStyle()
        {
            StringBuilder Sb = new StringBuilder();
            Sb.Append("#Title_Div {font-size: 13px;font-weight: bold;color: #666666;text-decoration: none;");
            Sb.Append("background-color: #EFEFEF;height: 20px;width: 497px;margin-right: auto;");
            Sb.Append("margin-left: auto;border: 1px solid #CCCCCC;padding-top: 5px;padding-left: 3px;}\n");
            Sb.Append("#Body_Div {height: 100px;width: 490px;margin-right: auto;margin-left: auto;padding: 5px;");
            Sb.Append("border-right-style: solid;border-bottom-style: solid;border-left-style: solid;border-right-width: 1px;border-bottom-width: 1px;");
            Sb.Append("border-left-width: 1px;border-right-color: #CECFCE;border-bottom-color: #CECFCE;border-left-color: #CECFCE;}\n");
            Sb.Append("#Body_Div li{font-size: 13px;color: #FF0000;text-decoration: none;line-height: 130%;}\n");
            Sb.Append("#Button_Div {background-color: #F6F6F6;height: 30px;width: 502px;margin-right: auto;margin-left: auto;");
            Sb.Append("border-bottom-width: 1px;border-bottom-style: solid;border-bottom-color: #CECFCE;}\n");
            Sb.Append("#Button_Div div {padding: 3px;}");
            Sb.Append("#Button_Div input {height: 21px;width: 80px;background-color: #759ADF;");
            Sb.Append("border: 1px solid #336699;font-size: 13px;color: #FFFFFF;text-decoration: none;}");
            return CreateStyle(Sb.ToString());
        }

        /// <summary>
        /// 生成一段Css标签,使其包含指定的Css 样式
        /// </summary>
        /// <param name="Css">样式表内容</param>
        /// <returns>返回生成的Css样式值</returns>
        private string CreateStyle(string Css)
        {
            string str = "<style type=\"text/css\">\n<!--\n";
            str += Css;
            str += "\n-->\n</style>";
            return str;
        }

        /// <summary>
        /// 生成按钮
        /// </summary>
        /// <returns>返回生成的字符串</returns>
        protected virtual string WriteButton()
        {
            string Str = "";
            if (this.Back)
                Str += "<input type=\"button\" value=\"返回上页\" onclick=\"javascript:history.back(-1);\" />&nbsp;";
            if (this.Close)
                Str += "<input type=\"button\" value=\"关闭该页\" onclick=\"javascript:window.close();\"/>&nbsp;";
            if (!string.IsNullOrEmpty(this.Jump))
                Str += string.Format("<input type=\"button\" value=\" 下一步 \" onclick=\"javascript:location.href='{0}';\"/>&nbsp;", this.Jump);//HttpUtility.UrlEncode(this.Jump)

            //附增按钮
            if (this.NButton != null && this.NButton.Count > 0)
            {
                IDictionaryEnumerator ie = this.NButton.GetEnumerator();

                while (ie.MoveNext())
                {
                    Str += string.Format("<input type=\"button\" value=\" {0} \" onclick=\"{1}\"/>&nbsp;", ie.Key.ToString(), ie.Value.ToString());
                }
            }

            return Str;
        }

        /// <summary>
        /// 生成所须要的html内容
        /// </summary>
        /// <returns>返回生成的html内容</returns>
        protected virtual string WriteHtml()
        {
            StringBuilder Sb = new StringBuilder();
            Sb.Append("<div id=\"Title_Div\">  *信息提示窗</div>");
            Sb.Append("<div id=\"Body_Div\"><ul>");

            Sb.Append(this.Li);  //插入列表

            Sb.Append("</ul></div>");
            Sb.Append("<div id=\"Button_Div\">");
            Sb.Append("<div align=\"center\">");

            //插入按钮
            Sb.Append(this.WriteButton());

            Sb.Append("</div></div>");

            return Sb.ToString();
        }

        /// <summary>
        /// 返回所生成的完整版本字符串
        /// </summary>
        /// <returns>返回所成结果</returns>
        public override string ToString()
        {
            return CreateHtml("信息窗!", this.WriteStyle(), this.WriteHtml());
        }

        /// <summary>
        /// 获取一组Html头尾文件 XHtml 1.0 标准
        /// </summary>
        /// <param name="Title">出现在Html的Title标记中的内容</param>
        /// <param name="Style">为其指定样式或样式表可为空,请为其加上开始与结束标记,该参数并不自动为其追加任何标记</param>
        /// <param name="Body">要产生的Html内容</param>
        /// <returns>返回结果为完整的一页数据</returns>
        private string CreateHtml(string Title, string Style, string Body)
        {
            string enc = Encoding.Default.HeaderName;
            StringBuilder Html = new StringBuilder();
            Html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n");
            Html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" lang=\"" + enc + "\">\n");
            Html.Append("<head><title>");
            Html.Append(Title);
            Html.Append("</title>\n");
            Html.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=" + enc + "\" />\n");
            Html.Append("<meta http-equiv=\"Content-Language\" content=\"" + enc + "\" />\n");
            Html.Append(Style);
            Html.Append("</head><body>\n");
            Html.Append(Body);
            Html.Append("\n</body></html>");
            return Html.ToString();
        }

    }
}
