using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Reflection;

namespace Aooshi.Ajax
{
    /// <summary>
    /// ��һ���������ת��
    /// </summary>
    internal class AjaxMakeObject
    {
        internal const string headerName = "X-Aooshi.Ajax-NAME";
        internal const string headerType = "X-Aooshi.Ajax-TYPE";
            /// <summary>
        /// ����ָ���Ķ������鷵��һ��������
        /// </summary>
        /// <param name="arr">����</param>
        private static string MakeArray(object[] arr)
        {
            StringBuilder fun = new StringBuilder();
            object tmp;
            Type type;
            fun.AppendLine("new Array(");
            for (int i = 0; i < arr.Length; i++)
            {
                tmp = arr[i];
                type = tmp.GetType();
                if (tmp == null)
                    fun.AppendLine("null");
                else if (type.IsArray)
                {
                    fun.Append(MakeArray((object[])tmp));
                }
                else if (type.GetCustomAttributes(typeof(AjaxObject), true).Length > 0)  //����
                {
                    fun.Append("new ");  //�º�����
                    fun.Append(MakeFunction(type, tmp));
                }
                else
                {
                    fun.AppendFormat("unescape('{0}')", AjaxUtils.AjaxEncode(tmp.ToString()));
                }
                if (i == arr.Length - 1)  //������ʱ����Ҫ�ټ�,��ֹ�����һ�����������µĴ���
                    fun.AppendLine();
                else
                    fun.AppendLine(",");
            }
            //�������һ��
            fun.AppendLine("    )");
            return fun.ToString();
        }
        /// <summary>
        /// ����ָ�������Դ��õ�������
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="obj">����</param>
        private static string MakeFunction(Type type, object obj)
        {
            object tmp;
            Type tp;
            PropertyInfo[] pi = type.GetProperties();
            StringBuilder fun = new StringBuilder();
            fun.AppendLine("function(){");
            foreach (PropertyInfo p in pi)
            {
                if (!p.CanRead) continue;
                tmp = p.GetValue(obj, null);
                fun.AppendFormat("  this.{0} = ", p.Name);
                //��boolֵ�Ĵ���
                if (tmp == null)
                {
                    fun.Append("null");
                    fun.AppendLine(";");
                    continue;
                }
                tp = tmp.GetType();
                if (tp == typeof(Boolean))
                {
                    fun.Append(tmp.ToString().ToLower());
                    fun.AppendLine(";");
                    continue;
                }
                if (tp.IsArray)//����
                {
                    fun.Append(MakeArray((object[])tmp));
                    continue;
                }
                if (tp.GetCustomAttributes(typeof(AjaxObject), true).Length > 0)  //����
                {
                    fun.Append("new ");
                    fun.Append(MakeFunction(tp,tmp));
                    continue;
                }
                //�����������
                fun.AppendFormat("unescape('{0}')", AjaxUtils.AjaxEncode(tmp.ToString()));
                fun.AppendLine(";");
            }
            fun.AppendLine("}");
            return fun.ToString();
        }
        /// <summary>
        /// ���ͷ����
        /// </summary>
        /// <param name="wr">���������</param>
        /// <param name="Name">�������������,�����</param>
        /// <param name="type">�������������,�����</param>
        private static void WriteHeader(HttpResponse wr, string Name, HeaderType type)
        {
            if (!string.IsNullOrEmpty(Name))
                wr.AppendHeader(headerName, Name);
            if (type != HeaderType.Empty)
                wr.AppendHeader(headerType, type.ToString());
        }
        /// <summary>
        /// ����һ�����������
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="type">��������</param>
        /// <param name="wr">�������(ע�˷��������Զ��ر�������,��ע�����ʱ�ر�)</param>
        internal static void RunatClassObject(object obj, Type type, HttpResponse wr)
        {
            //���󷵻�
            wr.Clear();
            wr.AppendHeader("Content-Type", "application/x-javascript");
            string name = string.Format("AjaxClass_{0}",type.Name);
            StringBuilder fun = new StringBuilder();
            WriteHeader(wr, name, HeaderType.CLASS);
            fun.AppendFormat("var {0} = ", name);
            fun.Append(MakeFunction(type,obj));   //���뺯����
            wr.Write(fun.ToString());
            wr.End();
        }
        /// <summary>
        /// ����һ�������������
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="type">��������</param>
        /// <param name="wr">�������</param>
        internal static void RunatArrayObject(object obj, Type type, HttpResponse wr)
        { 
            //���󷵻�
            wr.Clear();
            wr.AppendHeader("Content-Type", "application/x-javascript");
            string name = string.Format("AjaxArray_{0}", type.Name.Replace("[]",""));
            StringBuilder fun = new StringBuilder();
            WriteHeader(wr, name, HeaderType.ARRAY);
            fun.AppendFormat("var {0} = ", name);
            fun.Append(MakeArray((object[])obj));   //���뺯����
            fun.AppendLine(";"); //�������鶨�����
            wr.Write(fun.ToString());
            wr.End();
        }

    }
}
