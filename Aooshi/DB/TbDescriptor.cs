using System;
using System.ComponentModel;

namespace Aooshi.DB
{
    /// <summary>
    /// ����ֵ
    /// </summary>
    internal class TbDescriptor : PropertyDescriptor
    {
        string name;
        object value;

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">ֵ</param>
        public TbDescriptor(string name, object value)
            : base(name, null)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// ����ʱ���Ƿ������ֵ
        /// </summary>
        /// <param name="component">����</param>
        public override bool CanResetValue(object component)
        {
            return !((TableBase)component).IsNull(name);
        }

        /// <summary>
        /// �������
        /// </summary>
        public override Type ComponentType
        {
            get { return typeof(TableBase); }
        }

        /// <summary>
        /// �õ�������ֵ
        /// </summary>
        /// <param name="component">�������</param>
        public override object GetValue(object component)
        {
            return value; //((TableBase)component).Get(name);
        }

        /// <summary>
        /// �Ƿ�Ϊֻ��
        /// </summary>
        public override bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public override Type PropertyType
        {
            get
            {
                return value.GetType();
            }
        }

        /// <summary>
        /// ��������Ĭ��ֵ
        /// </summary>
        /// <param name="component">���</param>
        public override void ResetValue(object component)
        {
            ((TableBase)component).Remove(name);
        }

        /// <summary>
        /// ��������ֵ
        /// </summary>
        /// <param name="component">���</param>
        /// <param name="value">ֵ</param>
        public override void SetValue(object component, object value)
        {
            ((TableBase)component).Set(name, value);
        }

        /// <summary>
        /// �Ƿ����ñ���ֵ
        /// </summary>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
