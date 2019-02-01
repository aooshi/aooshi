using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Ajax
{
    /// <summary>
    /// 方法具有重载或重复注册
    /// </summary>
    public class AjaxMethodRepeat : Exception
    {
        /// <summary>
        /// 实例新异常
        /// </summary>
        /// <param name="TypeName">类型名称</param>
        /// <param name="Name">方法名称</param>
        public AjaxMethodRepeat(string TypeName, string Name) : base(string.Format("在类型'{0}'中的方法'{1}'具有多个重载或重复注册!; Type {0} is {1} repeat;", TypeName, Name)) { }
    }
}
