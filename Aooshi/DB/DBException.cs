using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// �쳣������
    /// </summary>
    public class DBException:Exception
    {
        /// <summary>
        /// ��ʼ���µ��쳣ʵ��
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public DBException(string message) : base(message) { }
    }
}
