using System;
using System.Collections.Generic;
using System.Data;

namespace Aooshi.DB
{
    /// <summary>
    /// 对象条件
    /// </summary>
    [Serializable]
    public class TableCondition
    {
        private string _s_Where;
        private string _s_Order;
        private string _s_Field;
        private int _s_Size;
        private string _s_split;
        private List<IDbDataParameter> _s_OtherParames = null;
        private string _s_tablename;
        private bool _IsStoredProcedure  = false;


        internal TableCondition(string tablename)
        {
            _s_Field = "*";
            _s_split = " AND ";
            _s_tablename = tablename;
        }

        /// <summary>
        /// 获取或设置操作表名，当为存储过程执行时该名称即为过程名
        /// </summary>
        public string TableName
        {
            get { return _s_tablename; }
            set { _s_tablename = value; }
        }

        /// <summary>
        /// 设置或获取排序规则，不带 ORDER BY 关键字
        /// </summary>
        public string Order
        {
            get { return _s_Order; }
            set { _s_Order = value; }
        }

        /// <summary>
        /// 获取或设置操作相关条数
        /// </summary>
        public int Size
        {
            get{ return _s_Size;}
            set { _s_Size = value;}
        }

        /// <summary>
        /// 获取或设置检索字段，默认使用 *
        /// </summary>
        public string Field
        {
            get{ return _s_Field;}
            set { _s_Field = value;}
        }

        /// <summary>
        /// 设置或获取操作条件，不带WHERE 关键字 使： a=1 and b=1
        /// </summary>
        public string Where
        {
            get{ return _s_Where;}
            set{ _s_Where = value; if (_s_Where == null) _s_Where = "";}
        }

        /// <summary>
        /// 获取或设置查询条件联接关系,默认为AND关系
        /// </summary>
        public string WhereSplit
        {
            get{ return _s_split;}
            set{ _s_split = value;}
        }

        /// <summary>
        /// 添加一个执行参数
        /// </summary>
        /// <param name="parame">要添加的参数</param>
        public void Add(IDbDataParameter parame)
        {

            if (_s_OtherParames == null)
                _s_OtherParames = new List<IDbDataParameter>();

            _s_OtherParames.Add(parame);
        }

        /// <summary>
        /// 获取或设置参数列表
        /// </summary>
        public List<IDbDataParameter> Parameters
        {
            get { return _s_OtherParames; }
            set { _s_OtherParames = value; }
        }

        /// <summary>
        /// 获取或设置是否以存储过程执行操作，当此值设置为true一般均需要将<see cref="TableName"/>设置为存储过程名称
        /// </summary>
        public bool IsStoredProcedure
        {
            get{ return _IsStoredProcedure;}
            set { _IsStoredProcedure = value;}
        }


    }
}
