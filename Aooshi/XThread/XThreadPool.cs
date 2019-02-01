using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Aooshi.XThread
{
    /// <summary>
    /// 自定义线程池
    /// </summary>
    public class XThreadPool : IDisposable
    {
        /// <summary>
        /// 线程数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 线程列表
        /// </summary>
        public List<XThread> List { get; private set; }

        //线程队列
        private Queue<XThread> Queue { get; set; }
        private AutoResetEvent StartEvent { get; set; }

        /// <summary>
        /// 初始化类型
        /// </summary>
        /// <param name="count">线程池线程数</param>
        public XThreadPool(int count)
        {
            if (count < 1)
            {
                throw new ArgumentOutOfRangeException("count", "count 必需大于 0");
            }

            this.List = new List<XThread>(count);
            this.Queue = new Queue<XThread>(count);
            this.StartEvent = new AutoResetEvent(false);
            while (this.List.Count < count)
            {
                var thread = new XThread();
                thread.Completed += new XThreadItemCompleted(Thread_Completed);

                this.List.Add(thread);
                this.Queue.Enqueue(thread);
            }
        }

        private void Thread_Completed(XThread thread)
        {
            lock (this.Queue)
            {
                this.Queue.Enqueue(thread);
            }
            this.StartEvent.Set();
        }

        /// <summary>
        /// 请求线程执行
        /// </summary>
        /// <param name="useradata">用户数据</param>
        /// <param name="callback">线程操作回调</param>
        public void Start(object useradata, WaitCallback callback)
        {
            var thread = this.GetThread();
            thread.Start(useradata,callback);
        }


        /// <summary>
        /// 获取一个线程
        /// </summary>
        /// <returns></returns>
        public XThread GetThread()
        {
            XThread thread = null;

            while (true)
            {
                lock (this.Queue)
                {
                    if (this.Queue.Count > 0)
                    {
                        thread = this.Queue.Dequeue();
                        break;
                    }
                }
                this.StartEvent.WaitOne();
            }

            if (thread.Thread.ThreadState == ThreadState.Aborted || thread.Thread.ThreadState == ThreadState.Stopped)
            {
                throw new ThreadStateException(string.Format("线程状态为： {0} 不可使用。", thread.Thread.ThreadState));
            }

            return thread;
        }

        #region IDisposable 成员
        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            if (this.List != null)
            {
                foreach (var thread in this.List)
                {
                    thread.Thread.Abort();
                }
            }
            this.List = null;
            this.Queue = null;
        }

        #endregion
    }
}
