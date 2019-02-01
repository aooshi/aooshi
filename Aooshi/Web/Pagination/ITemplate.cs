using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Aooshi.Web.Pagination
{
    /// <summary>
    /// ��ҳ��ʽ�ӿ�
    /// </summary>
    public interface ITemplate
    {
        /// <summary>
        /// ��ȡ��ǰ��ҳ���
        /// </summary>
        PaginationBase Pagination{get;}

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="writer">�������</param>
        void Render(HtmlTextWriter writer);

        /// <summary>
        /// ��CssClassΪ��ʱ�����Ĭ����ʽ
        /// </summary>
        /// <param name="writer">�������</param>
        void RenderStyle(HtmlTextWriter writer);

        /// <summary>
        /// ������ť
        /// </summary>
        /// <param name="href">���ӵ�ַ</param>
        /// <param name="text">��ʾ����</param>
        string CreateButton(string href, string text);
    }
}
