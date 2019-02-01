using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// Cookie Manage 配置元素
    /// </summary>
    public class CookieElement : ConfigurationElement
    {
        static readonly ConfigurationProperty _path = new ConfigurationProperty("path", typeof(string), "", ConfigurationPropertyOptions.None);
        static readonly ConfigurationProperty _domain = new ConfigurationProperty("domain", typeof(string), "", ConfigurationPropertyOptions.None);
        static readonly ConfigurationProperty _expires = new ConfigurationProperty("expires", typeof(int), 0, ConfigurationPropertyOptions.None);
        static readonly ConfigurationProperty _name = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _secure = new ConfigurationProperty("secure", typeof(bool), false, ConfigurationPropertyOptions.None);

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
        /// 初始化
        /// </summary>
        static CookieElement()
        {

            _properties.Add(_path);
            _properties.Add(_domain);
            _properties.Add(_expires);
            _properties.Add(_secure);
            _properties.Add(_name);
        }



        /// <summary>
        /// 获取或设置有效路径
        /// </summary>
        [ConfigurationProperty("path", DefaultValue = "")]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }

        /// <summary>
        /// 获取或设置有效域
        /// </summary>
        [ConfigurationProperty("domain", DefaultValue = "")]
        public string Domain
        {
            get { return (string)this["domain"]; }
            set { this["domain"] = value; }
        }
        /// <summary>
        /// 获取或设置cookie名称
        /// </summary>
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 获取或设置是否为安全https传输
        /// </summary>
        [ConfigurationProperty("secure", DefaultValue = false)]
        public bool Secure
        {
            get { return (bool)this["secure"]; }
            set { this["secure"] = value; }
        }

        /// <summary>
        /// 获取或设置有效时间(单位分钟)
        /// </summary>
        [ConfigurationProperty("expires", DefaultValue = 0)]
        public int Expires
        {
            get { return (int)this["expires"]; }
            set { this["expires"] = value; }
        }
    }
}
