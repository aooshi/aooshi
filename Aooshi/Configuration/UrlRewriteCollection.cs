using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// ����
    /// </summary>
    [ConfigurationCollection(typeof(UrlRewirteRule))]
    public class UrlRewriteCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// ����һ���µ�Ԫ��
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlRewirteRule();
        }

        /// <summary>
        /// ��ȡһ����ֵ
        /// </summary>
        /// <param name="element">��</param>
        protected override object GetElementKey(ConfigurationElement element)
        {

            return ((UrlRewirteRule)element).Source;
        }

        /// <summary>
        /// ��ȡָ�����Ƶ�����
        /// </summary>
        /// <param name="source">Դ·��</param>
        public new UrlRewirteRule this[string source]
        {
            get
            {
                return (UrlRewirteRule)base.BaseGet(source);
            }
        }


        /// <summary>
        /// ��ȡָ��������������
        /// </summary>
        /// <param name="index">����</param>
        public UrlRewirteRule this[int index]
        {
            get
            {
                return (UrlRewirteRule)base.BaseGet(index);
            }
        }
    }
}
