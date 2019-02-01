using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace Aooshi.Web
{
    /// <summary>
    /// 查询链接处理
    /// </summary>
    public class LinkQuery
    {
        //HttpContext context;
        //HttpRequest request;
        HttpServerUtility server;
        NameValueCollection query;
        Dictionary<string, string> dict;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context">对象</param>
        public LinkQuery(HttpContext context)
        {
            //this.context = context;
            this.server = context.Server;
            this.query = new NameValueCollection(context.Request.QueryString);
            this.dict = new Dictionary<string, string>();
        }

        /// <summary>
        /// 在当前实例中移除指定的项
        /// </summary>
        /// <param name="name">名称</param>
        public void Remove(string name)
        {
            this.query.Remove(name);
        }

        /// <summary>
        /// 在当前实例中移除指定的一组项
        /// </summary>
        /// <param name="names">名称数组</param>
        public void Remove(string[] names)
        {
            foreach (string name in names)
                this.query.Remove(name);
        }
        
        /// <summary>
        /// 获取链接
        /// </summary>
        /// <param name="name">链接名</param>
        /// <param name="value">链接当前值，该值可为<see cref="string.Empty"/></param>
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
        /// 获取链接
        /// </summary>
        /// <param name="name">查询名</param>
        /// <param name="value">查询当前值，该值可为<see cref="string.Empty"/></param>
        /// <param name="removes">要移除的一组查询名</param>
        public string GetLink(string name, object value, params string[] removes)
        {
            return this.GetLink(name, Convert.ToString(value), removes);
        }


        /// <summary>
        /// 获取链接
        /// </summary>
        /// <param name="name">查询名</param>
        /// <param name="value">查询当前值，该值可为<see cref="string.Empty"/></param>
        /// <param name="removes">要移除的一组查询名</param>
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
        /// 获取链接
        /// </summary>
        /// <param name="name">链接名</param>
        /// <param name="value">链接当前值，该值可为Null</param>
        public string GetLink(string name,object value)
        {
            return this.GetLink(name,Convert.ToString(value));
        }

        /// <summary>
        /// 获取指定请求中所有请求串
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="remove">要排除的项</param>
        public static string GetRmoveLink(HttpRequest request, string remove)
        {
            return LinkQuery.GetRmoveLink(request.QueryString, remove);
        }

        /// <summary>
        /// 获取查询中所有请求串
        /// </summary>
        /// <param name="query">查询串</param>
        /// <param name="remove">要排除的项</param>
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
