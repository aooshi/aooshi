using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// Smtp ���ý�
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
        /// ��ȡ���Լ�
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }

        /// <summary>
        /// ��ʼ��
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
        /// ��ȡ�����÷�������
        /// </summary>
        [ConfigurationProperty("FromName")]
        public string FromName
        {
            get { return (string)this["FromName"]; }
            set { this["FromName"] = value; }
        }

        /// <summary>
        /// ��ȡ�����÷��͵�ַ
        /// </summary>
        [ConfigurationProperty("FromAddress")]
        public string FromAddress
        {
            get { return (string)this["FromAddress"]; }
            set { this["FromAddress"] = value; }
        }

        /// <summary>
        /// ��ȡ������SMTP��¼����
        /// </summary>
        [ConfigurationProperty("Password")]
        public string Password
        {
            get { return (string)this["Password"]; }
            set { this["Password"] = value; }
        }
        /// <summary>
        /// ��ȡ������SMTP��¼�û���
        /// </summary>
        [ConfigurationProperty("UserName", DefaultValue = true)]
        public string UserName
        {
            get { return (string)this["UserName"]; }
            set { this["UserName"] = value; }
        }

        /// <summary>
        /// ��ȡ�����÷�����Smtp�˿�
        /// </summary>
        [ConfigurationProperty("Port", DefaultValue = 25)]
        public int Port
        {
            get { return (int)this["Port"]; }
            set { this["Port"] = value; }
        }

        /// <summary>
        /// ��ȡ�������ʼ���������ַ
        /// </summary>
        [ConfigurationProperty("Server", IsRequired = true)]
        public string Server
        {
            get { return (string)this["Server"]; }
            set { this["Server"] = value; }
        }
    }
}
