using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Ajax
{
    /// <summary>
    /// �ͻ��˵��÷���
    /// </summary>
    public class AjaxParame
    {
        string _Url;
        string _callBack;
        string _callError;
        bool _isPost;
        bool _isXml;
        Dictionary<string, string> _sendData;
        List<string> _Parames;


        /// <summary>
        /// ʵ��һ��������
        /// </summary>
        /// <param name="Url">Ajax����Url</param>
        public AjaxParame(string Url): this(Url, "", false, false)
        {
        }

        /// <summary>
        /// ʵ��һ��������
        /// </summary>
        /// <param name="Url">Ajax����Url</param>
        /// <param name="callBack">�ص�Js����</param>
        public AjaxParame(string Url,string callBack) : this(Url, callBack, false, false)
        {
        }

        /// <summary>
        /// ʵ��һ��������
        /// </summary>
        /// <param name="Url">Ajax����Url</param>
        /// <param name="isXml">�Ƿ񷵻� Xml�ĵ�</param>
        public AjaxParame(string Url, bool isXml): this(Url, "", false, isXml)
        {
        }

        /// <summary>
        /// ʵ��һ��������
        /// </summary>
        /// <param name="Url">Ajax����Url</param>
        /// <param name="isPost">�Ƿ�ʹ�� Post ����</param>
        /// <param name="isXml">�Ƿ񷵻� Xml�ĵ�</param>
        public AjaxParame(string Url, bool isPost, bool isXml): this(Url, "", isPost, isXml)
        {
        }

        /// <summary>
        /// ʵ��һ��������
        /// </summary>
        /// <param name="Url">Ajax����Url</param>
        /// <param name="callBack">�ص�Js����</param>
        /// <param name="isPost">�Ƿ�ʹ�� Post ����</param>
        public AjaxParame(string Url, string callBack, bool isPost):this(Url,callBack,isPost,false)
        {
        }

        /// <summary>
        /// ʵ��һ��������
        /// </summary>
        /// <param name="Url">Ajax����Url</param>
        /// <param name="callBack">�ص�Js����</param>
        /// <param name="isPost">�Ƿ�ʹ�� Post ����</param>
        /// <param name="isXml">�Ƿ񷵻� Xml�ĵ�</param>
        public AjaxParame(string Url,string callBack,bool isPost,bool isXml)
        {
            _sendData = new Dictionary<string, string>();
            _Url = Url;
            _callBack = callBack;
            _isPost = isPost;
            _isXml = isXml;
            _Parames = new List<string>();
        }

        /// <summary>
        /// ��ȡ�����÷�������������Щ����,��д�뷽���в�һ����� function x(�ڴ�д����Щ�б�,��������һͬ���������)
        /// </summary>
        public List<string> Parames
        {
            get { return _Parames; }
            set { _Parames = value; }
        }

        /// <summary>
        /// ��ȡ������Ajax��������,ע��:ֵ�в��ɺ��е��ǵ�����,�������ʹ����ʹ��\'����,���򽫳���
        /// </summary>
        public Dictionary<string, string> SendData
        {
            get { return _sendData; }
            set { _sendData = value; }
        }

        /// <summary>
        /// ��ȡ�����÷��صĶ����Ƿ�Ϊ����,Ĭ��Ϊ:�ַ���(false)
        /// </summary>
        public bool isXml
        {
            get { return _isXml; }
            set { _isXml = value; }
        }

        /// <summary>
        /// ��ȡ�������Ƿ����Post��ʽ�ص�,Ĭ��ΪGet��ʽ,��Ϊtrueʱ,���������������post��ʽ���᷵��һ��405�쳣,��:��һ��htmlҳ����
        /// </summary>
        public bool isPost
        {
            get { return _isPost; }
            set { _isPost = value; }
        }

        /// <summary>
        /// ��ȡ������Ҫ����Ajax������Զ��URL�����Ǿ��Ի����:ʾ��: http://www.aooshi.org/donet/eajax
        /// </summary>
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        /// <summary>
        /// ��ȡ������Ajax�첽�ص���������,����ʹ��ͬ����������,�ص�����ע�������ڿͻ���Js�д�������
        /// �첽�ص�ʾ��: callBack = "MycallBack"; �ͻ��˽ű�:function MycallBack(obj){alert(obj);/*����ʵ��*/} 
        /// </summary>
        public string callBack
        {
            get { return _callBack; }
            set { _callBack = value; }
        }

        /// <summary>
        /// ��ȡ������Ajax�����쳣�ص�������������ϵͳʹ��Ĭ��
        /// </summary>
        /// <remarks>
        /// �ص�������ʽΪ��  function(msg,body){}
        /// </remarks>
        public string callError
        {
            get { return _callError; }
            set { _callError = value; }
        }
    }
}
