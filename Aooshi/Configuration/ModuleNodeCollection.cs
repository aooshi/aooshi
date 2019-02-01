using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// ģ�����ü���
    /// </summary>
    [ConfigurationCollection(typeof(ModuleNodeElement),AddItemName="Module")]
    public class ModuleNodeCollection : ConfigurationElementCollection
    {
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
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
        /// ��������
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
        /// <summary>
        /// Ԫ������
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return "Module";
            }
        }


        /// <summary>
        /// CreateNewElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleNodeElement();
        }

        /// <summary>
        /// GetElementKey
        /// </summary>
        /// <param name="element">element</param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleNodeElement)element).Name;   
        }


        /// <summary>
        /// ��ȡ���м�ֵ
        /// </summary>
        public object[] AllKeys
        {
            get
            {
                return base.BaseGetAllKeys();
            }
        }

        /// <summary>
        /// ��ȡָ��������Ԫ��
        /// </summary>
        /// <param name="index">����</param>
        public ModuleNodeElement this[int index]
        {
            get
            {
                return (ModuleNodeElement)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// ��ȡָ�����Ƶ�Ԫ��
        /// </summary>
        /// <param name="name">��������</param>
        public new ModuleNodeElement this[string name]
        {
            get
            {
                return (ModuleNodeElement)base.BaseGet(name);
            }
        }


        /// <summary>
        /// ��ȡָ�����Ƶ�����ֵ
        /// </summary>
        /// <param name="modulename">ģ������</param>
        /// <param name="settingname">��������</param>
        public string GetValue(string modulename, string settingname)
        {
            ModuleNodeElement mne = this[modulename];
            if (mne == null) return null;
            return mne.GetValue(settingname);
        }

        /// <summary>
        /// ��ȡָ�����Ƶ�����ֵ�����δ�����������µ�ֵ����
        /// </summary>
        /// <param name="modulename">ģ������</param>
        /// <param name="settingname">��������</param>
        /// <param name="newvalue">�������ֵ</param>
        public string GetValue(string modulename, string settingname, string newvalue)
        {
            return this.GetValue(modulename, settingname) ?? newvalue;
        }
    }
}
