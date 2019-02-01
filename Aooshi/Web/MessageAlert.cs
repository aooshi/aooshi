using System;
using System.Web;


namespace Aooshi.Web
{
    /// <summary>
    /// 一个静态页面提示信息类
    /// </summary>
    public class MessageAlert
    {
        /// <summary>
        /// 提示信息处理,显示返回上一页按钮
        /// </summary>
        /// <param name="Msg">要进行提示信息</param>
        public static void Message(string Msg)
        {
            MessageAlert.Message(new string[] { Msg }, "");
        }

        /// <summary>
        /// 提示信息处理,显示多个信息项,显示返回上一页按钮
        /// </summary>
        /// <param name="Msgs">要进行提示信息数组</param>
        public static void Message(string[] Msgs)
        {
            MessageAlert.Message(Msgs, "");
        }

        /// <summary>
        /// 进行信息提示并转向指定的URL地址
        /// </summary>
        /// <param name="Msg">要显示的提示信息</param>
        /// <param name="Url">要转向的URL</param>
        public static void Message(string Msg, string Url)
        {
            MessageAlert.Message(new string[] { Msg }, Url);
        }

        /// <summary>
        /// 进行信息提示,并指定多个信息项,显示完成后转向指定的URL
        /// </summary>
        /// <param name="Msgs">要进行显示的提示信息数组</param>
        /// <param name="Url">要转向的URL</param>
        public static void Message(string[] Msgs, string Url)
        {
            Message Wm = new Message();

            foreach (string msg in Msgs) Wm.Add(msg);

            if (!string.IsNullOrEmpty(Url))
            {
                Wm.Jump = Url;
                Wm.Back = false;
            }

            HttpResponse Re = HttpContext.Current.Response;
            //信息写出
            Re.Clear();
            Re.Write(Wm.ToString());
            Re.End();
        }


        /// <summary>
        /// 显示提示信息并关闭浏览器
        /// </summary>
        /// <param name="Msg">要显示的信息</param>
        public static void Close(string Msg)
        {
            MessageAlert.Close(new string[] { Msg });
        }

        /// <summary>
        /// 显示信息组并关闭浏览器
        /// </summary>
        /// <param name="Msgs">要显示的信息组</param>
        public static void Close(string[] Msgs)
        {
            Message Wm = new Message();

            foreach (string msg in Msgs) Wm.Add(msg);

            Wm.Back = false;
            Wm.Close = true;

            //信息写出
            HttpResponse Re = HttpContext.Current.Response;
            Re.Clear();
            Re.Write(Wm.ToString());
            Re.End();
        }
    }
}
