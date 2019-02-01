using System;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;

namespace Aooshi.DB
{
    /// <summary>
    /// ���ݹ����ӿ�
    /// </summary>
    /// <include file='../docs/DB.Factory.xml' path='docs/*'/>
    public interface IFactory : IDisposable
    {
        /// <summary>
        /// ��ȡ��ǰ������������
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ���һ��Command SQL
        /// </summary>
        string LastSQL { get; }

        /// <summary>
        /// �ر����ݿ�����ʱ
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// ����һ��֧�ֱ�ʵ���ӿڵ�<see cref="SqlCreate"/>����
        /// </summary>
        /// <returns>�µ� SqlCreate ʵ��</returns>
        SqlCreate CreateSqlCreate();

        /// <summary>
        /// �����������ͷ���һ������ֱ��Ӧ����SQL���İ�ȫ�ַ���
        /// </summary>
        /// <param name="value">Ҫ��ӵ�ֵ</param>
        string GetSafeString(object value);


        /// <summary>
        /// �����������ͷ���һ������ֱ��Ӧ����SQL���İ�ȫ�ַ���
        /// </summary>
        /// <param name="value">Ҫ��ӵ�ֵ</param>
        /// <param name="type">����</param>
        string GetSafeString(object value, Type type);


        /// <summary>
        /// SQLע�����
        /// </summary>
        /// <param name="input">Ҫ���˵�����</param>
        string InjectReplace(string input);


        /// <summary>
        /// ��ȡCommand������ӷ�Ӧ��ִ���˶��ٴ����ݲ���
        /// </summary>
        int CommandCount { get; }


        /// <summary>
        /// �������ݿ��л�(ע��:�����κΰ汾���ݿ��֧�ִ˹���)
        /// </summary>
        /// <param name="NewDBName">�����ݿ���</param>
        void ChangeDataBase(string NewDBName);


        /// <summary>
        /// �����ݿ�����
        /// </summary>
        void Open();



        /// <summary>
        /// �ر����ݿ�����
        /// </summary>
        void Close();


        /// <summary>
        /// ��ȡ��ǰ���ݿ����Ӵ�
        /// </summary>
        IDbConnection Connection { get; }


        /// <summary>
        /// ��ȡDataLogic���ݲ����߼�
        /// </summary>
        /// <param name="typename">������,������һ���޲������죬��ʵ��<see cref="Aooshi.DB.ILogic"/></param>
        ILogic GetDataLogic(string typename);

        /// <summary>
        /// ��ȡDataLogic���ݲ����߼�
        /// </summary>
        /// <param name="type">����,������һ���޲������죬��ʵ��<see cref="Aooshi.DB.ILogic"/></param>
        ILogic GetDataLogic(Type type);

        #region ����

        /// <summary>
        /// ��ȡ�Ƿ�Ϊ���У�Ƕ�ף�����
        /// </summary>
        bool TransactionNested { get; }

        /// <summary>
        /// ��ȡΪ����(Ƕ��)����ʱ�ĵ�ǰ����㼶
        /// </summary>
        int TransactionLayer { get; }

        /// <summary>
        /// ��ʼһ������ִ��
        /// </summary>
        /// <returns>����Ƕ�ײ㼶</returns>
        int Begin();

        /// <summary>
        /// �ύһ������ִ��
        /// </summary>
        void Commit();

        /// <summary>
        /// �ύһ������ִ��
        /// </summary>
        /// <param name="endNested">�Ƿ�ֱ����ɲ�������,ע�⣺��ֵΪ<see cref="Boolean"/>ʱ������������������񣬷�����Χ�ύ����Ч.</param>
        void Commit(bool endNested);

        /// <summary>
        /// �ع�����,�����������ѻع�ʱ������
        /// </summary>
        /// <returns>����������ʱ�����Ƿ�ִ���˻ع��������ǲ������񷵻ع̶�<see cref="Boolean"/></returns>
        void Rollback();

        /// <summary>
        /// ��ȡ��ǰ����ִ�е�����
        /// </summary>
        IDbTransaction Transaction { get; }

        #endregion

        #region execute


        /// <summary>
        /// ִ��һ���������
        /// </summary>
        /// <param name="SqlString">Ҫִ�еĲ������</param>
        int Execute(string SqlString);

        /// <summary>
        /// ִ��һ���������
        /// </summary>
        /// <param name="SqlString">Ҫִ�еĲ������</param>
        /// <param name="StoredProcedure">�Ƿ��Դ洢����ִ��</param>
        int Execute(string SqlString, bool StoredProcedure);


        /// <summary>
        /// ִ��һ���������
        /// </summary>
        /// <param name="SqlString">Ҫִ�еĲ������</param>
        /// <param name="StoredProcedure">�Ƿ��Դ洢����ִ��</param>
        /// <param name="Parames">�����б�</param>
        int Execute(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames);


        /// <summary>
        /// ִ�����ݶ����������
        /// </summary>
        /// <param name="tablebase">���ݶ���</param>
        int Insert(TableBase tablebase);


        /// <summary>
        /// ִ�����ݶ���ɾ������
        /// </summary>
        /// <param name="tablebase">����</param>
        int Delete(TableBase tablebase);


        /// <summary>
        /// ִ�����ݶ����޸�
        /// </summary>
        /// <param name="tablebase">���ݶ���</param>
        /// <param name="where">��������</param>
        int Update(TableBase tablebase, TableBase where);


        #endregion

        #region get
        /// <summary>
        /// ��ȡ��һִ�����������������ֵ(�˹��ܲ�һ��֧���������ݿ������)��������󣬷���Ϊ������ͳ�ʼֵ
        /// </summary>
        /// <typeparam name="T">���ص���������</typeparam>
        T GetIdentity<T>();


        /// <summary>
        /// ����һ�����ݼ�
        /// </summary>
        /// <param name="tablebase">���ݶ���</param>
        DataSet GetDataSet(TableBase tablebase);


        /// <summary>
        /// ����һ�����ݼ�
        /// </summary>
        /// <param name="SqlString">Ҫִ�е����</param>
        DataSet GetDataSet(string SqlString);


        /// <summary>
        /// ����һ�����ݼ�
        /// </summary>
        /// <param name="SqlString">Ҫִ�е��������</param>
        /// <param name="StoredProcedure">�Ƿ�Ϊ�洢����</param>
        DataSet GetDataSet(string SqlString, bool StoredProcedure);


        /// <summary>
        /// ����һ�����ݼ�
        /// </summary>
        /// <param name="SqlString">Ҫִ�е��������</param>
        /// <param name="StoredProcedure">�Ƿ�Ϊ�洢����</param>
        /// <param name="Parames">�����б�</param>
        DataSet GetDataSet(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames);



        /// <summary>
        /// ����һ�����ݱ�
        /// </summary>
        /// <param name="tablebase">���ݶ���</param>
        DataTable GetDataTable(TableBase tablebase);


        /// <summary>
        /// ����һ�����ݱ�
        /// </summary>
        /// <param name="SqlString">Ҫִ�е����</param>
        DataTable GetDataTable(string SqlString);


        /// <summary>
        /// ����һ�����ݱ�
        /// </summary>
        /// <param name="SqlString">Ҫִ�е��������</param>
        /// <param name="StoredProcedure">�Ƿ�Ϊ�洢����</param>
        DataTable GetDataTable(string SqlString, bool StoredProcedure);


        /// <summary>
        /// ����һ�����ݱ�
        /// </summary>
        /// <param name="SqlString">Ҫִ�е��������</param>
        /// <param name="StoredProcedure">�Ƿ�Ϊ�洢����</param>
        /// <param name="Parames">�����б�</param>
        DataTable GetDataTable(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames);


        /// <summary>
        /// ��ȡ��һ�е�һ�е�����
        /// </summary>
        /// <param name="SqlString">Ҫִ�е����</param>
        Object GetScalar(string SqlString);


        /// <summary>
        /// ��ȡ��һ�е�һ�е�����
        /// </summary>
        /// <param name="SqlString">Ҫִ�е����</param>
        /// <param name="StoredProcedure">�Ƿ�Ϊ�洢����</param>
        Object GetScalar(string SqlString, bool StoredProcedure);


        /// <summary>
        /// ��ȡ��һ�е�һ�е�����
        /// </summary>
        /// <param name="SqlString">Ҫִ�е����</param>
        /// <param name="StoredProcedure">�Ƿ�Ϊ�洢����</param>
        /// <param name="Parames">�����б�</param>
        Object GetScalar(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames);



        /// <summary>
        /// ��ȡ��һ�е�һ�е�����
        /// </summary>
        /// <param name="Object">����</param>
        Object GetScalar(TableBase Object);


        /// <summary>
        /// ��ȡ��һ�е�һ�е�����
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="SqlString">Ҫִ�е����</param>
        T GetScalar<T>(string SqlString);


        /// <summary>
        /// ��ȡ��һ�е�һ�е�����
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="SqlString">Ҫִ�е����</param>
        /// <param name="StoredProcedure">�Ƿ�Ϊ�洢����</param>
        T GetScalar<T>(string SqlString, bool StoredProcedure);


        /// <summary>
        /// ��ȡ��һ�е�һ�е�����
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="SqlString">Ҫִ�е����</param>
        /// <param name="StoredProcedure">�Ƿ�Ϊ�洢����</param>
        /// <param name="Parames">�����б�</param>
        T GetScalar<T>(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames);


        /// <summary>
        /// ��ȡ��һ�е�һ�е�����
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="tablebase">����</param>
        T GetScalar<T>(TableBase tablebase);


        /// <summary>
        /// ��ȡָ������һ�е�һ����Int��������
        /// </summary>
        /// <param name="SqlString">ִ�����</param>
        /// <param name="StoredProcedure">�Ƿ��Թ���ִ��</param>
        int GetCount(string SqlString, bool StoredProcedure);


        /// <summary>
        /// ��ȡָ������һ�е�һ����Int��������
        /// </summary>
        /// <param name="SqlString">ִ�����</param>
        int GetCount(string SqlString);


        /// <summary>
        /// ��ȡָ������ļ�¼��
        /// </summary>
        /// <param name="tablebase">����</param>
        int GetCount(TableBase tablebase);


        /// <summary>
        /// ����һ�����ݶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="SqlString">��ѯ���</param>
        List<T> GetList<T>(string SqlString);


        /// <summary>
        /// ����һ�����ݶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="SqlString">��ѯ���</param>
        /// <param name="StoredProcedure">�Ƿ�Ϊ�洢����</param>
        List<T> GetList<T>(string SqlString, bool StoredProcedure);


        /// <summary>
        /// ����һ�����ݶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="SqlString">��ѯ���</param>
        /// <param name="StoredProcedure">�Ƿ�Ϊ�洢����</param>
        /// <param name="Parames">�����б�</param>
        List<T> GetList<T>(string SqlString, bool StoredProcedure, System.Data.IDbDataParameter[] Parames);


        /// <summary>
        /// ����һ�����ݶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="tablebase">��ѯ��������</param>
        List<T> GetList<T>(TableBase tablebase);


        /// <summary>
        /// ����һ�����ݶ���
        /// </summary>
        /// <typeparam name="T">���ݶ�������</typeparam>
        /// <param name="SqlString">��ѯ���</param>
        T GetOnlyRow<T>(string SqlString);


        /// <summary>
        /// ����һ�����ݶ���
        /// </summary>
        /// <typeparam name="T">���ݶ�������</typeparam>
        /// <param name="SqlString">��ѯ�������</param>
        /// <param name="StoredProcedure">�Ƿ��Դ洢����ִ��</param>
        T GetOnlyRow<T>(string SqlString, bool StoredProcedure);


        /// <summary>
        /// ����һ�����ݶ���
        /// </summary>
        /// <typeparam name="T">���ݶ�������</typeparam>
        /// <param name="SqlString">��ѯ�������</param>
        /// <param name="StoredProcedure">�Ƿ��Դ洢����ִ��</param>
        /// <param name="Parames">�����б�</param>
        T GetOnlyRow<T>(string SqlString, bool StoredProcedure, System.Data.IDbDataParameter[] Parames);


        /// <summary>
        /// ����һ�����ݶ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="tablebase">��ѯ����</param>
        T GetOnlyRow<T>(TableBase tablebase);
        #endregion

        #region Parames


        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="Name">����</param>
        /// <param name="Value">����ֵ</param>
        IDbDataParameter CreateParameter(string Name, object Value);
        #endregion

        #region ��ҳ
        /// <summary>
        /// ���ɷ�ҳ��Sql���
        /// </summary>
        /// <param name="Field">�ֶ�ֵ,ǰ�󲻴��ո�,λ�� Select �� From ֮����ֶβ���ʾ</param>
        /// <param name="Table">Ҫ���в�ѯ�����ݱ�,��Ϊ���</param>
        /// <param name="Term">Ҫ���в�ѯ�Ĳ�ѯ��,Where�ĺ�׺,���δ��,������Ϊ <c>string.Empty</c></param>
        /// <param name="Key">Ҫ����Not In �����ļ�ֵ,��Ϊ�����ֶ�����[]������</param>
        /// <param name="Sort">���򷽷�,���δ��������Ϊstring.Empty</param>
        /// <param name="RsStart">��¼��ʼ��</param>
        /// <param name="Size">��¼����</param>
        /// <returns>�������ɺ��Sql���</returns>
        string MakeSql(string Field, string Table, string Term, string Key, string Sort, long RsStart, long Size);

        /// <summary>
        /// ���ɷ�ҳ��Sql���
        /// </summary>
        /// <param name="Field">�ֶ�ֵ,ǰ�󲻴��ո�,λ�� Select �� From ֮����ֶβ���ʾ</param>
        /// <param name="Table">Ҫ���в�ѯ�����ݱ�,��Ϊ���</param>
        /// <param name="Term">Ҫ���в�ѯ�Ĳ�ѯ��,Where�ĺ�׺,���δ��,������Ϊ <c>string.Empty</c></param>
        /// <param name="Key">Ҫ�����ų������ļ�ֵ,��ֵһ��Ϊ����</param>
        /// <param name="Sort">���򷽷�,���δ��������Ϊstring.Empty</param>
        /// <param name="Group">���鷽������ʹ��������Ϊstring.Empty</param>
        /// <param name="RsStart">��¼��ʼ��</param>
        /// <param name="Size">��¼����</param>
        /// <param name="isDistinct">�Ƿ�ʹ��Distinct</param>
        /// <returns>�������ɺ��Sql���</returns>
        string MakeSql(string Field, string Table, string Term, string Key, string Sort, string Group, long RsStart, long Size, bool isDistinct);


        /// <summary>
        /// ���ɷ�ҳ�ü�Count���
        /// </summary>
        /// <param name="Table">Ҫ���в�ѯ�����ݱ�,��Ϊ���</param>
        /// <param name="condition">Ҫ���в�ѯ�Ĳ�ѯ��,Where�ĺ�׺,���δ��,������ΪEmpty</param>
        /// <returns>�������ɺ��Sql���</returns>
        string MakeCountSql(string Table, string condition);



        /// <summary>
        /// ���ݱ�����������һ��COUNTֵ
        /// </summary>
        /// <param name="Table">����</param>
        /// <param name="condition">����</param>
        int GetCountByMake(string Table, string condition);

        /// <summary>
        /// ����һ����ҳ�б����
        /// </summary>
        /// <param name="Field">�ֶ�ֵ,ǰ�󲻴��ո�,λ�� Select �� From ֮����ֶβ���ʾ</param>
        /// <param name="Table">Ҫ���в�ѯ�����ݱ�,��Ϊ���</param>
        /// <param name="Term">Ҫ���в�ѯ�Ĳ�ѯ��,Where�ĺ�׺,���δ��,������Ϊ <c>string.Empty</c></param>
        /// <param name="Key">Ҫ����Not In �����ļ�ֵ,��Ϊ�����ֶ�����[]������</param>
        /// <param name="Sort">���򷽷�,���δ��������Ϊstring.Empty</param>
        /// <param name="RsStart">��¼��ʼ��</param>
        /// <param name="Size">��¼����</param>
        /// <returns>�������ɺ��Sql���</returns>
        List<T> GetListByMakeSql<T>(string Field, string Table, string Term, string Key, string Sort, long RsStart, long Size);


        #endregion

        #region ע�ᵽ��ҳ

        /// <summary>
        /// �Ƿ���ע�ᵽҳ
        /// </summary>
        bool IsRegisteredWebPage { get; }


        /// <summary>
        /// ע��ҳ�Զ��ر�
        /// </summary>
        void RegisterWebPageClose();

        /// <summary>
        /// ������ע�ᵽҳ������ҳ����ʱ�Զ�ִ�йر����ݿ����
        /// </summary>
        /// <param name="page">Ҫע���ҳ</param>
        void RegisterWebPageClose(System.Web.UI.Page page);
        #endregion


    }
}
