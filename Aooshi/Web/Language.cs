using System;
using Aooshi.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Collections;

namespace Aooshi.Web
{
    ///// <summary>
    ///// 语言操作
    ///// </summary>
    //public class Language
    //{
    //    Dictionary<string, Dictionary<string, string>> _languagelist = new Dictionary<string, Dictionary<string, string>>();

    //    /// <summary>
    //    /// 初始化
    //    /// </summary>
    //    /// <example>
    //    /// <code>
    //    ///  class Common{
    //    ///   
    //    ///    public static Language language = new Language();
    //    /// 
    //    ///  }
    //    ///
    //    ///  string message = Common.language["message"]
    //    ///  string message = Common.language["message","a",b]; 
    //    /// 
    //    /// </code> 
    //    /// </example>
    //    public Language()
    //    {
    //    }

    //    /// <summary>
    //    /// 获取指定的语言
    //    /// </summary>
    //    /// <param name="format">格式化参数</param>
    //    /// <param name="name">语言名称</param>
    //    public string this[string name,params object[] format]
    //    {
    //        get
    //        {
    //            return string.Format(GetLanguage(Language.Current)[name],format);
    //        }
    //    }


    //    /// <summary>
    //    /// 更新现有语言配置
    //    /// </summary>
    //    public void UpdateLanguage()
    //    {
    //        lock (this)
    //        {
    //            _languagelist = new Dictionary<string, Dictionary<string,string>>();
    //        }
    //    }

    //    /// <summary>
    //    /// 获取指定的语言
    //    /// </summary>
    //    /// <param name="name">语言名称</param>
    //    public Dictionary<string, string> GetLanguage(string name)
    //    {
    //        string group = AooshiConfiguration.Configuration.Language.Group;
    //        string key = string.Format("{0}_{1}", group, name);


    //        Dictionary<string, string> lang;
    //        if (!_languagelist.ContainsKey(key))
    //        {
    //            lock (this)
    //            {
    //                lang = CreateLanguage(group, name);
    //                _languagelist.Add(key, lang);
    //            }
    //        }
    //        else
    //        {
    //            lang = _languagelist[key];
    //        }
    //        return lang;
    //    }

        
    //    /// <summary>
    //    /// 创建一个语言配置
    //    /// </summary>
    //    /// <param name="name">语言名称</param>
    //    protected virtual Dictionary<string, string> CreateLanguage(string name)
    //    {
    //        return CreateLanguage(AooshiConfiguration.Configuration.Language.Group, name);
    //    }

    //    /// <summary>
    //    /// 创建一个语言配置
    //    /// </summary>
    //    /// <param name="group">语言分组</param>
    //    /// <param name="name">语言名称</param>
    //    protected virtual Dictionary<string, string> CreateLanguage(string group, string name)
    //    {
    //        string path = AooshiConfiguration.Configuration.Language.Path;
    //        if (string.IsNullOrEmpty(path)) path = "~/App_Data/Language/";
    //        path = HttpContext.Current.Server.MapPath(path);
    //        path = Path.Combine(path, group + "/" + name + ".config");

    //        if (!File.Exists(path))
    //        {
    //            throw new AooshiException("not find language file \"" + path + "\".");
    //        }

    //        XmlDocument document = new XmlDocument();
    //        document.Load(path);

    //        Dictionary<string, string> nodelist = new Dictionary<string, string>();
    //        foreach (XmlNode child in document.DocumentElement.SelectNodes("Item"))
    //        {
    //            if (child.Attributes["name"] == null) continue;
    //            if (nodelist.ContainsKey(child.Attributes["name"].InnerText))
    //                throw new AooshiException("language is {" + child.Attributes["name"].InnerText + "} repater");
    //            else
    //                nodelist.Add(child.Attributes["name"].InnerText, child.InnerText);
    //        }

    //        return nodelist;

    //    }



    //    /// <summary>
    //    /// 得到CookieName
    //    /// </summary>
    //    public static string GetCookieName()
    //    {
    //        return string.Format("sitelang", AooshiConfiguration.Configuration.Language.Group);
    //    }


    //    /// <summary>
    //    /// 获取当前执行语言，如果未有配置则会引发异常
    //    /// </summary>
    //    public static string Current
    //    {
    //        get
    //        {
    //            string lang = "";
    //            HttpCookie cookie = HttpContext.Current.Request.Cookies[Language.GetCookieName()];
    //            if (cookie != null && Regex.IsMatch(cookie.Value, @"^(\w+)$") && cookie.Value.Trim() != "")
    //            {
    //                lang = cookie.Value.Trim();
    //            }
    //            else
    //            {
    //                lang = Language.Default;
    //            }
    //            //if (lang == "") throw new AooshiException("not setting language.");
    //            return lang;
    //        }
    //    }


    //    /// <summary>
    //    /// 获取系统默认的语言
    //    /// </summary>
    //    public static string Default
    //    {
    //        get
    //        {
    //            LanguageCollection language = AooshiConfiguration.Configuration.Language;
    //            string lang = language.Lang;
    //            if (string.IsNullOrEmpty(lang))
    //            {
    //                foreach (LanguageElement item in language)
    //                {
    //                    lang = item.Lang;
    //                    break;
    //                }
    //            }

    //            return lang ?? "";
    //        }
    //    }

    //    /// <summary>
    //    /// 设置用户语言选择
    //    /// </summary>
    //    /// <param name="name">语言名称</param>
    //    public static void SetUserLanguage(string name)
    //    {
    //        HttpCookie cookie = HttpContext.Current.Request.Cookies[Language.GetCookieName()];
    //        string domain = AooshiConfiguration.Configuration.Language.Domain;
    //        int expire = AooshiConfiguration.Configuration.Language.Expires;
    //        if (cookie != null)
    //        {
    //            if (cookie.Value == name) return;
    //        }
    //        else
    //        {
    //            cookie = new HttpCookie(Language.GetCookieName());
    //        }

    //        cookie.Value = name;

    //        if (!string.IsNullOrEmpty(domain))
    //            cookie.Domain = domain;

    //        if (expire > 0)
    //            cookie.Expires = DateTime.Now.Add(TimeSpan.FromMinutes(expire));

    //        cookie.Path = HttpContext.Current.Request.ApplicationPath;
    //        HttpContext.Current.Response.AppendCookie(cookie);
    //    }


    //}
}
