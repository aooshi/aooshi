using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// MySqlר���������
    /// </summary>
    public class MySqlTableToSQL:TableToSQL
    {
        /// <summary>
        /// ��ʼ���¶���
        /// </summary>
        /// <param name="Object">���ݶ���</param>
        /// <param name="factory">��ǰ��������</param>
        protected internal MySqlTableToSQL(TableBase Object,Factory factory):base(Object,factory,new List<IDbDataParameter>())
        {
        }

        /// <summary>
        /// get select sql
        /// </summary>
        public override String GetSelect()
        {
                StringBuilder tmp = new StringBuilder("SELECT ");
                //set select filed
                tmp.Append(Object.TableCondition.Field);
                tmp.Append(" ");

                //set select table
                tmp.Append("FROM ");
                tmp.Append(Object.TableCondition.TableName);
                tmp.Append(" ");

                //set select where
                tmp.Append(GetWhere());

                //set select order by
                if (Object.TableCondition.Order != null)
                {
                    tmp.Append(" ORDER BY ");
                    tmp.Append(Object.TableCondition.Order);
                }
                //set rows size
                if (Object.TableCondition.Size > 0)
                {
                    tmp.Append(" LIMIT ");
                    tmp.Append(Object.TableCondition.Size);
                }

                return tmp.ToString();
        }
    }
}
