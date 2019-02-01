using System;
using System.ComponentModel;

namespace Aooshi.DB
{
    /// <summary>
    /// 属性值
    /// </summary>
    internal class TbDescriptor : PropertyDescriptor
    {
        string name;
        object value;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">值</param>
        public TbDescriptor(string name, object value)
            : base(name, null)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// 重载时，是否更改其值
        /// </summary>
        /// <param name="component">对象</param>
        public override bool CanResetValue(object component)
        {
            return !((TableBase)component).IsNull(name);
        }

        /// <summary>
        /// 组件类型
        /// </summary>
        public override Type ComponentType
        {
            get { return typeof(TableBase); }
        }

        /// <summary>
        /// 得到该属性值
        /// </summary>
        /// <param name="component">属性组件</param>
        public override object GetValue(object component)
        {
            return value; //((TableBase)component).Get(name);
        }

        /// <summary>
        /// 是否为只读
        /// </summary>
        public override bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 属性类型
        /// </summary>
        public override Type PropertyType
        {
            get
            {
                return value.GetType();
            }
        }

        /// <summary>
        /// 重置属属默认值
        /// </summary>
        /// <param name="component">组件</param>
        public override void ResetValue(object component)
        {
            ((TableBase)component).Remove(name);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="component">组件</param>
        /// <param name="value">值</param>
        public override void SetValue(object component, object value)
        {
            ((TableBase)component).Set(name, value);
        }

        /// <summary>
        /// 是否永久保存值
        /// </summary>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
