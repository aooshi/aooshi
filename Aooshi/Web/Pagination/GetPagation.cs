using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Collections.Specialized;

namespace Aooshi.Web.Pagination
{
    /// <summary>
    /// URL���η�ҳ�ؼ�
    /// </summary>
    [Description("Aooshi.Org ����������ASP.Net WebӦ�ó���ķ�ҳ�ؼ�")]
    [ToolboxData("<{0}:GetPagination runat=server />")]
    public class GetPagination : PaginationBase
    {
        /// <summary>
        /// ����Load�¼�
        /// </summary>
        /// <param name="e">�¼�����</param>
        protected override void OnInit(EventArgs e)
        {
            base.Template = new DefaultTemplate(this);
            this.FileName = System.IO.Path.GetFileName(this.Page.Request.PhysicalPath);
            this.GetQueryData();
            base.OnInit(e);
        }

        string _FileName;
        /// <summary>
        /// ��ȡ��ǰ������ļ�����
        /// </summary>
        public string FileName
        {
            get { return this._FileName; }
            private set { this._FileName = value; }
        }

        /// <summary>
        /// ��ȡ�����ò�ѯ����ҳ������ѯ����������ؼ�ʱ�뽫������Ϊ�ؼ��䲻��ͬ
        /// </summary>
        public string QueryNameIndex
        {
            get { return base.GetViewData<string>("QueryNameIndex", "PageIndex"); }
            set { base.SetViewData("QueryNameIndex", value); }
        }

        /// <summary>
        /// ��ȡ�����ò�ѯ��������������ѯ����������ؼ�ʱ�뽫��Ϊ�ؼ��䲻��ͬ
        /// </summary>
        public string QueryNameCount
        {
            get { return base.GetViewData<string>("QueryNameCount", "DataCount"); }
            set { base.SetViewData("QueryNameCount", value); }
        }

        /// <summary>
        /// ��ȡ�����ò�����Url���ݵĲ�ѯ��������������Զ��ŷָ�
        /// </summary>
        public string NoQueryNames
        {
            get { return base.GetViewData<string>("NoQueryNames",null); }
            set { base.SetViewData("NoQueryNames", value); }
        }

        /// <summary>
        /// ��ȡ��ǰURL�����ݵĲ���ֵ
        /// </summary>
        /// <returns>���ؼ�¼��</returns>
        protected virtual void GetQueryData()
        {
            if (this.Count == 0)
            {
                long count;
                if (!long.TryParse(this.Page.Request.QueryString[this.QueryNameCount], out count)) count = 0;
                base.Count = count;
            }

            if (this.Index == 0)
            {
                int index;
                if (!int.TryParse(this.Page.Request.QueryString[this.QueryNameIndex], out index)) index = 1;
                base.Index = index;
            }
        }

        NameValueCollection _AppendQuery;
        /// <summary>
        /// ��ȡ����ӵ��²�ѯ��ֵ
        /// </summary>
        protected virtual NameValueCollection AppendQuery
        {
            get { return _AppendQuery; }
            set { this._AppendQuery = value; }
        }
        /// <summary>
        /// ���һ���µĲ�ѯ��
        /// </summary>
        /// <param name="name">��ѯ������</param>
        /// <param name="value">ֵ</param>
        public virtual void AddQuery(string name, object value)
        {
            if (this.AppendQuery == null) this.AppendQuery = new NameValueCollection();
            this.AppendQuery[name]= Convert.ToString(value);
        }

        string _QueryString = null;

        /// <summary>
        /// ��ȡ�������µĲ�ѯ��
        /// </summary>
        protected virtual string QueryString
        {
            get
            {
                if (this._QueryString == null)
                {
                    this.CreateQueryString();
                }
                return this._QueryString;
            }
            set { this._QueryString = value; }
        }

        /// <summary>
        /// �����µĲ�ѯ��
        /// </summary>
        protected virtual void CreateQueryString()
        {
            NameValueCollection nvc = new NameValueCollection(this.Page.Request.QueryString);

            if (this.AppendQuery != null)
                nvc.Add(this.AppendQuery);

            nvc.Remove(this.QueryNameCount);
            nvc.Remove(this.QueryNameIndex);

            if (!string.IsNullOrEmpty(this.NoQueryNames))
            {
                foreach (string name in this.NoQueryNames.Split(','))
                {
                    nvc.Remove(name);
                }
            }

            string result = "";
            for (int i = 0, count = nvc.Count; i < count; i++)
            {
                if (string.IsNullOrEmpty(nvc.GetKey(i))) continue;
                result += string.Format("&{0}={1}",nvc.GetKey(i), this.Page.Server.UrlEncode(nvc[i]) );
            }

            if (result != "")
                result = result.Substring(1);

            this.QueryString = result;
        }

        /// <summary>
        /// ����һ���µĲ�ѯ����
        /// </summary>
        /// <param name="index">����</param>
        public override string CreateLink(int index)
        {
            string p = "$PAGE$";
            if (index > 0)
                p = index.ToString();

            if (this.QueryString == "")
            {
                return string.Format("{0}?{1}={2}&{3}={4}",this.FileName,this.QueryNameIndex,p,this.QueryNameCount,base.Count);
            }
            return string.Format("{0}?{1}&{2}={3}&{4}={5}", this.FileName, this.QueryString, this.QueryNameIndex, p, this.QueryNameCount, base.Count);
        }
    }
}