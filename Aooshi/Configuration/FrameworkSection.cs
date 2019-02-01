using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// Aooshi 配置节
    /// </summary>
    public class FrameworkSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty _collection = new ConfigurationProperty("UrlRewrite", typeof(UrlRewriteCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        private static readonly ConfigurationProperty _DbProvider = new ConfigurationProperty("DbProvider", typeof(DbProviderCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        private static readonly ConfigurationProperty _VirtualMvc = new ConfigurationProperty("VirtualMvc", typeof(VMvcElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        //private static readonly ConfigurationProperty _template = new ConfigurationProperty("Template", typeof(TemplateElement), null, ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty _Cookies = new ConfigurationProperty("Cookies", typeof(CookieElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        private static readonly ConfigurationProperty _Modules = new ConfigurationProperty("Modules", typeof(ModuleNodeCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
        private static readonly ConfigurationProperty _smtp = new ConfigurationProperty("Smtp", typeof(SmtpElement), null, ConfigurationPropertyOptions.None);
        
        
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
        static FrameworkSection()
        {
            _properties.Add(_collection);
            _properties.Add(_DbProvider);
            _properties.Add(_VirtualMvc);
            _properties.Add(_Cookies);
            _properties.Add(_Modules);
            _properties.Add(_smtp);
        }


        /// <summary>
        /// 获取重写集
        /// </summary>
        [ConfigurationProperty("UrlRewrite", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public UrlRewriteCollection UrlRewrite
        {
            get
            {
                return (UrlRewriteCollection)base[_collection];
            }
        }


        /// <summary>
        /// 获取数据驱动集合
        /// </summary>
        [ConfigurationProperty("DbProvider", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public DbProviderCollection DbProvider
        {
            get
            {
                return (DbProviderCollection)base[_DbProvider];
            }
        }


        /// <summary>
        /// 获取数据驱动集合
        /// </summary>
        [ConfigurationProperty("Modules", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public ModuleNodeCollection Modules
        {
            get
            {
                return (ModuleNodeCollection)base[_Modules];
            }
        }



        /// <summary>
        /// 获取Cookie Manage 配置集合
        /// </summary>
        [ConfigurationProperty("Cookies", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public CookieElementCollection Cookies
        {
            get
            {
                return (CookieElementCollection)base[_Cookies];
            }
        }

        /// <summary>
        /// 获取键值配置集
        /// </summary>
        [ConfigurationProperty("VirtualMvc", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public VMvcElementCollection VirtualMvc
        {
            get
            {
                return (VMvcElementCollection)base[_VirtualMvc];
            }
        }



        /// <summary>
        /// 获取邮件发送配置
        /// </summary>
        [ConfigurationProperty("Smtp")]
        public SmtpElement Smtp
        {
            get
            {
                return (SmtpElement)base[_smtp];
            }
        }

        ///// <summary>
        ///// 获取公共模块配置节
        ///// </summary>
        //[ConfigurationProperty("PublicTemplate")]
        //public TemplateElement PublicTemplate
        //{
        //    get
        //    {
        //        return (TemplateElement)base[_publictemplate];
        //    }
        //}
    }
}
