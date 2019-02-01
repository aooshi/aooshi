using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// 模块配置节
    /// </summary>
    public class ModuleNodeElement : ConfigurationElement
    {
        static readonly ConfigurationProperty _name = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _guid = new ConfigurationProperty("guid", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _path = new ConfigurationProperty("path", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _ModuleElementCollection = new ConfigurationProperty(null, typeof(ModuleElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
        /// <summary>
        /// 获取属性对象
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }
        /// <summary>
        /// initialize
        /// </summary>
        static ModuleNodeElement()
        {
            _properties.Add(_name);
            _properties.Add(_guid);
            _properties.Add(_path);
            _properties.Add(_ModuleElementCollection);
        }

        /// <summary>
        /// 获取或设置模块名称
        /// </summary>
        [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)] //注意： 此处的Options 需要设置正确，否则将不可使用remove功能
        [StringValidator(MinLength = 1)]
        public string Name
        {
            get { return (string)base[_name]; }
            set { base[_name] = value; }
        }

        /// <summary>
        /// 获取或设置模块GUID
        /// </summary>
        [ConfigurationProperty("guid", Options = ConfigurationPropertyOptions.IsRequired)]
        [StringValidator(MinLength = 1)]
        public string Guid
        {
            get { return (string)base[_guid]; }
            set { base[_guid] = value; }
        }

        /// <summary>
        /// 获取或设置模块相关路径
        /// </summary>
        [ConfigurationProperty("path", Options = ConfigurationPropertyOptions.IsRequired)]
        [StringValidator(MinLength = 1)]
        public string Path
        {
            get { return (string)base[_path]; }
            set { base[_path] = value; }
        }


        /// <summary>
        /// 获取配置节点
        /// </summary>
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ModuleElementCollection Settings
        {
            get { return (ModuleElementCollection)base[_ModuleElementCollection]; }
        }

        /// <summary>
        /// 获取指定名称的配置
        /// </summary>
        /// <param name="name">节点名称</param>
        public new ModuleElement this[string name]
        {
            get
            {
                return this.Settings[name];
            }
        }

        /// <summary>
        /// 获取指定索引的配置
        /// </summary>
        /// <param name="index">索引</param>
        public ModuleElement this[int index]
        {
            get
            {
                return (ModuleElement)this.Settings[index];
            }
        }

        /// <summary>
        /// 获取指定名称的配置值
        /// </summary>
        /// <param name="name">配置名称</param>
        public string GetValue(string name)
        {
            ModuleElement me = this[name];
            if (me == null) return null;
            return me.Value;
        }

        /// <summary>
        /// 获取指定名称的配置值，如果未有配置则用新的值代替
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <param name="newvalue">代替的新值</param>
        public string GetValue(string name, string newvalue)
        {
            return this.GetValue(name) ?? newvalue;
        }


    }
}
