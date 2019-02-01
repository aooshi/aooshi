using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Ajax
{
    /// <summary>
    /// ajax exception
    /// </summary>
    public class AjaxException : Exception
    {
        /// <summary>
        /// initialize
        /// </summary>
        /// <param name="message">message</param>
        public AjaxException(string message) : base(message) { }

        /// <summary>
        /// initialize
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="formats">formats</param>
        public AjaxException(string message,params string[] formats) : base(string.Format(message,formats)) { }

        // /// <summary>
        ///// ʵ�����쳣
        ///// </summary>
        ///// <param name="TypeName">��������</param>
        ///// <param name="Name">��������</param>
        //public AjaxMethodNotPublic(string TypeName, string Name) : base(string.Format("������'{0}'�еķ���'{1}'��Ϊ����������!; Type {0} is {1} Not set public;", TypeName, Name)) { }
   

    }
}
