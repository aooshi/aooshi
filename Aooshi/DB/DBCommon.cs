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
    /// ���ݿⳣ�������
    /// </summary>
    public class DBCommon
    {
        /// <summary>
        /// ��ȡ��ǰϵͳ�Ƿ�Ϊ����״̬
        /// </summary>
        public static readonly bool SqlDebug;
        /// <summary>
        /// ��ȡ��ǰϵͳ��Ĭ�ϵ����������ַ���
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
        /// Ĭ�ϵ���������������
        /// </summary>
        public const string DefaultConnectionName = "Aooshi:Connection";


        /// <summary>
        /// ��ȡ΢��JET�������������ַ���,JETĬ�ϰ汾4.0
        /// </summary>
        /// <param name="databasepath">���ݿ�·��</param>
        public static string GetJetOleDbString(string databasepath)
        {
            return GetJetOleDbString(databasepath, "", "");
        }

        /// <summary>
        /// ��ȡ΢��JET�������������ַ���,JETĬ�ϰ汾4.0
        /// </summary>
        /// <param name="databasepath">���ݿ�·��</param>
        /// <param name="account">�����ʺ�</param>
        /// <param name="password">����</param>
        public static string GetJetOleDbString(string databasepath,string account, string password)
        {
            return GetJetOleDbString(databasepath, "4.0", account, password);
        }
        /// <summary>
        /// ��ȡ΢��JET�������������ַ���
        /// </summary>
        /// <param name="databasepath">���ݿ��ַ</param>
        /// <param name="jetversion">jet�汾,Ĭ������д4.0</param>
        /// <param name="account">�����ʺ�</param>
        /// <param name="password">����,û��������</param>
        public static string GetJetOleDbString(string databasepath,string jetversion,string account,string password)
        {
            string path = "Provider=Microsoft.Jet.OLEDB.{3};Data Source={0};User ID={1};Password={2};";
            return string.Format(path, databasepath, account, password, jetversion);
        }

        /// <summary>
        /// ����ȡ��ת��ΪDataTable
        /// </summary>
        /// <param name="reader">���ݶ�ȡ��</param>
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
        /// �������ݵİ�ȫ������,��ֹ����SQLע��
        /// </summary>
        /// <param name="input">Ҫ���й��˵����ݴ�</param>
        public static string Replace(string input)
        {
            if (null == input) return "";
            return input.Replace("'", "''");
        }


        /// <summary>
        /// ��ָ�������ݱ�ת��Ϊָ���Ķ����б�
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="table">���ݱ�</param>
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
        /// ��ָ���Ķ�ȡ��ת��Ϊָ�����Ͷ����б�
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="Reader">��ȡ��</param>
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
        /// ��ָ���Ķ�ȡ����,��һ��ת��Ϊָ�����Ͷ���
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="Reader">��ȡ��</param>
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
        /// ����Count���
        /// </summary>
        /// <param name="Table">Ҫ���в�ѯ�����ݱ�,��Ϊ���</param>
        /// <param name="Term">Ҫ���в�ѯ�Ĳ�ѯ��,Where�ĺ�׺,���δ��,������ΪEmpty</param>
        /// <returns>�������ɺ��Sql���</returns>
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
