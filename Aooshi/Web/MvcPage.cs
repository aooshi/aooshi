using System;
using System.IO;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.SessionState;

namespace Aooshi.Web
{
    /// <summary>
    /// �ؼ����
    /// </summary>
    public class MvcPage : System.Web.UI.Page,IRequiresSessionState
    {
        /// <summary>
        /// ҳ��ʼ��
        /// </summary>
        public MvcPage()
        {
            this._ViewDatas = new MvcDatas();
        }

        ///// <summary>
        ///// ����PreInit�¼�
        ///// </summary>
        ///// <param name="e">�¼�����</param>
        //protected override void OnPreInit(EventArgs e)
        //{
        //    this.MvcMaster();
        //    base.OnPreInit(e);
        //}

        /// <summary>
        /// ����Init�¼�
        /// </summary>
        /// <param name="e">�¼�����</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.MvcInit();
        }

        /// <summary>
        /// ҳ��ʼ��ʱ����
        /// </summary>
        public virtual void MvcInit()
        {

        }

        ///// <summary>
        ///// װ��ҳMaster
        ///// </summary>
        //public virtual void MvcMaster()
        //{
        //}

        /// <summary>
        /// ����Load�¼�
        /// </summary>
        /// <param name="e">�¼�����</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.MvcLoad();
        }

        /// <summary>
        /// ҳ����ʱ����
        /// </summary>
        public virtual void MvcLoad()
        {
        }

        MvcDatas _ViewDatas;
        /// <summary>
        /// ��ȡ��ͼ��̬���ݼ���
        /// </summary>
        public virtual MvcDatas ViewDatas
        {
            get { return _ViewDatas; }
        }

        int viewid = 0;

        /// <summary>
        /// ����һ���ڱ��������в��ظ�ID
        /// </summary>
        public virtual string CreateViewID()
        {
            lock (this)
            {
                this.viewid++;
            }

            return "mvc" + this.viewid.ToString();
        }

        string _ViewRootPath = "~/View/";
        /// <summary>
        /// ��ȡ��������ͼ��·����Ĭ��Ϊ ~/View/
        /// </summary>
        public virtual string ViewRootPath
        {
            get { return _ViewRootPath; }
            set { _ViewRootPath = Common.PathEndBackslash(value); _PhysicalViewRootPath = null; }
        }


        string _ViewGroupName = "default";

        /// <summary>
        /// ��ȡ��������ͼ�������ƣ�Ĭ��Ϊ default
        /// </summary>
        public virtual string ViewGroupName
        {
            get { return _ViewGroupName; }
            set { _ViewGroupName = value; }
        }


        string _PhysicalViewRootPath;
        /// <summary>
        /// ��ȡ��ͼ��·��������·��
        /// </summary>
        public virtual string PhysicalViewRootPath
        {
            get
            {
                if (this._PhysicalViewRootPath == null)
                {
                    this._PhysicalViewRootPath = Common.PathEndSlash(this.Request.MapPath(this.ViewRootPath));
                }

                return this._PhysicalViewRootPath;
            }
        }

        /// <summary>
        /// ����һ������ͼ
        /// </summary>
        /// <param name="viewpath">��ͼ·��.ע�������ļ���׺</param>
        /// <param name="model">����ģ��,��ֵ��Ϊnull</param>
        /// <remarks>ע���÷���Ϊһ����ͼ�Ĳ���������ͼ��������ر仯������д�÷���.</remarks>
        public virtual MvcView CreateView(string viewpath, object model)
        {
            string view = this.ViewRootPath + this.ViewGroupName;
            view += "/" + viewpath + ".ascx" ;
            MvcView mv = (MvcView)BuildManager.CreateInstanceFromVirtualPath(view, typeof(MvcView));
            mv.Model = model;
            return mv;
        }
        
        /// <summary>
        /// ����ҳ��װ��һ����ͼ
        /// </summary>
        /// <param name="viewpath">��ͼ·��.ע�������ļ���׺</param>
        public virtual MvcView LoadView(string viewpath)
        {
            return this.LoadView(viewpath, null);
        }

        /// <summary>
        /// ����ҳ��װ��һ����ͼ
        /// </summary>
        /// <param name="viewpath">��ͼ·��.ע�������ļ���׺</param>
        /// <param name="model">����ģ��,��ֵ��Ϊnull</param>
        public virtual MvcView LoadView(string viewpath, object model)
        {
            MvcView mv = this.CreateView(viewpath, model);
            this.Controls.Add(mv);
            return mv;
        }
        

        /// <summary>
        /// ����ҳ��װ��һ����ͼ
        /// </summary>
        /// <param name="control">Ҫ�����ͼ�Ŀؼ�</param>
        /// <param name="viewpath">��ͼ·��.ע�������ļ���׺</param>
        /// <param name="model">����ģ��,��ֵ��Ϊnull</param>
        public virtual MvcView LoadViewToControl(Control control, string viewpath, object model)
        {
            MvcView mv = this.CreateView(viewpath, model);
            control.Controls.Add(mv);
            return mv;
        }

        ///// <summary>
        ///// ��ȡһ����ͼ���
        ///// </summary>
        ///// <param name="viewpath">��ͼ·��,ע�������ļ���׺</param>
        //public string GetViewValue(string viewpath)
        //{
        //    return this.GetViewValue(viewpath, null);
        //}

        ///// <summary>
        ///// ��ȡһ����ͼ���
        ///// </summary>
        ///// <param name="viewpath">��ͼ·��,ע�������ļ���׺</param>
        ///// <param name="model">����ģ��,��ֵ��Ϊnull</param>
        ///// <remarks>ע�⣺��ȡ����ͼ�У������ͼ���õ��Զ��弶Page���ͣ��п��ܻ���쳣</remarks>
        //public string GetViewValue(string viewpath, object model)
        //{
        //    MvcPage mp = new MvcPage();
        //    mp.Controls.Add(this.CreateView(viewpath, model));
        //    mp.ProcessRequest(this.Context);
        //    return Common.GetWebControlString(mp);
        //}


        /// <summary>
        /// ��ȡָ����ͼ����Դ
        /// </summary>
        /// <param name="resourcepath">��ͼ��Դ·��</param>
        public string ViewResource(string resourcepath)
        {
            return this.ViewResource(this.ViewRootPath, this.ViewGroupName, resourcepath);
        }

        /// <summary>
        /// ��ȡָ����ͼ����Դ
        /// </summary>
        /// <param name="viewrootpath">��ͼ·��</param>
        /// <param name="viewgroupname">��ͼ������</param>
        /// <param name="resourcepath">��ͼ��Դ·��</param>
        public string ViewResource(string viewrootpath, string viewgroupname, string resourcepath)
        {
            string vp;

            if (resourcepath[0] == '~' || resourcepath[0] == '/')
            {
                vp = resourcepath;
            }
            else
            {
                vp = Common.PathEndBackslash(viewrootpath);
                vp = vp + viewgroupname + "//" + resourcepath;
            }
            //return base.ResolveClientUrl(vp);

            //return base.ResolveUrl(vp);

            return System.Web.VirtualPathUtility.ToAbsolute(vp, this.Request.ApplicationPath);
        }




        
        //MvcContext _MvcContext;

        ///// <summary>
        ///// ��ȡ��ǰMvc������
        ///// </summary>
        //public MvcContext Mvc
        //{
        //    get
        //    {
        //        return this._MvcContext;
        //    }
        //}

        //string _viewpath, _viewcachepath, _suffix, _name, _PhysicalViewCachePath, _PhysicalViewPath;

        ///// <summary>
        ///// ��ȡ������
        ///// </summary>
        //public virtual string ViewPath
        //{
        //}

        ///// <summary>
        ///// ��ʼ����
        ///// </summary>
        //protected MvcPage()
        //{
        //    this._MvcContext = new MvcContext(this.Context, this);
        //}
        
        ///// <summary>
        ///// ��ģ����ͼ������ҳ��
        ///// </summary>
        ///// <param name="viewname">ģ������,�磺 register ; account/register ��</param>
        //public virtual MvcView LoadView(string viewname)
        //{
        //    return this.Mvc.Template.LoadView(this.Page, viewname);
        //}

        ///// <summary>
        ///// ����ͼ��ҳ������ɾ��
        ///// </summary>
        ///// <param name="view">��ͼ</param>
        //public virtual void RemoveView(MvcView view)
        //{
        //    this.Controls.Remove(view);
        //}

        ///// <summary>
        ///// ��ȡ��ͼ��������
        ///// </summary>
        //public virtual string ViewGroupName
        //{
        //    get { return "default"; }
        //}

        ///// <summary>
        ///// ��ȡָ���ļ��ĵ�ǰģ��·��
        ///// </summary>
        ///// <param name="resource">Ҫ��ȡ���ļ���Ŀ¼��</param>
        //public string GetViewResource(string resource)
        //{
        //    return base.ResolveClientUrl(this.Mvc.Template.GetResource(resource));
        //}

        ///// <summary>
        ///// ��ȡָ��Ŀ¼�ļ��ĵ�ǰģ��·��
        ///// </summary>
        ///// <param name="dirname">Ŀ¼����</param>
        ///// <param name="resource">Ҫ��ȡ���ļ���Ŀ¼��</param>
        //public string GetViewResource(string dirname, string resource)
        //{
        //    return base.Page.ResolveClientUrl(this.Mvc.Template.GetResource(dirname, resource));
        //}
    }
}
