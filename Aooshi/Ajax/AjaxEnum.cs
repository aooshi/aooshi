using System;

namespace Aooshi.Ajax
{
    /// <summary>
    /// ���ͷ����
    /// </summary>
    internal enum HeaderType
    {
        CLASS,
        ARRAY,
        /// <summary>
        /// ��
        /// </summary>
        Empty
    }

    /// <summary>
    /// ʵ��Ajax���������õ���������
    /// </summary>
    public enum AjaxMethodType
    {
        /// <summary>
        /// ����Ϊ�������
        /// </summary>
        Array,
        /// <summary>
        /// ����Ϊ�������
        /// </summary>
        Object,
        /// <summary>
        /// Ĭ�ϵĲ���
        /// </summary>
        Default
    }
}
