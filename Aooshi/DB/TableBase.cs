using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;

namespace Aooshi.DB
{
    /// <summary>
    /// �������
    /// </summary>
    [Serializable]
    public abstract class TableBase : ICustomTypeDescriptor,ICloneable
    {
        bool isnotfindthrow = true; //��δ�ҵ�Ԫ��ʱ�Ƿ��׳��쳣,Ĭ��Ϊ���׳�
        Dictionary<string, object> _PropertyList;
        /// <summary>
        /// ��ǰ����ʵ��
        /// </summary>
        internal protected readonly Type theType;

        /// <summary>
        /// ��ȡ�����ò���ʱ�ĸ�������(��ע�⣬����Ʊ�ʱ��Ҫ������ֶ��ظ����ֶ��������򽫳���ϵͳ�쳣)
        /// </summary>
        public readonly TableCondition TableCondition;

        /// <summary>
        /// Initialize ObjectBase
        /// </summary>
        protected TableBase()
        {
            this.theType = this.GetType();
            _PropertyList = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this.TableCondition = new TableCondition(this.theType.Name);
        }

        /// <summary>
        /// ��ȡ���Ӳο�ֵ�����һ��where����,���� a=1 and b=1
        /// </summary>
        /// <param name="where">����</param>
        public TableCondition SetWhere(string where)
        {
            TableCondition.Where = where;
            return TableCondition;
        }

        /// <summary>
        /// ���δ�ҵ���,�Ƿ��׳��쳣,Ĭ��Ϊ�׳�
        /// </summary>
        /// <param name="value">��ֵ</param>
        public void IsNotFindThrow(bool value)
        {
            isnotfindthrow = value;
        }

        #region Property Auto method



        /// <summary>
        /// ��ӻ�����ָ��������
        /// </summary>
        /// <param name="Name">��������</param>
        /// <param name="Value">����ֵ</param>
        public virtual void Set(string Name, object Value)
        {
            //if (_PropertyList.ContainsKey(Name))
            _PropertyList[Name] = Value;
            //else
            //    _PropertyList.Add(Name, Value);
        }

        /// <summary>
        /// �Ƴ�ָ��������������
        /// </summary>
        /// <param name="Name">��������</param>
        public virtual void Remove(string Name)
        {
            //if (_PropertyList.ContainsKey(Name))
            //{
                _PropertyList.Remove(Name);
            //}
        }

        /// <summary>
        /// ����һ��ֵ,��ʾ�Ƿ��ѳ�ʼ�����ù�ָ�����Ƶ�����ֵ
        /// </summary>
        /// <param name="Name">��������</param>
        public virtual bool Contains(string Name)
        {
            return _PropertyList.ContainsKey(Name);
        }


        /// <summary>
        /// �����ѳ�ʼ���Ը���,���������������������
        /// </summary>
        public virtual int GetInitializePropertyCount()
        {
            return _PropertyList.Count;
        }
        
        /// <summary>
        /// ��ȡָ������ַ�����ʽ
        /// </summary>
        /// <param name="name">������</param>
        public virtual string GetString(string name)
        {
            //return Convert.ToString(this.Get(name));
            return (string)this.Get(name);
        }

        /// <summary>
        /// ��ȡָ����������,��δ�ҵ�ֵ,����׳��쳣��
        /// </summary>
        /// <param name="Name">������</param>
        /// <exception cref="ArgumentException">not initialize is this name</exception>
        public virtual object Get(string Name)
        {
            object o;
            if (!_PropertyList.TryGetValue(Name, out o))
            {
                if (isnotfindthrow)
                    throw new DBException(this.theType.Name + " Not Find Column '" + Name + "';");
                else
                    return null;
            }
            return DBNull.Value.Equals(o) ? null : o;
        }
        /// <summary>
        /// ��ȡָ��������ֵ��
        /// </summary>
        /// <param name="Name">������</param>
        public virtual T Get<T>(string Name)
        {
            var o = this.Get(Name);

            if (o == null)
            {
                return default(T);
            }
            
            return this.ChangeValue<T>(o);
        }

        /// <summary>
        /// ��ȡָ��������ֵ��
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="nullValue">��Ϊ��ʱĬ��</param>
        public virtual T Get<T>(string name,T nullValue)
        {
            var o = this.Get(name);

            if (o == null)
            {
                return nullValue;
            }

            return this.ChangeValue<T>(o);
        }

        /// <summary>
        /// ת�����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        private T ChangeValue<T>(object o)
        {
            //���� catch Ϊ���� �ɰ汾����
            try
            {
                return (T)o;
            }
            catch
            {
                return (T)Convert.ChangeType(o, typeof(T));

                //if (o.GetType() == typeof(T)) return (T)o;
                //try
                //{
                //    o = Convert.ChangeType(o, typeof(T));
                //}
                //catch
                //{
                //    return default(T);
                //}
            }
        }

        /// <summary>
        /// �ж�ָ���������Ƿ�Ϊnullֵ
        /// </summary>
        /// <param name="Name">��������</param>
        public virtual bool IsNull(string Name)
        {
            //return Get<object>(Name) == null;
            return Get(Name) == null;
        }

        #endregion

        #region ICustomTypeDescriptor ��Ա

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return new AttributeCollection(null);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return this.theType.Name;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return typeof(TableBase).Name;
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(this,true);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this,true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this,true);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true) ;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return new EventDescriptorCollection(null);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return ((ICustomTypeDescriptor)this).GetEvents(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            if (this.pcache == null)
            {

                Dictionary<string, object>.Enumerator ie = _PropertyList.GetEnumerator();
                List<TbDescriptor> mp = new List<TbDescriptor>();
                while (ie.MoveNext())
                    mp.Add(new TbDescriptor(ie.Current.Key, ie.Current.Value));
                this.pcache = new PropertyDescriptorCollection(mp.ToArray());
            }
            return this.pcache;
        }

        [NonSerialized]
        private PropertyDescriptorCollection pcache;
        

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion


        /// <summary>
        /// ��������õ��ֵ
        /// </summary>
        public virtual Dictionary<string, object>.Enumerator GetEnumerator()
        {
            return _PropertyList.GetEnumerator();
        }

        /// <summary>
        /// �����������Ƽ���
        /// </summary>
        public Dictionary<string, object>.KeyCollection GetPropertyNames()
        {
            return this._PropertyList.Keys;
        }

  
        /// <summary>
        /// �����������ݸ��Ƶ�ָ���Ķ���
        /// </summary>
        /// <param name="obj">�������ݵĶ���</param>
        public void ToObject(TableBase obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            foreach(KeyValuePair<string, object> kvp in this._PropertyList)
            {
                obj._PropertyList[kvp.Key] = kvp.Value;
            }
        }


        #region ICloneable ��Ա
        /// <summary>
        /// �������󸱱�
        /// </summary>
        public object Clone()
        {
            TableBase o = (TableBase)this.theType.Assembly.CreateInstance(this.theType.FullName);
            this.ToObject(o);
            return o;
        }

        #endregion
    }
}
