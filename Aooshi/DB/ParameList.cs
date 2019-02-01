using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Aooshi.DB
{
    /// <summary>
    /// 参数构造列表
    /// </summary>
    public class ParameList : List<IDataParameter>
    {
        /// <summary>
        /// 添加一个新元数
        /// </summary>
        /// <param name="parameter">参数</param>
        /// <returns>当前添加的参数</returns>
        public IDataParameter AddParameter(IDataParameter parameter)
        {
            base.Add(parameter);

            return parameter;
        }


        /// <summary>
        /// 插入一个新元数
        /// </summary>
        /// <param name="index">参数序列</param>
        /// <param name="parameter">参数</param>
        /// <returns>当前添加的参数</returns>
        public IDataParameter InsertParameter(int index,IDataParameter parameter)
        {
            base.Insert(index, parameter);
            return parameter;
        }

    }
}
