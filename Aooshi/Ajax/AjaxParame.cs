using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi.Ajax
{
    /// <summary>
    /// 客户端调用方法
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
        /// 实例一个方法体
        /// </summary>
        /// <param name="Url">Ajax操作Url</param>
        public AjaxParame(string Url): this(Url, "", false, false)
        {
        }

        /// <summary>
        /// 实例一个方法体
        /// </summary>
        /// <param name="Url">Ajax操作Url</param>
        /// <param name="callBack">回调Js方法</param>
        public AjaxParame(string Url,string callBack) : this(Url, callBack, false, false)
        {
        }

        /// <summary>
        /// 实例一个方法体
        /// </summary>
        /// <param name="Url">Ajax操作Url</param>
        /// <param name="isXml">是否返回 Xml文档</param>
        public AjaxParame(string Url, bool isXml): this(Url, "", false, isXml)
        {
        }

        /// <summary>
        /// 实例一个方法体
        /// </summary>
        /// <param name="Url">Ajax操作Url</param>
        /// <param name="isPost">是否使用 Post 方法</param>
        /// <param name="isXml">是否返回 Xml文档</param>
        public AjaxParame(string Url, bool isPost, bool isXml): this(Url, "", isPost, isXml)
        {
        }

        /// <summary>
        /// 实例一个方法体
        /// </summary>
        /// <param name="Url">Ajax操作Url</param>
        /// <param name="callBack">回调Js方法</param>
        /// <param name="isPost">是否使用 Post 方法</param>
        public AjaxParame(string Url, string callBack, bool isPost):this(Url,callBack,isPost,false)
        {
        }

        /// <summary>
        /// 实例一个方法体
        /// </summary>
        /// <param name="Url">Ajax操作Url</param>
        /// <param name="callBack">回调Js方法</param>
        /// <param name="isPost">是否使用 Post 方法</param>
        /// <param name="isXml">是否返回 Xml文档</param>
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
        /// 获取或设置方法参数例表这些参数,将写入方法中并一起调用 function x(在此写入这些列表,并将数据一同传入服务器)
        /// </summary>
        public List<string> Parames
        {
            get { return _Parames; }
            set { _Parames = value; }
        }

        /// <summary>
        /// 获取或设置Ajax发送数据,注意:值中不可含有单角单引号,如果必须使用请使用\'代替,否则将出错
        /// </summary>
        public Dictionary<string, string> SendData
        {
            get { return _sendData; }
            set { _sendData = value; }
        }

        /// <summary>
        /// 获取或设置返回的对象是否为类型,默认为:字符串(false)
        /// </summary>
        public bool isXml
        {
            get { return _isXml; }
            set { _isXml = value; }
        }

        /// <summary>
        /// 获取或设置是否进行Post方式回调,默认为Get方式,不为true时,如果服务器不接受post方式将会返回一个405异常,如:向一个html页发送
        /// </summary>
        public bool isPost
        {
            get { return _isPost; }
            set { _isPost = value; }
        }

        /// <summary>
        /// 获取或设置要进行Ajax操作的远程URL可以是绝对或相对:示例: http://www.aooshi.org/donet/eajax
        /// </summary>
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        /// <summary>
        /// 获取或设置Ajax异步回调方法名称,否则使用同步往返操作,回调方法注册后必须在客户端Js中创建函数
        /// 异步回调示例: callBack = "MycallBack"; 客户端脚本:function MycallBack(obj){alert(obj);/*其它实现*/} 
        /// </summary>
        public string callBack
        {
            get { return _callBack; }
            set { _callBack = value; }
        }

        /// <summary>
        /// 获取或设置Ajax错误异常回调函数名，否则系统使用默认
        /// </summary>
        /// <remarks>
        /// 回调函数格式为：  function(msg,body){}
        /// </remarks>
        public string callError
        {
            get { return _callError; }
            set { _callError = value; }
        }
    }
}
