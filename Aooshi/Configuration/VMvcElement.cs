using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// 模块配置元素
    /// </summary>
    public class VMvcElement : ConfigurationElement
    {
        static readonly ConfigurationProperty _path = new ConfigurationProperty("path", typeof(string), null, ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _assembly = new ConfigurationProperty("assembly", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _namespace = new ConfigurationProperty("namespace", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _rule = new ConfigurationProperty("rule", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
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
        static VMvcElement()
        {
            _properties.Add(_path);
            _properties.Add(_assembly);
            _properties.Add(_namespace);
            _properties.Add(_rule);
        }

        /// <summary>
        /// 获取或设置匹配路径
        /// </summary>
        /// <remarks>配置中，正则允许最大两个，最小一个的路径处理，如： ~/(.+).aspx 或 ~/(.+)/(.+).aspx </remarks>
        [ConfigurationProperty("path", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
        [StringValidator(MinLength = 1)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }


        /// <summary>
        /// 获取或设置类型所在的程序集名
        /// </summary>
        [ConfigurationProperty("assembly", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Assembly
        {
            get { return (string)base["assembly"]; }
            set { base["assembly"] = value; }
        }
        /// <summary>
        /// 匹配规则说明,以逗号分隔的数,其中以T代表类型,P代表全称空间，其它则代表每个参数
        /// </summary>
        [ConfigurationProperty("rule", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Rule
        {
            get { return (string)base["rule"]; }
            set { base["rule"] = value; }
        }

        /// <summary>
        /// 获取或设置类型所在的命名空间
        /// </summary>
        [ConfigurationProperty("namespace", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string NameSpace
        {
            get { return (string)base["namespace"]; }
            set { base["namespace"] = value; }
        }
    }
}
