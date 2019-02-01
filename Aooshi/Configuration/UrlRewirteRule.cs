using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// ����Ԫ��
    /// </summary>
    public class UrlRewirteRule : ConfigurationElement
    {

        static readonly ConfigurationProperty _Object = new ConfigurationProperty("object", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        static readonly ConfigurationProperty _Source = new ConfigurationProperty("source", typeof(string), "", ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        //static readonly ConfigurationProperty _Template = new ConfigurationProperty("template", typeof(string), "", ConfigurationPropertyOptions.None);
        static readonly ConfigurationProperty _Redirect = new ConfigurationProperty("redirect", typeof(bool),false, ConfigurationPropertyOptions.None);
        //static readonly ConfigurationProperty _reclientpath = new ConfigurationProperty("reclientpath", typeof(bool), true, ConfigurationPropertyOptions.None);
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
        /// ��ʼ��
        /// </summary>
        static UrlRewirteRule()
        {
            _properties.Add(_Object);
            _properties.Add(_Source);
            //_properties.Add(_Template);
            _properties.Add(_Redirect);
            //_properties.Add(_reclientpath);
        }

        ///// <summary>
        ///// ��ʹ��MVCʱ��Ĭ��ģ��
        ///// </summary>
        //[ConfigurationProperty("template")]
        //public string Template
        //{
        //    get { return (string)this["template"]; }
        //    set { this["template"] = value; }
        //}

        /// <summary>
        /// �Ƿ�Ϊת�����Ϊת�����Զ�ת��ָ����ַ
        /// </summary>
        [ConfigurationProperty("redirect",DefaultValue=false)]
        public bool Redirect
        {
            get { return (bool)this["redirect"]; }
            set { this["redirect"] = value; }
        }
        ///// <summary>
        ///// �Ƿ���������·������������<see cref="Aooshi.Web.HttpModule"/>ʱ��Ч����Ĭ��Ϊtrue
        ///// </summary>
        //[ConfigurationProperty("reclientpath", DefaultValue = true)]
        //public bool ReClientPath
        //{
        //    get { return (bool)this["reclientpath"]; }
        //    set { this["reclientpath"] = value; }
        //}

        /// <summary>
        /// ��ȡ������Դ
        /// </summary>
        [ConfigurationProperty("source",IsKey=true,IsRequired=true)]
        public string Source
        {
            get { return (string)this["source"]; }
            set { this["source"] = value; }
        }

        /// <summary>
        /// ��ȡ������Ŀ��
        /// </summary>
        [ConfigurationProperty("object",IsRequired=true)]
        public string Object
        {
            get
            {
                return (string)this["object"];
            }
            set
            {
                this["object"] = value;
            }
        }

        
    }
}
