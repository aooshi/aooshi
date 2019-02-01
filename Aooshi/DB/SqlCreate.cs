using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// Sql串生成类
    /// </summary>
    public class SqlCreate
    {
        string _selectFild;
        String _group;
        string _where;
        string _table;
        int _size;
        string _order;
        string _key;
        bool _distinct;

        /// <summary>
        /// 初始化
        /// </summary>
        protected internal SqlCreate()
        {
            this._selectFild = "*";
            this._size = -1;
            this._where = "";
            this._distinct = false;
        }

        /// <summary>
        /// 新增加条件串
        /// </summary>
        /// <param name="Sql">要新增加的条件串</param>
        public virtual void AddAndWhere(string Sql)
        {
            if (string.IsNullOrEmpty(Where))
            {
                Where = Sql;
            }
            else
            {
                Where += " AND " + Sql;
            }
        }

        /// <summary>
        /// 新增加条件串
        /// </summary>
        /// <param name="Sql">要新增加的条件串</param>
        /// <param name="data">填充数组</param>
        public virtual void AddAndWhere(string Sql,params object[] data)
        {
            this.AddAndWhere(string.Format(Sql, data));
        }

        /// <summary>
        /// 新增加条件串
        /// </summary>
        /// <param name="Sql">要新增加的条件串</param>
        public virtual void AddOrWhere(string Sql)
        {
            if (string.IsNullOrEmpty(Where))
            {
                Where = Sql;
            }
            else
            {
                Where += " OR " + Sql;
            }
        }

        
        /// <summary>
        /// 新增加条件串
        /// </summary>
        /// <param name="Sql">要新增加的条件串</param>
        /// <param name="data">填充数组</param>
        public virtual void AddOrWhere(string Sql, params object[] data)
        {
            this.AddOrWhere(string.Format(Sql, data));
        }


        /// <summary>
        /// 增加条件排序
        /// </summary>
        /// <param name="item">要增加的排序</param>
        public virtual void AddOrder(string item)
        {
            if (!string.IsNullOrEmpty(Order)) Order += ",";
            Order += item;
        }

        /// <summary>
        /// 增加分组条件
        /// </summary>
        /// <param name="item">要增加的排序</param>
        public virtual void AddGroup(string item)
        {
            if (!string.IsNullOrEmpty(Group)) Group += ",";
            Group += item;
        }

        /// <summary>
        /// 获取或设置键值
        /// </summary>
        public string Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        /// <summary>
        /// 返回或设置条件排序
        /// </summary>
        public string Order
        {
            get { return _order; }
            set { _order = value; }
        }

        /// <summary>
        /// 返回或设置分组
        /// </summary>
        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }

        /// <summary>
        /// 返回或设置查询条件串
        /// </summary>
        public string Where
        {
            get { return _where; }
            set { _where = value; }
        }

        /// <summary>
        /// 返回或设置数量
        /// </summary>
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// 返回或设置查询字段,默认为 *
        /// </summary>
        public string Filed
        {
            get { return _selectFild; }
            set { _selectFild = value; }
        }

        /// <summary>
        /// 返回或设置查询的表
        /// </summary>
        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }



        /// <summary>
        /// 获取或设置是否进行distinct唯一性处理
        /// </summary>
        public bool Distinct
        {
            get { return this._distinct; }
            set { this._distinct = value; }
        }

        /// <summary>
        /// 获取where条件，带where关键字
        /// </summary>
        public string GetWhere
        {
            get
            {
                return string.IsNullOrEmpty(Where) ? "" : (" WHERE " + Where);
            }
        }

        /// <summary>
        /// 获取order条件，带order by关键字
        /// </summary>
        public string GetOrder
        {
            get
            {
                return string.IsNullOrEmpty(Order) ? "" : (" ORDER BY " + Order);
            }
        }

        /// <summary>
        /// 获取group条件，带group by关键字
        /// </summary>
        public string GetGroup
        {
            get
            {
                return string.IsNullOrEmpty(Group) ? "" : (" GROUP BY " + Group);
            }
        }


        /// <summary>
        /// 生成Sql串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            string sql = "SELECT ";

            if (this.Distinct)
            {
                sql += "DISTINCT ";
            }

            if (Size != -1)
            {
                sql += "TOP " + Size.ToString();
            }

            sql += " " + Filed;

            sql += " FROM ";

            sql += Table;

            return sql + GetWhere + GetOrder + GetGroup;
        }
    }
}
