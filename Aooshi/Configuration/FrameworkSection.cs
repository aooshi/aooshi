using System;
using System.Configuration;

namespace Aooshi.Configuration
{
    /// <summary>
    /// Aooshi ���ý�
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
        /// ��ȡ��д��
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
        /// ��ȡ������������
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
        /// ��ȡ������������
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
        /// ��ȡCookie Manage ���ü���
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
        /// ��ȡ��ֵ���ü�
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
        /// ��ȡ�ʼ���������
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
        ///// ��ȡ����ģ�����ý�
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
