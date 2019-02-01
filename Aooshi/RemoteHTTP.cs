using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Aooshi
{
    
    /// <summary>
    /// 远程HTTP数据操作
    /// </summary>
    public class RemoteHTTP
    {
        string _message,_reslut;
        Encoding _encoding;
        /// <summary>
        /// 实例化数据
        /// </summary>
        public RemoteHTTP()
        {
            _message = "";
            _encoding = Encoding.UTF8;
        }

        /// <summary>
        /// 获取最后一个操作产生的消息
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// 获取或设置数据编码,默认为UTF-8
        /// </summary>
        public Encoding Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        /// <summary>
        /// 获取一个远程数据的读取
        /// </summary>
        /// <param name="uri">URL地址</param>
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
        /// 返回上一次操作是否执行成功
        /// </summary>
        public bool Success
        {
            get{return _Success;}
            private set { _Success = value; }
        }

        /// <summary>
        /// 获取上一次获取的数据
        /// </summary>
        public string Result
        {
            get { return _reslut; }
        }
    }
}
