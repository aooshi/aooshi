using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Aooshi.DB
{
    /// <summary>
    /// MySql 数据操作类
    /// </summary>
    public class MySQL:Factory
    {
        /// <summary>
        /// 初始化操作类型
        /// </summary>
        /// <param name="connectionstring">数据库连接字符串</param>
        public MySQL(string connectionstring) : base(MySqlClientFactory.Instance, new MySqlConnection(connectionstring))
        {
        }


        /// <summary>
        /// 已重载,语句创建
        /// </summary>
        /// <param name="tablebase">语句创建</param>
        protected override TableToSQL CreateTableToSQL(TableBase tablebase)
        {
            return new MySqlTableToSQL(tablebase, this);
        }

        /// <summary>
        /// 操作对象的名称描述
        /// </summary>
        public const string FactoryName = "MySQL";

        /// <summary>
        /// 获取当前操作对象的名称描述,该名与常量<see cref="FactoryName"/>一致
        /// </summary>
        public override string Name
        {
            get
            {
                return MySQL.FactoryName;
            }
        }

        /// <summary>
        /// 生成分页用Sql语句
        /// </summary>
        /// <param name="Field">字段值,前后不带空格,位于 Select 与 From 之间的字段部表示</param>
        /// <param name="Table">要进行查询的数据表,可为多个</param>
        /// <param name="Term">要进行查询的查询串,Where的后缀,如果未有,请设置为 <c>string.Empty</c></param>
        /// <param name="Key">此参数在此位置无任何意文，建议填写NULL</param>
        /// <param name="Sort">排序方法,如果未有排序则为string.Empty</param>
        /// <param name="Group">分组方法，不使用请设置为string.Empty</param>
        /// <param name="RsStart">记录开始数</param>
        /// <param name="Size">记录总数</param>
        /// <param name="isDistinct">是否使用Distinct</param>
        /// <returns>返回生成后的Sql语句</returns>
        public override string MakeSql(string Field, string Table, string Term, string Key, string Sort, string Group, long RsStart, long Size, bool isDistinct)
        {
            string dist = (isDistinct) ? " DISTINCT" : "";

            //分页
            System.Text.StringBuilder Sb = new System.Text.StringBuilder();
            Sb.AppendFormat("select{0} {1} from {2}", dist, Field, Table);

            if (!string.IsNullOrEmpty(Term))
                Sb.Append(" where " + Term);

            if (!string.IsNullOrEmpty(Group))
                Sb.Append(" group by " + Group);

            if (!string.IsNullOrEmpty(Sort))
                Sb.Append(" order by " + Sort);

            if (RsStart > 0)
                Sb.AppendFormat(" limit {0},{1}",RsStart,Size);
            else
                Sb.AppendFormat(" limit {0}",Size);

            return Sb.ToString();
        }

        /// <summary>
        /// 获取创建参数时,使用的前缀,默认使用@符,如果当前操作引擎不是,请重写该属性
        /// </summary>
        protected override string GetParameterSplitChar
        {
            get { return "?"; }
        }
        /// <summary>
        /// 获取上一执行语句所产生的自增值(此功能不一定支持所有数据库服务器)，如果有误，返回为结果类型初始值
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        public override T GetIdentity<T>()
        {
            //object o = ((MySqlCommand)base.Connection.CreateCommand()).LastInsertedId;
            //return (T)Convert.ChangeType(o, typeof(T));
            return (T)Convert.ChangeType(base.GetScalar("SELECT LAST_INSERT_ID()"), typeof(T));
        }

        /// <summary>
        /// 获取一个实例SqlCreate创建
        /// </summary>
        public override SqlCreate CreateSqlCreate()
        {
            return new MySqlCreate();
        }

    }
}
