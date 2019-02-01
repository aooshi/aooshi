using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System.Security.Permissions;
using System.ComponentModel;

namespace Aooshi.Ajax
{
    /// <summary>
    /// JavaScript Web �ؼ�
    /// </summary>
     [ToolboxItem(false)]
    internal class AjaxScriptSrcControl : AjaxScriptControl
    {
        /// <summary>
        /// ��ʼ��
        /// </summary>
        public AjaxScriptSrcControl() : base() { }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="src">ָ�����ӵ�ַ</param>
        public AjaxScriptSrcControl(string src) : base() {
            this.Src = src;
        }
    }
}
