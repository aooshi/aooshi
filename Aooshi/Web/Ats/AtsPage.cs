using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Compilation;
using System.IO;

namespace Aooshi.Web.Ats
{
    /// <summary>
    /// Ats��ҳ����
    /// </summary>
    public class AtsPage : MvcPage
    {
        string _PhysicalViewCachePath = null;
        /// <summary>
        /// ��ȡ��ͼ��������·��
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
        /// ��ȡ��������ͼ����·��,Ĭ��ֵ: ~/App_Data/Cache/Ats/View/
        /// </summary>
        public virtual string ViewCachePath
        {
            get { return this._ViewCachePath; }
            set { this._ViewCachePath = Common.PathEndBackslash(value); this._PhysicalViewCachePath = null; }
        }
        /// <summary>
        /// ��ȡ��ͼĬ�Ϻ�׺
        /// </summary>
        public virtual string AtsSuffix
        {
            get { return ".html"; }
        }

        /// <summary>
        /// �Ƿ��Զ�����Ats
        /// </summary>
        public virtual bool AtsAutoUpdate
        {
            get { return true; }
        }


        private AtsFactory _factory = null;
        /// <summary>
        /// ��ȡAts������
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
        /// ATS��ͼ����
        /// </summary>
        /// <param name="viewpath">��ͼ·��,ע�������ļ���׺</param>
        /// <param name="model">����ģ��</param>
        public override MvcView CreateView(string viewpath, object model)
        {
            string vp = Path.Combine(this.PhysicalViewCachePath, base.ViewGroupName + "\\" + viewpath + ".ascx");
            string vcp = this.ViewCachePath + base.ViewGroupName + "/" + viewpath + ".ascx";
            //�Ƿ������ͼ
            if (!File.Exists(vp))
            {
                Factory.MakeTemplate(base.ViewGroupName, viewpath);
            }


            //�Զ�����auto
            if (this.AtsAutoUpdate)
            {
                Factory.UpdateTemplate(base.ViewGroupName, viewpath);
            }

            //��������
            AtsView av = (AtsView)BuildManager.CreateInstanceFromVirtualPath(vcp, typeof(AtsView));
            av.Model = model;
            return av;
        }

    }
}
