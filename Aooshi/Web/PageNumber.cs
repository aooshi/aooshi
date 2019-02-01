using System;
using System.Collections.Generic;
using System.Text;
using Aooshi.Web;

namespace Aooshi.Web
{
    /// <summary>
    /// 页序处理
    /// </summary>
    public class PageNumber
    {

        int totalrows;
        /// <summary>
        /// 获取或设置总数
        /// </summary>
        public int Totalrows
        {
            get { return totalrows; }
            set { totalrows = value; }
        }
        int pagesize;
        /// <summary>
        /// 获取或设置页大小
        /// </summary>
        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value; ; }
        }
        int pageindex;
        /// <summary>
        /// 获取或设置索引
        /// </summary>
        public int PageIndex
        {
            get { return pageindex; }
            set { pageindex = value; }
        }
        int lastindex;
        /// <summary>
        /// 获取或设置总页数
        /// </summary>
        public int LastIndex
        {
            get { return lastindex; }
            set { lastindex = value; }
        }
        int nextindex;
        /// <summary>
        /// 获取或设置下一页索引
        /// </summary>
        public int NextIndex
        {
            get { return nextindex; }
            set { nextindex = value; }
        }
        int preindex;
        /// <summary>
        /// 获取或设置上一页索引
        /// </summary>
        public int PrevIndex
        {
            get { return preindex; }
            set { preindex = value; }
        }

        long recordindex;
        /// <summary>
        /// 获取索引开始记录数
        /// </summary>
        public long RecordIndex
        {
            get { return recordindex; }
        }

        int btncount = 8;

        /// <summary>
        /// 获取或设置一个传递变量，该变量表示输出界面显示的按钮数，请注意，该值应该为偶数，默认为8
        /// </summary>
        public int BtnCount
        {
            get { return this.btncount;}
            set { this.btncount = value;}
        }

        int _Style = 0;
        /// <summary>
        /// 获取或设置一个传递变量，该变量表示输出界面显示的样式
        /// </summary>
        public int Style
        {
            get { return this._Style; }
            set { this._Style = value; }
        }

        int _GoStyle = 0;
        /// <summary>
        /// 获取或设置一个传递变量，该变量表示输出界面显示跳转样式
        /// </summary>
        public int GoStyle
        {
            get { return this._GoStyle; }
            set { this._GoStyle = value; }
        }

        bool _NoDataDisplay = false;

        /// <summary>
        /// 当未有数据时是否输出界面显示
        /// </summary>
        public bool NoDataDisplay
        {
            get { return this._NoDataDisplay; }
            set { this._NoDataDisplay = value; }
        }

        string _QueryNameIndex = "PageIndex"; 
        /// <summary>
        /// 获取或设置URL传递参数中页索引的名称，默认为: PageIndex
        /// </summary>
        public string QueryNameIndex
        {
            get { return this._QueryNameIndex; }
            set { this._QueryNameIndex = value; }
        }

        string _QueryNameTotalrows = "Totalrows";
        /// <summary>
        /// 获取或设置URL传递参数中总量的名称，默认为：Totalrows
        /// </summary>
        public string QueryNameTotalrows
        {
            get { return _QueryNameTotalrows; }
            set { _QueryNameTotalrows = value; }
        }


        /// <summary>
        /// 初始化新实例
        ///    --用法:
        ///    PageNumber pn = new PageNumber();
        ///    --数据处理
        ///    pn.DisposePageIndex('总数');
        ///    --获取值
        /// </summary>
        public PageNumber() : this(10, null,null) { }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="PageSize">页大小</param>
        public PageNumber(int PageSize) : this(PageSize, null,null) { }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="QueryNameIndex">查询串中索引串查询名,当为Null时默认为PageIndex</param>
        /// <param name="QueryNameTotalrows">查询串中总量串查询名,当为Null时默认为Totalrows</param>
        public PageNumber(int PageSize, string QueryNameIndex,string QueryNameTotalrows)
        {
            if (!string.IsNullOrEmpty(QueryNameIndex)) this.QueryNameIndex = QueryNameIndex;
            else this.QueryNameIndex = "PageIndex";

            if (!string.IsNullOrEmpty(QueryNameTotalrows)) this.QueryNameTotalrows = QueryNameTotalrows;
            else this.QueryNameTotalrows = "Totalrows";


            if (!int.TryParse(WebCommon.Query(this.QueryNameIndex), out pageindex)) this.pageindex = 1;
            if (!int.TryParse(WebCommon.Query(this.QueryNameTotalrows), out totalrows)) this.totalrows = 0;

            pagesize = PageSize;
        }

        string _link = "";

        /// <summary>
        /// 创建URL字符串
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="request">页请求</param>
        public string CreateLink(int index, System.Web.HttpRequest request)
        {
            return this.CreateLink(index, request, null);
        }

        /// <summary>
        /// 创建URL字符串
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="request">页请求</param>
        /// <param name="exclude">原始链接查询中的项排除</param>
        public string CreateLink(int index,System.Web.HttpRequest request,string[] exclude)
        {
            if (this._link == "")
            {
                this._link = string.Format("?{0}={1}",this.QueryNameTotalrows,this.totalrows);

                if (exclude == null) exclude = new string[] { };
                System.Collections.Specialized.NameValueCollection qs = request.QueryString;
                foreach(string key in qs.AllKeys)
                {
                    if (string.IsNullOrEmpty(key)) continue;
                    if (key == this.QueryNameIndex) continue;
                    if (key == this.QueryNameTotalrows) continue;
                    if (Array.IndexOf(exclude, key) != -1) continue;
                    this._link += string.Format("&{0}={1}",key,System.Web.HttpUtility.UrlEncode(qs[key] ?? ""));
                }
            }

            return this._link + string.Format("&{0}={1}",this.QueryNameIndex,index);
        }

        string _vlink = "";

        /// <summary>
        /// 根据传入的querys参数名数组创建一个虚拟URL地址
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="querys">要包含的查询组合</param>
        /// <param name="compart">分隔符，如： - 或 _ 等</param>
        /// <param name="request">查询对象</param>
        public string CreateLink(int index, string[] querys,string compart, System.Web.HttpRequest request)
        {
            if (_vlink == "")
            {
                System.Collections.Specialized.NameValueCollection qs = request.QueryString;
                foreach (string key in querys)
                {
                    if (key == this.QueryNameIndex)
                    {
                        _vlink += "{PageIndex}";
                        continue; 
                    }
                    _vlink += compart + System.Web.HttpUtility.UrlEncode(qs[key] ?? "");
                }

                if (_vlink != "")
                    _vlink = _vlink.Substring(1);
            }

            return _vlink.Replace("{PageIndex}",index.ToString());
        }


        /// <summary>
        /// 处理页序
        /// </summary>
        /// <param name="totalrows">总页数</param>
        public void DisposePageIndex(int totalrows)
        {
            lastindex = Convert.ToInt32(Math.Ceiling((double)totalrows / pagesize));

            if (pageindex < 1) pageindex = 1;
            if (pageindex > lastindex) pageindex = lastindex;

            nextindex = pageindex + 1;
            preindex = pageindex - 1;
            if (nextindex > lastindex) nextindex = lastindex;
            if (preindex < 1) preindex = 1;


            this.totalrows = totalrows;

            recordindex = (pageindex - 1) * pagesize;
        }


        /// <summary>
        /// 处理页序
        /// </summary>
        public void DisposePageIndex()
        {
            this.DisposePageIndex(this.totalrows);
        }
    }
}