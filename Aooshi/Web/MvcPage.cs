using System;
using System.IO;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.SessionState;

namespace Aooshi.Web
{
    /// <summary>
    /// 控件输出
    /// </summary>
    public class MvcPage : System.Web.UI.Page,IRequiresSessionState
    {
        /// <summary>
        /// 页初始化
        /// </summary>
        public MvcPage()
        {
            this._ViewDatas = new MvcDatas();
        }

        ///// <summary>
        ///// 引发PreInit事件
        ///// </summary>
        ///// <param name="e">事件数据</param>
        //protected override void OnPreInit(EventArgs e)
        //{
        //    this.MvcMaster();
        //    base.OnPreInit(e);
        //}

        /// <summary>
        /// 引发Init事件
        /// </summary>
        /// <param name="e">事件数据</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.MvcInit();
        }

        /// <summary>
        /// 页初始化时调用
        /// </summary>
        public virtual void MvcInit()
        {

        }

        ///// <summary>
        ///// 装载页Master
        ///// </summary>
        //public virtual void MvcMaster()
        //{
        //}

        /// <summary>
        /// 引发Load事件
        /// </summary>
        /// <param name="e">事件数据</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.MvcLoad();
        }

        /// <summary>
        /// 页载入时调用
        /// </summary>
        public virtual void MvcLoad()
        {
        }

        MvcDatas _ViewDatas;
        /// <summary>
        /// 获取视图动态数据集合
        /// </summary>
        public virtual MvcDatas ViewDatas
        {
            get { return _ViewDatas; }
        }

        int viewid = 0;

        /// <summary>
        /// 创建一个在本次请求中不重复ID
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
        /// 获取或设置视图根路径，默认为 ~/View/
        /// </summary>
        public virtual string ViewRootPath
        {
            get { return _ViewRootPath; }
            set { _ViewRootPath = Common.PathEndBackslash(value); _PhysicalViewRootPath = null; }
        }


        string _ViewGroupName = "default";

        /// <summary>
        /// 获取或设置视图分组名称，默认为 default
        /// </summary>
        public virtual string ViewGroupName
        {
            get { return _ViewGroupName; }
            set { _ViewGroupName = value; }
        }


        string _PhysicalViewRootPath;
        /// <summary>
        /// 获取视图根路径的物理路径
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
        /// 创建一个新视图
        /// </summary>
        /// <param name="viewpath">视图路径.注：不带文件后缀</param>
        /// <param name="model">数据模型,该值可为null</param>
        /// <remarks>注：该方法为一切视图的产生，如视图产生有相关变化，请重写该方法.</remarks>
        public virtual MvcView CreateView(string viewpath, object model)
        {
            string view = this.ViewRootPath + this.ViewGroupName;
            view += "/" + viewpath + ".ascx" ;
            MvcView mv = (MvcView)BuildManager.CreateInstanceFromVirtualPath(view, typeof(MvcView));
            mv.Model = model;
            return mv;
        }
        
        /// <summary>
        /// 在网页中装载一个视图
        /// </summary>
        /// <param name="viewpath">视图路径.注：不带文件后缀</param>
        public virtual MvcView LoadView(string viewpath)
        {
            return this.LoadView(viewpath, null);
        }

        /// <summary>
        /// 在网页中装载一个视图
        /// </summary>
        /// <param name="viewpath">视图路径.注：不带文件后缀</param>
        /// <param name="model">数据模型,该值可为null</param>
        public virtual MvcView LoadView(string viewpath, object model)
        {
            MvcView mv = this.CreateView(viewpath, model);
            this.Controls.Add(mv);
            return mv;
        }
        

        /// <summary>
        /// 在网页中装载一个视图
        /// </summary>
        /// <param name="control">要添加视图的控件</param>
        /// <param name="viewpath">视图路径.注：不带文件后缀</param>
        /// <param name="model">数据模型,该值可为null</param>
        public virtual MvcView LoadViewToControl(Control control, string viewpath, object model)
        {
            MvcView mv = this.CreateView(viewpath, model);
            control.Controls.Add(mv);
            return mv;
        }

        ///// <summary>
        ///// 获取一个视图结果
        ///// </summary>
        ///// <param name="viewpath">视图路径,注：不带文件后缀</param>
        //public string GetViewValue(string viewpath)
        //{
        //    return this.GetViewValue(viewpath, null);
        //}

        ///// <summary>
        ///// 获取一个视图结果
        ///// </summary>
        ///// <param name="viewpath">视图路径,注：不带文件后缀</param>
        ///// <param name="model">数据模型,该值可为null</param>
        ///// <remarks>注意：获取的视图中，如果视图引用到自定义级Page类型，有可能会出异常</remarks>
        //public string GetViewValue(string viewpath, object model)
        //{
        //    MvcPage mp = new MvcPage();
        //    mp.Controls.Add(this.CreateView(viewpath, model));
        //    mp.ProcessRequest(this.Context);
        //    return Common.GetWebControlString(mp);
        //}


        /// <summary>
        /// 获取指定视图的资源
        /// </summary>
        /// <param name="resourcepath">视图资源路径</param>
        public string ViewResource(string resourcepath)
        {
            return this.ViewResource(this.ViewRootPath, this.ViewGroupName, resourcepath);
        }

        /// <summary>
        /// 获取指定视图的资源
        /// </summary>
        /// <param name="viewrootpath">视图路径</param>
        /// <param name="viewgroupname">视图分组名</param>
        /// <param name="resourcepath">视图资源路径</param>
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
        ///// 获取当前Mvc上下文
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
        ///// 获取或设置
        ///// </summary>
        //public virtual string ViewPath
        //{
        //}

        ///// <summary>
        ///// 初始化类
        ///// </summary>
        //protected MvcPage()
        //{
        //    this._MvcContext = new MvcContext(this.Context, this);
        //}
        
        ///// <summary>
        ///// 将模板视图导入网页中
        ///// </summary>
        ///// <param name="viewname">模板名称,如： register ; account/register 等</param>
        //public virtual MvcView LoadView(string viewname)
        //{
        //    return this.Mvc.Template.LoadView(this.Page, viewname);
        //}

        ///// <summary>
        ///// 将视图从页集合中删除
        ///// </summary>
        ///// <param name="view">视图</param>
        //public virtual void RemoveView(MvcView view)
        //{
        //    this.Controls.Remove(view);
        //}

        ///// <summary>
        ///// 获取视图分组名称
        ///// </summary>
        //public virtual string ViewGroupName
        //{
        //    get { return "default"; }
        //}

        ///// <summary>
        ///// 获取指定文件的当前模板路径
        ///// </summary>
        ///// <param name="resource">要获取的文件或目录名</param>
        //public string GetViewResource(string resource)
        //{
        //    return base.ResolveClientUrl(this.Mvc.Template.GetResource(resource));
        //}

        ///// <summary>
        ///// 获取指定目录文件的当前模板路径
        ///// </summary>
        ///// <param name="dirname">目录名称</param>
        ///// <param name="resource">要获取的文件或目录名</param>
        //public string GetViewResource(string dirname, string resource)
        //{
        //    return base.Page.ResolveClientUrl(this.Mvc.Template.GetResource(dirname, resource));
        //}
    }
}
