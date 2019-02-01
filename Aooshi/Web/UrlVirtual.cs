using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using System.Text.RegularExpressions;

namespace Aooshi.Web
{
    /// <summary>
    /// Url虚拟化操作类
    /// </summary>
    public class UrlVirtual
    {
        /// <summary>
        /// 格式规范匹配正则
        /// </summary>
        protected static readonly Regex MatchRegex = new Regex(@"\{(.+?)\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        NameValueCollection Query = new NameValueCollection();
        NameValueCollection Make = null;


        /// <summary>
        /// 初始化类型
        /// </summary>
        /// <param name="urlcriterion">URL的格式规范,其中的中{}扩起来的字符代表参数名</param>
        /// <example>
        /// <code>
        /// new UrlVirtual("list-{p1}-{p2}-{p3}.html");
        /// </code>
        /// </example>
        public UrlVirtual(string urlcriterion)
        {
            this.UrlCriterion = urlcriterion;
        }

        string _UrlCriterion = "";
        /// <summary>
        /// 获取或设置URL规范,其中的中{}扩起来的字符代表参数名
        /// </summary>
        /// <example>
        /// <code>
        /// new UrlVirtual("list-{p1}-{p2}-{p3}.html");
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">属性值为空时引生</exception>
        public virtual string UrlCriterion
        {
            get
            {
                return _UrlCriterion;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("UrlCriterion");
                _UrlCriterion = value;
            }
        }


        /// <summary>
        /// 获取或设置URL参数值
        /// </summary>
        /// <param name="name">参数名称</param>
        public virtual string this[string name]
        {
            get { return this.Query[name]; }
            set{this.Query[name] = value;}
        }

        HttpContext _Context = null;

        /// <summary>
        /// 获取当前操作的HttpContext对象
        /// </summary>
        public virtual HttpContext Context
        {
            get
            {
                if (this._Context == null) this._Context = HttpContext.Current;
                return this._Context;
            }
        }

        bool _IsMatchingQuery;
        /// <summary>
        /// 获取或设置是否已调用了<see cref="MatchingQuery"/>
        /// </summary>
        protected virtual bool IsMatchingQuery
        {
            get { return _IsMatchingQuery; }
            set { this._IsMatchingQuery = value; }
        }

        /// <summary>
        /// 根据URL规范自动匹配现有网页查询
        /// </summary>
        public virtual void MatchingQuery()
        {
            if (this.IsMatchingQuery) return;
            NameValueCollection qs = this.Context.Request.QueryString;
            string name;
            foreach (Match m in UrlVirtual.MatchRegex.Matches(this.UrlCriterion))
            {
                if (m.Success)
                {
                    name = m.Groups[1].Value;
                    this[name] = qs[name];
                }
            }
            this.IsMatchingQuery = true;
        }

        /// <summary>
        /// 根据现有数据格式化一个URL地址
        /// </summary>
        /// <param name="name">格式化查询名</param>
        /// <param name="value">格式化查询值</param>
        public virtual string FormatUrl(string name, string value)
        {
            lock (this)
            {
                this.Make = new NameValueCollection(this.Query);
                if (string.IsNullOrEmpty(name))
                {
                    this.Make[name] = value;
                }
                return UrlVirtual.MatchRegex.Replace(this.UrlCriterion, new MatchEvaluator(FormatMatch));
            }
        }

        /// <summary>
        /// URL格式化时的替换规则
        /// </summary>
        /// <param name="m">匹配</param>
        protected virtual string FormatMatch(Match m)
        {
            return this.GetEncode(this.Make[m.Groups[1].Value]);
        }

        /// <summary>
        /// 获取值编码
        /// </summary>
        /// <param name="value">值</param>
        protected virtual string GetEncode(string value)
        {
            if (value != null) return this.Context.Server.UrlEncode(value);
            return "";
        }

        /// <summary>
        /// 获取生成的URL地址
        /// </summary>
        public override string ToString()
        {
            return this.FormatUrl(null, null);
        }
    }
}
