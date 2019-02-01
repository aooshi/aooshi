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
    /// JavaScript Web 控件
    /// </summary>
     [ToolboxItem(false)]
    internal class AjaxScriptSrcControl : AjaxScriptControl
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public AjaxScriptSrcControl() : base() { }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="src">指定连接地址</param>
        public AjaxScriptSrcControl(string src) : base() {
            this.Src = src;
        }
    }
}
