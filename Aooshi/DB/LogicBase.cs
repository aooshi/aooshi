using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// 逻辑基类
    /// </summary>
    public abstract class LogicBase : ILogic
    {
        IFactory _Factory;

        /// <summary>
        /// 获取数据操作对象
        /// </summary>
        public virtual IFactory Factory
        {
            get { return _Factory; }
            private set { this._Factory = value;}
        }

        /// <summary>
        /// 设置逻辑实现
        /// </summary>
        /// <param name="factory">数据操作对象</param>
        public virtual void Initialize(IFactory factory)
        {
            this.Factory = factory;
        }
    }
}
