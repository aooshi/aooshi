using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// 模块配置集合
    /// </summary>
    [ConfigurationCollection(typeof(ModuleNodeElement),AddItemName="Module")]
    public class ModuleNodeCollection : ConfigurationElementCollection
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

        /// <summary>
        /// 集合类型
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
                return "Module";
            }
        }


        /// <summary>
        /// CreateNewElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleNodeElement();
        }

        /// <summary>
        /// GetElementKey
        /// </summary>
        /// <param name="element">element</param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleNodeElement)element).Name;   
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
        public ModuleNodeElement this[int index]
        {
            get
            {
                return (ModuleNodeElement)base.BaseGet(index);
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
        public new ModuleNodeElement this[string name]
        {
            get
            {
                return (ModuleNodeElement)base.BaseGet(name);
            }
        }


        /// <summary>
        /// 获取指定名称的配置值
        /// </summary>
        /// <param name="modulename">模块名称</param>
        /// <param name="settingname">配置名称</param>
        public string GetValue(string modulename, string settingname)
        {
            ModuleNodeElement mne = this[modulename];
            if (mne == null) return null;
            return mne.GetValue(settingname);
        }

        /// <summary>
        /// 获取指定名称的配置值，如果未有配置则用新的值代替
        /// </summary>
        /// <param name="modulename">模块名称</param>
        /// <param name="settingname">配置名称</param>
        /// <param name="newvalue">代替的新值</param>
        public string GetValue(string modulename, string settingname, string newvalue)
        {
            return this.GetValue(modulename, settingname) ?? newvalue;
        }
    }
}
