using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using System.Text.RegularExpressions;

namespace Aooshi.Web
{
    /// <summary>
    /// Url���⻯������
    /// </summary>
    public class UrlVirtual
    {
        /// <summary>
        /// ��ʽ�淶ƥ������
        /// </summary>
        protected static readonly Regex MatchRegex = new Regex(@"\{(.+?)\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        NameValueCollection Query = new NameValueCollection();
        NameValueCollection Make = null;


        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="urlcriterion">URL�ĸ�ʽ�淶,���е���{}���������ַ����������</param>
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
        /// ��ȡ������URL�淶,���е���{}���������ַ����������
        /// </summary>
        /// <example>
        /// <code>
        /// new UrlVirtual("list-{p1}-{p2}-{p3}.html");
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">����ֵΪ��ʱ����</exception>
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
        /// ��ȡ������URL����ֵ
        /// </summary>
        /// <param name="name">��������</param>
        public virtual string this[string name]
        {
            get { return this.Query[name]; }
            set{this.Query[name] = value;}
        }

        HttpContext _Context = null;

        /// <summary>
        /// ��ȡ��ǰ������HttpContext����
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
        /// ��ȡ�������Ƿ��ѵ�����<see cref="MatchingQuery"/>
        /// </summary>
        protected virtual bool IsMatchingQuery
        {
            get { return _IsMatchingQuery; }
            set { this._IsMatchingQuery = value; }
        }

        /// <summary>
        /// ����URL�淶�Զ�ƥ��������ҳ��ѯ
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
        /// �����������ݸ�ʽ��һ��URL��ַ
        /// </summary>
        /// <param name="name">��ʽ����ѯ��</param>
        /// <param name="value">��ʽ����ѯֵ</param>
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
        /// URL��ʽ��ʱ���滻����
        /// </summary>
        /// <param name="m">ƥ��</param>
        protected virtual string FormatMatch(Match m)
        {
            return this.GetEncode(this.Make[m.Groups[1].Value]);
        }

        /// <summary>
        /// ��ȡֵ����
        /// </summary>
        /// <param name="value">ֵ</param>
        protected virtual string GetEncode(string value)
        {
            if (value != null) return this.Context.Server.UrlEncode(value);
            return "";
        }

        /// <summary>
        /// ��ȡ���ɵ�URL��ַ
        /// </summary>
        public override string ToString()
        {
            return this.FormatUrl(null, null);
        }
    }
}
