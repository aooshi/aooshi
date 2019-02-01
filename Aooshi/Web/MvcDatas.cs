using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;

namespace Aooshi.Web
{
    /// <summary>
    /// Mvc 操作上下文
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
        /// 设置网页通用变量
        /// </summary>
        /// <param name="varname">变量名</param>
        /// <param name="variable">变量值</param>
        public virtual void SetVariable(string varname, object variable)
        {
            this._Items[varname] = variable;
        }

        /// <summary>
        /// 获取或设置变量
        /// </summary>
        /// <param name="name">变量名称</param>
        public virtual object this[string name]
        {
            get { return this.GetVariable(name); }
            set { this.SetVariable(name, value); }
        }

        /// <summary>
        /// 获取一个网页通用变量
        /// </summary>
        /// <param name="varname">变量名</param>
        public virtual object GetVariable(string varname)
        {
            object o;
            this._Items.TryGetValue(varname, out o);
            return o;
        }

        /// <summary>
        /// 设置网页通用变量
        /// </summary>
        /// <param name="varname">变量名</param>
        /// <typeparam name="T">变量类型</typeparam>
        public virtual T GetVariable<T>(string varname)
        {
            object o = this.GetVariable(varname);
            if (o == null) return default(T);
            return (T)o;
        }

        /// <summary>
        /// 获取一个值，该值表示是否已设置一个网页通用变量
        /// </summary>
        /// <param name="varname">变量名</param>
        public virtual bool IsVariable(string varname)
        {
            return this._Items.ContainsKey(varname);
        }

        /// <summary>
        /// 移除一个变量
        /// </summary>
        /// <param name="varname">变量名称</param>
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
