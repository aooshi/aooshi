using System;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;

namespace Aooshi.DB
{
    /// <summary>
    /// 数据工厂接口
    /// </summary>
    /// <include file='../docs/DB.Factory.xml' path='docs/*'/>
    public interface IFactory : IDisposable
    {
        /// <summary>
        /// 获取当前工厂名称描述
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 最后一个Command SQL
        /// </summary>
        string LastSQL { get; }

        /// <summary>
        /// 关闭数据库连接时
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// 创建一个支持本实例接口的<see cref="SqlCreate"/>对象
        /// </summary>
        /// <returns>新的 SqlCreate 实例</returns>
        SqlCreate CreateSqlCreate();

        /// <summary>
        /// 根据数据类型返回一个可以直接应用于SQL语句的安全字符串
        /// </summary>
        /// <param name="value">要添加的值</param>
        string GetSafeString(object value);


        /// <summary>
        /// 根据数据类型返回一个可以直接应用于SQL语句的安全字符串
        /// </summary>
        /// <param name="value">要添加的值</param>
        /// <param name="type">类型</param>
        string GetSafeString(object value, Type type);


        /// <summary>
        /// SQL注入过滤
        /// </summary>
        /// <param name="input">要过滤的数据</param>
        string InjectReplace(string input);


        /// <summary>
        /// 获取Command数，间接反应了执行了多少次数据操作
        /// </summary>
        int CommandCount { get; }


        /// <summary>
        /// 进行数据库切换(注意:不是任何版本数据库均支持此功能)
        /// </summary>
        /// <param name="NewDBName">新数据库名</param>
        void ChangeDataBase(string NewDBName);


        /// <summary>
        /// 打开数据库连接
        /// </summary>
        void Open();



        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        void Close();


        /// <summary>
        /// 获取当前数据库连接串
        /// </summary>
        IDbConnection Connection { get; }


        /// <summary>
        /// 获取DataLogic数据操作逻辑
        /// </summary>
        /// <param name="typename">类型名,必需有一个无参数构造，且实现<see cref="Aooshi.DB.ILogic"/></param>
        ILogic GetDataLogic(string typename);

        /// <summary>
        /// 获取DataLogic数据操作逻辑
        /// </summary>
        /// <param name="type">类型,必需有一个无参数构造，且实现<see cref="Aooshi.DB.ILogic"/></param>
        ILogic GetDataLogic(Type type);

        #region 事务

        /// <summary>
        /// 获取是否为并行（嵌套）事务
        /// </summary>
        bool TransactionNested { get; }

        /// <summary>
        /// 获取为并行(嵌套)事务时的当前事务层级
        /// </summary>
        int TransactionLayer { get; }

        /// <summary>
        /// 开始一个事务执行
        /// </summary>
        /// <returns>事务嵌套层级</returns>
        int Begin();

        /// <summary>
        /// 提交一个事务执行
        /// </summary>
        void Commit();

        /// <summary>
        /// 提交一个事务执行
        /// </summary>
        /// <param name="endNested">是否直接完成并行事务,注意：当值为<see cref="Boolean"/>时你必需重新启用新事务，否则外围提交将无效.</param>
        void Commit(bool endNested);

        /// <summary>
        /// 回滚事务,当并行事务已回滚时则跳过
        /// </summary>
        /// <returns>当并行事务时反回是否执行了回滚操作，非并行事务返回固定<see cref="Boolean"/></returns>
        void Rollback();

        /// <summary>
        /// 获取当前正在执行的事务
        /// </summary>
        IDbTransaction Transaction { get; }

        #endregion

        #region execute


        /// <summary>
        /// 执行一个语句或过程
        /// </summary>
        /// <param name="SqlString">要执行的操作语句</param>
        int Execute(string SqlString);

        /// <summary>
        /// 执行一个语句或过程
        /// </summary>
        /// <param name="SqlString">要执行的操作语句</param>
        /// <param name="StoredProcedure">是否以存储过程执行</param>
        int Execute(string SqlString, bool StoredProcedure);


        /// <summary>
        /// 执行一个语句或过程
        /// </summary>
        /// <param name="SqlString">要执行的操作语句</param>
        /// <param name="StoredProcedure">是否以存储过程执行</param>
        /// <param name="Parames">参数列表</param>
        int Execute(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames);


        /// <summary>
        /// 执行数据对象插入请求
        /// </summary>
        /// <param name="tablebase">数据对象</param>
        int Insert(TableBase tablebase);


        /// <summary>
        /// 执行数据对象删除处理
        /// </summary>
        /// <param name="tablebase">对象</param>
        int Delete(TableBase tablebase);


        /// <summary>
        /// 执行数据对象修改
        /// </summary>
        /// <param name="tablebase">数据对象</param>
        /// <param name="where">更新条件</param>
        int Update(TableBase tablebase, TableBase where);


        #endregion

        #region get
        /// <summary>
        /// 获取上一执行语句所产生的自增值(此功能不一定支持所有数据库服务器)，如果有误，返回为结果类型初始值
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        T GetIdentity<T>();


        /// <summary>
        /// 返回一个数据集
        /// </summary>
        /// <param name="tablebase">数据对象</param>
        DataSet GetDataSet(TableBase tablebase);


        /// <summary>
        /// 返回一个数据集
        /// </summary>
        /// <param name="SqlString">要执行的语句</param>
        DataSet GetDataSet(string SqlString);


        /// <summary>
        /// 返回一个数据集
        /// </summary>
        /// <param name="SqlString">要执行的语句或过程</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        DataSet GetDataSet(string SqlString, bool StoredProcedure);


        /// <summary>
        /// 返回一个数据集
        /// </summary>
        /// <param name="SqlString">要执行的语句或过程</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        /// <param name="Parames">参数列表</param>
        DataSet GetDataSet(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames);



        /// <summary>
        /// 返回一个数据表
        /// </summary>
        /// <param name="tablebase">数据对象</param>
        DataTable GetDataTable(TableBase tablebase);


        /// <summary>
        /// 返回一个数据表
        /// </summary>
        /// <param name="SqlString">要执行的语句</param>
        DataTable GetDataTable(string SqlString);


        /// <summary>
        /// 返回一个数据表
        /// </summary>
        /// <param name="SqlString">要执行的语句或过程</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        DataTable GetDataTable(string SqlString, bool StoredProcedure);


        /// <summary>
        /// 返回一个数据表
        /// </summary>
        /// <param name="SqlString">要执行的语句或过程</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        /// <param name="Parames">参数列表</param>
        DataTable GetDataTable(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames);


        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="SqlString">要执行的语句</param>
        Object GetScalar(string SqlString);


        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="SqlString">要执行的语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        Object GetScalar(string SqlString, bool StoredProcedure);


        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="SqlString">要执行的语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        /// <param name="Parames">参数列表</param>
        Object GetScalar(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames);



        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="Object">对象</param>
        Object GetScalar(TableBase Object);


        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">要执行的语句</param>
        T GetScalar<T>(string SqlString);


        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">要执行的语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        T GetScalar<T>(string SqlString, bool StoredProcedure);


        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">要执行的语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        /// <param name="Parames">参数列表</param>
        T GetScalar<T>(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames);


        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="tablebase">对象</param>
        T GetScalar<T>(TableBase tablebase);


        /// <summary>
        /// 获取指定语句第一行第一例的Int类型数据
        /// </summary>
        /// <param name="SqlString">执行语句</param>
        /// <param name="StoredProcedure">是否以过程执行</param>
        int GetCount(string SqlString, bool StoredProcedure);


        /// <summary>
        /// 获取指定语句第一行第一例的Int类型数据
        /// </summary>
        /// <param name="SqlString">执行语句</param>
        int GetCount(string SqlString);


        /// <summary>
        /// 获取指定对象的记录数
        /// </summary>
        /// <param name="tablebase">对象</param>
        int GetCount(TableBase tablebase);


        /// <summary>
        /// 返回一个数据对象集
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">查询语句</param>
        List<T> GetList<T>(string SqlString);


        /// <summary>
        /// 返回一个数据对象集
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">查询语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        List<T> GetList<T>(string SqlString, bool StoredProcedure);


        /// <summary>
        /// 返回一个数据对象集
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">查询语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        /// <param name="Parames">参数列表</param>
        List<T> GetList<T>(string SqlString, bool StoredProcedure, System.Data.IDbDataParameter[] Parames);


        /// <summary>
        /// 返回一个数据对象集
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="tablebase">查询条件对象</param>
        List<T> GetList<T>(TableBase tablebase);


        /// <summary>
        /// 返回一个数据对象
        /// </summary>
        /// <typeparam name="T">数据对象类型</typeparam>
        /// <param name="SqlString">查询语句</param>
        T GetOnlyRow<T>(string SqlString);


        /// <summary>
        /// 返回一个数据对象
        /// </summary>
        /// <typeparam name="T">数据对象类型</typeparam>
        /// <param name="SqlString">查询语句或过程</param>
        /// <param name="StoredProcedure">是否以存储过程执行</param>
        T GetOnlyRow<T>(string SqlString, bool StoredProcedure);


        /// <summary>
        /// 返回一个数据对象
        /// </summary>
        /// <typeparam name="T">数据对象类型</typeparam>
        /// <param name="SqlString">查询语句或过程</param>
        /// <param name="StoredProcedure">是否以存储过程执行</param>
        /// <param name="Parames">参数列表</param>
        T GetOnlyRow<T>(string SqlString, bool StoredProcedure, System.Data.IDbDataParameter[] Parames);


        /// <summary>
        /// 返回一个数据对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="tablebase">查询对象</param>
        T GetOnlyRow<T>(TableBase tablebase);
        #endregion

        #region Parames


        /// <summary>
        /// 创建一个参数
        /// </summary>
        /// <param name="Name">参数</param>
        /// <param name="Value">参数值</param>
        IDbDataParameter CreateParameter(string Name, object Value);
        #endregion

        #region 分页
        /// <summary>
        /// 生成分页用Sql语句
        /// </summary>
        /// <param name="Field">字段值,前后不带空格,位于 Select 与 From 之间的字段部表示</param>
        /// <param name="Table">要进行查询的数据表,可为多个</param>
        /// <param name="Term">要进行查询的查询串,Where的后缀,如果未有,请设置为 <c>string.Empty</c></param>
        /// <param name="Key">要进行Not In 方法的键值,如为敏感字段请用[]扩起来</param>
        /// <param name="Sort">排序方法,如果未有排序则为string.Empty</param>
        /// <param name="RsStart">记录开始数</param>
        /// <param name="Size">记录总数</param>
        /// <returns>返回生成后的Sql语句</returns>
        string MakeSql(string Field, string Table, string Term, string Key, string Sort, long RsStart, long Size);

        /// <summary>
        /// 生成分页用Sql语句
        /// </summary>
        /// <param name="Field">字段值,前后不带空格,位于 Select 与 From 之间的字段部表示</param>
        /// <param name="Table">要进行查询的数据表,可为多个</param>
        /// <param name="Term">要进行查询的查询串,Where的后缀,如果未有,请设置为 <c>string.Empty</c></param>
        /// <param name="Key">要进行排除方法的键值,该值一般为主键</param>
        /// <param name="Sort">排序方法,如果未有排序则为string.Empty</param>
        /// <param name="Group">分组方法，不使用请设置为string.Empty</param>
        /// <param name="RsStart">记录开始数</param>
        /// <param name="Size">记录总数</param>
        /// <param name="isDistinct">是否使用Distinct</param>
        /// <returns>返回生成后的Sql语句</returns>
        string MakeSql(string Field, string Table, string Term, string Key, string Sort, string Group, long RsStart, long Size, bool isDistinct);


        /// <summary>
        /// 生成分页用计Count语句
        /// </summary>
        /// <param name="Table">要进行查询的数据表,可为多个</param>
        /// <param name="condition">要进行查询的查询串,Where的后缀,如果未有,请设置为Empty</param>
        /// <returns>返回生成后的Sql语句</returns>
        string MakeCountSql(string Table, string condition);



        /// <summary>
        /// 根据表与条件返回一个COUNT值
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="condition">条件</param>
        int GetCountByMake(string Table, string condition);

        /// <summary>
        /// 返回一个分页列表对象
        /// </summary>
        /// <param name="Field">字段值,前后不带空格,位于 Select 与 From 之间的字段部表示</param>
        /// <param name="Table">要进行查询的数据表,可为多个</param>
        /// <param name="Term">要进行查询的查询串,Where的后缀,如果未有,请设置为 <c>string.Empty</c></param>
        /// <param name="Key">要进行Not In 方法的键值,如为敏感字段请用[]扩起来</param>
        /// <param name="Sort">排序方法,如果未有排序则为string.Empty</param>
        /// <param name="RsStart">记录开始数</param>
        /// <param name="Size">记录总数</param>
        /// <returns>返回生成后的Sql语句</returns>
        List<T> GetListByMakeSql<T>(string Field, string Table, string Term, string Key, string Sort, long RsStart, long Size);


        #endregion

        #region 注册到网页

        /// <summary>
        /// 是否已注册到页
        /// </summary>
        bool IsRegisteredWebPage { get; }


        /// <summary>
        /// 注册页自动关闭
        /// </summary>
        void RegisterWebPageClose();

        /// <summary>
        /// 将操作注册到页，并在页结束时自动执行关闭数据库操作
        /// </summary>
        /// <param name="page">要注册的页</param>
        void RegisterWebPageClose(System.Web.UI.Page page);
        #endregion


    }
}
