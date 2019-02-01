using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace Aooshi.Web
{
    /// <summary>
    /// ��ѯ���Ӵ���
    /// </summary>
    public class LinkQuery
    {
        //HttpContext context;
        //HttpRequest request;
        HttpServerUtility server;
        NameValueCollection query;
        Dictionary<string, string> dict;
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="context">����</param>
        public LinkQuery(HttpContext context)
        {
            //this.context = context;
            this.server = context.Server;
            this.query = new NameValueCollection(context.Request.QueryString);
            this.dict = new Dictionary<string, string>();
        }

        /// <summary>
        /// �ڵ�ǰʵ�����Ƴ�ָ������
        /// </summary>
        /// <param name="name">����</param>
        public void Remove(string name)
        {
            this.query.Remove(name);
        }

        /// <summary>
        /// �ڵ�ǰʵ�����Ƴ�ָ����һ����
        /// </summary>
        /// <param name="names">��������</param>
        public void Remove(string[] names)
        {
            foreach (string name in names)
                this.query.Remove(name);
        }
        
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">���ӵ�ǰֵ����ֵ��Ϊ<see cref="string.Empty"/></param>
        public string GetLink(string name, string value)
        {
            string result;
            if (!dict.TryGetValue(name, out result))
            {
                result = LinkQuery.GetRmoveLink(this.query, name);
                dict.Add(name, result);
            }

            if (!string.IsNullOrEmpty(value))
                value = this.server.UrlEncode(value);

            if (string.IsNullOrEmpty(result))
                return string.Format("?{0}={1}", name, value);

            return result + string.Format("&{0}={1}", name, value);
        }



        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="name">��ѯ��</param>
        /// <param name="value">��ѯ��ǰֵ����ֵ��Ϊ<see cref="string.Empty"/></param>
        /// <param name="removes">Ҫ�Ƴ���һ���ѯ��</param>
        public string GetLink(string name, object value, params string[] removes)
        {
            return this.GetLink(name, Convert.ToString(value), removes);
        }


        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="name">��ѯ��</param>
        /// <param name="value">��ѯ��ǰֵ����ֵ��Ϊ<see cref="string.Empty"/></param>
        /// <param name="removes">Ҫ�Ƴ���һ���ѯ��</param>
        public string GetLink(string name, string value,params string[] removes)
        {
            NameValueCollection query = new NameValueCollection(this.query);
            foreach (string n in removes)
                query.Remove(n);

            string result = LinkQuery.GetRmoveLink(query, name);
                
            if (!string.IsNullOrEmpty(value))
                value = this.server.UrlEncode(value);

            if (string.IsNullOrEmpty(result))
                return string.Format("?{0}={1}", name, value);

            return result + string.Format("&{0}={1}", name, value);
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">���ӵ�ǰֵ����ֵ��ΪNull</param>
        public string GetLink(string name,object value)
        {
            return this.GetLink(name,Convert.ToString(value));
        }

        /// <summary>
        /// ��ȡָ����������������
        /// </summary>
        /// <param name="request">�������</param>
        /// <param name="remove">Ҫ�ų�����</param>
        public static string GetRmoveLink(HttpRequest request, string remove)
        {
            return LinkQuery.GetRmoveLink(request.QueryString, remove);
        }

        /// <summary>
        /// ��ȡ��ѯ����������
        /// </summary>
        /// <param name="query">��ѯ��</param>
        /// <param name="remove">Ҫ�ų�����</param>
        public static string GetRmoveLink(NameValueCollection query, string remove)
        {
            string result = "";

            foreach (string n in query)
            {
                if (n == remove) continue;
                result += string.Format("&{0}={1}", n, System.Web.HttpUtility.UrlEncode(query[n]));
            }

            if (result != "")
                result = "?" + result.Remove(0, 1);

            return result;
        }
    }
}
