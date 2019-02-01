using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// 集合
    /// </summary>
    [ConfigurationCollection(typeof(UrlRewirteRule))]
    public class UrlRewriteCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 创建一个新的元素
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlRewirteRule();
        }

        /// <summary>
        /// 获取一个键值
        /// </summary>
        /// <param name="element">键</param>
        protected override object GetElementKey(ConfigurationElement element)
        {

            return ((UrlRewirteRule)element).Source;
        }

        /// <summary>
        /// 获取指定名称的配置
        /// </summary>
        /// <param name="source">源路径</param>
        public new UrlRewirteRule this[string source]
        {
            get
            {
                return (UrlRewirteRule)base.BaseGet(source);
            }
        }


        /// <summary>
        /// 获取指定索引处的配置
        /// </summary>
        /// <param name="index">索引</param>
        public UrlRewirteRule this[int index]
        {
            get
            {
                return (UrlRewirteRule)base.BaseGet(index);
            }
        }
    }
}
