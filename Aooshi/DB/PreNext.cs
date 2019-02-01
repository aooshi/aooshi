using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.DB
{
    /// <summary>
    /// ���ݶ���,��һ��,����,��һ��,����¼SQL�������
    /// </summary>
    public class PreNext
    {
        string _KeyName;
        string _KeyValue;
        string _Table;
        string _Where;
        string _KeyStr;

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="KeyName">������(�˼�����Ϊ�������������������Ըü�����������)</param>
        /// <param name="KeyValue">����ֵ</param>
        /// <param name="Table">������</param>
        public PreNext(string KeyName, string KeyValue, string Table)
            : this(KeyName, KeyValue, Table, null)
        {
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="KeyName">������(�˼�����Ϊ�������������������Ըü�����������)</param>
        /// <param name="KeyValue">����ֵ</param>
        /// <param name="Table">������</param>
        /// <param name="Where">�����������,������������Ϊnull</param>
        public PreNext(string KeyName, string KeyValue, string Table, string Where)
        {
            _KeyName = DBCommon.Replace(KeyName);
            _Table = DBCommon.Replace(Table);
            _KeyValue = KeyValue;
            _Where = string.IsNullOrEmpty(Where) ? "" : Where;
            _KeyStr = "";
        }


        /*
        select top 1 * from infotab where iid < '200709162040515754' order by iid desc
        select top 1 * from infotab where iid = '200709162040515754'
        select top 1 * from infotab where iid > '200709162040515754' order by iid asc
        */

        /// <summary>
        /// ������һ�����������
        /// </summary>
        public virtual string Pre() { return Pre(null); }

        /// <summary>
        /// ������һ�����������
        /// </summary>
        /// <param name="Fields">���ص��ֶ�����</param>
        public virtual string Pre(string Fields)
        {
            Fields = RepField(Fields);
            string sql = "SELECT TOP 1 {0} FROM {1} WHERE {2}<{3} {4} ORDER BY {2} DESC";
            return string.Format(sql, Fields, _Table, _KeyName, KeyValue, _Where);
        }

        /// <summary>
        /// ���ɱ������������
        /// </summary>
        public virtual string This() { return This(null); }
        /// <summary>
        /// ���ɱ������������
        /// </summary>
        /// <param name="Fields">���ص��ֶ�����</param>
        public virtual string This(string Fields)
        {
            Fields = RepField(Fields);
            string sql = "SELECT TOP 1 {0} FROM {1} WHERE {2}={3} {4}";
            return string.Format(sql, Fields, _Table, _KeyName, KeyValue, _Where);
        }


        /// <summary>
        /// ������һ�����������
        /// </summary>
        public virtual string Next() { return Next(null); }

        /// <summary>
        /// ������һ�����������
        /// </summary>
        /// <param name="Fields">���ص��ֶ�����</param>
        public virtual string Next(string Fields)
        {
            Fields = RepField(Fields);
            string sql = "SELECT TOP 1 {0} FROM {1} WHERE {2}>{3} {4} ORDER BY {2} ASC";
            return string.Format(sql, Fields, _Table, _KeyName, KeyValue, _Where);
        }

        /// <summary>
        /// �����ֶθ�ֵ����
        /// </summary>
        /// <param name="Fields">�ֶ�</param>
        protected virtual string RepField(string Fields)
        {
            return string.IsNullOrEmpty(Fields) ? "*" : DBCommon.Replace(Fields);
        }

        /// <summary>
        /// ��ȡֵ
        /// </summary>
        protected string KeyValue
        {
            get { return KeyStr + _KeyValue + KeyStr; }
        }
        /// <summary>
        /// ��ȡ������ֵ����Ҫ�İ�����,Ĭ��û���κΰ�����,��:�ַ���ֵӦ�üӵ����Ž���������;
        /// </summary>
        public string KeyStr
        {
            get { return _KeyStr; }
            set { _KeyStr = (_KeyValue == null) ? "" : value; }
        }
    }
}
