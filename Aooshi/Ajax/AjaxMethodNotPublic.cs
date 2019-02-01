using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Ajax
{
    /// <summary>
    /// 方法不为public修饰
    /// </summary>
    public class AjaxMethodNotPublic : Exception
    {
        /// <summary>
        /// 实例新异常
        /// </summary>
        /// <param name="TypeName">类型名称</param>
        /// <param name="Name">方法名称</param>
        public AjaxMethodNotPublic(string TypeName, string Name) : base(string.Format("在类型'{0}'中的方法'{1}'不为公共访问性!; Type {0} is {1} Not set public;", TypeName, Name)) { }
    }
}
