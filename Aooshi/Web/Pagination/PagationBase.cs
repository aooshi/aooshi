using System;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;

namespace Aooshi.Web.Pagination
{
    /// <summary>
    /// 分页控件基础类
    /// </summary>
    public abstract class PaginationBase : System.Web.UI.WebControls.WebControl
    {
        /// <summary>
        /// 初始化控件
        /// </summary>
        public PaginationBase() : base(HtmlTextWriterTag.Div) { base.CssClass = "Pagination"; }


        /// <summary>
        /// 设置一个视图变量
        /// </summary>
        /// <param name="name">变量名称</param>
        /// <param name="v">变量值</param>
        protected virtual void SetViewData(string name, object v)
        {
            this.ViewState[name] = v;
        }

        /// <summary>
        /// 获取一个视图变量
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">变量名称</param>
        /// <param name="default">当未有设置时的默认值</param>
        protected virtual T GetViewData<T>(string name, T @default)
        {
            object o = this.ViewState[name];
            if (o == null) return @default;
            return (T)o;
        }

        /// <summary>
        /// 是否为回发组件
        /// </summary>
        public virtual bool IsPost
        {
            get { return false; }
        }


        long _RecordIndex;

        /// <summary>
        /// 获取进行数据处理后当前页当前最大数值，该值可做为数据处理索引开始值
        /// </summary>
        public virtual long RecordIndex
        {
            get { return this._RecordIndex; }
            private set { this._RecordIndex = value; }
        }

        int _NextIndex;
        /// <summary>
        /// 获取下一页页序
        /// </summary>
        public virtual int NextIndex
        {
            get { return this._NextIndex; }
            private set { this._NextIndex = value; }
        }
        int _PreIndex;
        /// <summary>
        /// 获取上一页页序
        /// </summary>
        public virtual int PreIndex
        {
            get { return this._PreIndex; }
            private set { this._PreIndex = value; }
        }

        int _LastIndex;
        /// <summary>
        /// 获取最后一页页序
        /// </summary>
        public virtual int LastIndex
        {
            get { return this._LastIndex; }
            private set { this._LastIndex = value; }
        }

        /// <summary>
        /// 当未有数据时,是否显示,默认为显示
        /// </summary>
        public bool ZeroVisible
        {
            get { return this.GetViewData<bool>("ZeroVisible", true); }
            set { this.SetViewData("ZeroVisible", value); }
        }

        /// <summary>
        /// 当未有数据时,是否显示,默认为显示
        /// </summary>
        public virtual HorizontalAlign Align
        {
            get { return this.GetViewData<HorizontalAlign>("Align", HorizontalAlign.Center); }
            set { this.SetViewData("Align", value); }
        }

        ///// <summary>
        ///// 获取或设置默认配置按钮背景色
        ///// </summary>
        //public virtual System.Drawing.Color ButtonBackColor
        //{
        //    get { return this.GetViewData<System.Drawing.Color>("ButtonBackColor", System.Drawing.Color.White); }
        //    set { this.SetViewData("ButtonBackColor", value); }
        //}
        ///// <summary>
        ///// 获取或设置默认配置选中按钮背景色
        ///// </summary>
        //public virtual System.Drawing.Color ButtonSelectBackColor
        //{
        //    get { return this.GetViewData<System.Drawing.Color>("ButtonSelectBackColor", System.Drawing.ColorTranslator.FromHtml("#0099CC")); }
        //    set { this.SetViewData("ButtonSelectBackColor", value); }
        //}

        ///// <summary>
        ///// 获取或设置默认配置按钮字色
        ///// </summary>
        //public virtual System.Drawing.Color ButtonColor
        //{
        //    get { return this.GetViewData<System.Drawing.Color>("ButtonColor", System.Drawing.Color.Empty); }
        //    set { this.SetViewData("ButtonColor", value); }
        //}

        ///// <summary>
        ///// 获取或设置默认配置选中按钮字色
        ///// </summary>
        //public virtual System.Drawing.Color ButtonSelectColor
        //{
        //    get { return this.GetViewData<System.Drawing.Color>("ButtonColor", System.Drawing.Color.White); }
        //    set { this.SetViewData("ButtonColor", value); }
        //}

        ///// <summary>
        ///// 获取或设置默认配置按钮边框色
        ///// </summary>
        //public virtual System.Drawing.Color ButtonBorderColor
        //{
        //    get { return this.GetViewData<System.Drawing.Color>("ButtonBorderColor", System.Drawing.ColorTranslator.FromHtml("#336699")); }
        //    set { this.SetViewData("ButtonBorderColor", value); }
        //}
        

        /// <summary>
        /// 下一页铵钮
        /// </summary>
        public virtual string NextText
        {
            get { return this.GetViewData<string>("NextText", "&gt;"); }
            set { this.SetViewData("NextText", value); }
        }

        /// <summary>
        /// 上一页按钮
        /// </summary>
        public virtual string PreText
        {
            get { return this.GetViewData<string>("PreText", "&lt;"); }
            set { this.SetViewData("PreText", value); }
        }

        /// <summary>
        /// 尾页按钮
        /// </summary>
        public virtual string LastText
        {
            get { return this.GetViewData<string>("LastText", "&gt;&gt;"); }
            set { this.SetViewData("LastText", value); }
        }

        /// <summary>
        /// 首页按钮
        /// </summary>
        public virtual string FistText
        {
            get { return this.GetViewData<string>("FistText", "&lt;&lt;"); }
            set { this.SetViewData("FistText", value); }
        }

        ITemplate _Template;
        /// <summary>
        /// 获取或设置视图样式类
        /// </summary>
        public virtual ITemplate Template
        {
            get { return _Template; }
            set { _Template = value; }
        }

        /// <summary>
        /// 获取或设置<see cref="Template"/>属性设置中输出样式的序号,默认为 0
        /// </summary>
        public virtual int TemplateIndex
        {
            get { return this.GetViewData<int>("TemplateIndex", 0); }
            set { this.SetViewData("TemplateIndex", value); }
        }

        /// <summary>
        /// 是否输出<see cref="Template"/>所设置样式中的CSS样式表
        /// </summary>
        public virtual bool WriteStyle
        {
            get { return this.GetViewData<bool>("WriteStyle", true); }
            set { this.SetViewData("WriteStyle", value); }
        }

        /// <summary>
        /// 获取或设置<see cref="Template"/>所设置样式中的按钮数，默认显示0个
        /// </summary>
        public virtual int ButtonCount
        {
            get { return this.GetViewData<int>("ButtonCount", 0); }
            set { this.SetViewData("ButtonCount", value); }
        }

        /// <summary>
        /// 获取或设置客户端单击事件方法
        /// </summary>
        public virtual string OnClientClick
        {
            get { return this.GetViewData<string>("OnClientClick", ""); }
            set { this.SetViewData("OnClientClick", value); }
        }

        bool _IsCompute = false;
        /// <summary>
        /// 获取是否已进行计算
        /// </summary>
        public bool IsCompute
        {
            get { return _IsCompute; }
            private set { this._IsCompute = value; }
        }

        /// <summary>
        /// 进行数据处理计算
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


                //得到尾页//及最大面
                last = (count % size == 0) ? Convert.ToInt32(count / size) : Convert.ToInt32(count / size + 1);


                //判断当前页序是否正确
                if (index <= 1)
                {
                    index = 1;
                    up = 1;   //上页
                    start = 0;  //开始记录数
                    next = 2;
                }
                else
                {
                    if (index > last) index = last;
                    start = (index - 1) * size;  //开始索引

                    up = index - 1;  //上页
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


        #region 属性


        /// <summary>
        /// 获取或设置出现跳转操作的样式,0则不显示，默认不显示
        /// </summary>
        public virtual int JumpStyle
        {
            get { return this.GetViewData<int>("JumpStyle", 0); }
            set { this.SetViewData("JumpStyle", value); }
        }

        /// <summary>
        /// 获取或设置总记录数
        /// </summary>
        public virtual long Count
        {
            get { return this.GetViewData<long>("Count", 0); }
            set { this.SetViewData("Count", value); this.IsCompute = false; }
        }


        /// <summary>
        /// 获取或设置分页大小，默认为10
        /// </summary>
        public virtual int Size
        {
            get { return this.GetViewData<int>("Size", 10); }
            set { this.SetViewData("Size", value); this.IsCompute = false; }
        }

        int _Index = 0;

        /// <summary>
        /// 获取或设置当前页序
        /// </summary>
        public virtual int Index
        {
            get { return this._Index; }
            set { this._Index = value; this.IsCompute = false ; }
        }


        #endregion

        /// <summary>
        /// 创建URL链接
        /// </summary>
        /// <param name="index">索引,当为零时将以$PAGE$代替</param>
        public virtual string CreateLink(int index)
        {
            return "";
        }

        /// <summary>
        /// 重写始值，并自动判断是否通过数据计算，如果未有则进行数据计算<see cref="Compute"/>
        /// </summary>
        /// <param name="writer">对象</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.Write("\r\n");
            writer.WriteLine("<!--  Aooshi WebControl Start (Aooshi.Net Framework : http://www.aooshi.org/) -->");
            base.RenderBeginTag(writer);
        }

        /// <summary>
        /// 重写尾值
        /// </summary>
        /// <param name="writer">对象</param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
            writer.WriteLine("<!-- Aooshi WebControl End -->");
        }

        /// <summary>
        /// 输出控件内容
        /// </summary>
        /// <param name="writer">输出对象</param>
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
