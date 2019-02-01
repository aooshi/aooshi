using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Aooshi.Web
{
    /// <summary>
    /// 网页错误处理
    /// </summary>
    public class WebError
    {
        /// <summary>
        /// 错误报告类型
        /// </summary>
        public enum ReportType
        {
            /// <summary>
            /// 打印所有消息
            /// </summary>
            Error,
            
            /// <summary>
            /// 打印提示信息
            /// </summary>
            Message
        }
        
        /// <summary>
        /// 打印网页错误报告
        /// </summary>
        /// <param name="page">执行操作的网页</param>
        public static void RegisterReport(Page page)
        {
            WebError.RegisterReport(page, ReportType.Error);
        }
        /// <summary>
        /// 打印网页错误报告
        /// </summary>
        /// <param name="page">执行操作的网页</param>
        /// <param name="type">类型</param>
        public static void RegisterReport(Page page,ReportType type)
        {
            switch (type)
            {
                case ReportType.Message:
                   page.Error += new EventHandler(Page_ErrorReport_Message);
                    break;
                case ReportType.Error:
                    page.Error += new EventHandler(Page_ErrorReport);
                    break;
            }
        }


        /// <summary>
        /// error report message
        /// </summary>
        /// <param name="sender">page</param>
        /// <param name="e">event</param>
        private static void Page_ErrorReport_Message(object sender, EventArgs e)
        {
            Page page = (Page)sender;
            page.Response.Clear();
            page.Response.Write(AR.WebPageErrorReport.Replace("$ERROR", page.Server.GetLastError().Message));

            page.Server.ClearError();
            page.Response.End();
        }

        /// <summary>
        /// error report
        /// </summary>
        /// <param name="sender">page</param>
        /// <param name="e">event</param>
        private static void Page_ErrorReport(object sender, EventArgs e)
        {
            Page page = (Page)sender;
            page.Response.Clear();
            page.Response.Write(AR.WebPageErrorReport.Replace("$ERROR", page.Server.GetLastError().ToString()));

            page.Server.ClearError();
            page.Response.End();
        }

    }
}
