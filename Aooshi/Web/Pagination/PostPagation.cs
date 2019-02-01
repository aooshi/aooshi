using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace Aooshi.Web.Pagination
{
    /// <summary>
    /// Post�ط�ʽ��ҳ�ؼ�
    /// </summary>
    [Description("Aooshi.Org ����������ASP.Net WebӦ�ó���ķ�ҳ�ؼ�")]
    [ToolboxData("<{0}:PostPagination runat=server />")]
    public class PostPagination : PaginationBase, IPostBackEventHandler//, IPostBackDataHandler
    {
        //�ÿؼ����¼�Ψһ��ʶ��
        private static readonly object EventPageChanged = new object();
        /// <summary>
        /// �¼�����ί��
        /// </summary>
        public delegate void PageChangedEventHandler(object src, PostEventArgs e);

        /// <summary>
        /// ������ҳ��ʱ����
        /// </summary>
        public event PageChangedEventHandler PageChanged;

        /// <summary>
        /// ����ҳ�����¼�
        /// </summary>
        /// <param name="e">�¼�����</param>
        protected virtual void OnPageChanged(PostEventArgs e)
        {
            if (this.PageChanged != null)
                this.PageChanged(this, e);

        }

        /// <summary>
        /// ��ֵΪ��ֵ����ʾ�����Ϊ�ط����
        /// </summary>
        public override bool IsPost
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// ����Init�¼�
        /// </summary>
        /// <param name="e">�¼�����</param>
        protected override void OnInit(EventArgs e)
        {
            base.Template = new DefaultTemplate(this);
            base.OnInit(e);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="index">��������</param>
        public override string CreateLink(int index)
        {
            string p = "$PAGE$";
            if (index > 0) p = index.ToString();
            return this.Page.ClientScript.GetPostBackClientHyperlink(this, p);         
        }

        /// <summary>
        /// ��ȡ�����õ�ǰҳ��
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
                this.ViewState["_Index"]= value; //����״̬�洢���Ա�ط���ȡ
            }
        }

        /// <summary>
        /// ��ȡ��һ״̬�洢�е�����
        /// </summary>
        /// <param name="savedState">״̬����</param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                base.LoadViewState(savedState);
                base.Index = (int)this.ViewState["_Index"];
            }
        }

        //#region IPostBackDataHandler ��Ա
        ///// <summary>
        ///// �����ص�����
        ///// </summary>
        ///// <param name="postDataKey">���ݼ�ֵ</param>
        ///// <param name="postCollection">���ݼ���</param>
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
        ///// ���ݸ����¼�
        ///// </summary>
        //public void RaisePostDataChangedEvent()
        //{
        //    this.OnPageChanged(new PostEventArgs(base.Index));
        //}

        //#endregion

        #region IPostBackEventHandler ��Ա
        /// <summary>
        /// �ط��¼�����
        /// </summary>
        /// <param name="eventArgument">�¼�����</param>
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
