using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// ģ�����ü���
    /// </summary>
    [ConfigurationCollection(typeof(VMvcElement), AddItemName = "set")]
    public class VMvcElementCollection : ConfigurationElementCollection
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

        //���ϴ����Բſ����� additemname�����¼������Ƴ�����

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
                return "set";
            }
        }

        /// <summary>
        /// CreateNewElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new VMvcElement();
        }

        /// <summary>
        /// GetElementKey
        /// </summary>
        /// <param name="element">element</param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((VMvcElement)element).Path;
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
        public VMvcElement this[int index]
        {
            get
            {
                return (VMvcElement)base.BaseGet(index);
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
        public new VMvcElement this[string name]
        {
            get
            {
                return (VMvcElement)base.BaseGet(name);
            }
        }
    }
}
