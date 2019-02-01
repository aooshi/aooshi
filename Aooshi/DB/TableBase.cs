using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;

namespace Aooshi.DB
{
    /// <summary>
    /// 对象基类
    /// </summary>
    [Serializable]
    public abstract class TableBase : ICustomTypeDescriptor,ICloneable
    {
        bool isnotfindthrow = true; //当未找到元素时是否抛出异常,默认为则抛出
        Dictionary<string, object> _PropertyList;
        /// <summary>
        /// 当前对象实例
        /// </summary>
        internal protected readonly Type theType;

        /// <summary>
        /// 获取或设置操作时的附加设置(请注意，在设计表时不要有与该字段重复的字段名，否则将出现系统异常)
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
        /// 获取附加参考值并添加一个where条件,例： a=1 and b=1
        /// </summary>
        /// <param name="where">条件</param>
        public TableCondition SetWhere(string where)
        {
            TableCondition.Where = where;
            return TableCondition;
        }

        /// <summary>
        /// 如果未找到列,是否抛出异常,默认为抛出
        /// </summary>
        /// <param name="value">新值</param>
        public void IsNotFindThrow(bool value)
        {
            isnotfindthrow = value;
        }

        #region Property Auto method



        /// <summary>
        /// 添加或设置指定的属性
        /// </summary>
        /// <param name="Name">参数名称</param>
        /// <param name="Value">属性值</param>
        public virtual void Set(string Name, object Value)
        {
            //if (_PropertyList.ContainsKey(Name))
            _PropertyList[Name] = Value;
            //else
            //    _PropertyList.Add(Name, Value);
        }

        /// <summary>
        /// 移除指定的已设置属性
        /// </summary>
        /// <param name="Name">属性名称</param>
        public virtual void Remove(string Name)
        {
            //if (_PropertyList.ContainsKey(Name))
            //{
                _PropertyList.Remove(Name);
            //}
        }

        /// <summary>
        /// 返回一个值,表示是否已初始或设置过指定名称的属性值
        /// </summary>
        /// <param name="Name">属性名称</param>
        public virtual bool Contains(string Name)
        {
            return _PropertyList.ContainsKey(Name);
        }


        /// <summary>
        /// 返回已初始属性个数,包念标量定义与隐含定义
        /// </summary>
        public virtual int GetInitializePropertyCount()
        {
            return _PropertyList.Count;
        }
        
        /// <summary>
        /// 获取指定项的字符串格式
        /// </summary>
        /// <param name="name">属性名</param>
        public virtual string GetString(string name)
        {
            //return Convert.ToString(this.Get(name));
            return (string)this.Get(name);
        }

        /// <summary>
        /// 获取指定的属性名,如未找到值,则会抛出异常。
        /// </summary>
        /// <param name="Name">属性名</param>
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
        /// 获取指定的属性值。
        /// </summary>
        /// <param name="Name">属性名</param>
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
        /// 获取指定的属性值。
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="nullValue">当为空时默认</param>
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
        /// 转换输出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        private T ChangeValue<T>(object o)
        {
            //以下 catch 为兼容 旧版本所需
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
        /// 判断指定的属性是否为null值
        /// </summary>
        /// <param name="Name">属性名称</param>
        public virtual bool IsNull(string Name)
        {
            //return Get<object>(Name) == null;
            return Get(Name) == null;
        }

        #endregion

        #region ICustomTypeDescriptor 成员

        /// <summary>
        /// 获取属性列表
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
        /// 返回属性玫举值
        /// </summary>
        public virtual Dictionary<string, object>.Enumerator GetEnumerator()
        {
            return _PropertyList.GetEnumerator();
        }

        /// <summary>
        /// 返回属性名称集合
        /// </summary>
        public Dictionary<string, object>.KeyCollection GetPropertyNames()
        {
            return this._PropertyList.Keys;
        }

  
        /// <summary>
        /// 将本对象数据复制到指定的对象
        /// </summary>
        /// <param name="obj">接收数据的对象</param>
        public void ToObject(TableBase obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            foreach(KeyValuePair<string, object> kvp in this._PropertyList)
            {
                obj._PropertyList[kvp.Key] = kvp.Value;
            }
        }


        #region ICloneable 成员
        /// <summary>
        /// 创建对象副本
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
