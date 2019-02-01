using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Ajax
{
    /// <summary>
    /// 页面文件未注册为runat=server
    /// </summary>
    public class AjaxHeadNotServer : Exception
    {
        internal AjaxHeadNotServer() : base("页未将Head标记注册为runat=server;Page Head not Runat=server;") { }
    }
}
