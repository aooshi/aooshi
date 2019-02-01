using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;

namespace Aooshi.Web.Pagination
{
    /// <summary>
    /// 虚拟URL，Get分页控件
    /// </summary>
    [Description("Aooshi.Org 发布的用于ASP.Net Web应用程序的分页控件")]
    [ToolboxData("<{0}:VirtualPagination runat=server />")]
    public class VirtualPagination : GetPagination
    {
        UrlVirtual _UrlVirtual;
        /// <summary>
        /// 获取或设置虚拟化Url对象
        /// </summary>
        public virtual UrlVirtual UrlVirtual
        {
            get { return _UrlVirtual; }
            set { this._UrlVirtual = value; }
        }

        /// <summary>
        /// 控件导入
        /// </summary>
        /// <param name="e">事件</param>
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
        /// 创建Url地址
        /// </summary>
        /// <param name="index">索引</param>
        public override string CreateLink(int index)
        {
            if (index == 0)
                return this.UrlVirtual.FormatUrl(this.QueryNameIndex, "$PAGE$");

            return this.UrlVirtual.FormatUrl(this.QueryNameIndex, index.ToString());
        }
    }
}
