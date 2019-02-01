using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// �߼�����
    /// </summary>
    public abstract class LogicBase : ILogic
    {
        IFactory _Factory;

        /// <summary>
        /// ��ȡ���ݲ�������
        /// </summary>
        public virtual IFactory Factory
        {
            get { return _Factory; }
            private set { this._Factory = value;}
        }

        /// <summary>
        /// �����߼�ʵ��
        /// </summary>
        /// <param name="factory">���ݲ�������</param>
        public virtual void Initialize(IFactory factory)
        {
            this.Factory = factory;
        }
    }
}
