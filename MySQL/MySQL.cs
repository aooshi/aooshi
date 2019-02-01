using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Aooshi.DB
{
    /// <summary>
    /// MySql ���ݲ�����
    /// </summary>
    public class MySQL:Factory
    {
        /// <summary>
        /// ��ʼ����������
        /// </summary>
        /// <param name="connectionstring">���ݿ������ַ���</param>
        public MySQL(string connectionstring) : base(MySqlClientFactory.Instance, new MySqlConnection(connectionstring))
        {
        }


        /// <summary>
        /// ������,��䴴��
        /// </summary>
        /// <param name="tablebase">��䴴��</param>
        protected override TableToSQL CreateTableToSQL(TableBase tablebase)
        {
            return new MySqlTableToSQL(tablebase, this);
        }

        /// <summary>
        /// �����������������
        /// </summary>
        public const string FactoryName = "MySQL";

        /// <summary>
        /// ��ȡ��ǰ�����������������,�����볣��<see cref="FactoryName"/>һ��
        /// </summary>
        public override string Name
        {
            get
            {
                return MySQL.FactoryName;
            }
        }

        /// <summary>
        /// ���ɷ�ҳ��Sql���
        /// </summary>
        /// <param name="Field">�ֶ�ֵ,ǰ�󲻴��ո�,λ�� Select �� From ֮����ֶβ���ʾ</param>
        /// <param name="Table">Ҫ���в�ѯ�����ݱ�,��Ϊ���</param>
        /// <param name="Term">Ҫ���в�ѯ�Ĳ�ѯ��,Where�ĺ�׺,���δ��,������Ϊ <c>string.Empty</c></param>
        /// <param name="Key">�˲����ڴ�λ�����κ����ģ�������дNULL</param>
        /// <param name="Sort">���򷽷�,���δ��������Ϊstring.Empty</param>
        /// <param name="Group">���鷽������ʹ��������Ϊstring.Empty</param>
        /// <param name="RsStart">��¼��ʼ��</param>
        /// <param name="Size">��¼����</param>
        /// <param name="isDistinct">�Ƿ�ʹ��Distinct</param>
        /// <returns>�������ɺ��Sql���</returns>
        public override string MakeSql(string Field, string Table, string Term, string Key, string Sort, string Group, long RsStart, long Size, bool isDistinct)
        {
            string dist = (isDistinct) ? " DISTINCT" : "";

            //��ҳ
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
        /// ��ȡ��������ʱ,ʹ�õ�ǰ׺,Ĭ��ʹ��@��,�����ǰ�������治��,����д������
        /// </summary>
        protected override string GetParameterSplitChar
        {
            get { return "?"; }
        }
        /// <summary>
        /// ��ȡ��һִ�����������������ֵ(�˹��ܲ�һ��֧���������ݿ������)��������󣬷���Ϊ������ͳ�ʼֵ
        /// </summary>
        /// <typeparam name="T">���ص���������</typeparam>
        public override T GetIdentity<T>()
        {
            //object o = ((MySqlCommand)base.Connection.CreateCommand()).LastInsertedId;
            //return (T)Convert.ChangeType(o, typeof(T));
            return (T)Convert.ChangeType(base.GetScalar("SELECT LAST_INSERT_ID()"), typeof(T));
        }

        /// <summary>
        /// ��ȡһ��ʵ��SqlCreate����
        /// </summary>
        public override SqlCreate CreateSqlCreate()
        {
            return new MySqlCreate();
        }

    }
}
