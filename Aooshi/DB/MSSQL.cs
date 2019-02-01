using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Aooshi.DB
{
    /// <summary>
    /// MSSQL数据库基础操作
    /// </summary>
    /// <include file='../docs/DB.Factory.xml' path='docs/*'/>
    public class MSSQL : Factory,IDisposable,IFactory
    {
        /// <summary>
        /// 从配置中获取默认的配置连接串进行初始化
        /// </summary>
        public MSSQL():base(SqlClientFactory.Instance, new SqlConnection(DBCommon.DefaultConnectionString))
        {
        }
        /// <summary>
        /// 根据数据连接字符串创建一个新的数据实列
        /// </summary>
        /// <param name="connectionstring">数据连接字符串</param>
        public MSSQL(string connectionstring) : base(SqlClientFactory.Instance, new SqlConnection(connectionstring))
        {
        }
        /// <summary>
        /// 创建新的实例
        /// </summary>
        /// <param name="connection">数据库连接</param>
        public MSSQL(SqlConnection connection): base(SqlClientFactory.Instance, connection)
        {
        }


        /// <summary>
        /// 操作对象的名称描述
        /// </summary>
        public const string FactoryName = "MSSQL";

        /// <summary>
        /// 获取当前操作对象的名称描述,该名与常量<see cref="FactoryName"/>一致
        /// </summary>
        public override string Name
        {
            get
            {
                return MSSQL.FactoryName;
            }
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        public new SqlConnection Connection
        {
            get { return (SqlConnection)base.Connection; }
        }

        /// <summary>
        /// 生成分页用Sql语句
        /// </summary>
        /// <param name="Field">字段值,前后不带空格,位于 Select 与 From 之间的字段部表示</param>
        /// <param name="Table">要进行查询的数据表,可为多个</param>
        /// <param name="Term">要进行查询的查询串,Where的后缀,如果未有,请设置为 <c>string.Empty</c></param>
        /// <param name="Key">此参数在此位置无任何意义，不管其填充为何值，均会自动设置为null</param>
        /// <param name="Sort">排序方法,如果未有排序则为string.Empty</param>
        /// <param name="Group">分组方法，不使用请设置为string.Empty</param>
        /// <param name="RsStart">记录开始数</param>
        /// <param name="Size">记录总数</param>
        /// <param name="isDistinct">是否使用Distinct</param>
        /// <returns>返回生成后的Sql语句</returns>
        /// <exception cref="ArgumentNullException">Sort为空时异常</exception>
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

            if (string.IsNullOrEmpty(Sort)) throw new ArgumentNullException("Sort", "分页时排序规则不得为空！");

            //分页
            System.Text.StringBuilder Sb = new System.Text.StringBuilder();
            Sb.AppendFormat("WITH _SYS_Orderd AS (SELECT{0} TOP {1} {2},ROW_NUMBER() OVER (Order By {3}) AS _SYS_Rows FROM {4}",dist, RsStart + Size, Field, Sort, Table);
            if (!string.IsNullOrEmpty(Term)) Sb.Append(" WHERE " + Term);
            if (!string.IsNullOrEmpty(Group)) Sb.Append(" GROUP BY  " + Group);
            Sb.AppendFormat(") SELECT * FROM _SYS_Orderd WHERE _SYS_Rows>{0} ", RsStart);
            Sb.Append("ORDER BY _SYS_Rows ASC");
            //Sb.Append(Sort);
            return Sb.ToString();


        }
    }
}
