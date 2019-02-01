using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Aooshi.DB
{
    /// <summary>
    /// ���������б�
    /// </summary>
    public class ParameList : List<IDataParameter>
    {
        /// <summary>
        /// ���һ����Ԫ��
        /// </summary>
        /// <param name="parameter">����</param>
        /// <returns>��ǰ��ӵĲ���</returns>
        public IDataParameter AddParameter(IDataParameter parameter)
        {
            base.Add(parameter);

            return parameter;
        }


        /// <summary>
        /// ����һ����Ԫ��
        /// </summary>
        /// <param name="index">��������</param>
        /// <param name="parameter">����</param>
        /// <returns>��ǰ��ӵĲ���</returns>
        public IDataParameter InsertParameter(int index,IDataParameter parameter)
        {
            base.Insert(index, parameter);
            return parameter;
        }

    }
}
