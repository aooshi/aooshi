using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Web.Ats
{
    /// <summary>
    /// Ats视图基础类
    /// </summary>
    public class AtsView : MvcView
    {
        //Type _ModelType = null;
        ///// <summary>
        ///// 模型类型
        ///// </summary>
        //public virtual Type ModelType
        //{
        //    get
        //    {
        //        if (this._ModelType == null) this._ModelType = this.Model.GetType();
        //        return this._ModelType;
        //    }
        //}

        ///// <summary>
        ///// 数据模型
        ///// </summary>
        ///// <remarks>当重写时请同步重写ModelType以便于实时获取ModelType类型</remarks>
        //public override object Model
        //{
        //    get
        //    {
        //        return base.Model;
        //    }
        //    set
        //    {
        //        base.Model = value;
        //        this._ModelType = null;
        //    }
        //}

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="input">编码字符串</param>
        public virtual string UrlEncode(object input)
        {
            return this.Context.Server.UrlEncode(Convert.ToString(input));
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="input">解码字符串</param>
        public virtual string UrlDecode(object input)
        {
            return this.Context.Server.UrlDecode(Convert.ToString(input));
        }

        /// <summary>
        /// 判断一个值是否为字符串空或null
        /// </summary>
        /// <param name="o">值</param>
        public virtual bool IsEmpty(object o)
        {
            if (o == null) return true;
            if (o.ToString() == "") return true;
            return false;
        }


        /// <summary>
        /// 将指定的对象进行字符串化
        /// </summary>
        /// <param name="input">变量</param>
        public virtual string String(object input)
        {
            if (input == null) return "";
            if (DBNull.Value.Equals(input)) return "";
            return input.ToString();
        }

        /// <summary>
        /// 将指定的对象数字化
        /// </summary>
        /// <param name="input">变量</param>
        public virtual int Integer(object input)
        {
            if (input == null) return 0;
            if (DBNull.Value.Equals(input)) return 0;
            return Convert.ToInt32(input);
        }

        /// <summary>
        /// 将指定的对象浮点化
        /// </summary>
        /// <param name="input">变量</param>
        public virtual double Double(object input)
        {
            if (input == null) return 0;
            if (DBNull.Value.Equals(input)) return 0;
            return Convert.ToDouble(input);
        }

        ///// <summary>
        ///// 设置数据模型值
        ///// </summary>
        ///// <param name="propertyname">模型数据变量名称</param>
        ///// <param name="value">模型数据值</param>
        //public virtual void SetModelValue(string propertyname, object value)
        //{
        //    System.Reflection.PropertyInfo p = this.ModelType.GetProperty(propertyname, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
        //    p.SetValue(this.Model, value, null);
        //}

        ///// <summary>
        ///// 获取数据模型值
        ///// </summary>
        ///// <param name="propertyname">模型数据变量名称</param>
        //public virtual object GetModelValue(string propertyname)
        //{
        //    System.Reflection.PropertyInfo p = this.ModelType.GetProperty(propertyname, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
        //    return p.GetValue(this.Model, null);
        //}

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="timestamp">日期</param>
        /// <param name="format">格式化串</param>
        public virtual string DataFormat(int timestamp, string format)
        {
            return this.DataFormat(Aooshi.Common.UnixTimestampToDateTime(timestamp), format);
        }

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="format">格式化串</param>
        public virtual string DataFormat(DateTime dt, string format)
        {
            return dt.ToString(format);
        }


    }

    /// <summary>
    /// Ats视图基础类
    /// </summary>
    /// <typeparam name="T">模型类型</typeparam>
    public class AtsView<T> : AtsView
    {
        /// <summary>
        /// 获取或设置当前模型引用
        /// </summary>
        public new T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }
    }
}
