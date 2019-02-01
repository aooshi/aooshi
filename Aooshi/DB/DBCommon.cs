using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace Aooshi.DB
{
    /// <summary>
    /// 数据库常规操作项
    /// </summary>
    public class DBCommon
    {
        /// <summary>
        /// 获取当前系统是否为调试状态
        /// </summary>
        public static readonly bool SqlDebug;
        /// <summary>
        /// 获取当前系统中默认的配置连接字符串
        /// </summary>
        public static readonly string DefaultConnectionString;

        /// <summary>
        /// static initialize
        /// </summary>
        static DBCommon()
        {
            SqlDebug = Common.GetAppSetting("SqlDebug") == "true";
            DefaultConnectionString = Common.GetConnection(DefaultConnectionName);
        }

        /// <summary>
        /// 默认的数据配置项名称
        /// </summary>
        public const string DefaultConnectionName = "Aooshi:Connection";


        /// <summary>
        /// 获取微软JET数据引擎连接字符串,JET默认版本4.0
        /// </summary>
        /// <param name="databasepath">数据库路径</param>
        public static string GetJetOleDbString(string databasepath)
        {
            return GetJetOleDbString(databasepath, "", "");
        }

        /// <summary>
        /// 获取微软JET数据引擎连接字符串,JET默认版本4.0
        /// </summary>
        /// <param name="databasepath">数据库路径</param>
        /// <param name="account">访问帐号</param>
        /// <param name="password">密码</param>
        public static string GetJetOleDbString(string databasepath,string account, string password)
        {
            return GetJetOleDbString(databasepath, "4.0", account, password);
        }
        /// <summary>
        /// 获取微软JET数据引擎连接字符串
        /// </summary>
        /// <param name="databasepath">数据库地址</param>
        /// <param name="jetversion">jet版本,默认请填写4.0</param>
        /// <param name="account">访问帐号</param>
        /// <param name="password">密码,没有请留空</param>
        public static string GetJetOleDbString(string databasepath,string jetversion,string account,string password)
        {
            string path = "Provider=Microsoft.Jet.OLEDB.{3};Data Source={0};User ID={1};Password={2};";
            return string.Format(path, databasepath, account, password, jetversion);
        }

        /// <summary>
        /// 将读取器转换为DataTable
        /// </summary>
        /// <param name="reader">数据读取器</param>
        public static DataTable IDataReaderToDataTable(IDataReader reader)
        {
            DataTable dt = new DataTable();

            bool init = false;
            dt.BeginLoadData();
            object[] vals = new object[0];
            while (reader.Read())
            {
                if (!init)
                {
                    init = true;
                    int fieldCount = reader.FieldCount;
                    for (int i = 0; i < fieldCount; ++i)
                    {
                        dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                    }
                    //vals = new object[fieldCount - 1];
                    vals = new object[fieldCount];
                }
                reader.GetValues(vals);
                dt.LoadDataRow(vals, true);
            }
            reader.Close();
            dt.EndLoadData();

            return dt;
        }

        /// <summary>
        /// 进行数据的安全串过滤,防止常规SQL注入
        /// </summary>
        /// <param name="input">要进行过滤的数据串</param>
        public static string Replace(string input)
        {
            if (null == input) return "";
            return input.Replace("'", "''");
        }


        /// <summary>
        /// 将指定的数据表转换为指定的对象列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="table">数据表</param>
        public static List<T> DataTableToDBItem<T>(DataTable table)
        {
            if (table == null) return new List<T>();

            Type t = typeof(T);
            T RO;
            List<T> list = new List<T>();
            int i;
            PropertyInfo pi;
            foreach (DataRow dr in table.Rows)
            {
                RO = Activator.CreateInstance<T>();
                i = -1;
                while (++i < table.Columns.Count)
                {
                    pi = t.GetProperty(table.Columns[i].ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (null != pi)
                        pi.SetValue(RO, (dr[i] == DBNull.Value) ? null : dr[i], null);
                }

                list.Add(RO);
            }

            return list;
        }
        /// <summary>
        /// 将指定的读取器转换为指定类型对象列表
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="Reader">读取器</param>
        public static List<T> DataReaderToDBItems<T>(IDataReader Reader)
        {
            if (Reader == null) return new List<T>();
            List<T> list = new List<T>();
            Type type = typeof(T);
            PropertyInfo info;
            while (Reader.Read())
            {
                T obj = Activator.CreateInstance<T>();
                for (int index = 0; index < Reader.FieldCount; index++)
                {
                    info = type.GetProperty(Reader.GetName(index), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (info != null)
                        info.SetValue(obj, Reader.IsDBNull(index) ? null : Reader.GetValue(index), null);
                }
                list.Add(obj);
            }
            Reader.Close();
            return list;
        }

        /// <summary>
        /// 将指定的读取器中,第一行转换为指定类型对象
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="Reader">读取器</param>
        public static T DataReaderToDBItem<T>(IDataReader Reader)
        {
            if (Reader != null && Reader.Read())
            {
                Type type = typeof(T);
                T obj = Activator.CreateInstance<T>();
                PropertyInfo info;
                for (int index = 0; index < Reader.FieldCount; index++)
                {
                    info = type.GetProperty(Reader.GetName(index), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (null != info)
                        info.SetValue(obj, Reader.IsDBNull(index) ? null : Reader.GetValue(index), null);
                }
                Reader.Close();
                return obj;
            }
            return default(T);
        }

        /// <summary>
        /// 生成Count语句
        /// </summary>
        /// <param name="Table">要进行查询的数据表,可为多个</param>
        /// <param name="Term">要进行查询的查询串,Where的后缀,如果未有,请设置为Empty</param>
        /// <returns>返回生成后的Sql语句</returns>
        public static string MakeCountSql(string Table, string Term)
        {
            string sql = "SELECT COUNT(*) FROM " + Table;

            if (Term != null && Term != "")
            {
                sql += " WHERE " + Term;
            }

            return sql;
        }
    }
}
