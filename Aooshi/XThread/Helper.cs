using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.XThread
{
    /// <summary>
    /// 异常回调
    /// </summary>
    /// <param name="thread">线程数据</param>
    /// <param name="exception">异常</param>
    public delegate void XThreadExceptionCallback(XThread thread, Exception exception);

    /// <summary>
    /// 一个项执行完成
    /// </summary>
    /// <param name="thread">一个线程项执行完成</param>
    public delegate void XThreadItemCompleted(XThread thread);
}
