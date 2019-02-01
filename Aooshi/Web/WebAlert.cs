using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Aooshi.Web
{
    /// <summary>
    /// JS提示信息操作,注:该类中方法将会在弹出后使页结束执行
    /// </summary>
    public class WebAlert
    {
        /// <summary>
        /// 弹出指定的消息,并使框架页所有转向指定的URL
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="url">URL</param>
        /// <param name="msgFormat">消息格式化串</param>
        public static void TopAlert(string msg, string url,params object[] msgFormat)
        {
            if (msgFormat.Length > 0)
            {
                msg = string.Format(msg, msgFormat);
            }
            TopAlert(new string[] { msg }, url);
        }

        /// <summary>
        /// 弹出指定的消息,并使框架页所有转向指定的URL
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="url">URL</param>
        public static void TopAlert(string[] msg, string url)
        {
            url = url ?? "";
            JavaScript js = new JavaScript();
            js.AppendAlert(msg);
            js.AppendLine(string.Format("top.location.href='{0}';",url.Replace("'","\\'") ));
            HttpContext.Current.Response.Write(js.GetString());
            HttpContext.Current.Response.End();
        }


        /// <summary>
        /// 弹出指定的消息,使用此方法必须将页head 标记为 runat=server
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="format">格式化串</param>
        public static void Alert(string msg,params object[] format)
        {
            if (format.Length > 0)
            {
                msg = string.Format(msg, format);
            }

            Alert(new string[] { msg });
        }

        /// <summary>
        /// 弹出指定的消息,使用此方法必须将页head 标记为 runat=server
        /// </summary>
        /// <param name="msg">消息</param>
        public static void Alert(string[] msg)
        {
            JavaScript js = new JavaScript();
            js.AppendAlert(msg);
            Page pg = (Page)HttpContext.Current.Handler;
            if (pg == null || pg.Header == null) throw new Exception("未将页head标记注册为 runat=server;");
            pg.Header.Controls.Add(js);
        }
        /// <summary>
        /// 弹出指定的消息,返回指定的URL，并结束页运行
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="url">URL</param>
        public static void Alert(string[] msg, string url)
        {
            JavaScript js = new JavaScript();
            js.AppendAlert(msg);
            js.AppendGoUrl(url);
            HttpContext.Current.Response.Write(js.GetString());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出指定的消息,返回指定的URL，并结束页运行
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="url">URL</param>
        /// <param name="msgFormat">格式化串</param>
        public static void Alert(string msg, string url,params object[] msgFormat)
        {
            if (msgFormat.Length > 0)
            {
                msg = string.Format(msg, msgFormat);
            }
            Alert(new string[] { msg }, url);
        }



        /// <summary>
        /// 弹出指定的消息，并结束页运行
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="format">格式化串</param>
        public static void AlertEnd(string msg,params object[] format)
        {
            if (format.Length > 0)
            {
                msg = string.Format(msg, format);
            }

            AlertEnd(new string[] { msg });
        }

        /// <summary>
        /// 弹出指定的消息，并结束页运行
        /// </summary>
        /// <param name="msg">消息</param>
        public static void AlertEnd(string[] msg)
        {
            JavaScript js = new JavaScript();
            js.AppendAlert(msg);
            HttpContext.Current.Response.Write(js.GetString());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出指定消息后,返回上页，并结束页运行
        /// </summary>
        /// <param name="msg">消息</param>
        public static void AlertBack(string[] msg)
        {

            JavaScript js = new JavaScript();
            js.AppendAlert(msg);
            js.AppendBack(1);
            HttpContext.Current.Response.Write(js.GetString());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出指定消息后,返回上页，并结束页运行
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="format">格式化串</param>
        public static void AlertBack(string msg,params object[] format)
        {
            if (format.Length > 0)
            {
                msg = string.Format(msg, format);
            }

            AlertBack(new string[] { msg });
        }
        /// <summary>
        /// 执行提交信息组并关闭窗体，并结束页运行
        /// </summary>
        /// <param name="msg">信息组</param>
        public static void AlertClose(string[] msg)
        {
            JavaScript js = new JavaScript();
            js.AppendAlert(msg);
            js.AppendWindowClose();
            HttpContext.Current.Response.Write(js.GetString());
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 执行提交信息并关闭窗体，并结束页运行
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="format">格式化串</param>
        public static void AlertClose(string msg,params object[] format)
        {
            if (format.Length > 0)
            {
                msg = string.Format(msg, format);
            }

            AlertClose(new string[] { msg });
        }
    }
}
