using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Aooshi.DB
{
    /// <summary>
    /// ACCESS 资源产生类
    /// </summary>
    public class AccessTableToSQL : TableToSQL
    {
        string tablename;

        /// <summary>
        /// 初始化新对象
        /// </summary>
        /// <param name="Object">数据对象</param>
        /// <param name="factory">当前操作对象</param>
        protected internal AccessTableToSQL(TableBase Object, Factory factory) : base(Object, factory, new List<IDbDataParameter>())
        {
            tablename = Object.TableCondition.TableName;
        }

        private static Type dateType = typeof(DateTime);


        /// <summary>
        /// get object where
        /// </summary>
        /// <param name="Object">指定的条件生成对象</param>
        public override String GetWhere(TableBase Object)
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

                    if (etor.Current.Value != null && etor.Current.Value.GetType() == dateType)
                    {
                        tmp.AppendFormat("{0}=#{1}#", etor.Current.Key, etor.Current.Value);
                    }
                    else
                    {
                        tmp.AppendFormat("{0}={1}{0}", etor.Current.Key, Factory.GetParameterSplitChar);
                        parameter.Add(Factory.CreateParameter(etor.Current.Key, etor.Current.Value));
                    }
                }
                if (tmp.Length > 0) return " WHERE " + tmp.Remove(0, split.Length).ToString();
                return "";
        }

        /// <summary>
        /// get insert sql
        /// </summary>
        public override String GetInsert()
        {
                StringBuilder Filed = new StringBuilder();
                StringBuilder Values = new StringBuilder();
                //object property
                Dictionary<string, object>.Enumerator etor = Object.GetEnumerator();
                //Type type;
                while (etor.MoveNext())
                {
                    Filed.Append("," + etor.Current.Key);
                    if (etor.Current.Value != null && etor.Current.Value.GetType() == dateType)
                    {
                        Values.AppendFormat(",#{0}#", etor.Current.Value);
                    }
                    else
                    {
                        Values.AppendFormat(",{0}{1}", Factory.GetParameterSplitChar, etor.Current.Key);
                        parameter.Add(Factory.CreateParameter(etor.Current.Key, etor.Current.Value));
                    }
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


                return string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tablename, Filed.ToString(), Values.ToString());
            
        }
        /// <summary>
        /// get update sql
        /// </summary>
        /// <param name="where">更新条件</param>
        public override String GetUpdate(TableBase where)
        {
                StringBuilder tmp = new StringBuilder();
                //object property
                Dictionary<string, object>.Enumerator etor = Object.GetEnumerator();
                //Type type;
                while (etor.MoveNext())
                {
                    if (etor.Current.Value != null && etor.Current.Value.GetType() == dateType)
                    {
                        tmp.AppendFormat(",{0}=#{1}#", etor.Current.Key,etor.Current.Value);
                    }
                    else
                    {
                        tmp.AppendFormat(",{0}={1}{0}", etor.Current.Key, Factory.GetParameterSplitChar);
                        parameter.Add(Factory.CreateParameter(etor.Current.Key, etor.Current.Value));
                    }
                }
                if (tmp.Length > 0)
                {
                    return string.Format("UPDATE {0} SET {1}{2}", tablename, tmp.Remove(0, 1).ToString(), GetWhere(where));
                }
                else
                {
                    return null;
                }
        }
    }
}
