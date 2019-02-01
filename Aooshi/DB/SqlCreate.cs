using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// Sql��������
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
        /// ��ʼ��
        /// </summary>
        protected internal SqlCreate()
        {
            this._selectFild = "*";
            this._size = -1;
            this._where = "";
            this._distinct = false;
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="Sql">Ҫ�����ӵ�������</param>
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
        /// ������������
        /// </summary>
        /// <param name="Sql">Ҫ�����ӵ�������</param>
        /// <param name="data">�������</param>
        public virtual void AddAndWhere(string Sql,params object[] data)
        {
            this.AddAndWhere(string.Format(Sql, data));
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="Sql">Ҫ�����ӵ�������</param>
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
        /// ������������
        /// </summary>
        /// <param name="Sql">Ҫ�����ӵ�������</param>
        /// <param name="data">�������</param>
        public virtual void AddOrWhere(string Sql, params object[] data)
        {
            this.AddOrWhere(string.Format(Sql, data));
        }


        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="item">Ҫ���ӵ�����</param>
        public virtual void AddOrder(string item)
        {
            if (!string.IsNullOrEmpty(Order)) Order += ",";
            Order += item;
        }

        /// <summary>
        /// ���ӷ�������
        /// </summary>
        /// <param name="item">Ҫ���ӵ�����</param>
        public virtual void AddGroup(string item)
        {
            if (!string.IsNullOrEmpty(Group)) Group += ",";
            Group += item;
        }

        /// <summary>
        /// ��ȡ�����ü�ֵ
        /// </summary>
        public string Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        /// <summary>
        /// ���ػ�������������
        /// </summary>
        public string Order
        {
            get { return _order; }
            set { _order = value; }
        }

        /// <summary>
        /// ���ػ����÷���
        /// </summary>
        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }

        /// <summary>
        /// ���ػ����ò�ѯ������
        /// </summary>
        public string Where
        {
            get { return _where; }
            set { _where = value; }
        }

        /// <summary>
        /// ���ػ���������
        /// </summary>
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// ���ػ����ò�ѯ�ֶ�,Ĭ��Ϊ *
        /// </summary>
        public string Filed
        {
            get { return _selectFild; }
            set { _selectFild = value; }
        }

        /// <summary>
        /// ���ػ����ò�ѯ�ı�
        /// </summary>
        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }



        /// <summary>
        /// ��ȡ�������Ƿ����distinctΨһ�Դ���
        /// </summary>
        public bool Distinct
        {
            get { return this._distinct; }
            set { this._distinct = value; }
        }

        /// <summary>
        /// ��ȡwhere��������where�ؼ���
        /// </summary>
        public string GetWhere
        {
            get
            {
                return string.IsNullOrEmpty(Where) ? "" : (" WHERE " + Where);
            }
        }

        /// <summary>
        /// ��ȡorder��������order by�ؼ���
        /// </summary>
        public string GetOrder
        {
            get
            {
                return string.IsNullOrEmpty(Order) ? "" : (" ORDER BY " + Order);
            }
        }

        /// <summary>
        /// ��ȡgroup��������group by�ؼ���
        /// </summary>
        public string GetGroup
        {
            get
            {
                return string.IsNullOrEmpty(Group) ? "" : (" GROUP BY " + Group);
            }
        }


        /// <summary>
        /// ����Sql��
        /// </summary>
        /// <returns>�ַ���</returns>
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
