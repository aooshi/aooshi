using System;
using Aooshi.Configuration;
using System.Configuration;

namespace Aooshi.DB
{
    /// <summary>
    /// �����������
    /// </summary>
    public class DbProvider
    {

        /// <summary>
        /// ��ʼ��������������
        /// </summary>
        /// <param name="name">������</param>
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
        /// ��ʼ��������������
        /// </summary>
        /// <param name="index">��������</param>
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
        /// ��ȡ����������
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
        /// ��ȡ������������
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
        /// ��ȡƥ��������������
        /// </summary>
        public ConnectionStringSettings Connect
        {
            get { return this._Connect; }
        }


        /// <summary>
        /// �Ƿ����ƥ��ɹ�
        /// </summary>
        public bool RuleSuccess
        {
            get { return this.RuleElement != null; }
        }

        /// <summary>
        /// �Ƿ��ȡ��������ݿ�����
        /// </summary>
        public bool Success
        {
            get { return this._Connect != null; }
        }

        /// <summary>
        /// ��ȡ�����õ�����
        /// </summary>
        public static DbProviderCollection Configuration
        {
            get { return Common.Configuration.DbProvider; }
        }
    }
}
