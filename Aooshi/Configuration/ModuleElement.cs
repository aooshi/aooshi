using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// ģ������Ԫ��
    /// </summary>
    public class ModuleElement : ConfigurationElement
    {
        static readonly ConfigurationProperty _name = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _value = new ConfigurationProperty("value", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
        /// <summary>
        /// ��ȡ���Զ���
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
        /// ��ȡ��������������
        /// </summary>
        [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)] //ע�⣺ �˴���Options ��Ҫ������ȷ�����򽫲���ʹ��remove����
        [StringValidator(MinLength = 1)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }


        /// <summary>
        /// ��ȡ����������ֵ
        /// </summary>
        [ConfigurationProperty("value", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Value
        {
            get { return (string)base["value"]; }
            set { base["value"] = value; }
        }

    }
}
