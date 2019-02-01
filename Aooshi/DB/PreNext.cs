using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// 数据对象,上一条,该条,下一条,三记录SQL语句生成
    /// </summary>
    public class PreNext
    {
        string _KeyName;
        string _KeyValue;
        string _Table;
        string _Where;
        string _KeyStr;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="KeyName">键名称(此键必须为可排序且上下条数是以该键的排序所得)</param>
        /// <param name="KeyValue">键的值</param>
        /// <param name="Table">表名称</param>
        public PreNext(string KeyName, string KeyValue, string Table)
            : this(KeyName, KeyValue, Table, null)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="KeyName">键名称(此键必须为可排序且上下条数是以该键的排序所得)</param>
        /// <param name="KeyValue">键的值</param>
        /// <param name="Table">表名称</param>
        /// <param name="Where">附加条件语句,不设置请设置为null</param>
        public PreNext(string KeyName, string KeyValue, string Table, string Where)
        {
            _KeyName = DBCommon.Replace(KeyName);
            _Table = DBCommon.Replace(Table);
            _KeyValue = KeyValue;
            _Where = string.IsNullOrEmpty(Where) ? "" : Where;
            _KeyStr = "";
        }


        /*
        select top 1 * from infotab where iid < '200709162040515754' order by iid desc
        select top 1 * from infotab where iid = '200709162040515754'
        select top 1 * from infotab where iid > '200709162040515754' order by iid asc
        */

        /// <summary>
        /// 生成上一条数据用语句
        /// </summary>
        public virtual string Pre() { return Pre(null); }

        /// <summary>
        /// 生成上一条数据用语句
        /// </summary>
        /// <param name="Fields">返回的字段数据</param>
        public virtual string Pre(string Fields)
        {
            Fields = RepField(Fields);
            string sql = "SELECT TOP 1 {0} FROM {1} WHERE {2}<{3} {4} ORDER BY {2} DESC";
            return string.Format(sql, Fields, _Table, _KeyName, KeyValue, _Where);
        }

        /// <summary>
        /// 生成本条数据用语句
        /// </summary>
        public virtual string This() { return This(null); }
        /// <summary>
        /// 生成本条数据用语句
        /// </summary>
        /// <param name="Fields">返回的字段数据</param>
        public virtual string This(string Fields)
        {
            Fields = RepField(Fields);
            string sql = "SELECT TOP 1 {0} FROM {1} WHERE {2}={3} {4}";
            return string.Format(sql, Fields, _Table, _KeyName, KeyValue, _Where);
        }


        /// <summary>
        /// 生成下一条数据用语句
        /// </summary>
        public virtual string Next() { return Next(null); }

        /// <summary>
        /// 生成下一条数据用语句
        /// </summary>
        /// <param name="Fields">返回的字段数据</param>
        public virtual string Next(string Fields)
        {
            Fields = RepField(Fields);
            string sql = "SELECT TOP 1 {0} FROM {1} WHERE {2}>{3} {4} ORDER BY {2} ASC";
            return string.Format(sql, Fields, _Table, _KeyName, KeyValue, _Where);
        }

        /// <summary>
        /// 进行字段赋值处理
        /// </summary>
        /// <param name="Fields">字段</param>
        protected virtual string RepField(string Fields)
        {
            return string.IsNullOrEmpty(Fields) ? "*" : DBCommon.Replace(Fields);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        protected string KeyValue
        {
            get { return KeyStr + _KeyValue + KeyStr; }
        }
        /// <summary>
        /// 获取或设置值所须要的包括符,默认没有任何包括符,如:字符串值应该加单引号将其括起来;
        /// </summary>
        public string KeyStr
        {
            get { return _KeyStr; }
            set { _KeyStr = (_KeyValue == null) ? "" : value; }
        }
    }
}
