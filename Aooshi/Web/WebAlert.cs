using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Aooshi.Web
{
    /// <summary>
    /// JS��ʾ��Ϣ����,ע:�����з��������ڵ�����ʹҳ����ִ��
    /// </summary>
    public class WebAlert
    {
        /// <summary>
        /// ����ָ������Ϣ,��ʹ���ҳ����ת��ָ����URL
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        /// <param name="url">URL</param>
        /// <param name="msgFormat">��Ϣ��ʽ����</param>
        public static void TopAlert(string msg, string url,params object[] msgFormat)
        {
            if (msgFormat.Length > 0)
            {
                msg = string.Format(msg, msgFormat);
            }
            TopAlert(new string[] { msg }, url);
        }

        /// <summary>
        /// ����ָ������Ϣ,��ʹ���ҳ����ת��ָ����URL
        /// </summary>
        /// <param name="msg">��Ϣ</param>
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
        /// ����ָ������Ϣ,ʹ�ô˷������뽫ҳhead ���Ϊ runat=server
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        /// <param name="format">��ʽ����</param>
        public static void Alert(string msg,params object[] format)
        {
            if (format.Length > 0)
            {
                msg = string.Format(msg, format);
            }

            Alert(new string[] { msg });
        }

        /// <summary>
        /// ����ָ������Ϣ,ʹ�ô˷������뽫ҳhead ���Ϊ runat=server
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        public static void Alert(string[] msg)
        {
            JavaScript js = new JavaScript();
            js.AppendAlert(msg);
            Page pg = (Page)HttpContext.Current.Handler;
            if (pg == null || pg.Header == null) throw new Exception("δ��ҳhead���ע��Ϊ runat=server;");
            pg.Header.Controls.Add(js);
        }
        /// <summary>
        /// ����ָ������Ϣ,����ָ����URL��������ҳ����
        /// </summary>
        /// <param name="msg">��Ϣ</param>
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
        /// ����ָ������Ϣ,����ָ����URL��������ҳ����
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        /// <param name="url">URL</param>
        /// <param name="msgFormat">��ʽ����</param>
        public static void Alert(string msg, string url,params object[] msgFormat)
        {
            if (msgFormat.Length > 0)
            {
                msg = string.Format(msg, msgFormat);
            }
            Alert(new string[] { msg }, url);
        }



        /// <summary>
        /// ����ָ������Ϣ��������ҳ����
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        /// <param name="format">��ʽ����</param>
        public static void AlertEnd(string msg,params object[] format)
        {
            if (format.Length > 0)
            {
                msg = string.Format(msg, format);
            }

            AlertEnd(new string[] { msg });
        }

        /// <summary>
        /// ����ָ������Ϣ��������ҳ����
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        public static void AlertEnd(string[] msg)
        {
            JavaScript js = new JavaScript();
            js.AppendAlert(msg);
            HttpContext.Current.Response.Write(js.GetString());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// ����ָ����Ϣ��,������ҳ��������ҳ����
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        public static void AlertBack(string[] msg)
        {

            JavaScript js = new JavaScript();
            js.AppendAlert(msg);
            js.AppendBack(1);
            HttpContext.Current.Response.Write(js.GetString());
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// ����ָ����Ϣ��,������ҳ��������ҳ����
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        /// <param name="format">��ʽ����</param>
        public static void AlertBack(string msg,params object[] format)
        {
            if (format.Length > 0)
            {
                msg = string.Format(msg, format);
            }

            AlertBack(new string[] { msg });
        }
        /// <summary>
        /// ִ���ύ��Ϣ�鲢�رմ��壬������ҳ����
        /// </summary>
        /// <param name="msg">��Ϣ��</param>
        public static void AlertClose(string[] msg)
        {
            JavaScript js = new JavaScript();
            js.AppendAlert(msg);
            js.AppendWindowClose();
            HttpContext.Current.Response.Write(js.GetString());
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// ִ���ύ��Ϣ���رմ��壬������ҳ����
        /// </summary>
        /// <param name="msg">��Ϣ</param>
        /// <param name="format">��ʽ����</param>
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
