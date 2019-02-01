using System;

namespace Aooshi.Ajax
{
    /// <summary>
    /// 输出头内型
    /// </summary>
    internal enum HeaderType
    {
        CLASS,
        ARRAY,
        /// <summary>
        /// 空
        /// </summary>
        Empty
    }

    /// <summary>
    /// 实现Ajax方法所调用的属性类型
    /// </summary>
    public enum AjaxMethodType
    {
        /// <summary>
        /// 参数为数组接收
        /// </summary>
        Array,
        /// <summary>
        /// 参数为对象接收
        /// </summary>
        Object,
        /// <summary>
        /// 默认的参数
        /// </summary>
        Default
    }
}
