using System;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

namespace Aooshi.Web.Pagination
{
    /// <summary>
    /// ��ҳ�ؼ�������
    /// </summary>
    public abstract class PaginationBase : System.Web.UI.WebControls.WebControl
    {
        /// <summary>
        /// ��ʼ���ؼ�
        /// </summary>
        public PaginationBase() : base(HtmlTextWriterTag.Div) { base.CssClass = "Pagination"; }


        /// <summary>
        /// ����һ����ͼ����
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="v">����ֵ</param>
        protected virtual void SetViewData(string name, object v)
        {
            this.ViewState[name] = v;
        }

        /// <summary>
        /// ��ȡһ����ͼ����
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="name">��������</param>
        /// <param name="default">��δ������ʱ��Ĭ��ֵ</param>
        protected virtual T GetViewData<T>(string name, T @default)
        {
            object o = this.ViewState[name];
            if (o == null) return @default;
            return (T)o;
        }

        /// <summary>
        /// �Ƿ�Ϊ�ط����
        /// </summary>
        public virtual bool IsPost
        {
            get { return false; }
        }


        long _RecordIndex;

        /// <summary>
        /// ��ȡ�������ݴ����ǰҳ��ǰ�����ֵ����ֵ����Ϊ���ݴ���������ʼֵ
        /// </summary>
        public virtual long RecordIndex
        {
            get { return this._RecordIndex; }
            private set { this._RecordIndex = value; }
        }

        int _NextIndex;
        /// <summary>
        /// ��ȡ��һҳҳ��
        /// </summary>
        public virtual int NextIndex
        {
            get { return this._NextIndex; }
            private set { this._NextIndex = value; }
        }
        int _PreIndex;
        /// <summary>
        /// ��ȡ��һҳҳ��
        /// </summary>
        public virtual int PreIndex
        {
            get { return this._PreIndex; }
            private set { this._PreIndex = value; }
        }

        int _LastIndex;
        /// <summary>
        /// ��ȡ���һҳҳ��
        /// </summary>
        public virtual int LastIndex
        {
            get { return this._LastIndex; }
            private set { this._LastIndex = value; }
        }

        /// <summary>
        /// ��δ������ʱ,�Ƿ���ʾ,Ĭ��Ϊ��ʾ
        /// </summary>
        public bool ZeroVisible
        {
            get { return this.GetViewData<bool>("ZeroVisible", true); }
            set { this.SetViewData("ZeroVisible", value); }
        }

        /// <summary>
        /// ��δ������ʱ,�Ƿ���ʾ,Ĭ��Ϊ��ʾ
        /// </summary>
        public virtual HorizontalAlign Align
        {
            get { return this.GetViewData<HorizontalAlign>("Align", HorizontalAlign.Center); }
            set { this.SetViewData("Align", value); }
        }

        ///// <summary>
        ///// ��ȡ������Ĭ�����ð�ť����ɫ
        ///// </summary>
        //public virtual System.Drawing.Color ButtonBackColor
        //{
        //    get { return this.GetViewData<System.Drawing.Color>("ButtonBackColor", System.Drawing.Color.White); }
        //    set { this.SetViewData("ButtonBackColor", value); }
        //}
        ///// <summary>
        ///// ��ȡ������Ĭ������ѡ�а�ť����ɫ
        ///// </summary>
        //public virtual System.Drawing.Color ButtonSelectBackColor
        //{
        //    get { return this.GetViewData<System.Drawing.Color>("ButtonSelectBackColor", System.Drawing.ColorTranslator.FromHtml("#0099CC")); }
        //    set { this.SetViewData("ButtonSelectBackColor", value); }
        //}

        ///// <summary>
        ///// ��ȡ������Ĭ�����ð�ť��ɫ
        ///// </summary>
        //public virtual System.Drawing.Color ButtonColor
        //{
        //    get { return this.GetViewData<System.Drawing.Color>("ButtonColor", System.Drawing.Color.Empty); }
        //    set { this.SetViewData("ButtonColor", value); }
        //}

        ///// <summary>
        ///// ��ȡ������Ĭ������ѡ�а�ť��ɫ
        ///// </summary>
        //public virtual System.Drawing.Color ButtonSelectColor
        //{
        //    get { return this.GetViewData<System.Drawing.Color>("ButtonColor", System.Drawing.Color.White); }
        //    set { this.SetViewData("ButtonColor", value); }
        //}

        ///// <summary>
        ///// ��ȡ������Ĭ�����ð�ť�߿�ɫ
        ///// </summary>
        //public virtual System.Drawing.Color ButtonBorderColor
        //{
        //    get { return this.GetViewData<System.Drawing.Color>("ButtonBorderColor", System.Drawing.ColorTranslator.FromHtml("#336699")); }
        //    set { this.SetViewData("ButtonBorderColor", value); }
        //}
        

        /// <summary>
        /// ��һҳ�ť
        /// </summary>
        public virtual string NextText
        {
            get { return this.GetViewData<string>("NextText", "&gt;"); }
            set { this.SetViewData("NextText", value); }
        }

        /// <summary>
        /// ��һҳ��ť
        /// </summary>
        public virtual string PreText
        {
            get { return this.GetViewData<string>("PreText", "&lt;"); }
            set { this.SetViewData("PreText", value); }
        }

        /// <summary>
        /// βҳ��ť
        /// </summary>
        public virtual string LastText
        {
            get { return this.GetViewData<string>("LastText", "&gt;&gt;"); }
            set { this.SetViewData("LastText", value); }
        }

        /// <summary>
        /// ��ҳ��ť
        /// </summary>
        public virtual string FistText
        {
            get { return this.GetViewData<string>("FistText", "&lt;&lt;"); }
            set { this.SetViewData("FistText", value); }
        }

        ITemplate _Template;
        /// <summary>
        /// ��ȡ��������ͼ��ʽ��
        /// </summary>
        public virtual ITemplate Template
        {
            get { return _Template; }
            set { _Template = value; }
        }

        /// <summary>
        /// ��ȡ������<see cref="Template"/>���������������ʽ�����,Ĭ��Ϊ 0
        /// </summary>
        public virtual int TemplateIndex
        {
            get { return this.GetViewData<int>("TemplateIndex", 0); }
            set { this.SetViewData("TemplateIndex", value); }
        }

        /// <summary>
        /// �Ƿ����<see cref="Template"/>��������ʽ�е�CSS��ʽ��
        /// </summary>
        public virtual bool WriteStyle
        {
            get { return this.GetViewData<bool>("WriteStyle", true); }
            set { this.SetViewData("WriteStyle", value); }
        }

        /// <summary>
        /// ��ȡ������<see cref="Template"/>��������ʽ�еİ�ť����Ĭ����ʾ0��
        /// </summary>
        public virtual int ButtonCount
        {
            get { return this.GetViewData<int>("ButtonCount", 0); }
            set { this.SetViewData("ButtonCount", value); }
        }

        /// <summary>
        /// ��ȡ�����ÿͻ��˵����¼�����
        /// </summary>
        public virtual string OnClientClick
        {
            get { return this.GetViewData<string>("OnClientClick", ""); }
            set { this.SetViewData("OnClientClick", value); }
        }

        bool _IsCompute = false;
        /// <summary>
        /// ��ȡ�Ƿ��ѽ��м���
        /// </summary>
        public bool IsCompute
        {
            get { return _IsCompute; }
            private set { this._IsCompute = value; }
        }

        /// <summary>
        /// �������ݴ������
        /// </summary>
        public virtual void Compute()
        {
            if (this.IsCompute) return;

            if (this.Count == 0)
            {
                this.Index = 1;

                this.LastIndex = this.NextIndex = this.PreIndex = 0;
                this.RecordIndex = 0;
            }
            else
            {
                int index, last, next, up, size;
                long start, count;

                count = this.Count;
                size = this.Size;
                index = this.Index;


                //�õ�βҳ//�������
                last = (count % size == 0) ? Convert.ToInt32(count / size) : Convert.ToInt32(count / size + 1);


                //�жϵ�ǰҳ���Ƿ���ȷ
                if (index <= 1)
                {
                    index = 1;
                    up = 1;   //��ҳ
                    start = 0;  //��ʼ��¼��
                    next = 2;
                }
                else
                {
                    if (index > last) index = last;
                    start = (index - 1) * size;  //��ʼ����

                    up = index - 1;  //��ҳ
                    next = index + 1;

                    if (up < 1) up = 1;
                }


                if (next > last) next = last;

                this.PreIndex = up;
                this.NextIndex = next;
                this.LastIndex = last;
                this.Index = index;
                this.RecordIndex = start;
            }
            this.IsCompute = true;


            if (this.Count == 0 && !this.ZeroVisible)
            {
                this.Visible = false;
            }
        }


        #region ����


        /// <summary>
        /// ��ȡ�����ó�����ת��������ʽ,0����ʾ��Ĭ�ϲ���ʾ
        /// </summary>
        public virtual int JumpStyle
        {
            get { return this.GetViewData<int>("JumpStyle", 0); }
            set { this.SetViewData("JumpStyle", value); }
        }

        /// <summary>
        /// ��ȡ�������ܼ�¼��
        /// </summary>
        public virtual long Count
        {
            get { return this.GetViewData<long>("Count", 0); }
            set { this.SetViewData("Count", value); this.IsCompute = false; }
        }


        /// <summary>
        /// ��ȡ�����÷�ҳ��С��Ĭ��Ϊ10
        /// </summary>
        public virtual int Size
        {
            get { return this.GetViewData<int>("Size", 10); }
            set { this.SetViewData("Size", value); this.IsCompute = false; }
        }

        int _Index = 0;

        /// <summary>
        /// ��ȡ�����õ�ǰҳ��
        /// </summary>
        public virtual int Index
        {
            get { return this._Index; }
            set { this._Index = value; this.IsCompute = false ; }
        }


        #endregion

        /// <summary>
        /// ����URL����
        /// </summary>
        /// <param name="index">����,��Ϊ��ʱ����$PAGE$����</param>
        public virtual string CreateLink(int index)
        {
            return "";
        }

        /// <summary>
        /// ��дʼֵ�����Զ��ж��Ƿ�ͨ�����ݼ��㣬���δ����������ݼ���<see cref="Compute"/>
        /// </summary>
        /// <param name="writer">����</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.Write("\r\n");
            writer.WriteLine("<!--  Aooshi WebControl Start (Aooshi.Net Framework : http://www.aooshi.org/) -->");
            base.RenderBeginTag(writer);
        }

        /// <summary>
        /// ��дβֵ
        /// </summary>
        /// <param name="writer">����</param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
            writer.WriteLine("<!-- Aooshi WebControl End -->");
        }

        /// <summary>
        /// ����ؼ�����
        /// </summary>
        /// <param name="writer">�������</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            //this.Compute();

            base.RenderContents(writer);
            if (this.Template != null)
            {
                if (this.WriteStyle)
                    this.Template.RenderStyle(writer);

                this.Template.Render(writer);
            }
        }
    }
}
