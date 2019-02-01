using System;
using System.Collections.Generic;
using System.Data;

namespace Aooshi.DB
{
    /// <summary>
    /// ��������
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
        /// ��ȡ�����ò�����������Ϊ�洢����ִ��ʱ�����Ƽ�Ϊ������
        /// </summary>
        public string TableName
        {
            get { return _s_tablename; }
            set { _s_tablename = value; }
        }

        /// <summary>
        /// ���û��ȡ������򣬲��� ORDER BY �ؼ���
        /// </summary>
        public string Order
        {
            get { return _s_Order; }
            set { _s_Order = value; }
        }

        /// <summary>
        /// ��ȡ�����ò����������
        /// </summary>
        public int Size
        {
            get{ return _s_Size;}
            set { _s_Size = value;}
        }

        /// <summary>
        /// ��ȡ�����ü����ֶΣ�Ĭ��ʹ�� *
        /// </summary>
        public string Field
        {
            get{ return _s_Field;}
            set { _s_Field = value;}
        }

        /// <summary>
        /// ���û��ȡ��������������WHERE �ؼ��� ʹ�� a=1 and b=1
        /// </summary>
        public string Where
        {
            get{ return _s_Where;}
            set{ _s_Where = value; if (_s_Where == null) _s_Where = "";}
        }

        /// <summary>
        /// ��ȡ�����ò�ѯ�������ӹ�ϵ,Ĭ��ΪAND��ϵ
        /// </summary>
        public string WhereSplit
        {
            get{ return _s_split;}
            set{ _s_split = value;}
        }

        /// <summary>
        /// ���һ��ִ�в���
        /// </summary>
        /// <param name="parame">Ҫ��ӵĲ���</param>
        public void Add(IDbDataParameter parame)
        {

            if (_s_OtherParames == null)
                _s_OtherParames = new List<IDbDataParameter>();

            _s_OtherParames.Add(parame);
        }

        /// <summary>
        /// ��ȡ�����ò����б�
        /// </summary>
        public List<IDbDataParameter> Parameters
        {
            get { return _s_OtherParames; }
            set { _s_OtherParames = value; }
        }

        /// <summary>
        /// ��ȡ�������Ƿ��Դ洢����ִ�в���������ֵ����Ϊtrueһ�����Ҫ��<see cref="TableName"/>����Ϊ�洢��������
        /// </summary>
        public bool IsStoredProcedure
        {
            get{ return _IsStoredProcedure;}
            set { _IsStoredProcedure = value;}
        }


    }
}
