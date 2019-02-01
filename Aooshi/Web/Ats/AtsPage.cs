using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Compilation;
using System.IO;

namespace Aooshi.Web.Ats
{
    /// <summary>
    /// Ats网页基类
    /// </summary>
    public class AtsPage : MvcPage
    {
        string _PhysicalViewCachePath = null;
        /// <summary>
        /// 获取视图缓存物理路径
        /// </summary>
        public virtual string PhysicalViewCachePath
        {
            get
            {
                if (this._PhysicalViewCachePath == null)
                    this._PhysicalViewCachePath = Common.PathEndSlash(base.Request.MapPath(this.ViewCachePath));
                return _PhysicalViewCachePath;
            }
        }

        string _ViewCachePath = "~/App_Data/Cache/Ats/View/";
        /// <summary>
        /// 获取或设置视图缓存路径,默认值: ~/App_Data/Cache/Ats/View/
        /// </summary>
        public virtual string ViewCachePath
        {
            get { return this._ViewCachePath; }
            set { this._ViewCachePath = Common.PathEndBackslash(value); this._PhysicalViewCachePath = null; }
        }
        /// <summary>
        /// 获取视图默认后缀
        /// </summary>
        public virtual string AtsSuffix
        {
            get { return ".html"; }
        }

        /// <summary>
        /// 是否自动更新Ats
        /// </summary>
        public virtual bool AtsAutoUpdate
        {
            get { return true; }
        }


        private AtsFactory _factory = null;
        /// <summary>
        /// 获取Ats生成器
        /// </summary>
        public virtual AtsFactory Factory
        {
            get
            {
                if (_factory == null)
                    _factory = new AtsFactory(this.ViewRootPath, this.PhysicalViewRootPath, this.ViewCachePath, this.PhysicalViewCachePath, this.AtsSuffix);

                return _factory;
            }
        }


        /// <summary>
        /// ATS视图创建
        /// </summary>
        /// <param name="viewpath">视图路径,注：不带文件后缀</param>
        /// <param name="model">数据模型</param>
        public override MvcView CreateView(string viewpath, object model)
        {
            string vp = Path.Combine(this.PhysicalViewCachePath, base.ViewGroupName + "\\" + viewpath + ".ascx");
            string vcp = this.ViewCachePath + base.ViewGroupName + "/" + viewpath + ".ascx";
            //是否存在视图
            if (!File.Exists(vp))
            {
                Factory.MakeTemplate(base.ViewGroupName, viewpath);
            }


            //自动更新auto
            if (this.AtsAutoUpdate)
            {
                Factory.UpdateTemplate(base.ViewGroupName, viewpath);
            }

            //载入数据
            AtsView av = (AtsView)BuildManager.CreateInstanceFromVirtualPath(vcp, typeof(AtsView));
            av.Model = model;
            return av;
        }

    }
}
