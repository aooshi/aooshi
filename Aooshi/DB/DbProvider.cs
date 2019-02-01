using System;
using Aooshi.Configuration;
using System.Configuration;

namespace Aooshi.DB
{
    /// <summary>
    /// 驱动规则表处理
    /// </summary>
    public class DbProvider
    {

        /// <summary>
        /// 初始化驱动规则表操作
        /// </summary>
        /// <param name="name">规则名</param>
        public DbProvider(string name)
        {
            this._RuleElement = Common.Configuration.DbProvider[name];
            if (this._RuleElement != null)
            {
                this._Connect = ConfigurationManager.ConnectionStrings[this.RuleElement.Target];
            }
            else
            {
                this._Connect = ConfigurationManager.ConnectionStrings[name];
            }
        }

        /// <summary>
        /// 初始化驱动规则表操作
        /// </summary>
        /// <param name="index">规则索引</param>
        public DbProvider(int index)
        {
            this._RuleElement = Common.Configuration.DbProvider[index];
            if (this._RuleElement != null)
            {
                this._Connect = ConfigurationManager.ConnectionStrings[this.RuleElement.Target];
            }
            else
            {
                this._Connect = ConfigurationManager.ConnectionStrings[index];
            }
        }

        string _Provider = null;
        /// <summary>
        /// 获取或设置驱动
        /// </summary>
        public string Provider
        {
            get
            {
                if (this._Provider == null)
                {
                    if (this.Connect != null && this.Connect.ConnectionString != string.Empty) this._Provider = this.Connect.ProviderName;
                    if (this.RuleElement != null)
                    {
                        this._Provider = this.RuleElement.Provider;
                    }
                    if (this._Provider == null) this._Provider = "";
                }
                return _Provider;
            }
            set { this._Provider = value; }
        }

        DbProviderElement _RuleElement;

        /// <summary>
        /// 获取驱动规则配置
        /// </summary>
        public DbProviderElement RuleElement
        {
            get
            {
                return _RuleElement;
            }
        }

        ConnectionStringSettings _Connect;
        /// <summary>
        /// 获取匹配规则的链接配置
        /// </summary>
        public ConnectionStringSettings Connect
        {
            get { return this._Connect; }
        }


        /// <summary>
        /// 是否规则匹配成功
        /// </summary>
        public bool RuleSuccess
        {
            get { return this.RuleElement != null; }
        }

        /// <summary>
        /// 是否获取到相关数据库连接
        /// </summary>
        public bool Success
        {
            get { return this._Connect != null; }
        }

        /// <summary>
        /// 获取对配置的引用
        /// </summary>
        public static DbProviderCollection Configuration
        {
            get { return Common.Configuration.DbProvider; }
        }
    }
}
