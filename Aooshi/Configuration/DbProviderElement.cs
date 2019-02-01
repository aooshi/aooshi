using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// 数据驱动元素
    /// </summary>
    public class DbProviderElement : ConfigurationElement
    {
        static readonly ConfigurationProperty _name = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _provider = new ConfigurationProperty("provider", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _targetr = new ConfigurationProperty("target", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _convert = new ConfigurationProperty("convert", typeof(bool), false, ConfigurationPropertyOptions.None);
        static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
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
        /// initialize
        /// </summary>
        static DbProviderElement()
        {
            _properties.Add(_name);
            _properties.Add(_provider);
            _properties.Add(_targetr);
            _properties.Add(_convert);
        }

        /// <summary>
        /// 获取或设置数据库配置<see cref="System.Configuration.ConnectionStringSettings"/>的配置名称
        /// </summary>
        [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired, DefaultValue = "")] //注意： 此处的Options 需要设置正确，否则将不可使用remove功能
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        /// <summary>
        /// 获取或设置驱动
        /// </summary>
        [ConfigurationProperty("provider", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Provider
        {
            get { return (string)this["provider"]; }
            set { this["provider"] = value; }
        }

        /// <summary>
        /// 获取或设置目标配置名 目标
        /// </summary>
        [ConfigurationProperty("target", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Target
        {
            get { return (string)this["target"]; }
            set { this["target"] = value; }
        }
        /// <summary>
        /// 获取或设置是否存在目标转换
        /// </summary>
        [ConfigurationProperty("convert", DefaultValue = false)]
        public bool Convert
        {
            get { return (bool)this["convert"]; }
            set { this["convert"] = value; }
        }
    }
}
