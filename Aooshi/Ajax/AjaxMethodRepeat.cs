using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Ajax
{
    /// <summary>
    /// �����������ػ��ظ�ע��
    /// </summary>
    public class AjaxMethodRepeat : Exception
    {
        /// <summary>
        /// ʵ�����쳣
        /// </summary>
        /// <param name="TypeName">��������</param>
        /// <param name="Name">��������</param>
        public AjaxMethodRepeat(string TypeName, string Name) : base(string.Format("������'{0}'�еķ���'{1}'���ж�����ػ��ظ�ע��!; Type {0} is {1} repeat;", TypeName, Name)) { }
    }
}
