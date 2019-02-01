using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// Smtp 配置节
    /// </summary>
    public class SmtpElement :  ConfigurationElement
    {
        static readonly ConfigurationProperty _Serverh = new ConfigurationProperty("Server", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _Port = new ConfigurationProperty("Port", typeof(int), 25, ConfigurationPropertyOptions.None);
        static readonly ConfigurationProperty _UserName = new ConfigurationProperty("UserName", typeof(string), "", ConfigurationPropertyOptions.None);
        static readonly ConfigurationProperty _Password = new ConfigurationProperty("Password", typeof(string), "", ConfigurationPropertyOptions.None);
        static readonly ConfigurationProperty _FromAddress = new ConfigurationProperty("FromAddress", typeof(string), "default", ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _FromName = new ConfigurationProperty("FromName", typeof(string), "", ConfigurationPropertyOptions.None);
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
        static SmtpElement()
        {
            _properties.Add(_Serverh);
            _properties.Add(_Port);
            _properties.Add(_UserName);
            _properties.Add(_Password);
            _properties.Add(_FromAddress);
            _properties.Add(_FromName);
        }
        /// <summary>
        /// 获取或设置发送名称
        /// </summary>
        [ConfigurationProperty("FromName")]
        public string FromName
        {
            get { return (string)this["FromName"]; }
            set { this["FromName"] = value; }
        }

        /// <summary>
        /// 获取或设置发送地址
        /// </summary>
        [ConfigurationProperty("FromAddress")]
        public string FromAddress
        {
            get { return (string)this["FromAddress"]; }
            set { this["FromAddress"] = value; }
        }

        /// <summary>
        /// 获取或设置SMTP登录密码
        /// </summary>
        [ConfigurationProperty("Password")]
        public string Password
        {
            get { return (string)this["Password"]; }
            set { this["Password"] = value; }
        }
        /// <summary>
        /// 获取或设置SMTP登录用户名
        /// </summary>
        [ConfigurationProperty("UserName", DefaultValue = true)]
        public string UserName
        {
            get { return (string)this["UserName"]; }
            set { this["UserName"] = value; }
        }

        /// <summary>
        /// 获取或设置服务器Smtp端口
        /// </summary>
        [ConfigurationProperty("Port", DefaultValue = 25)]
        public int Port
        {
            get { return (int)this["Port"]; }
            set { this["Port"] = value; }
        }

        /// <summary>
        /// 获取或设置邮件服务器地址
        /// </summary>
        [ConfigurationProperty("Server", IsRequired = true)]
        public string Server
        {
            get { return (string)this["Server"]; }
            set { this["Server"] = value; }
        }
    }
}
