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
        ///// 实例新异常
        ///// </summary>
        ///// <param name="TypeName">类型名称</param>
        ///// <param name="Name">方法名称</param>
        //public AjaxMethodNotPublic(string TypeName, string Name) : base(string.Format("在类型'{0}'中的方法'{1}'不为公共访问性!; Type {0} is {1} Not set public;", TypeName, Name)) { }
   

    }
}
