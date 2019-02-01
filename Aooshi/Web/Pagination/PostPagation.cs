using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace Aooshi.Web.Pagination
{
    /// <summary>
    /// Post回发式分页控件
    /// </summary>
    [Description("Aooshi.Org 发布的用于ASP.Net Web应用程序的分页控件")]
    [ToolboxData("<{0}:PostPagination runat=server />")]
    public class PostPagination : PaginationBase, IPostBackEventHandler//, IPostBackDataHandler
    {
        //该控件的事件唯一标识符
        private static readonly object EventPageChanged = new object();
        /// <summary>
        /// 事件处理委托
        /// </summary>
        public delegate void PageChangedEventHandler(object src, PostEventArgs e);

        /// <summary>
        /// 当更改页序时发生
        /// </summary>
        public event PageChangedEventHandler PageChanged;

        /// <summary>
        /// 引发页更改事件
        /// </summary>
        /// <param name="e">事件数据</param>
        protected virtual void OnPageChanged(PostEventArgs e)
        {
            if (this.PageChanged != null)
                this.PageChanged(this, e);

        }

        /// <summary>
        /// 该值为定值，表示该组件为回发组件
        /// </summary>
        public override bool IsPost
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 引发Init事件
        /// </summary>
        /// <param name="e">事件数据</param>
        protected override void OnInit(EventArgs e)
        {
            base.Template = new DefaultTemplate(this);
            base.OnInit(e);
        }
        /// <summary>
        /// 创建链接
        /// </summary>
        /// <param name="index">链接索引</param>
        public override string CreateLink(int index)
        {
            string p = "$PAGE$";
            if (index > 0) p = index.ToString();
            return this.Page.ClientScript.GetPostBackClientHyperlink(this, p);         
        }

        /// <summary>
        /// 获取或设置当前页序
        /// </summary>
        public override int Index
        {
            get
            {
                return base.Index;
            }
            set
            {
                base.Index = value;
                this.ViewState["_Index"]= value; //存入状态存储中以便回发获取
            }
        }

        /// <summary>
        /// 读取上一状态存储中的数据
        /// </summary>
        /// <param name="savedState">状态对象</param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                base.LoadViewState(savedState);
                base.Index = (int)this.ViewState["_Index"];
            }
        }

        //#region IPostBackDataHandler 成员
        ///// <summary>
        ///// 处理返回的数据
        ///// </summary>
        ///// <param name="postDataKey">数据键值</param>
        ///// <param name="postCollection">数据集合</param>
        //public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        //{
        //    string s = p[postDataKey + "_CpNum"];
        //    if (s != null || s.Trim() != "")
        //    {
        //        this.GD(s);
        //        return true;
        //    }

        //    this.Index = 1;
        //    return false;
        //}
        ///// <summary>
        ///// 数据更改事件
        ///// </summary>
        //public void RaisePostDataChangedEvent()
        //{
        //    this.OnPageChanged(new PostEventArgs(base.Index));
        //}

        //#endregion

        #region IPostBackEventHandler 成员
        /// <summary>
        /// 回发事件处理
        /// </summary>
        /// <param name="eventArgument">事件数据</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            int index;
            if (!int.TryParse(eventArgument, out index)) index = 0;
            this.Index = index;
            this.Compute();
            this.OnPageChanged(new PostEventArgs(base.Index,this));
        }

        #endregion
    }
}
