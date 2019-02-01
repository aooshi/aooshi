using System;
using System.Collections.Generic;
using System.Text;
using Aooshi.Configuration;
using System.Web;
using Aooshi.DB;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web.UI;

namespace Aooshi.Web
{
    /// <summary>
    /// ���ݿ⾲̬��������
    /// </summary>
    public static class DbManager
    {
        /// <summary>
        /// ���ݿ����Ӵ�����
        /// </summary>
        internal static Regex ConnectionRegex = new Regex(@"(\{ServerMap:(.+?)\})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// ��ָ���ַ����е�{ServerMap:?}�滻Ϊ��ִ���ַ���
        /// </summary>
        /// <param name="connect_string">Ҫ������ַ���</param>
        public static string ReplaceServerMap(string connect_string)
        {
            foreach (Match mc in ConnectionRegex.Matches(connect_string))
            {
                return ConnectionRegex.Replace(connect_string, HttpContext.Current.Request.MapPath(mc.Groups[2].Value));
            }

            return connect_string;
        }


        /// <summary>
        /// ��ȡĬ�ϵ�һ�����õ����ݿ�ʵ��
        /// </summary>
        /// <param name="autoclose">�Ƿ���ҳִ�����ʱ�Զ��ر�����,��Ϊtrueʱ,����ִ��ҳ������ <see cref="System.Web.UI.Page"/> ����������Ϊfalse</param>
        public static IFactory GetInstance(bool autoclose)
        {
            if (ConfigurationManager.ConnectionStrings.Count == 0) throw new AooshiException("not setting conneciontstring to web.config");
            return DbManager.GetInstance(ConfigurationManager.ConnectionStrings[0], autoclose);
        }

        /// <summary>
        /// ��ȡָ���������õ����ݿ�ʵ��
        /// </summary>
        /// <param name="name">����������</param>
        /// <param name="autoclose">�Ƿ���ҳִ�����ʱ�Զ��ر�����,��Ϊtrueʱ,����ִ��ҳ������ <see cref="System.Web.UI.Page"/> ����������Ϊfalse</param>
        public static IFactory GetInstance(string name, bool autoclose)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            return DbManager.GetInstance(ConfigurationManager.ConnectionStrings[name], autoclose);
        }

        /// <summary>
        /// �������ӻ�ȡһ��ʵ��
        /// </summary>
        /// <param name="item">���ݿ�����</param>
        /// <param name="autoclose">�Ƿ���ҳִ�����ʱ�Զ��ر�����,��Ϊtrueʱ,����ִ��ҳ������ <see cref="System.Web.UI.Page"/> ����������Ϊfalse</param>
        public static IFactory GetInstance(ConnectionStringSettings item, bool autoclose)
        {
            return DbManager.GetInstance(item.Name, item.ConnectionString, item.ProviderName, false, autoclose);
        }

        /// <summary>
        /// ��ȡһ��ʵ������
        /// </summary>
        /// <param name="item">���ݿ�����</param>
        /// <param name="provider">����</param>
        /// <param name="autoclose">�Ƿ���ҳִ�����ʱ�Զ��ر�����,��Ϊtrueʱ,����ִ��ҳ������ <see cref="System.Web.UI.Page"/> ����������Ϊfalse</param>
        public static IFactory GetInstance(ConnectionStringSettings item, string provider, bool autoclose)
        {
            if (string.IsNullOrEmpty(provider)) throw new ArgumentNullException("not config db connect provider. or provider is null");
            return DbManager.GetInstance(item.Name, item.ConnectionString, provider, false, autoclose);
        }


        /// <summary>
        /// ��ȡָ������·�����õ����ݿ�ʵ��
        /// </summary>
        /// <param name="name">����������</param>
        /// <param name="autoclose">�Ƿ���ҳִ�����ʱ�Զ��ر�����,��Ϊtrueʱ,����ִ��ҳ������ <see cref="System.Web.UI.Page"/> ����������Ϊfalse</param>
        public static IFactory GetInstanceByDbProvider(string name, bool autoclose)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            DbProvider dp = new DbProvider(name);
            if (!dp.Success) throw new ArgumentNullException("not find DbProvider " + name);
            return DbManager.GetInstance(name,dp.Connect.ConnectionString,dp.Provider,dp.RuleElement.Convert,autoclose);
        }

        /// <summary>
        /// ��ȡһ�����ݲ���ʵ��
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="connstring">���Ӵ�</param>
        /// <param name="provider">����</param>
        /// <param name="isconvert">�Ƿ����ServerMapת��</param>
        /// <param name="autoclose">�Ƿ������Զ��ر�</param>
        public static IFactory GetInstance(string name, string connstring, string provider,bool isconvert, bool autoclose)
        {
            string object_name = "__aooshi_db_" + name + provider;
            object o = HttpContext.Current.Items[object_name];
            if (o == null)
            {
                if (isconvert)
                {
                    foreach (Match mc in ConnectionRegex.Matches(connstring))
                    {
                        connstring = ConnectionRegex.Replace(connstring, HttpContext.Current.Server.MapPath(mc.Groups[2].Value));
                    }
                }

                IFactory faction = (IFactory)Factory.GetInstance(connstring, provider);

                if (autoclose)
                    faction.RegisterWebPageClose();

                HttpContext.Current.Items.Add(object_name, faction);

                return faction;
            }

            return (IFactory)o;
        }

        ///// <summary>
        ///// ��ȡһ��ʵ������
        ///// </summary>
        ///// <param name="connectstring">�����ַ���</param>
        ///// <param name="name">ʵ������</param>
        ///// <param name="provider">����</param>
        ///// <param name="autoclose">�Ƿ���ҳִ�����ʱ�Զ��ر�����,��Ϊtrueʱ,����ִ��ҳ������ <see cref="System.Web.UI.Page"/> ����������Ϊfalse</param>
        //public static IFactory GetInstance(string connectstring,string name, string provider, bool autoclose)
        //{
        //    if (string.IsNullOrEmpty(provider)) throw new ArgumentNullException("provider");
        //    if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
        //    if (string.IsNullOrEmpty(connectstring)) throw new ArgumentNullException("connectstring");

        //    string object_name = "__aooshi_db_" + name + provider;
        //    object o = HttpContext.Current.Items[object_name];
        //    if (o == null)
        //    {
        //        IFactory faction = (IFactory)Factory.GetInstance(conn, provider);

        //        if (autoclose)
        //            faction.RegisterWebPageClose();

        //        HttpContext.Current.Items.Add(object_name, faction);

        //        return faction;
        //    }

        //    return (IFactory)o;
        //}

    }
}
