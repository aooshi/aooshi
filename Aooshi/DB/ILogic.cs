using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// �߼�ʵ�ֽӿ�
    /// </summary>
    public interface ILogic
    {
        /// <summary>
        /// ��ȡ���ݲ�������
        /// </summary>
        IFactory Factory { get;}

        /// <summary>
        /// ��ʼ�߼�ʵ��
        /// </summary>
        /// <param name="factory">���ݲ�������</param>
        void Initialize(IFactory factory);
    }
}
