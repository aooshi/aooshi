using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Aooshi.Web
{
    /// <summary>
    /// ��ҳ������
    /// </summary>
    public class WebError
    {
        /// <summary>
        /// ���󱨸�����
        /// </summary>
        public enum ReportType
        {
            /// <summary>
            /// ��ӡ������Ϣ
            /// </summary>
            Error,
            
            /// <summary>
            /// ��ӡ��ʾ��Ϣ
            /// </summary>
            Message
        }
        
        /// <summary>
        /// ��ӡ��ҳ���󱨸�
        /// </summary>
        /// <param name="page">ִ�в�������ҳ</param>
        public static void RegisterReport(Page page)
        {
            WebError.RegisterReport(page, ReportType.Error);
        }
        /// <summary>
        /// ��ӡ��ҳ���󱨸�
        /// </summary>
        /// <param name="page">ִ�в�������ҳ</param>
        /// <param name="type">����</param>
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
