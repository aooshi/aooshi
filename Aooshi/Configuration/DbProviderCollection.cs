using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// 数据驱动集合
    /// </summary>
    [ConfigurationCollection(typeof(DbProviderElement),AddItemName="set")]
    public class DbProviderCollection : ConfigurationElementCollection
    {

        /// <summary>
        /// initialize
        /// </summary>
        public DbProviderCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// 属性类型
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        /// <summary>
        /// 元素名称
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return "set";
            }
        }

        /// <summary>
        /// CreateNewElement
        /// </summary>
        /// <returns>DbProviderElement</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new DbProviderElement();
        }

        /// <summary>
        /// GetElementKey
        /// </summary>
        /// <param name="element">element</param>
        /// <returns>object</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DbProviderElement)element).Name;
        }

        /// <summary>
        /// 获取指定名称的配置
        /// </summary>
        /// <param name="name">名称</param>
        public new DbProviderElement this[string name]
        {
            get
            {
                return (DbProviderElement)base.BaseGet(name);
            }
        }

        /// <summary>
        /// 获取指定索引处的配置
        /// </summary>
        /// <param name="index">索引</param>
        public DbProviderElement this[int index]
        {
            get
            {
                return (DbProviderElement)base.BaseGet(index);
            }
        }
    }
}
