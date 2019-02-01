using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;

namespace Aooshi.Web
{
    /// <summary>
    /// Json输出方法集
    /// </summary>
    public class JsonOut
    {
        /// <summary>
        /// 输出结果，并指定message属性
        /// </summary>
        /// <param name="success">是否执行成功</param>
        /// <param name="message">相关消息</param>
        public static void Write(bool success, string message)
        {
            Hashtable h = new Hashtable();
            h.Add("success",success);
            h.Add("message", message);

            JsonOut.Write(h);

        }

        /// <summary>
        /// 输出结果
        /// </summary>
        /// <param name="success">是否执行成功</param>
        public static void Write(bool success)
        {
            JsonOut.Write(success, "");
        }

        /// <summary>
        /// 输出结果并附加一组数组,数组通过json的data属性获取
        /// </summary>
        /// <param name="success">是否执行成功</param>
        /// <param name="data">数组数据</param>
        public static void Write(bool success, IList data)
        {
            Hashtable h = new Hashtable();
            h.Add("success", success);
            h.Add("message", "");
            h.Add("data", data);

            JsonOut.Write(h);
        }
        /// <summary>
        /// 输出结果并附加一组数组,数组通过json的data属性获取
        /// </summary>
        /// <param name="success">是否执行成功</param>
        /// <param name="data">数组数据</param>
        public static void Write(bool success, string[] data)
        {
            Hashtable h = new Hashtable();
            h.Add("success", success);
            h.Add("message", "");
            h.Add("data", data);

            JsonOut.Write(h);
        }

        /// <summary>
        /// 输出结果并附加一组数据
        /// </summary>
        /// <param name="success">是否执行成功</param>
        /// <param name="appdate">数据</param>
        public static void Write(bool success, Hashtable appdate)
        {
            Hashtable h = new Hashtable();
            h.Add("success", success);
            h.Add("message", "");

            IDictionaryEnumerator e = appdate.GetEnumerator();
            while (e.MoveNext())
            {
                h[e.Key] = e.Value;
            }

            JsonOut.Write(h);
            
        }

        /// <summary>
        /// 输出结果
        /// </summary>
        /// <param name="data">数据</param>
        private static void Write(Hashtable data)
        {
            HttpResponse r = HttpContext.Current.Response;
            r.Write(Json.Encode(data));
            r.End();
        }
    }
}
