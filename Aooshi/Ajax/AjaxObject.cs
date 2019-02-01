using System;
namespace Aooshi.Ajax
{
    /// <summary>
    /// 实现Ajax返回对象注明,只有注册了此属性的对象才可返回为客户端调用对象,注意:对象只会是为公有可读属性
    /// </summary>
    public class AjaxObject : Attribute
    {
    }
}
