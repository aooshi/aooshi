using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Ajax
{
    /// <summary>
    /// 未找到要注册的方法
    /// </summary>
    public class AjaxNotFindMethodException : Exception
    {
        /// <summary>
        /// 实例新异常
        /// </summary>
        /// <param name="TypeName">类型名称</param>
        /// <param name="Name">方法名称</param>
        public AjaxNotFindMethodException(string TypeName, string Name) : base(string.Format("在类型'{0}'中未找到方法'{1}'; Type {0}  Not Find {1};", TypeName, Name)) { }


        /// <summary>
        /// 实例新异常
        /// </summary>
        /// <param name="text">说明</param>
        public AjaxNotFindMethodException(string text) : base(text) { }
    
    }


    /// <summary>
    /// 注册类型方法参数错误
    /// </summary>
    public class AjaxMethodParameterException : Exception
    {
        /// <summary>
        /// 实例新异常
        /// </summary>
        /// <param name="message">消息</param>
        public AjaxMethodParameterException(string message) : base(message) { }
    }
}
