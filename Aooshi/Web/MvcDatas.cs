using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;

namespace Aooshi.Web
{
    /// <summary>
    /// Mvc ����������
    /// </summary>
    public class MvcDatas
    {
        /// <summary>
        /// initialize
        /// </summary>
        internal MvcDatas()
        {
            this._Items = new Dictionary<string, object>();
        }


        Dictionary<string, object> _Items;

        /// <summary>
        /// ������ҳͨ�ñ���
        /// </summary>
        /// <param name="varname">������</param>
        /// <param name="variable">����ֵ</param>
        public virtual void SetVariable(string varname, object variable)
        {
            this._Items[varname] = variable;
        }

        /// <summary>
        /// ��ȡ�����ñ���
        /// </summary>
        /// <param name="name">��������</param>
        public virtual object this[string name]
        {
            get { return this.GetVariable(name); }
            set { this.SetVariable(name, value); }
        }

        /// <summary>
        /// ��ȡһ����ҳͨ�ñ���
        /// </summary>
        /// <param name="varname">������</param>
        public virtual object GetVariable(string varname)
        {
            object o;
            this._Items.TryGetValue(varname, out o);
            return o;
        }

        /// <summary>
        /// ������ҳͨ�ñ���
        /// </summary>
        /// <param name="varname">������</param>
        /// <typeparam name="T">��������</typeparam>
        public virtual T GetVariable<T>(string varname)
        {
            object o = this.GetVariable(varname);
            if (o == null) return default(T);
            return (T)o;
        }

        /// <summary>
        /// ��ȡһ��ֵ����ֵ��ʾ�Ƿ�������һ����ҳͨ�ñ���
        /// </summary>
        /// <param name="varname">������</param>
        public virtual bool IsVariable(string varname)
        {
            return this._Items.ContainsKey(varname);
        }

        /// <summary>
        /// �Ƴ�һ������
        /// </summary>
        /// <param name="varname">��������</param>
        public virtual void Remove(string varname)
        {
            lock (this._Items)
            {
                if (this._Items.ContainsKey(varname))
                    this._Items.Remove(varname);
            }
        }
       
    }
}
