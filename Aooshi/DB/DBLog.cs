using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Aooshi.DB
{
    /// <summary>
    /// 数据库操作日志
    /// </summary>
    public class DBLog
    {
        List<string> list;
        /// <summary>
        /// 初始化新实例
        /// </summary>
        public DBLog()
        {
            list = new List<string>();
        }

        /// <summary>
        /// 操作内容
        /// </summary>
        /// <param name="body">内容</param>
        public void add(string body)
        {
            add(body, null);
        }
        /// <summary>
        /// 操作内容
        /// </summary>
        /// <param name="body">内容</param>
        /// <param name="Parames">参数列表</param>
        public void add(string body,IDbDataParameter[] Parames)
        {
            add(body, Parames, false);
        }
        /// <summary>
        /// 操作内容
        /// </summary>
        /// <param name="body">内容</param>
        /// <param name="Parames">参数列表</param>
        /// <param name="StoredProcedure">是否以过程运行</param>
        public void add(string body, IDbDataParameter[] Parames,bool StoredProcedure)
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.AppendLine("DB Query Begin");
            sBuilder.AppendLine("Time   >>  " + DateTime.Now.ToString("HH:mm:ss"));
            sBuilder.AppendLine("Type   >>  " + (StoredProcedure ? "StoredProcedure" : "Text") );
            sBuilder.AppendLine("Query  >>  " + body);
            if (Parames != null)
            {
                sBuilder.AppendLine("   Parames:");
                foreach (IDbDataParameter Parame in Parames)
                {
                    sBuilder.AppendFormat("       {0}   >>    {1}   >>  {2}", Parame.Direction.ToString(), Parame.ParameterName, Parame.Value == DBNull.Value ? "" : Convert.ToString(Parame.Value));
                }
            }
            sBuilder.AppendLine("DB Query End");
            sBuilder.AppendLine();
            list.Add(sBuilder.ToString());
        }
        /// <summary>
        /// 获取数据总数
        /// </summary>
        public int Count
        {
            get
            {
                return list.Count;
            }
        }
        /// <summary>
        /// 获取最后进行查询的数据串
        /// </summary>
        public string LastQuery
        {
            get
            {
                if (list.Count > 0) return list[0];
                return "";
            }
        }
        /// <summary>
        /// 获取所有操作的数据字符串
        /// </summary>
        public string[] Lists
        {
            get
            {
                return list.ToArray();
            }
        }
        /// <summary>
        /// 获取所有的数据操作文本;
        /// </summary>
        public string Text
        {
            get
            {
                StringBuilder sBuiler = new StringBuilder();
                string[] lists = list.ToArray();
                foreach (string str in lists)
                {
                    sBuiler.AppendLine(str);
                }
                return sBuiler.ToString();
            }
        }
        /// <summary>
        /// 将日志保存至指定的路径
        /// </summary>
        /// <param name="Path">保存路径</param>
        public void Save(string Path)
        {
            lock (this)
            {
                using (System.IO.StreamWriter sWriter = new System.IO.StreamWriter(string.Format("{0}\\{1}.log",Path,DateTime.Now.ToString("yyMMdd.log")),true))
                {
                    sWriter.Write(Text);
                    sWriter.Close();
                }
            }
        }
    }
}
