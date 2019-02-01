using System;
using System.Data;
using System.Data.OleDb;

namespace Aooshi.DB
{
    /// <summary>
    /// Access数据库操作对象
    /// </summary>
    /// <include file='../docs/DB.Factory.xml' path='docs/*'/>
    public class Access : Factory, IDisposable, IFactory
    {
        /// <summary>
        /// 根据数据连接字符串创建一个新的数据实列
        /// </summary>
        /// <param name="connectionstring">数据连接字符串</param>
        public Access(string connectionstring) : base(OleDbFactory.Instance,new OleDbConnection(connectionstring))
        {
        }
        /// <summary>
        /// 创建新的实例
        /// </summary>
        /// <param name="connection">数据库连接</param>
        public Access(OleDbConnection connection) : base(OleDbFactory.Instance, connection)
        {
        }

        /// <summary>
        /// 操作对象的名称描述
        /// </summary>
        public const string FactoryName = "Access";

        /// <summary>
        /// 获取当前操作对象的名称描述,该名与常量<see cref="FactoryName"/>一致
        /// </summary>
        public override string Name
        {
            get
            {
                return Access.FactoryName;
            }
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        public new OleDbConnection Connection
        {
            get { return (OleDbConnection)base.Connection; }
        }

        /// <summary>
        /// 已重载
        /// </summary>
        /// <param name="tablebase"></param>
        /// <returns></returns>
        protected override TableToSQL CreateTableToSQL(TableBase tablebase)
        {
            return new AccessTableToSQL(tablebase,this);
        }

        /// <summary>
        /// 生成分页用Sql语句
        /// </summary>
        /// <param name="Field">字段值,前后不带空格,位于 Select 与 From 之间的字段部表示</param>
        /// <param name="Table">要进行查询的数据表,可为多个</param>
        /// <param name="Term">要进行查询的查询串,Where的后缀,如果未有,请设置为 <c>string.Empty</c></param>
        /// <param name="Key">进行行行not in 分页的唯一主键</param>
        /// <param name="Sort">排序方法,如果未有排序则为string.Empty</param>
        /// <param name="Group">分组方法，不使用请设置为string.Empty</param>
        /// <param name="RsStart">记录开始数</param>
        /// <param name="Size">记录总数</param>
        /// <param name="isDistinct">是否使用Distinct</param>
        /// <returns>返回生成后的Sql语句</returns>
        public override string MakeSql(string Field, string Table, string Term, string Key, string Sort, string Group, long RsStart, long Size, bool isDistinct)
        {
            string dist = (isDistinct) ? " DISTINCT" : "";
            //第一页
            if (RsStart < Size)
            {
                string Sql = string.Format("Select{0} Top {1} {2} From {3}",dist, Size, Field, Table);

                if (!string.IsNullOrEmpty(Term))
                    Sql += " Where " + Term;

                if (!string.IsNullOrEmpty(Group))
                    Sql += " Group By " + Group;

                if (!string.IsNullOrEmpty(Sort))
                    Sql += " Order By " + Sort;

                return Sql;
            }

            //分页
            System.Text.StringBuilder Sb = new System.Text.StringBuilder();

            Sb.AppendFormat("Select{0} Top {1} {2} From {3} Where {4} Not In (",dist, Size, Field, Table, Key);

            Sb.AppendFormat("Select{0} Top {1} {2} From {3}",dist, RsStart, Key, Table);

            if (!string.IsNullOrEmpty(Term))
                Sb.Append(" Where " + Term);

            if (!string.IsNullOrEmpty(Group))
                Sb.Append(" Group By " + Group);

            if (!string.IsNullOrEmpty(Sort))
                Sb.Append(" Order By " + Sort);

            Sb.Append(")"); //条件完成

            if (!string.IsNullOrEmpty(Term))
                Sb.Append(" And " + Term);

            if (!string.IsNullOrEmpty(Group))
                Sb.Append(" Group By " + Group);

            if (!string.IsNullOrEmpty(Sort))
                Sb.Append(" Order By " + Sort);

            return Sb.ToString();
        }

        /// <summary>
        /// 根据类型返回可直接用于SQL语句的安全字符串
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="type">类型</param>
        public override string GetSafeString(object value, Type type)
        {
            if (type == TypeConst.typeDateTime)  return "#" + Convert.ToString(value) + "#";


            if (type == TypeConst.typeBoolean) return Convert.ToString(value);

            return base.GetSafeString(value, type);
        }
    }
}
