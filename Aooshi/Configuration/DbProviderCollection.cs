using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// ������������
    /// </summary>
    [ConfigurationCollection(typeof(DbProviderElement),AddItemName="set")]
    public class DbProviderCollection : ConfigurationElementCollection
    {

        /// <summary>
        /// initialize
        /// </summary>
        public DbProviderCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
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
                return "set";
            }
        }

        /// <summary>
        /// CreateNewElement
        /// </summary>
        /// <returns>DbProviderElement</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new DbProviderElement();
        }

        /// <summary>
        /// GetElementKey
        /// </summary>
        /// <param name="element">element</param>
        /// <returns>object</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DbProviderElement)element).Name;
        }

        /// <summary>
        /// ��ȡָ�����Ƶ�����
        /// </summary>
        /// <param name="name">����</param>
        public new DbProviderElement this[string name]
        {
            get
            {
                return (DbProviderElement)base.BaseGet(name);
            }
        }

        /// <summary>
        /// ��ȡָ��������������
        /// </summary>
        /// <param name="index">����</param>
        public DbProviderElement this[int index]
        {
            get
            {
                return (DbProviderElement)base.BaseGet(index);
            }
        }
    }
}
