using System;
using System.Collections.Generic;
using System.Text;
using Aooshi.Configuration;
using System.Web;
using System.Text.RegularExpressions;

namespace Aooshi.Web
{
    /// <summary>
    /// Cookie管理器
    /// </summary>
    public class CookieManage
    {
        string _ConfigName;
        /// <summary>
        /// 配置名称
        /// </summary>
        public string ConfigName
        {
            get
            {
                return _ConfigName;
            }
        }

        CookieElement _Setting;

        /// <summary>
        /// 相关配置
        /// </summary>
        public CookieElement Setting
        {
            get { return _Setting; }
        }

        HttpContext Context;

        /// <summary>
        /// 初始化类型
        /// </summary>
        /// <param name="configname">配置名称</param>
        public CookieManage(string configname)
        {
            this._Setting = Aooshi.Common.Configuration.Cookies[configname];
            if (this._Setting == null)
            {
                throw new AooshiException("not find '"+ configname +"' by cookie name setting");
            }
            this._ConfigName = configname;
            this.Context = HttpContext.Current;


            HttpCookie cookie = this.Context.Request.Cookies[this.Setting.Name];
            if (cookie != null)
            {
                this._Value = cookie.Value.Trim();
                //if (value == "" || !Regex.IsMatch(value, @"^([\w\-]+)$")) return "";
            }
        }

        string _Value;

        /// <summary>
        /// 获取或设置Cookie值
        /// </summary>
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                HttpCookie cookie = this.Context.Request.Cookies[this.Setting.Name];

                if (cookie == null) cookie = new HttpCookie(this.Setting.Name);
                if (cookie.Value != value)
                {
                    cookie.Value = value;
                    this._Value = value;

                    if (!string.IsNullOrEmpty(this.Setting.Path)) cookie.Path = this.Setting.Path;

                    if (!string.IsNullOrEmpty(this.Setting.Domain)) cookie.Domain = this.Setting.Domain;

                    if (this.Setting.Expires > 0)
                        cookie.Expires = DateTime.Now.AddMinutes(this.Setting.Expires);

                    cookie.Secure = this.Setting.Secure;

                    this.Context.Response.AppendCookie(cookie);

                }
            }
        }

    }
}
