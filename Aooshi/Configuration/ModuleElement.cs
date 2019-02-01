using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// 模块配置元素
    /// </summary>
    public class ModuleElement : ConfigurationElement
    {
        static readonly ConfigurationProperty _name = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _value = new ConfigurationProperty("value", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
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
        static ModuleElement()
        {
            _properties.Add(_name);
            _properties.Add(_value);
        }

        /// <summary>
        /// 获取或设置配置名称
        /// </summary>
        [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)] //注意： 此处的Options 需要设置正确，否则将不可使用remove功能
        [StringValidator(MinLength = 1)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }


        /// <summary>
        /// 获取或设置配置值
        /// </summary>
        [ConfigurationProperty("value", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Value
        {
            get { return (string)base["value"]; }
            set { base["value"] = value; }
        }

    }
}
