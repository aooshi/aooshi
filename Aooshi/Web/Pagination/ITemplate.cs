using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Aooshi.Web.Pagination
{
    /// <summary>
    /// 分页样式接口
    /// </summary>
    public interface ITemplate
    {
        /// <summary>
        /// 获取当前分页组件
        /// </summary>
        PaginationBase Pagination{get;}

        /// <summary>
        /// 输出数据
        /// </summary>
        /// <param name="writer">输出对象</param>
        void Render(HtmlTextWriter writer);

        /// <summary>
        /// 当CssClass为空时输出的默认样式
        /// </summary>
        /// <param name="writer">输出对象</param>
        void RenderStyle(HtmlTextWriter writer);

        /// <summary>
        /// 创建按钮
        /// </summary>
        /// <param name="href">链接地址</param>
        /// <param name="text">显示文字</param>
        string CreateButton(string href, string text);
    }
}
