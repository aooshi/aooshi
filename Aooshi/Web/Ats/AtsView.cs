using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Web.Ats
{
    /// <summary>
    /// Ats��ͼ������
    /// </summary>
    public class AtsView : MvcView
    {
        //Type _ModelType = null;
        ///// <summary>
        ///// ģ������
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
        ///// ����ģ��
        ///// </summary>
        ///// <remarks>����дʱ��ͬ����дModelType�Ա���ʵʱ��ȡModelType����</remarks>
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
        /// Url����
        /// </summary>
        /// <param name="input">�����ַ���</param>
        public virtual string UrlEncode(object input)
        {
            return this.Context.Server.UrlEncode(Convert.ToString(input));
        }

        /// <summary>
        /// Url����
        /// </summary>
        /// <param name="input">�����ַ���</param>
        public virtual string UrlDecode(object input)
        {
            return this.Context.Server.UrlDecode(Convert.ToString(input));
        }

        /// <summary>
        /// �ж�һ��ֵ�Ƿ�Ϊ�ַ����ջ�null
        /// </summary>
        /// <param name="o">ֵ</param>
        public virtual bool IsEmpty(object o)
        {
            if (o == null) return true;
            if (o.ToString() == "") return true;
            return false;
        }


        /// <summary>
        /// ��ָ���Ķ�������ַ�����
        /// </summary>
        /// <param name="input">����</param>
        public virtual string String(object input)
        {
            if (input == null) return "";
            if (DBNull.Value.Equals(input)) return "";
            return input.ToString();
        }

        /// <summary>
        /// ��ָ���Ķ������ֻ�
        /// </summary>
        /// <param name="input">����</param>
        public virtual int Integer(object input)
        {
            if (input == null) return 0;
            if (DBNull.Value.Equals(input)) return 0;
            return Convert.ToInt32(input);
        }

        /// <summary>
        /// ��ָ���Ķ��󸡵㻯
        /// </summary>
        /// <param name="input">����</param>
        public virtual double Double(object input)
        {
            if (input == null) return 0;
            if (DBNull.Value.Equals(input)) return 0;
            return Convert.ToDouble(input);
        }

        ///// <summary>
        ///// ��������ģ��ֵ
        ///// </summary>
        ///// <param name="propertyname">ģ�����ݱ�������</param>
        ///// <param name="value">ģ������ֵ</param>
        //public virtual void SetModelValue(string propertyname, object value)
        //{
        //    System.Reflection.PropertyInfo p = this.ModelType.GetProperty(propertyname, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
        //    p.SetValue(this.Model, value, null);
        //}

        ///// <summary>
        ///// ��ȡ����ģ��ֵ
        ///// </summary>
        ///// <param name="propertyname">ģ�����ݱ�������</param>
        //public virtual object GetModelValue(string propertyname)
        //{
        //    System.Reflection.PropertyInfo p = this.ModelType.GetProperty(propertyname, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
        //    return p.GetValue(this.Model, null);
        //}

        /// <summary>
        /// ���ڸ�ʽ��
        /// </summary>
        /// <param name="timestamp">����</param>
        /// <param name="format">��ʽ����</param>
        public virtual string DataFormat(int timestamp, string format)
        {
            return this.DataFormat(Aooshi.Common.UnixTimestampToDateTime(timestamp), format);
        }

        /// <summary>
        /// ���ڸ�ʽ��
        /// </summary>
        /// <param name="dt">����</param>
        /// <param name="format">��ʽ����</param>
        public virtual string DataFormat(DateTime dt, string format)
        {
            return dt.ToString(format);
        }


    }

    /// <summary>
    /// Ats��ͼ������
    /// </summary>
    /// <typeparam name="T">ģ������</typeparam>
    public class AtsView<T> : AtsView
    {
        /// <summary>
        /// ��ȡ�����õ�ǰģ������
        /// </summary>
        public new T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }
    }
}
