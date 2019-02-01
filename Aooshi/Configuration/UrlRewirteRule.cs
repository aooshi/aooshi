using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// 配置元素
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
        /// 获取属性集
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }
        /// <summary>
        /// 初始化
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
        ///// 当使用MVC时的默认模板
        ///// </summary>
        //[ConfigurationProperty("template")]
        //public string Template
        //{
        //    get { return (string)this["template"]; }
        //    set { this["template"] = value; }
        //}

        /// <summary>
        /// 是否为转向，如果为转向则将自动转向指定地址
        /// </summary>
        [ConfigurationProperty("redirect",DefaultValue=false)]
        public bool Redirect
        {
            get { return (bool)this["redirect"]; }
            set { this["redirect"] = value; }
        }
        ///// <summary>
        ///// 是否重置虚拟路径（此属性于<see cref="Aooshi.Web.HttpModule"/>时有效），默认为true
        ///// </summary>
        //[ConfigurationProperty("reclientpath", DefaultValue = true)]
        //public bool ReClientPath
        //{
        //    get { return (bool)this["reclientpath"]; }
        //    set { this["reclientpath"] = value; }
        //}

        /// <summary>
        /// 获取或设置源
        /// </summary>
        [ConfigurationProperty("source",IsKey=true,IsRequired=true)]
        public string Source
        {
            get { return (string)this["source"]; }
            set { this["source"] = value; }
        }

        /// <summary>
        /// 获取或设置目标
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
