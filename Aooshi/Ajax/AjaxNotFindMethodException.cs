using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Ajax
{
    /// <summary>
    /// δ�ҵ�Ҫע��ķ���
    /// </summary>
    public class AjaxNotFindMethodException : Exception
    {
        /// <summary>
        /// ʵ�����쳣
        /// </summary>
        /// <param name="TypeName">��������</param>
        /// <param name="Name">��������</param>
        public AjaxNotFindMethodException(string TypeName, string Name) : base(string.Format("������'{0}'��δ�ҵ�����'{1}'; Type {0}  Not Find {1};", TypeName, Name)) { }


        /// <summary>
        /// ʵ�����쳣
        /// </summary>
        /// <param name="text">˵��</param>
        public AjaxNotFindMethodException(string text) : base(text) { }
    
    }


    /// <summary>
    /// ע�����ͷ�����������
    /// </summary>
    public class AjaxMethodParameterException : Exception
    {
        /// <summary>
        /// ʵ�����쳣
        /// </summary>
        /// <param name="message">��Ϣ</param>
        public AjaxMethodParameterException(string message) : base(message) { }
    }
}
