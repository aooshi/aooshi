using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Aooshi
{
    
    /// <summary>
    /// Զ��HTTP���ݲ���
    /// </summary>
    public class RemoteHTTP
    {
        string _message,_reslut;
        Encoding _encoding;
        /// <summary>
        /// ʵ��������
        /// </summary>
        public RemoteHTTP()
        {
            _message = "";
            _encoding = Encoding.UTF8;
        }

        /// <summary>
        /// ��ȡ���һ��������������Ϣ
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// ��ȡ���������ݱ���,Ĭ��ΪUTF-8
        /// </summary>
        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        /// <summary>
        /// ��ȡһ��Զ�����ݵĶ�ȡ
        /// </summary>
        /// <param name="uri">URL��ַ</param>
        public bool GetRemote(string uri)
        {
            WebClient webclient = new WebClient();
            webclient.Encoding = _encoding;
            _message = "";
            _reslut = "";
            try
            {
                 _reslut = webclient.DownloadString(uri);

            }
            catch (Exception err)
            {
                _message = err.Message;
                return this.Success = false;
            }
            return this.Success = true;
        }

        bool _Success;
        /// <summary>
        /// ������һ�β����Ƿ�ִ�гɹ�
        /// </summary>
        public bool Success
        {
            get{return _Success;}
            private set { _Success = value; }
        }

        /// <summary>
        /// ��ȡ��һ�λ�ȡ������
        /// </summary>
        public string Result
        {
            get { return _reslut; }
        }
    }
}
