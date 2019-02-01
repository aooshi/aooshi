using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using Aooshi.Configuration;
using System.Configuration;

namespace Aooshi.DB
{
    /// <summary>
    /// 数据操作基类
    /// </summary>
    /// <include file='../docs/DB.Factory.xml' path='docs/*'/>
    public abstract class Factory : IDisposable , IFactory
    {
        /// <summary>
        /// 根据config配置节，获取指定配置名称的数据库操作实例,类型初始函数必需有一个仅有一个字符串链接参数的初始函数
        /// </summary>
        /// <param name="settingname">数据库配置名</param>
        public static Factory GetInstance(string settingname)
        {
            //DbProvider dp = new DbProvider(settingname);
            ConnectionStringSettings css = ConfigurationManager.ConnectionStrings[settingname];
            if (css == null || string.IsNullOrEmpty(css.ProviderName))
            {
                throw new AooshiException("not config "+ settingname +" and connect provider.");
            }

            return Factory.GetInstance(css.ConnectionString,css.ProviderName);
        }


        /// <summary>
        /// 根据指定的链接与类型，创建一个工厂模式数据库操作,类型初始函数必需有一个仅有一个字符串链接参数的初始函数
        /// </summary>
        /// <param name="connstring">数据库链接串</param>
        /// <param name="classtype">类型</param>
        public static Factory GetInstance(string connstring, string classtype)
        {
            Type type = Type.GetType(classtype);
            if (type == null)
            {
                throw new AooshiException("class type "+ classtype +" is null.");
            }
            return (Factory)Activator.CreateInstance(type,connstring);
        }

        /// <summary>
        /// 根据DbProvider配置节，获取指定配置名称的数据库操作实例,类型初始函数必需有一个仅有一个字符串链接参数的初始函数
        /// </summary>
        /// <param name="settingname">数据库配置名</param>
        public static Factory GetInstanceByDbProvider(string settingname)
        {
            DbProvider dp = new DbProvider(settingname);

            if (!dp.Success || string.IsNullOrEmpty(dp.Provider))
            {
                throw new AooshiException("not config " + settingname + " and connect provider.");
            }

            return Factory.GetInstance(dp.Connect.ConnectionString, dp.Provider);
        }

        /// <summary>
        /// 获取当前操作对象的名称描述
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 关闭数据库连接时
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// 构造参数时的委托
        /// </summary>
        /// <param name="parames">参数列表</param>
        public delegate void BuildParameaterHandler(IDataParameterCollection parames);

        /// <summary>
        /// 构造参数时委托
        /// </summary>
        public event BuildParameaterHandler BuildParameater;

        #region new member
        /// <summary>
        /// initialize
        /// </summary>
        /// <param name="connection">对象连接</param>
        /// <param name="factory">工厂</param>
        public Factory(DbProviderFactory factory, IDbConnection connection)
        {
            this.DbProviderFactory = factory;
            this.Connection = connection;
            this.Transaction = null;

            if (DBCommon.SqlDebug)
            {
                //if (System.Web.HttpContext.Current != null)
                //    DBLogPath = System.Web.HttpContext.Current.Server.MapPath("~/DbLog/");
                _DBLog = new DBLog();
            }
        }

        /// <summary>
        /// 创建一个支持本示例接口的<see cref="SqlCreate"/>对象
        /// </summary>
        /// <returns>新的 SqlCreate 实例</returns>
        public virtual SqlCreate CreateSqlCreate()
        {
            return new SqlCreate();
        }

        /// <summary>
        /// 创建一个语句创建
        /// </summary>
        /// <param name="tablebase">数据</param>
        protected virtual TableToSQL CreateTableToSQL(TableBase tablebase)
        {
            return new TableToSQL(tablebase, this);
        }

        /// <summary>
        /// 根据数据类型返回一个可以直接应用于SQL语句的安全字符串
        /// </summary>
        /// <param name="value">要添加的值</param>
        public virtual string GetSafeString(object value)
        {
            if (value == null) return "''";
            return GetSafeString(value, value.GetType());
        }
        
        /// <summary>
        /// 根据数据类型返回一个可以直接应用于SQL语句的安全字符串
        /// </summary>
        /// <param name="value">要添加的值</param>
        /// <param name="type">类型</param>
        public virtual string GetSafeString(object value, Type type)
        {
            string v = Convert.ToString(value);

            if (type == TypeConst.typeInt16) return v;
            else if (type == TypeConst.typeInt32) return v;
            else if (type == TypeConst.typeInt64) return v;
            else if (type == TypeConst.typeSByte) return v;
            else if (type == TypeConst.typeByte) return v;
            else if (type == TypeConst.typeSingle) return v;
            else if (type == TypeConst.typeUInt16) return v;
            else if (type == TypeConst.typeUInt32) return v;
            else if (type == TypeConst.typeUInt64) return v;
            else if (type == TypeConst.typeDouble) return v;
            else if (type == TypeConst.typeBoolean) return true.Equals(value) ? "1" : "0";

            return "'" + this.InjectReplace(v) + "'";
        }

        /// <summary>
        /// SQL注入过滤
        /// </summary>
        /// <param name="input">要过滤的数据</param>
        public virtual string InjectReplace(string input)
        {
            return DBCommon.Replace(input);
        }

        /// <summary>
        /// 获取Command数，间接反应了执行了多少次数据操作
        /// </summary>
        public int CommandCount
        {
            get { return _commandcount; }
        }

        /// <summary>
        /// 获取当前数据数据处理方法集
        /// </summary>
        protected DbProviderFactory DbProviderFactory
        {
            get;
            private set;
        }

        int _commandcount = 0;

        string _lastsql;

        /// <summary>
        /// 获取最后一个Command SQL
        /// </summary>
        public virtual string LastSQL
        {
            get { return this._lastsql; }
        }

        /// <summary>
        /// 根据指定数据返回Command对象,所有的数据操作均应执行此方法来获取Command对象
        /// </summary>
        /// <param name="SqlString">执行串</param>
        /// <param name="StoredProcedure">是否以存储过程执行</param>
        /// <param name="Parames">参数列表</param>
        protected virtual IDbCommand GetCommand(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames)
        {
            //local variable
            IDbCommand command = this.Connection.CreateCommand();
            command.CommandText = SqlString;

            this._lastsql = SqlString;

            if (StoredProcedure) command.CommandType = CommandType.StoredProcedure;
            if (null != Parames)
                foreach (IDbDataParameter Parame in Parames) command.Parameters.Add(Parame);

            if (null != this.BuildParameater)
                this.BuildParameater(command.Parameters);

            command.Connection = this.Connection;

            if (this.Transaction != null)
                command.Transaction = this.Transaction;

            if (_DBLog != null)
                _DBLog.add(command.CommandText, Parames,StoredProcedure);

            _commandcount++;

            return command;
        }

        /// <summary>
        /// 创建一个新的数据填充对象
        /// </summary>
        protected virtual IDbDataAdapter CreateAdapter()
        {
            return this.DbProviderFactory.CreateDataAdapter();
        }

        
        /// <summary>
        /// 获取DataLogic数据操作逻辑
        /// </summary>
        /// <param name="typename">类型名,必需有一个无参数构造，且实现<see cref="Aooshi.DB.ILogic"/></param>
        public virtual ILogic GetDataLogic(string typename)
        {
            return this.GetDataLogic(Type.GetType(typename,true,true));
        }

        /// <summary>
        /// 获取DataLogic数据操作逻辑
        /// </summary>
        /// <param name="type">类型,必需有一个无参数构造，且实现<see cref="Aooshi.DB.ILogic"/></param>
        public virtual ILogic GetDataLogic(Type type)
        {
            string typename = type.FullName;
            ILogic logic;
            if (!this.DataLogicList.TryGetValue(typename, out logic))
            {
                logic = (ILogic)Activator.CreateInstance(type);
                logic.Initialize(this);
                this.DataLogicList.Add(typename,logic);
            }
            return logic;
        }

        Dictionary<string, ILogic> _DataLogicList;
        private Dictionary<string, ILogic> DataLogicList
        {
            get { return this._DataLogicList ?? (this._DataLogicList = new Dictionary<string,ILogic>()); }
        }

        #endregion

        #region 事务
        
        /// <summary>
        /// 获取当前正在执行的事务
        /// </summary>
        public virtual IDbTransaction Transaction
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否为并行（嵌套）事务
        /// </summary>
        public virtual bool TransactionNested
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取为并行(嵌套)事务时的当前事务层级
        /// </summary>
        public virtual int TransactionLayer
        {
            get;
            private set;
        }

        /// <summary>
        /// 减少一个嵌套层级
        /// </summary>
        private void TransactionDecrease()
        {
            this.TransactionLayer--;
            if (this.TransactionLayer < 1)
            {
                this.TransactionLayer = 1;
            }
        }

        /// <summary>
        /// 开始一个事务执行,当多个事务并行时，以最外层有效
        /// </summary>
        /// <returns>事务嵌套层级</returns>
        public virtual int Begin()
        {
            //if (this._tran != null)
            //{
            //    throw new DBException("There is unfinished this.Transaction.");
            //}

            lock (this.Connection)
            {
                if (this.Transaction != null)
                {
                    this.TransactionLayer++;
                    this.TransactionNested = true;
                }
                else
                {
                    this.Transaction = this.Connection.BeginTransaction();
                    this.TransactionLayer = 1;
                    this.TransactionNested = false;
                }

                return this.TransactionLayer;
            }
        }


        /// <summary>
        /// 事务完成
        /// </summary>
        private void Commited()
        {
            if (this.Transaction == null)
            {
                throw new DBException("Not Invoke this.Transaction Begin.");
            }

            this.Transaction.Commit();
            this.Transaction = null;
        }

        /// <summary>
        /// 提交一个事务执行,并行事务时非最外层则跳过
        /// </summary>
        public virtual void Commit()
        {
            this.Commit(false);
        }

        /// <summary>
        /// 提交一个事务执行
        /// </summary>
        /// <param name="endNested">是否直接完成并行事务,注意：当值为<see cref="Boolean"/>时你必需重新启用新事务，否则外围提交将无效.</param>
        public virtual void Commit(bool endNested)
        {
            lock (this.Connection)
            {
                //并行 嵌套
                if (this.TransactionNested)
                {
                    //为首层或强制完成
                    if (this.TransactionLayer == 1 || endNested)
                    {
                        this.Commited();
                    }

                    //首层完成
                    if (this.TransactionLayer == 1)
                    {
                        this.TransactionNested = false;
                    }
                    //减少层级
                    else
                    {
                        this.TransactionDecrease();
                    }
                }

                //非嵌套
                else
                {
                    this.Commited();
                }
            }
        }

        /// <summary>
        /// 回滚事务,当并行事务已回滚时则跳过
        /// </summary>
        /// <returns>当并行事务时反回是否执行了回滚操作，非并行事务返回固定<see cref="Boolean"/></returns>
        public virtual void Rollback()
        {
            //if (this._tran == null)
            //{
            //    throw new DBException("Not Invoke TranBegin.");
            //}

            lock (this.Connection)
            {
                if (this.Transaction != null)
                {
                    this.Transaction.Rollback();
                    this.Transaction = null;
                }

                if (this.TransactionNested)
                {
                    //已完成
                    if (this.TransactionLayer == 1)
                    {
                        this.TransactionNested = false;
                    }
                    //减少层级
                    else
                    {
                        this.TransactionDecrease();
                    }
                }
            }
        }

        #endregion

        #region IFactory 成员

        #region connection

        /// <summary>
        /// 获取当前数据连接字符串
        /// </summary>
        protected virtual string CurrentConnectionString
        {
            get { return this.Connection.ConnectionString; }
        }

        /// <summary>
        /// 进行数据库切换(注意:不是任何版本数据库均支持此功能)
        /// </summary>
        /// <param name="NewDBName">新数据库名</param>
        public virtual void ChangeDataBase(string NewDBName)
        {
            this.Connection.ChangeDatabase(NewDBName);
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public virtual void Open()
        {
            if (this.Connection.State == ConnectionState.Closed)
            this.Connection.Open();
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public virtual void Close()
        {
            if (this.Connection.State != ConnectionState.Closed)
                this.Connection.Close();

            if (Closed != null)
                Closed(this, EventArgs.Empty);
        }

        /// <summary>
        /// 获取当前数据库连接串
        /// </summary>
        public virtual IDbConnection Connection { get; protected set; }

        #endregion

        #region execute

        /// <summary>
        /// 执行一个语句或过程
        /// </summary>
        /// <param name="SqlString">要执行的操作语句</param>
        public virtual int Execute(string SqlString)
        {
            return Execute(SqlString, false, null);
        }
        /// <summary>
        /// 执行一个语句或过程
        /// </summary>
        /// <param name="SqlString">要执行的操作语句</param>
        /// <param name="StoredProcedure">是否以存储过程执行</param>
        public virtual int Execute(string SqlString, bool StoredProcedure)
        {
            return Execute(SqlString, StoredProcedure, null);
        }
        /// <summary>
        /// 执行一个语句或过程
        /// </summary>
        /// <param name="SqlString">要执行的操作语句</param>
        /// <param name="StoredProcedure">是否以存储过程执行</param>
        /// <param name="Parames">参数列表</param>
        public virtual int Execute(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames)
        {
            using (IDbCommand cmd = this.GetCommand(SqlString, StoredProcedure, Parames))
            {
                //try
                //{
                    return cmd.ExecuteNonQuery();
                //}
                //catch(Exception e)
                //{
                //    Aooshi.Web.WebCommon.WriteEnd(SqlString + e.ToString());
                //    return 0;
                //}
            }
        }

        /// <summary>
        /// 执行数据对象插入请求
        /// </summary>
        /// <param name="tablebase">数据对象</param>
        public virtual int Insert(TableBase tablebase)
        {
            if (tablebase.TableCondition.IsStoredProcedure)
                return Execute(tablebase.TableCondition.TableName,true,CreateParameter(tablebase));

            TableToSQL ts = this.CreateTableToSQL(tablebase);
            return Execute(ts.GetInsert(), false,ts.GetPartemter);
        }

        /// <summary>
        /// 执行数据对象删除处理
        /// </summary>
        /// <param name="tablebase">对象</param>
        public virtual int Delete(TableBase tablebase)
        {
            if (tablebase.TableCondition.IsStoredProcedure)
                return Execute(tablebase.TableCondition.TableName, true, CreateParameter(tablebase));
            TableToSQL ts = this.CreateTableToSQL(tablebase);
            return Execute(ts.GetDelete(), false, ts.GetPartemter);
        }

        /// <summary>
        /// 执行数据对象修改
        /// </summary>
        /// <param name="tablebase">数据对象</param>
        /// <param name="where">条件对像</param>
        public virtual int Update(TableBase tablebase,TableBase where)
        {
            if (tablebase.TableCondition.IsStoredProcedure)
                return Execute(tablebase.TableCondition.TableName, true, CreateParameter(tablebase));
            TableToSQL ts = this.CreateTableToSQL(tablebase);
            return Execute(ts.GetUpdate(where), false, ts.GetPartemter);
        }

        #endregion

        #region get
        /// <summary>
        /// 获取上一执行语句所产生的自增值(此功能不一定支持所有数据库服务器)，如果有误，返回为结果类型初始值
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        public virtual T GetIdentity<T>()
        {
            using (IDbCommand cmd = GetCommand("SELECT @@IDENTITY", false, null))
            {
                object o = cmd.ExecuteScalar();
                if (DBNull.Value.Equals(o)) return default(T);
                return (T)Convert.ChangeType(o, typeof(T));
            }
        }

        /// <summary>
        /// 返回一个数据集
        /// </summary>
        /// <param name="tablebase">数据对象</param>
        public virtual DataSet GetDataSet(TableBase tablebase)
        {
            if (tablebase.TableCondition.IsStoredProcedure)
                return GetDataSet(tablebase.TableCondition.TableName, true, CreateParameter(tablebase));

            TableToSQL ts = this.CreateTableToSQL(tablebase);
            return GetDataSet(ts.GetSelect(), false, ts.GetPartemter);
        }
        /// <summary>
        /// 返回一个数据集
        /// </summary>
        /// <param name="SqlString">要执行的语句</param>
        public virtual DataSet GetDataSet(string SqlString)
        {
            return GetDataSet(SqlString, false, null);
        }
        /// <summary>
        /// 返回一个数据集
        /// </summary>
        /// <param name="SqlString">要执行的语句或过程</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        public virtual DataSet GetDataSet(string SqlString, bool StoredProcedure)
        {
            return GetDataSet(SqlString, StoredProcedure, null);
        }
        /// <summary>
        /// 返回一个数据集
        /// </summary>
        /// <param name="SqlString">要执行的语句或过程</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        /// <param name="Parames">参数列表</param>
        public virtual DataSet GetDataSet(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames)
        {
            DataSet dataset = new DataSet();
            IDbDataAdapter adapter = CreateAdapter();
            using (adapter.SelectCommand = GetCommand(SqlString, StoredProcedure, Parames))
            {
                adapter.Fill(dataset);
                return dataset;
            }
        }


        /// <summary>
        /// 返回一个数据表
        /// </summary>
        /// <param name="tablebase">数据对象</param>
        public virtual DataTable GetDataTable(TableBase tablebase)
        {
            if (tablebase.TableCondition.IsStoredProcedure)
                return GetDataTable(tablebase.TableCondition.TableName, true, CreateParameter(tablebase));
            TableToSQL ts = this.CreateTableToSQL(tablebase);
            return GetDataTable(ts.GetSelect(), false, ts.GetPartemter);
        }
        /// <summary>
        /// 返回一个数据表
        /// </summary>
        /// <param name="SqlString">要执行的语句</param>
        public virtual DataTable GetDataTable(string SqlString)
        {
            return GetDataTable(SqlString, false, null);
        }
        /// <summary>
        /// 返回一个数据表
        /// </summary>
        /// <param name="SqlString">要执行的语句或过程</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        public virtual DataTable GetDataTable(string SqlString, bool StoredProcedure)
        {
            return GetDataTable(SqlString, StoredProcedure, null) ;
        }
        /// <summary>
        /// 返回一个数据表
        /// </summary>
        /// <param name="SqlString">要执行的语句或过程</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        /// <param name="Parames">参数列表</param>
        public virtual DataTable GetDataTable(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames)
        {
            return DBCommon.IDataReaderToDataTable(GetCommand(SqlString, StoredProcedure, Parames).ExecuteReader(CommandBehavior.SequentialAccess));
        }



        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="SqlString">要执行的语句</param>
        public virtual Object GetScalar(string SqlString)
        {
            return GetScalar<Object>(SqlString, false, null);
        }
        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="SqlString">要执行的语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        public virtual Object GetScalar(string SqlString, bool StoredProcedure)
        {
            return GetScalar<Object>(SqlString, StoredProcedure, null);
        }
        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="SqlString">要执行的语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        /// <param name="Parames">参数列表</param>
        public virtual Object GetScalar(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames)
        {
            return GetScalar<Object>(SqlString, StoredProcedure, Parames) ;
        }

        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="Object">对象</param>
        public virtual Object GetScalar(TableBase Object)
        {
            return GetScalar<object>(Object);
        }


        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">要执行的语句</param>
        public virtual T GetScalar<T>(string SqlString)
        {
            return GetScalar<T>(SqlString, false, null);
        }
        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">要执行的语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        public virtual T GetScalar<T>(string SqlString, bool StoredProcedure)
        {
            return GetScalar<T>(SqlString, StoredProcedure, null);
        }
        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">要执行的语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        /// <param name="Parames">参数列表</param>
        public virtual T GetScalar<T>(string SqlString, bool StoredProcedure, IDbDataParameter[] Parames)
        {
            using (IDbCommand cmd = GetCommand(SqlString, StoredProcedure, Parames))
            {
                object o = cmd.ExecuteScalar();
                if (o == DBNull.Value) return default(T);
                return (T)o;
            }
        }
        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="tablebase">对象</param>
        public virtual T GetScalar<T>(TableBase tablebase)
        {
            if (tablebase.TableCondition.IsStoredProcedure)
                return GetScalar<T>(tablebase.TableCondition.TableName, true, CreateParameter(tablebase));
            TableToSQL ts = this.CreateTableToSQL(tablebase);
            return GetScalar<T>(ts.GetSelect(), false, ts.GetPartemter);
        }

        /// <summary>
        /// 获取指定语句第一行第一例的Int类型数据
        /// </summary>
        /// <param name="SqlString">执行语句</param>
        /// <param name="StoredProcedure">是否以过程执行</param>
        public virtual int GetCount(string SqlString, bool StoredProcedure)
        {
                return Convert.ToInt32(GetScalar(SqlString, StoredProcedure));
        }


        /// <summary>
        /// 获取指定语句第一行第一例的Int类型数据
        /// </summary>
        /// <param name="SqlString">执行语句</param>
        public virtual int GetCount(string SqlString)
        {
            return Convert.ToInt32(GetScalar(SqlString));
        }

        /// <summary>
        /// 获取指定对象的记录数
        /// </summary>
        /// <param name="tablebase">对象</param>
        public virtual int GetCount(TableBase tablebase)
        {
            if (tablebase.TableCondition.IsStoredProcedure)
                return GetScalar<int>(tablebase.TableCondition.TableName, true, CreateParameter(tablebase));

            tablebase.TableCondition.Size = 1;
            tablebase.TableCondition.Field = "COUNT(*)";
            TableToSQL ts = this.CreateTableToSQL(tablebase);
            return Convert.ToInt32(GetScalar(ts.GetSelect(), false, ts.GetPartemter));
        }
        /// <summary>
        /// 返回一个数据对象集
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">查询语句</param>
        public virtual List<T> GetList<T>(string SqlString)
        {
            return GetList<T>(SqlString, false, null);
        }
        /// <summary>
        /// 返回一个数据对象集
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">查询语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        public virtual List<T> GetList<T>(string SqlString, bool StoredProcedure)
        {
            return GetList<T>(SqlString, StoredProcedure, null);
        }
        /// <summary>
        /// 返回一个数据对象集
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="SqlString">查询语句</param>
        /// <param name="StoredProcedure">是否为存储过程</param>
        /// <param name="Parames">参数列表</param>
        public virtual List<T> GetList<T>(string SqlString, bool StoredProcedure, System.Data.IDbDataParameter[] Parames)
        {
            //Type tType = typeof(T);
            //MethodInfo mInfo = tType.GetMethod("Set");
            //if (mInfo == null) throw new Exception("is T not base ObjectBase");


            //T ObjectItem;
            //List<T> list = new List<T>();
            //using (IDbCommand cmd = GetCommand(SqlString, StoredProcedure, Parames))
            //{
            //    using (IDataReader Reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
            //    {
            //        while (Reader.Read())
            //        {
            //            ObjectItem = Activator.CreateInstance<T>();
            //            for (int index = 0; index < Reader.FieldCount; index++)
            //                mInfo.Invoke(ObjectItem, new object[] { Reader.GetName(index), Reader.GetValue(index) });

            //            list.Add(ObjectItem);
            //        }
            //    }
            //}



            object ObjectItem;
            List<T> list = new List<T>();
            using (IDbCommand cmd = GetCommand(SqlString, StoredProcedure, Parames))
            {
                using (IDataReader Reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                {
                    while (Reader.Read())
                    {
                        ObjectItem = Activator.CreateInstance<T>();
                        for (int index = 0; index < Reader.FieldCount; index++)
                        {
                            var v = Reader.GetValue(index);
                            ((TableBase)ObjectItem).Set(Reader.GetName(index), DBNull.Value.Equals(v) ? null : v);
                        }

                        list.Add((T)ObjectItem);
                    }
                }
            }

            return list;
        }
        /// <summary>
        /// 返回一个数据对象集
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="tablebase">查询条件对象</param>
        public List<T> GetList<T>(TableBase tablebase)
        {
            if (tablebase.TableCondition.IsStoredProcedure)
                return GetList<T>(tablebase.TableCondition.TableName, true, CreateParameter(tablebase));

            TableToSQL ts = this.CreateTableToSQL(tablebase);
            return GetList<T>(ts.GetSelect(), false, ts.GetPartemter);
        }
        /// <summary>
        /// 返回一个数据对象
        /// </summary>
        /// <typeparam name="T">数据对象类型</typeparam>
        /// <param name="SqlString">查询语句</param>
        public T GetOnlyRow<T>(string SqlString)
        {
            return GetOnlyRow<T>(SqlString, false, null);
        }
        /// <summary>
        /// 返回一个数据对象
        /// </summary>
        /// <typeparam name="T">数据对象类型</typeparam>
        /// <param name="SqlString">查询语句或过程</param>
        /// <param name="StoredProcedure">是否以存储过程执行</param>
        public T GetOnlyRow<T>(string SqlString, bool StoredProcedure)
        {
            return GetOnlyRow<T>(SqlString, false, null);
        }
        /// <summary>
        /// 返回一个数据对象
        /// </summary>
        /// <typeparam name="T">数据对象类型</typeparam>
        /// <param name="SqlString">查询语句或过程</param>
        /// <param name="StoredProcedure">是否以存储过程执行</param>
        /// <param name="Parames">参数列表</param>
        public T GetOnlyRow<T>(string SqlString, bool StoredProcedure, System.Data.IDbDataParameter[] Parames)
        {
            //Type tType = typeof(T);
            //MethodInfo mInfo = tType.GetMethod("Set");
            //if (mInfo == null) throw new Exception("is T not base ObjectBase");

            //T ObjectItem;
            //using (IDbCommand cmd = GetCommand(SqlString, StoredProcedure, Parames))
            //{
            //    using (IDataReader Reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
            //    {
            //        if (Reader.Read())
            //        {
            //            ObjectItem = Activator.CreateInstance<T>();
            //            for (int index = 0; index < Reader.FieldCount; index++)
            //                mInfo.Invoke(ObjectItem, new object[] { Reader.GetName(index), Reader.GetValue(index) });

            //            return ObjectItem;

            //        }
            //    }
            //}

            object ObjectItem;
            using (IDbCommand cmd = GetCommand(SqlString, StoredProcedure, Parames))
            {
                using (IDataReader Reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                {
                    if (Reader.Read())
                    {
                        ObjectItem = Activator.CreateInstance<T>() ;
                        for (int index = 0; index < Reader.FieldCount; index++)
                        {
                            var v = Reader.GetValue(index);
                            ((TableBase)ObjectItem).Set(Reader.GetName(index), DBNull.Value.Equals(v) ? null : v);
                        }

                        return (T)ObjectItem;

                    }
                }
            }

            return default(T);
        }
        /// <summary>
        /// 返回一个数据对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="tablebase">查询对象</param>
        public T GetOnlyRow<T>(TableBase tablebase)
        {
            if (tablebase.TableCondition.IsStoredProcedure)
                return GetOnlyRow<T>(tablebase.TableCondition.TableName, true, CreateParameter(tablebase));
            tablebase.TableCondition.Size = 1;
            TableToSQL ts = this.CreateTableToSQL(tablebase);
            return GetOnlyRow<T>(ts.GetSelect(), false, ts.GetPartemter);
        }

        #endregion

        #region Parames
        /// <summary>
        /// 获取创建参数时,使用的前缀,默认使用@符,如果当前操作引擎不是,请重写该属性
        /// </summary>
        protected internal virtual string GetParameterSplitChar
        {
            get { return "@"; }
        }
        /// <summary>
        /// 创建一个参数
        /// </summary>
        /// <param name="Name">参数</param>
        /// <param name="Value">参数值</param>
        public IDbDataParameter CreateParameter(string Name, object Value)
        {
            IDbDataParameter pm = this.DbProviderFactory.CreateParameter();
            pm.ParameterName = Name;
            pm.Value = Value;
            return pm;
        }

        /// <summary>
        /// 根据对象创建参数列表,使用<see cref="GetParameterSplitChar"/>所指定的参数前缀
        /// </summary>
        /// <param name="obj">对象</param>
        protected virtual IDbDataParameter[] CreateParameter(TableBase obj)
        {
            //object property
            Dictionary<string, object>.Enumerator etor = obj.GetEnumerator();
            List<IDbDataParameter> list = new List<IDbDataParameter>();
            while (etor.MoveNext())
                list.Add(CreateParameter(GetParameterSplitChar + etor.Current.Key, etor.Current.Value));
            //other parames
            if (null != obj.TableCondition.Parameters)
                list.AddRange(obj.TableCondition.Parameters);

            //return out
            return list.ToArray();
        }

        #endregion

        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 释放所占用的资源
        /// </summary>
        public void Dispose()
        {
            if (_DBLog != null)
                _DBLog.Save(DBLogPath);
            this.Close();
            //this.Connection.Dispose();
        }

        #endregion

        #region IFactory >> 日志

        private DBLog _DBLog;
        private string _DbLogPath;
         /// <summary>
        /// 获取当前正在执行的DBLog,注意:只有当SqlDebug配置有效时，此项才有效；
        /// </summary>
        protected virtual DBLog GetCurrentDBLog
        {
            get { return _DBLog; }
        }
        /// <summary>
        /// 获取或设置日志文件基路径
        /// </summary>
        protected virtual string DBLogPath
        {
            get
            {
                if (_DbLogPath == null) return "";// this.GetType().Name;
                return _DbLogPath;
            }
            set
            {
                _DbLogPath = value;
            }
        }

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
        public virtual string MakeSql(string Field, string Table, string Term, string Key, string Sort, long RsStart, long Size)
        {
            return this.MakeSql(Field,Table,Term,Key,Sort,"",RsStart,Size,false);
        }

        /// <summary>
        /// 生成分页用Sql语句
        /// </summary>
        /// <param name="Field">字段值,前后不带空格,位于 Select 与 From 之间的字段部表示</param>
        /// <param name="Table">要进行查询的数据表,可为多个</param>
        /// <param name="Term">要进行查询的查询串,Where的后缀,如果未有,请设置为 <c>string.Empty</c></param>
        /// <param name="Key">要进行Not In 方法的键值,如为敏感字段请用[]扩起来</param>
        /// <param name="Sort">排序方法,如果未有排序则为string.Empty</param>
        /// <param name="Group">分组方法，不使用请设置为string.Empty</param>
        /// <param name="RsStart">记录开始数</param>
        /// <param name="Size">记录总数</param>
        /// <param name="isDistinct">是否使用Distinct</param>
        /// <returns>返回生成后的Sql语句</returns>
        public abstract string MakeSql(string Field, string Table, string Term, string Key, string Sort, string Group, long RsStart, long Size, bool isDistinct);

        /// <summary>
        /// 生成分页用计Count语句
        /// </summary>
        /// <param name="Table">要进行查询的数据表,可为多个</param>
        /// <param name="condition">要进行查询的查询串,Where的后缀,如果未有,请设置为Empty</param>
        /// <returns>返回生成后的Sql语句</returns>
        public virtual string MakeCountSql(string Table, string condition)
        {
            string sql = "SELECT COUNT(*) FROM " + Table;

            if (condition != null && condition != "")
            {
                sql += " WHERE " + condition;
            }

            return sql;
        }

        /// <summary>
        /// 根据表与条件返回一个COUNT值
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="condition">条件</param>
        public virtual int GetCountByMake(string Table, string condition)
        {
            return this.GetCount(this.MakeCountSql(Table, condition));
        }


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
        public virtual List<T> GetListByMakeSql<T>(string Field, string Table, string Term, string Key, string Sort, long RsStart, long Size)
        {
            return GetList<T>(this.MakeSql(Field, Table, Term, Key, Sort, RsStart, Size));
        }

        #endregion

        #region 注册到网页

        bool _IsRegisteredWebPage = false;
        /// <summary>
        /// 是否已注册到页
        /// </summary>
        public bool IsRegisteredWebPage
        {
            get { return _IsRegisteredWebPage; }
        }

        bool _IsSetToWebPage = false;
        /// <summary>
        /// 是否已将实例设置至网页上下文
        /// </summary>
        public bool IsSetToWebPage
        {
            get { return _IsSetToWebPage; }
        }

        /// <summary>
        /// 注册页至网页自动关闭
        /// </summary>
        public virtual void RegisterWebPageClose()
        {
            if (_IsRegisteredWebPage) return;
            if (System.Web.HttpContext.Current != null)
            {
                var page = Aooshi.Web.WebCommon.CurrentPage();
                if (page != null)
                {
                    this.RegisterWebPageClose(page);
                }
            }
        }
        /// <summary>
        /// 将操作注册到页，并在页结束时自动执行关闭数据库操作
        /// </summary>
        /// <param name="page">要注册的页</param>
        public virtual void RegisterWebPageClose(System.Web.UI.Page page)
        {
            if (!_IsRegisteredWebPage)
            {
                page.Unload += new EventHandler(Page_Unload);
                _IsRegisteredWebPage = true;
            }
        }

        private void Page_Unload(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

    }
}
