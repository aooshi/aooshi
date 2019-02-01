using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// ģ�����ý�
    /// </summary>
    public class ModuleNodeElement : ConfigurationElement
    {
        static readonly ConfigurationProperty _name = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _guid = new ConfigurationProperty("guid", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _path = new ConfigurationProperty("path", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _ModuleElementCollection = new ConfigurationProperty(null, typeof(ModuleElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
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
        static ModuleNodeElement()
        {
            _properties.Add(_name);
            _properties.Add(_guid);
            _properties.Add(_path);
            _properties.Add(_ModuleElementCollection);
        }

        /// <summary>
        /// ��ȡ������ģ������
        /// </summary>
        [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)] //ע�⣺ �˴���Options ��Ҫ������ȷ�����򽫲���ʹ��remove����
        [StringValidator(MinLength = 1)]
        public string Name
        {
            get { return (string)base[_name]; }
            set { base[_name] = value; }
        }

        /// <summary>
        /// ��ȡ������ģ��GUID
        /// </summary>
        [ConfigurationProperty("guid", Options = ConfigurationPropertyOptions.IsRequired)]
        [StringValidator(MinLength = 1)]
        public string Guid
        {
            get { return (string)base[_guid]; }
            set { base[_guid] = value; }
        }

        /// <summary>
        /// ��ȡ������ģ�����·��
        /// </summary>
        [ConfigurationProperty("path", Options = ConfigurationPropertyOptions.IsRequired)]
        [StringValidator(MinLength = 1)]
        public string Path
        {
            get { return (string)base[_path]; }
            set { base[_path] = value; }
        }


        /// <summary>
        /// ��ȡ���ýڵ�
        /// </summary>
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ModuleElementCollection Settings
        {
            get { return (ModuleElementCollection)base[_ModuleElementCollection]; }
        }

        /// <summary>
        /// ��ȡָ�����Ƶ�����
        /// </summary>
        /// <param name="name">�ڵ�����</param>
        public new ModuleElement this[string name]
        {
            get
            {
                return this.Settings[name];
            }
        }

        /// <summary>
        /// ��ȡָ������������
        /// </summary>
        /// <param name="index">����</param>
        public ModuleElement this[int index]
        {
            get
            {
                return (ModuleElement)this.Settings[index];
            }
        }

        /// <summary>
        /// ��ȡָ�����Ƶ�����ֵ
        /// </summary>
        /// <param name="name">��������</param>
        public string GetValue(string name)
        {
            ModuleElement me = this[name];
            if (me == null) return null;
            return me.Value;
        }

        /// <summary>
        /// ��ȡָ�����Ƶ�����ֵ�����δ�����������µ�ֵ����
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="newvalue">�������ֵ</param>
        public string GetValue(string name, string newvalue)
        {
            return this.GetValue(name) ?? newvalue;
        }


    }
}
