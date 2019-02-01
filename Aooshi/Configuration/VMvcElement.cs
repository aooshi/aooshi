using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// ģ������Ԫ��
    /// </summary>
    public class VMvcElement : ConfigurationElement
    {
        static readonly ConfigurationProperty _path = new ConfigurationProperty("path", typeof(string), null, ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _assembly = new ConfigurationProperty("assembly", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _namespace = new ConfigurationProperty("namespace", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _rule = new ConfigurationProperty("rule", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
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
        static VMvcElement()
        {
            _properties.Add(_path);
            _properties.Add(_assembly);
            _properties.Add(_namespace);
            _properties.Add(_rule);
        }

        /// <summary>
        /// ��ȡ������ƥ��·��
        /// </summary>
        /// <remarks>�����У��������������������Сһ����·�������磺 ~/(.+).aspx �� ~/(.+)/(.+).aspx </remarks>
        [ConfigurationProperty("path", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
        [StringValidator(MinLength = 1)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }


        /// <summary>
        /// ��ȡ�������������ڵĳ�����
        /// </summary>
        [ConfigurationProperty("assembly", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Assembly
        {
            get { return (string)base["assembly"]; }
            set { base["assembly"] = value; }
        }
        /// <summary>
        /// ƥ�����˵��,�Զ��ŷָ�����,������T��������,P����ȫ�ƿռ䣬���������ÿ������
        /// </summary>
        [ConfigurationProperty("rule", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string Rule
        {
            get { return (string)base["rule"]; }
            set { base["rule"] = value; }
        }

        /// <summary>
        /// ��ȡ�������������ڵ������ռ�
        /// </summary>
        [ConfigurationProperty("namespace", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string NameSpace
        {
            get { return (string)base["namespace"]; }
            set { base["namespace"] = value; }
        }
    }
}
