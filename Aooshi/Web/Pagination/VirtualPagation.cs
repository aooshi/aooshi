using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace Aooshi.Web.Pagination
{
    /// <summary>
    /// ����URL��Get��ҳ�ؼ�
    /// </summary>
    [Description("Aooshi.Org ����������ASP.Net WebӦ�ó���ķ�ҳ�ؼ�")]
    [ToolboxData("<{0}:VirtualPagination runat=server />")]
    public class VirtualPagination : GetPagination
    {
        UrlVirtual _UrlVirtual;
        /// <summary>
        /// ��ȡ���������⻯Url����
        /// </summary>
        public virtual UrlVirtual UrlVirtual
        {
            get { return _UrlVirtual; }
            set { this._UrlVirtual = value; }
        }

        /// <summary>
        /// �ؼ�����
        /// </summary>
        /// <param name="e">�¼�</param>
        protected override void OnLoad(EventArgs e)
        {
            if (this.UrlVirtual == null)
            {
                throw new AooshiException("Not Set Property UrlVirtual.");
            }
            this.UrlVirtual.MatchingQuery();
            this.UrlVirtual[this.QueryNameIndex] = this.Index.ToString();
            this.UrlVirtual[this.QueryNameCount] = this.Count.ToString();
            base.OnLoad(e);
        }

        /// <summary>
        /// ����Url��ַ
        /// </summary>
        /// <param name="index">����</param>
        public override string CreateLink(int index)
        {
            if (index == 0)
                return this.UrlVirtual.FormatUrl(this.QueryNameIndex, "$PAGE$");

            return this.UrlVirtual.FormatUrl(this.QueryNameIndex, index.ToString());
        }
    }
}
