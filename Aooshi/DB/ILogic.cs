using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// 逻辑实现接口
    /// </summary>
    public interface ILogic
    {
        /// <summary>
        /// 获取数据操作对象
        /// </summary>
        IFactory Factory { get;}

        /// <summary>
        /// 初始逻辑实现
        /// </summary>
        /// <param name="factory">数据操作对象</param>
        void Initialize(IFactory factory);
    }
}
