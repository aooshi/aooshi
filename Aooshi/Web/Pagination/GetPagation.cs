using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Collections.Specialized;

namespace Aooshi.Web.Pagination
{
    /// <summary>
    /// URL传参分页控件
    /// </summary>
    [Description("Aooshi.Org 发布的用于ASP.Net Web应用程序的分页控件")]
    [ToolboxData("<{0}:GetPagination runat=server />")]
    public class GetPagination : PaginationBase
    {
        /// <summary>
        /// 引发Load事件
        /// </summary>
        /// <param name="e">事件数据</param>
        protected override void OnInit(EventArgs e)
        {
            base.Template = new DefaultTemplate(this);
            this.FileName = System.IO.Path.GetFileName(this.Page.Request.PhysicalPath);
            this.GetQueryData();
            base.OnInit(e);
        }

        string _FileName;
        /// <summary>
        /// 获取当前请求的文件名称
        /// </summary>
        public string FileName
        {
            get { return this._FileName; }
            private set { this._FileName = value; }
        }

        /// <summary>
        /// 获取或设置查询串中页索引查询名，当多个控件时请将此设置为控件间不相同
        /// </summary>
        public string QueryNameIndex
        {
            get { return base.GetViewData<string>("QueryNameIndex", "PageIndex"); }
            set { base.SetViewData("QueryNameIndex", value); }
        }

        /// <summary>
        /// 获取或设置查询串中数据总量查询名，当多个控件时请将此为控件间不相同
        /// </summary>
        public string QueryNameCount
        {
            get { return base.GetViewData<string>("QueryNameCount", "DataCount"); }
            set { base.SetViewData("QueryNameCount", value); }
        }

        /// <summary>
        /// 获取或设置不进行Url传递的查询参数，多个参数以逗号分隔
        /// </summary>
        public string NoQueryNames
        {
            get { return base.GetViewData<string>("NoQueryNames",null); }
            set { base.SetViewData("NoQueryNames", value); }
        }

        /// <summary>
        /// 获取当前URL所传递的参数值
        /// </summary>
        /// <returns>返回记录数</returns>
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
        /// 获取所添加的新查询串值
        /// </summary>
        protected virtual NameValueCollection AppendQuery
        {
            get { return _AppendQuery; }
            set { this._AppendQuery = value; }
        }
        /// <summary>
        /// 添加一个新的查询串
        /// </summary>
        /// <param name="name">查询串名称</param>
        /// <param name="value">值</param>
        public virtual void AddQuery(string name, object value)
        {
            if (this.AppendQuery == null) this.AppendQuery = new NameValueCollection();
            this.AppendQuery[name]= Convert.ToString(value);
        }

        string _QueryString = null;

        /// <summary>
        /// 获取或设置新的查询串
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
        /// 创建新的查询串
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
        /// 创建一个新的查询链接
        /// </summary>
        /// <param name="index">索引</param>
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