using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;

namespace Aooshi.Web
{
    /// <summary>
    /// Json���������
    /// </summary>
    public class JsonOut
    {
        /// <summary>
        /// ����������ָ��message����
        /// </summary>
        /// <param name="success">�Ƿ�ִ�гɹ�</param>
        /// <param name="message">�����Ϣ</param>
        public static void Write(bool success, string message)
        {
            Hashtable h = new Hashtable();
            h.Add("success",success);
            h.Add("message", message);

            JsonOut.Write(h);

        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="success">�Ƿ�ִ�гɹ�</param>
        public static void Write(bool success)
        {
            JsonOut.Write(success, "");
        }

        /// <summary>
        /// ������������һ������,����ͨ��json��data���Ի�ȡ
        /// </summary>
        /// <param name="success">�Ƿ�ִ�гɹ�</param>
        /// <param name="data">��������</param>
        public static void Write(bool success, IList data)
        {
            Hashtable h = new Hashtable();
            h.Add("success", success);
            h.Add("message", "");
            h.Add("data", data);

            JsonOut.Write(h);
        }
        /// <summary>
        /// ������������һ������,����ͨ��json��data���Ի�ȡ
        /// </summary>
        /// <param name="success">�Ƿ�ִ�гɹ�</param>
        /// <param name="data">��������</param>
        public static void Write(bool success, string[] data)
        {
            Hashtable h = new Hashtable();
            h.Add("success", success);
            h.Add("message", "");
            h.Add("data", data);

            JsonOut.Write(h);
        }

        /// <summary>
        /// ������������һ������
        /// </summary>
        /// <param name="success">�Ƿ�ִ�гɹ�</param>
        /// <param name="appdate">����</param>
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
        /// ������
        /// </summary>
        /// <param name="data">����</param>
        private static void Write(Hashtable data)
        {
            HttpResponse r = HttpContext.Current.Response;
            r.Write(Json.Encode(data));
            r.End();
        }
    }
}
