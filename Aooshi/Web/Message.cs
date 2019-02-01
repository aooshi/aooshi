using System;
using System.Text;
using System.Web;
using System.Collections;

namespace Aooshi.Web
{
    /// <summary>
    /// ��װһ���ɶ�Web��Ϣд����ʾ��Ϣ���ַ����汾
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
        /// ������Ĭ��ʵ��,ֻ��ʾ���ذ�ť
        /// </summary>
        public Message()
            : this(null, true, false, null)
        {
        }

        /// <summary>
        /// �����µ�ʵ��,����ʾ������һҳ��ť
        /// </summary>
        /// <param name="Message">Ҫ������ʾ����Ϣ</param>
        public Message(string Message)
            : this(Message, true, false, null)
        {
        }


        /// <summary>
        /// �����µ�ʵ��,��ʾ���ذ�ť,��ָʾ�Ƿ���ʾ�رհ�ť
        /// </summary>
        /// <param name="Message">Ҫ������ʾ����Ϣ</param>
        /// <param name="Close">�Ƿ���ʾ�رհ�ť</param>
        public Message(string Message, bool Close)
            : this(Message, true, Close, null)
        {
        }

        /// <summary>
        /// �����µ�ʵ������ʾһ��ת��URL��ť
        /// </summary>
        /// <param name="Message">Ҫ������ʾ����Ϣ</param>
        /// <param name="Jump">Ҫ����ת���URL�ص�</param>
        public Message(string Message, string Jump)
            : this(Message, false, false, Jump)
        {
        }

        /// <summary>
        /// �����µ�ʵ��
        /// </summary>
        /// <param name="Message">Ҫ������ʾ����Ϣ</param>
        /// <param name="Back">�Ƿ���ʾ���ذ�ť</param>
        /// <param name="Close">�Ƿ���ʾ�رհ�ť</param>
        /// <param name="Jump">Ҫ��ת��URL,�����ΪNull��Empty����ʾһ����ת��URL�İ�ť</param>
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

            //������Ϣ
            if (!string.IsNullOrEmpty(Message))
                this.Add(Message);
        }


        /// <summary>
        /// �����µ���ʾ��Ϣ��
        /// </summary>
        /// <param name="Message">��ʾ��Ϣ</param>
        public virtual void Add(string Message)
        {
            this.count++;
            this.Li.AppendFormat("<li>{0}</li>", Message);
        }

        /// <summary>
        /// ����һ���µİ�ť
        /// </summary>
        /// <param name="Text">��ť����</param>
        /// <param name="OnClick">������ʱ��Ҫ�����JavaScript����</param>
        public virtual void AddButton(string Text, string OnClick)
        {
            if (Text == null || OnClick == null) return;

            if (this.NButton == null) this.NButton = new Hashtable();

            this.NButton.Add(Text, OnClick);
        }

        /*/// <summary>
        /// ��ȡ������һ��ֵ����ֵ��ʾ��Ҫ��ʾ�İ�ť������ʾ�������Ƿ�Ϊ���ģ�Ĭ��ֵΪtrue
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
        /// ��ȡһ��ֵ����ֵʾ���Ƿ�����Ҫд��������
        /// </summary>
        public virtual bool IsAdd
        {
            get
            {
                return this.count > 0;
            }
        }

        /// <summary>
        /// ��ȡһ��ֵ����ֵ��ʾ�ж�����������
        /// </summary>
        public virtual int Count
        {
            get
            {
                return this.count;
            }
        }

        /// <summary>
        /// ��ȡ������һ��ֵ.��ֵ��ʾ�Ƿ���ʾ������һҳ��ť
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
        /// ��ȡ������һ��ֵ,��ֵ��ʾ�Ƿ���ʾ�رհ�ť
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
        /// ��ȡ������ҳ��תʱ��URL����ΪNull��Emptyʱ��������ʾ�ð�ť
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
        /// д����ʽ��
        /// </summary>
        /// <returns>�����ַ������ı����ʽ</returns>
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
        /// ����һ��Css��ǩ,ʹ�����ָ����Css ��ʽ
        /// </summary>
        /// <param name="Css">��ʽ������</param>
        /// <returns>�������ɵ�Css��ʽֵ</returns>
        private string CreateStyle(string Css)
        {
            string str = "<style type=\"text/css\">\n<!--\n";
            str += Css;
            str += "\n-->\n</style>";
            return str;
        }

        /// <summary>
        /// ���ɰ�ť
        /// </summary>
        /// <returns>�������ɵ��ַ���</returns>
        protected virtual string WriteButton()
        {
            string Str = "";
            if (this.Back)
                Str += "<input type=\"button\" value=\"������ҳ\" onclick=\"javascript:history.back(-1);\" />&nbsp;";
            if (this.Close)
                Str += "<input type=\"button\" value=\"�رո�ҳ\" onclick=\"javascript:window.close();\"/>&nbsp;";
            if (!string.IsNullOrEmpty(this.Jump))
                Str += string.Format("<input type=\"button\" value=\" ��һ�� \" onclick=\"javascript:location.href='{0}';\"/>&nbsp;", this.Jump);//HttpUtility.UrlEncode(this.Jump)

            //������ť
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
        /// ��������Ҫ��html����
        /// </summary>
        /// <returns>�������ɵ�html����</returns>
        protected virtual string WriteHtml()
        {
            StringBuilder Sb = new StringBuilder();
            Sb.Append("<div id=\"Title_Div\">  *��Ϣ��ʾ��</div>");
            Sb.Append("<div id=\"Body_Div\"><ul>");

            Sb.Append(this.Li);  //�����б�

            Sb.Append("</ul></div>");
            Sb.Append("<div id=\"Button_Div\">");
            Sb.Append("<div align=\"center\">");

            //���밴ť
            Sb.Append(this.WriteButton());

            Sb.Append("</div></div>");

            return Sb.ToString();
        }

        /// <summary>
        /// ���������ɵ������汾�ַ���
        /// </summary>
        /// <returns>�������ɽ��</returns>
        public override string ToString()
        {
            return CreateHtml("��Ϣ��!", this.WriteStyle(), this.WriteHtml());
        }

        /// <summary>
        /// ��ȡһ��Htmlͷβ�ļ� XHtml 1.0 ��׼
        /// </summary>
        /// <param name="Title">������Html��Title����е�����</param>
        /// <param name="Style">Ϊ��ָ����ʽ����ʽ���Ϊ��,��Ϊ����Ͽ�ʼ��������,�ò��������Զ�Ϊ��׷���κα��</param>
        /// <param name="Body">Ҫ������Html����</param>
        /// <returns>���ؽ��Ϊ������һҳ����</returns>
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
