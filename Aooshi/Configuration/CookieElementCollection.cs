using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// cookie manage collection
    /// </summary>
    [ConfigurationCollection(typeof(CookieElement), AddItemName = "cookie")]
    public class CookieElementCollection : ConfigurationElementCollection
    {
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
        /// <summary>
        /// 获取属性集
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        //加上此属性才可配置 additemname，且下级不可移除该项

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
                return "cookie";
            }
        }

        /// <summary>
        /// CreateNewElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new CookieElement();
        }

        /// <summary>
        /// GetElementKey
        /// </summary>
        /// <param name="element">element</param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CookieElement)element).Name;
        }


        /// <summary>
        /// 获取所有键值
        /// </summary>
        public object[] AllKeys
        {
            get
            {
                return base.BaseGetAllKeys();
            }
        }

        /// <summary>
        /// 获取指定索引的元素
        /// </summary>
        /// <param name="index">索引</param>
        public CookieElement this[int index]
        {
            get
            {
                return (CookieElement)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// 获取指定名称的元素
        /// </summary>
        /// <param name="name">配置名称</param>
        public new CookieElement this[string name]
        {
            get
            {
                return (CookieElement)base.BaseGet(name);
            }
        }
    }
}
