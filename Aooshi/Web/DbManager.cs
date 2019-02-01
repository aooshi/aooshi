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
    /// 数据库静态管理方法集
    /// </summary>
    public static class DbManager
    {
        /// <summary>
        /// 数据库链接串处理
        /// </summary>
        internal static Regex ConnectionRegex = new Regex(@"(\{ServerMap:(.+?)\})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 将指定字符串中的{ServerMap:?}替换为可执行字符串
        /// </summary>
        /// <param name="connect_string">要处理的字符串</param>
        public static string ReplaceServerMap(string connect_string)
        {
            foreach (Match mc in ConnectionRegex.Matches(connect_string))
            {
                return ConnectionRegex.Replace(connect_string, HttpContext.Current.Request.MapPath(mc.Groups[2].Value));
            }

            return connect_string;
        }


        /// <summary>
        /// 获取默认第一个配置的数据库实例
        /// </summary>
        /// <param name="autoclose">是否在页执行完成时自动关闭链接,当为true时,顶层执行页必需是 <see cref="System.Web.UI.Page"/> 否则请设置为false</param>
        public static IFactory GetInstance(bool autoclose)
        {
            if (ConfigurationManager.ConnectionStrings.Count == 0) throw new AooshiException("not setting conneciontstring to web.config");
            return DbManager.GetInstance(ConfigurationManager.ConnectionStrings[0], autoclose);
        }

        /// <summary>
        /// 获取指定名称配置的数据库实例
        /// </summary>
        /// <param name="name">数据配置名</param>
        /// <param name="autoclose">是否在页执行完成时自动关闭链接,当为true时,顶层执行页必需是 <see cref="System.Web.UI.Page"/> 否则请设置为false</param>
        public static IFactory GetInstance(string name, bool autoclose)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            return DbManager.GetInstance(ConfigurationManager.ConnectionStrings[name], autoclose);
        }

        /// <summary>
        /// 根据链接获取一个实例
        /// </summary>
        /// <param name="item">数据库配置</param>
        /// <param name="autoclose">是否在页执行完成时自动关闭链接,当为true时,顶层执行页必需是 <see cref="System.Web.UI.Page"/> 否则请设置为false</param>
        public static IFactory GetInstance(ConnectionStringSettings item, bool autoclose)
        {
            return DbManager.GetInstance(item.Name, item.ConnectionString, item.ProviderName, false, autoclose);
        }

        /// <summary>
        /// 获取一个实例对象
        /// </summary>
        /// <param name="item">数据库配置</param>
        /// <param name="provider">驱动</param>
        /// <param name="autoclose">是否在页执行完成时自动关闭链接,当为true时,顶层执行页必需是 <see cref="System.Web.UI.Page"/> 否则请设置为false</param>
        public static IFactory GetInstance(ConnectionStringSettings item, string provider, bool autoclose)
        {
            if (string.IsNullOrEmpty(provider)) throw new ArgumentNullException("not config db connect provider. or provider is null");
            return DbManager.GetInstance(item.Name, item.ConnectionString, provider, false, autoclose);
        }


        /// <summary>
        /// 获取指定名称路由配置的数据库实例
        /// </summary>
        /// <param name="name">数据配置名</param>
        /// <param name="autoclose">是否在页执行完成时自动关闭链接,当为true时,顶层执行页必需是 <see cref="System.Web.UI.Page"/> 否则请设置为false</param>
        public static IFactory GetInstanceByDbProvider(string name, bool autoclose)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            DbProvider dp = new DbProvider(name);
            if (!dp.Success) throw new ArgumentNullException("not find DbProvider " + name);
            return DbManager.GetInstance(name,dp.Connect.ConnectionString,dp.Provider,dp.RuleElement.Convert,autoclose);
        }

        /// <summary>
        /// 获取一个数据操作实例
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="connstring">连接串</param>
        /// <param name="provider">驱动</param>
        /// <param name="isconvert">是否进行ServerMap转换</param>
        /// <param name="autoclose">是否启用自动关闭</param>
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
        ///// 获取一个实例对象
        ///// </summary>
        ///// <param name="connectstring">连接字符串</param>
        ///// <param name="name">实例名称</param>
        ///// <param name="provider">驱动</param>
        ///// <param name="autoclose">是否在页执行完成时自动关闭链接,当为true时,顶层执行页必需是 <see cref="System.Web.UI.Page"/> 否则请设置为false</param>
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
