using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Ajax
{
    /// <summary>
    /// ҳ���ļ�δע��Ϊrunat=server
    /// </summary>
    public class AjaxHeadNotServer : Exception
    {
        internal AjaxHeadNotServer() : base("ҳδ��Head���ע��Ϊrunat=server;Page Head not Runat=server;") { }
    }
}
