using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Ajax
{
    /// <summary>
    /// ������Ϊpublic����
    /// </summary>
    public class AjaxMethodNotPublic : Exception
    {
        /// <summary>
        /// ʵ�����쳣
        /// </summary>
        /// <param name="TypeName">��������</param>
        /// <param name="Name">��������</param>
        public AjaxMethodNotPublic(string TypeName, string Name) : base(string.Format("������'{0}'�еķ���'{1}'��Ϊ����������!; Type {0} is {1} Not set public;", TypeName, Name)) { }
    }
}
