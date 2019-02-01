using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Aooshi.Web
{
    /// <summary>
    /// Mvc 视图基础类
    /// </summary>
    /// <typeparam name="T">模型类型</typeparam>
    public class MvcView<T> : MvcView{

        /// <summary>
        /// 获取或设置当前模型引用
        /// </summary>
        public new T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }
    }

    /// <summary>
    /// Mvc 视图基础类
    /// </summary>
    public class MvcView : System.Web.UI.UserControl
    {
        object _Model;
        /// <summary>
        /// 获取或设置当前模型引用
        /// </summary>
        public virtual object Model
        {
            get { return this._Model; }
            set { this._Model = value; }
        }

        MvcPage _ViewPage;
        /// <summary>
        /// 获取视图父级页
        /// </summary>
        public virtual MvcPage ViewPage
        {
            get
            {
                if (_ViewPage == null) _ViewPage = base.Page as MvcPage;
                return _ViewPage;
            }
        }

        /// <summary>
        /// 获取当前视图数据集合
        /// </summary>
        public MvcDatas ViewDatas
        {
            get
            {
                return this.ViewPage.ViewDatas;
            }
        }

        /// <summary>
        /// 获取指定视图的资源
        /// </summary>
        /// <param name="resourcepath">视图资源路径</param>
        public string ViewResource(string resourcepath)
        {
            return this.ViewPage.ViewResource(resourcepath);
        }

        /// <summary>
        /// 获取指定视图的资源
        /// </summary>
        /// <param name="viewrootpath">视图路径</param>
        /// <param name="viewgroupname">视图分组名</param>
        /// <param name="resourcepath">视图资源路径</param>
        public string ViewResource(string viewrootpath, string viewgroupname, string resourcepath)
        {
            return this.ViewPage.ViewResource(viewrootpath,viewgroupname,resourcepath);
        }

        ///// <summary>
        ///// 输出一个视图结果
        ///// </summary>
        ///// <param name="viewpath">视图路径,注：不带文件后缀</param>
        //public void ViewWrite(string viewpath)
        //{
        //    this.ViewWrite(viewpath, null);
        //}

        ///// <summary>
        ///// 输出一个视图结果
        ///// </summary>
        ///// <param name="viewpath">视图路径,注：不带文件后缀</param>
        ///// <param name="model">数据模型,该值可为null</param>
        //public void ViewWrite(string viewpath, object model)
        //{
        //    Response.Write(this.ViewPage.GetViewValue(viewpath, model));
        //}

        
        /// <summary>
        /// 调用一个父页方法
        /// </summary>
        /// <param name="funname">函数名称</param>
        public object PageMethod(string funname)
        {
            return this.PageMethod(funname, new object[] { });
        }
        /// <summary>
        /// 调用一个父页方法
        /// </summary>
        /// <param name="funname">函数名称</param>
        /// <param name="paramters">参数</param>
        public object PageMethod(string funname, object[] paramters)
        {
            Type type = this.Page.GetType();
            System.Reflection.MethodInfo mi = type.GetMethod(funname, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public);
            if (mi == null) throw new AooshiException(type.FullName + "no find " + funname);
            return mi.Invoke(this.Page, paramters);
        }

        /// <summary>
        /// 将数据源绑定到指定的控件
        /// </summary>
        /// <param name="controlname">控件名称</param>
        /// <param name="data">数据源</param>
        /// <param name="nofindisthrow">未找到时是否抛出异常</param>
        public void DataBindToControl(string controlname, object data, bool nofindisthrow)
        {
            WebCommon.DataBindToFindControl(this, controlname, data, nofindisthrow);
        }


        /// <summary>
        /// 将数据源绑定到指定的控件
        /// </summary>
        /// <param name="controlname">控件名称</param>
        /// <param name="data">数据源</param>
        public void DataBindToControl(string controlname, object data)
        {
            this.DataBindToControl(controlname, data, true);
        }

        
        /// <summary>
        /// 在指事实上的控件中插入一个视图
        /// </summary>
        /// <param name="controlid">控件ID</param>
        /// <param name="view">视图</param>
        public void InsertView(string controlid, MvcView view)
        {
            this.InsertView(controlid, view, true);
        }

        /// <summary>
        /// 在指事实上的控件中插入一个视图
        /// </summary>
        /// <param name="controlid">控件ID</param>
        /// <param name="view">视图</param>
        /// <param name="nofindisthrow">未找到时是否抛出异常</param>
        public void InsertView(string controlid, MvcView view, bool nofindisthrow)
        {
            Control c = this.FindControl(controlid);

            if (c == null && nofindisthrow)
            {
                throw new AooshiException("no find " + controlid);
            }

            c.Controls.Add(view);
        }
    }
}
 