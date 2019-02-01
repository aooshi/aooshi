using System;
namespace Aooshi.Ajax
{
    /// <summary>
    /// 实现Ajax方法调用的属性:注意:所注册的方法必须具有public属性,且必须是唯一没有重载的,当为重载时会出现异常;
    /// </summary>
    public class AjaxMethod : Attribute
    {
        private AjaxMethodType _type;
        /// <summary>
        /// 初始化新实例
        /// </summary>
        public AjaxMethod()
        {
            this._type =  AjaxMethodType.Default;
        }

        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="methodtype">设置执行操作类型</param>
        public AjaxMethod(AjaxMethodType methodtype)
        {
            this._type = methodtype;
        }

        /// <summary>
        /// 获取或设置方法类型
        /// </summary>
        public AjaxMethodType MethodType
        {
            get { return _type; }
            //set { _type = value; }
        }
    }
}
