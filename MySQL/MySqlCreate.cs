using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// MySql��乹��
    /// </summary>
    public class MySqlCreate : Aooshi.DB.SqlCreate
    {
        /// <summary>
        /// ����Sql��
        /// </summary>
        /// <returns>�ַ���</returns>
        public override string ToString()
        {
            string sql = "SELECT ";

            if (base.Distinct)
                sql += "DISTINCT ";
            
            sql += string.Format("{0} FROM {1}", Filed, Table) + GetWhere + GetOrder + GetGroup;
            if (Size != -1)
                sql += " LIMIT " + Size.ToString();

            return sql;
        }
    }
}
