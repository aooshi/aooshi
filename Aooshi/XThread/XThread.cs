using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Aooshi.XThread
{
    /// <summary>
    /// 自定义线程
    /// </summary>
    public class XThread
    {
        private AutoResetEvent ResetEvent { get; set; }
        private AutoResetEvent CallResetEvent { get; set; }

        /// <summary>
        /// 当前正执行的线程
        /// </summary>
        public Thread Thread { get; set; }

        /// <summary>
        /// 线程执行委托
        /// </summary>
        private WaitCallback Callback { get; set; }

        /// <summary>
        /// 当前线程操作数据
        /// </summary>
        public object UserDate { get; set; }

        /// <summary>
        /// 异常事件
        /// </summary>
        public event XThreadExceptionCallback Exception;

        /// <summary>
        /// 完成后事件
        /// </summary>
        public event XThreadItemCompleted Completed;

        /// <summary>
        /// 初始线程
        /// </summary>
        public XThread()
        {
            this.Exception = null;
            this.ResetEvent = new AutoResetEvent(false);
            this.CallResetEvent = new AutoResetEvent(true);
            this.Thread = new Thread(this.Do);
            this.Thread.Start();
        }

        private void Do()
        {
            while (true)
            {
                this.Run();
            }
        }

        private void Run()
        {
            this.ResetEvent.WaitOne();
            try
            {
                this.Callback(this.UserDate);
            }
            catch (ThreadAbortException)
            {
                //忽略
            }
            catch (Exception exception)
            {
                if (this.Exception != null)
                {
                    //try
                    //{
                    this.Exception(this, exception);
                    //}
                    //catch(Exception)
                    //{
                    //    //吃掉
                    //}
                }
            }
            if (this.Completed != null)
            {
                this.Completed(this);
            }
            this.CallResetEvent.Set();
        }

        /// <summary>
        /// 执行一个数据
        /// </summary>
        /// <param name="userdate"></param>
        /// <param name="callback"></param>
        public void Start(object userdate,WaitCallback callback)
        {
            this.CallResetEvent.WaitOne();
            this.Callback = callback;
            this.UserDate = userdate;
            this.ResetEvent.Set();
        }
    }
}
