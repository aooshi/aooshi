using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// ��������Ԫ��
    /// </summary>
    public class DbProviderElement : ConfigurationElement
    {
        static readonly ConfigurationProperty _name = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _provider = new ConfigurationProperty("provider", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _targetr = new ConfigurationProperty("target", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _convert = new ConfigurationProperty("convert", typeof(bool), false, ConfigurationPropertyOptions.None);
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
        /// ��ȡ���������ݿ�����<see cref="System.Configuration.ConnectionStringSettings"/>����������
        /// </summary>
        [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired, DefaultValue = "")] //ע�⣺ �˴���Options ��Ҫ������ȷ�����򽫲���ʹ��remove����
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        /// <summary>
        /// ��ȡ����������
        /// </summary>
        [ConfigurationProperty("provider", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Provider
        {
            get { return (string)this["provider"]; }
            set { this["provider"] = value; }
        }

        /// <summary>
        /// ��ȡ������Ŀ�������� Ŀ��
        /// </summary>
        [ConfigurationProperty("target", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Target
        {
            get { return (string)this["target"]; }
            set { this["target"] = value; }
        }
        /// <summary>
        /// ��ȡ�������Ƿ����Ŀ��ת��
        /// </summary>
        [ConfigurationProperty("convert", DefaultValue = false)]
        public bool Convert
        {
            get { return (bool)this["convert"]; }
            set { this["convert"] = value; }
        }
    }
}
