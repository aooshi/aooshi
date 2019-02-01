using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Aooshi.Web
{
    /// <summary>
    /// Mvc ��ͼ������
    /// </summary>
    /// <typeparam name="T">ģ������</typeparam>
    public class MvcView<T> : MvcView{

        /// <summary>
        /// ��ȡ�����õ�ǰģ������
        /// </summary>
        public new T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }
    }

    /// <summary>
    /// Mvc ��ͼ������
    /// </summary>
    public class MvcView : System.Web.UI.UserControl
    {
        object _Model;
        /// <summary>
        /// ��ȡ�����õ�ǰģ������
        /// </summary>
        public virtual object Model
        {
            get { return this._Model; }
            set { this._Model = value; }
        }

        MvcPage _ViewPage;
        /// <summary>
        /// ��ȡ��ͼ����ҳ
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
        /// ��ȡ��ǰ��ͼ���ݼ���
        /// </summary>
        public MvcDatas ViewDatas
        {
            get
            {
                return this.ViewPage.ViewDatas;
            }
        }

        /// <summary>
        /// ��ȡָ����ͼ����Դ
        /// </summary>
        /// <param name="resourcepath">��ͼ��Դ·��</param>
        public string ViewResource(string resourcepath)
        {
            return this.ViewPage.ViewResource(resourcepath);
        }

        /// <summary>
        /// ��ȡָ����ͼ����Դ
        /// </summary>
        /// <param name="viewrootpath">��ͼ·��</param>
        /// <param name="viewgroupname">��ͼ������</param>
        /// <param name="resourcepath">��ͼ��Դ·��</param>
        public string ViewResource(string viewrootpath, string viewgroupname, string resourcepath)
        {
            return this.ViewPage.ViewResource(viewrootpath,viewgroupname,resourcepath);
        }

        ///// <summary>
        ///// ���һ����ͼ���
        ///// </summary>
        ///// <param name="viewpath">��ͼ·��,ע�������ļ���׺</param>
        //public void ViewWrite(string viewpath)
        //{
        //    this.ViewWrite(viewpath, null);
        //}

        ///// <summary>
        ///// ���һ����ͼ���
        ///// </summary>
        ///// <param name="viewpath">��ͼ·��,ע�������ļ���׺</param>
        ///// <param name="model">����ģ��,��ֵ��Ϊnull</param>
        //public void ViewWrite(string viewpath, object model)
        //{
        //    Response.Write(this.ViewPage.GetViewValue(viewpath, model));
        //}

        
        /// <summary>
        /// ����һ����ҳ����
        /// </summary>
        /// <param name="funname">��������</param>
        public object PageMethod(string funname)
        {
            return this.PageMethod(funname, new object[] { });
        }
        /// <summary>
        /// ����һ����ҳ����
        /// </summary>
        /// <param name="funname">��������</param>
        /// <param name="paramters">����</param>
        public object PageMethod(string funname, object[] paramters)
        {
            Type type = this.Page.GetType();
            System.Reflection.MethodInfo mi = type.GetMethod(funname, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public);
            if (mi == null) throw new AooshiException(type.FullName + "no find " + funname);
            return mi.Invoke(this.Page, paramters);
        }

        /// <summary>
        /// ������Դ�󶨵�ָ���Ŀؼ�
        /// </summary>
        /// <param name="controlname">�ؼ�����</param>
        /// <param name="data">����Դ</param>
        /// <param name="nofindisthrow">δ�ҵ�ʱ�Ƿ��׳��쳣</param>
        public void DataBindToControl(string controlname, object data, bool nofindisthrow)
        {
            WebCommon.DataBindToFindControl(this, controlname, data, nofindisthrow);
        }


        /// <summary>
        /// ������Դ�󶨵�ָ���Ŀؼ�
        /// </summary>
        /// <param name="controlname">�ؼ�����</param>
        /// <param name="data">����Դ</param>
        public void DataBindToControl(string controlname, object data)
        {
            this.DataBindToControl(controlname, data, true);
        }

        
        /// <summary>
        /// ��ָ��ʵ�ϵĿؼ��в���һ����ͼ
        /// </summary>
        /// <param name="controlid">�ؼ�ID</param>
        /// <param name="view">��ͼ</param>
        public void InsertView(string controlid, MvcView view)
        {
            this.InsertView(controlid, view, true);
        }

        /// <summary>
        /// ��ָ��ʵ�ϵĿؼ��в���һ����ͼ
        /// </summary>
        /// <param name="controlid">�ؼ�ID</param>
        /// <param name="view">��ͼ</param>
        /// <param name="nofindisthrow">δ�ҵ�ʱ�Ƿ��׳��쳣</param>
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
 