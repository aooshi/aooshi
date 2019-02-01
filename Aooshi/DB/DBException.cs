using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// 异常基础类
    /// </summary>
    public class DBException:Exception
    {
        /// <summary>
        /// 初始化新的异常实体
        /// </summary>
        /// <param name="message">消息</param>
        public DBException(string message) : base(message) { }
    }
}
