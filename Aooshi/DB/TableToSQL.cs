using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Aooshi.DB
{
    /// <summary>
    /// 将指定的对象转换为SQL语句
    /// </summary>
    /// <remarks>
    /// where 条件方式:
    ///     1.  Object 所设置的 Where 
    ///     2.  Object 所设置的 WhereObject
    ///     3.  Object 本身已设置属性量
    ///     4.  如须空Where 请设置Where字符串为Null
    /// </remarks>
    public class TableToSQL
    {
        /// <summary>
        /// 操作参数列表
        /// </summary>
        protected List<IDbDataParameter> parameter;
        string tablename;
        /// <summary>
        /// 初始化新对象
        /// </summary>
        /// <param name="Object">数据对象</param>
        /// <param name="factory">当前操作对象</param>
        protected internal TableToSQL(TableBase Object,Factory factory):this(Object,factory,new List<IDbDataParameter>())
        {
        }

        /// <summary>
        /// 初始化带参对象
        /// </summary>
        /// <param name="tablebase">数据对象</param>
        /// <param name="factory">当前操作对象</param>
        /// <param name="paramelist">参数列表</param>
        protected internal TableToSQL(TableBase tablebase, Factory factory, List<IDbDataParameter> paramelist)
        {
            _Object = tablebase;
            _factory = factory;
            parameter = paramelist;
            tablename = tablebase.TableCondition.TableName;
        }
        private Factory _factory;
        /// <summary>
        /// 获取当前调用此方法的工厂对象
        /// </summary>
        protected Factory Factory
        {
            get { return _factory; }
        }
        private TableBase _Object;
        /// <summary>
        /// 获取当前正在进行处理的Object
        /// </summary>
        protected TableBase Object
        {
            get { return _Object; }
        }

        /// <summary>
        /// 得到对象参数列表
        /// </summary>
        public IDbDataParameter[] GetPartemter
        {
            get
            {
                if (Object.TableCondition.Parameters != null)
                    parameter.AddRange(Object.TableCondition.Parameters);
                return parameter.ToArray();
            }
        }

        /// <summary>
        /// get object where
        /// </summary>
        /// <param name="Object">指定的条件生成对象</param>
        public virtual String GetWhere(TableBase Object)
        {
           
                if (!string.IsNullOrEmpty(Object.TableCondition.Where)) return " WHERE " + Object.TableCondition.Where;
                //where to object
                //if (Object.Condition.WhereObject != null) return new TableToSQL(Object.Condition.WhereObject, Factory, parameter).GetWhere;
                //object property
                StringBuilder tmp = new StringBuilder();
                string split = Object.TableCondition.WhereSplit + " ";
                Dictionary<string, object>.Enumerator etor = Object.GetEnumerator();
                //Type type;
                while (etor.MoveNext())
                {
                    tmp.Append(split);
                    tmp.AppendFormat("{0}={1}{0}", etor.Current.Key, Factory.GetParameterSplitChar);
                    parameter.Add(Factory.CreateParameter(etor.Current.Key, etor.Current.Value));
                    /*//type = etor.Current.Value.GetType();
                    if (type == typeof(int) || type == typeof(bool))
                        tmp.AppendFormat("{0}={1}", etor.Current.Key, etor.Current.Value);
                    else if (etor.Current.Value == null || etor.Current.Value == DBNull.Value)
                        tmp.AppendFormat("{0}=NULL", etor.Current.Key);
                    else
                        tmp.AppendFormat("{0}='{1}'", etor.Current.Key, SQLUtility.Replace(etor.Current.Value.ToString()));*/
                }
                if (tmp.Length > 0) return " WHERE " + tmp.Remove(0, split.Length).ToString();
                return "";
        }

        
        /// <summary>
        /// get object where
        /// </summary>
        public virtual String GetWhere()
        {
            return this.GetWhere(Object);
        }


        /// <summary>
        /// get delete sql
        /// </summary>
        public virtual String GetDelete()
        {
                return string.Format("DELETE FROM {0}{1}",tablename,GetWhere());
        }
        /// <summary>
        /// get insert sql
        /// </summary>
        public virtual String GetInsert()
        {
                StringBuilder Filed  = new StringBuilder();
                StringBuilder Values = new StringBuilder();
                //object property
                Dictionary<string, object>.Enumerator etor = Object.GetEnumerator();
                //Type type;
                while (etor.MoveNext())
                {
                    //type = etor.Current.Value.GetType();
                    Filed.Append("," + etor.Current.Key);
                    Values.AppendFormat(",{0}{1}", Factory.GetParameterSplitChar, etor.Current.Key);
                    parameter.Add(Factory.CreateParameter(etor.Current.Key, etor.Current.Value));
                    //Values.AppendFormat(",{0}", etor.Current.Value);
                    /*if (type == typeof(int) || type == typeof(bool))
                        Values.AppendFormat(",{0}", etor.Current.Value);
                    else if (etor.Current.Value == null || etor.Current.Value == DBNull.Value)
                        Values.Append(",NULL");
                    else
                        Values.AppendFormat(",'{0}'", SQLUtility.Replace(etor.Current.Value.ToString()));*/
                }

                //remove comma
                if (Filed.Length > 0)
                {
                    Filed = Filed.Remove(0, 1);
                    Values = Values.Remove(0, 1);
                }
                else
                {
                    return null;
                }


                return string.Format("INSERT INTO {0} ({1}) VALUES ({2})",tablename,Filed.ToString(),Values.ToString());
            
        }
        /// <summary>
        /// get update sql
        /// </summary>
        /// <param name="where">更新条件</param>
        public virtual String GetUpdate(TableBase where)
        {
                StringBuilder tmp = new StringBuilder();
                //object property
                Dictionary<string, object>.Enumerator etor = Object.GetEnumerator();
                //Type type;
                while (etor.MoveNext())
                {
                    //type = etor.Current.Value.GetType();
                    //if (type == typeof(int) || type == typeof(bool))
                    //    tmp.AppendFormat(",{0}={1}",etor.Current.Key,etor.Current.Value);
                    //else if (etor.Current.Value == null || etor.Current.Value == DBNull.Value)
                    //    tmp.AppendFormat(",{0}=NULL", etor.Current.Key);
                    //else
                    //    tmp.AppendFormat(",{0}='{1}'",etor.Current.Key, SQLUtility.Replace(etor.Current.Value.ToString()));
                    tmp.AppendFormat(",{0}={1}{0}",etor.Current.Key,Factory.GetParameterSplitChar);
                    parameter.Add(Factory.CreateParameter(etor.Current.Key, etor.Current.Value));
                }
                if (tmp.Length > 0)
                {
                    //return string.Format("UPDATE {0} SET {1}{2}",tablename, tmp.Remove(0, 1).ToString(),GetWhere);
                    return string.Format("UPDATE {0} SET {1}{2}", tablename, tmp.Remove(0, 1).ToString(), GetWhere(where));
                }
                else
                {
                    return null;
                }
        }
        /// <summary>
        /// get select sql
        /// </summary>
        public virtual String GetSelect()
        {
                StringBuilder tmp = new StringBuilder("SELECT ");
                //set rows size
                if (Object.TableCondition.Size > 0)
                {
                    tmp.Append("TOP ");
                    tmp.Append(Object.TableCondition.Size);
                    tmp.Append(" ");
                }
                //set select filed
                tmp.Append(Object.TableCondition.Field);
                tmp.Append(" ");

                //set select table
                tmp.Append("FROM ");
                tmp.Append(tablename);
                tmp.Append(" ");

                //set select where
                tmp.Append(GetWhere());

                //set select order by
                if (Object.TableCondition.Order != null)
                {
                    tmp.Append(" ORDER BY ");
                    tmp.Append(Object.TableCondition.Order);
                }

                return tmp.ToString();
        }
    }
}

