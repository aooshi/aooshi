using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Reflection;

namespace Aooshi.Ajax
{
    /// <summary>
    /// 将一个对象进行转换
    /// </summary>
    internal class AjaxMakeObject
    {
        internal const string headerName = "X-Aooshi.Ajax-NAME";
        internal const string headerType = "X-Aooshi.Ajax-TYPE";
            /// <summary>
        /// 根据指定的对象数组返回一个数组体
        /// </summary>
        /// <param name="arr">数组</param>
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
                else if (type.GetCustomAttributes(typeof(AjaxObject), true).Length > 0)  //对象
                {
                    fun.Append("new ");  //新涵数体
                    fun.Append(MakeFunction(type, tmp));
                }
                else
                {
                    fun.AppendFormat("unescape('{0}')", AjaxUtils.AjaxEncode(tmp.ToString()));
                }
                if (i == arr.Length - 1)  //当等于时不须要再加,防止因最后一个增加所导致的错误
                    fun.AppendLine();
                else
                    fun.AppendLine(",");
            }
            //加上最后一个
            fun.AppendLine("    )");
            return fun.ToString();
        }
        /// <summary>
        /// 根据指定的属性串得到函数体
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
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
                //对bool值的处理
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
                if (tp.IsArray)//数组
                {
                    fun.Append(MakeArray((object[])tmp));
                    continue;
                }
                if (tp.GetCustomAttributes(typeof(AjaxObject), true).Length > 0)  //对象
                {
                    fun.Append("new ");
                    fun.Append(MakeFunction(tp,tmp));
                    continue;
                }
                //以外所有输出
                fun.AppendFormat("unescape('{0}')", AjaxUtils.AjaxEncode(tmp.ToString()));
                fun.AppendLine(";");
            }
            fun.AppendLine("}");
            return fun.ToString();
        }
        /// <summary>
        /// 输出头内容
        /// </summary>
        /// <param name="wr">流输出对象</param>
        /// <param name="Name">输出的内容名称,如果有</param>
        /// <param name="type">输出的内容类型,如果有</param>
        private static void WriteHeader(HttpResponse wr, string Name, HeaderType type)
        {
            if (!string.IsNullOrEmpty(Name))
                wr.AppendHeader(headerName, Name);
            if (type != HeaderType.Empty)
                wr.AppendHeader(headerType, type.ToString());
        }
        /// <summary>
        /// 处理一个类对象数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="type">对象类型</param>
        /// <param name="wr">输出对象(注此方法不会自动关闭流对象,请注意调用时关闭)</param>
        internal static void RunatClassObject(object obj, Type type, HttpResponse wr)
        {
            //对象返回
            wr.Clear();
            wr.AppendHeader("Content-Type", "application/x-javascript");
            string name = string.Format("AjaxClass_{0}",type.Name);
            StringBuilder fun = new StringBuilder();
            WriteHeader(wr, name, HeaderType.CLASS);
            fun.AppendFormat("var {0} = ", name);
            fun.Append(MakeFunction(type,obj));   //加入函数体
            wr.Write(fun.ToString());
            wr.End();
        }
        /// <summary>
        /// 处理一个数组对象数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="type">对象类型</param>
        /// <param name="wr">输出对象</param>
        internal static void RunatArrayObject(object obj, Type type, HttpResponse wr)
        { 
            //对象返回
            wr.Clear();
            wr.AppendHeader("Content-Type", "application/x-javascript");
            string name = string.Format("AjaxArray_{0}", type.Name.Replace("[]",""));
            StringBuilder fun = new StringBuilder();
            WriteHeader(wr, name, HeaderType.ARRAY);
            fun.AppendFormat("var {0} = ", name);
            fun.Append(MakeArray((object[])obj));   //加入函数体
            fun.AppendLine(";"); //增加数组定义结束
            wr.Write(fun.ToString());
            wr.End();
        }

    }
}
