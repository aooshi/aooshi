using System;
using System.Collections.Generic;

namespace Aooshi.Ajax
{

    /// <summary>
    /// 通用Ajax简易返回对象
    /// </summary>
    [AjaxObject]
    public class AjaxResult
    {
        bool _Success;
        List<string> _MessageArray;
        object _TagA,_TagB;
        string _Message;


        #region 成功的操作数据

        /// <summary>
        /// 返回一个无任何附加信息的成功操作对象
        /// </summary>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateSuccessResult()
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            return Result;
        }

        /// <summary>
        /// 返回一个的成功操作对象,并附加一个附加信息
        /// </summary>
        /// <param name="TagA">附加信息</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateSuccessResult(object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result._TagA = TagA;
            return Result;
        }


        /// <summary>
        /// 返回一个的成功操作对象,并附加一个附加信息
        /// </summary>
        /// <param name="TagA">附加信息</param>
        /// <param name="TagB">附加信息B</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateSuccessResult(object TagA,object TagB)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result._TagA = TagA;
            Result._TagB = TagB;
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息的成功操作对象
        /// </summary>
        /// <param name="Message">要附加的信息</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateSuccessResult(string Message)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result._Message = Message;
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息的成功操作对象,并附加一个附加对象
        /// </summary>
        /// <param name="Message">要附加的信息</param>
        /// <param name="TagA">附加信息</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateSuccessResult(string Message,object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result._Message = Message;
            Result._TagA = TagA;
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息的成功操作对象,并附加一个附加对象
        /// </summary>
        /// <param name="Message">要附加的信息</param>
        /// <param name="TagA">附加信息</param>
        /// <param name="TagB">附加信息B</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateSuccessResult(string Message, object TagA,object TagB)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result._Message = Message;
            Result._TagA = TagA;
            Result._TagB = TagB;
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息组的成功操作对象
        /// </summary>
        /// <param name="MessageArray">要附加的信息组</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateSuccessResult(string[] MessageArray)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result.AppendMessage(MessageArray);
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息组的成功操作对象,并附加一个附加对象
        /// </summary>
        /// <param name="MessageArray">要附加的信息组</param>
        /// <param name="TagA">要附加的对象</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateSuccessResult(string[] MessageArray,object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result.AppendMessage(MessageArray);
            Result._TagA = TagA;
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息组的成功操作对象,并附加一个附加对象
        /// </summary>
        /// <param name="MessageArray">要附加的信息组</param>
        /// <param name="TagA">要附加的对象</param>
        /// <param name="TagB">要附加的对象</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateSuccessResult(string[] MessageArray, object TagA,object TagB)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result.AppendMessage(MessageArray);
            Result._TagA = TagA;
            Result._TagB = TagB;
            return Result;
        }


        #endregion


        #region 失败的操作数据

        /// <summary>
        /// 返回一个无任何附加信息的失败操作对象
        /// </summary>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateNotSuccessResult()
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            return Result;
        }

        /// <summary>
        /// 返回一个的失败操作对象,并附加一个附加信息
        /// </summary>
        /// <param name="TagA">附加信息</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateNotSuccessResult(object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result._TagA = TagA;
            return Result;
        }


        /// <summary>
        /// 返回一个的失败操作对象,并附加一个附加信息
        /// </summary>
        /// <param name="TagA">附加信息</param>
        /// <param name="TagB">附加信息B</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateNotSuccessResult(object TagA, object TagB)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result._TagA = TagA;
            Result._TagB = TagB;
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息的失败操作对象
        /// </summary>
        /// <param name="Message">要附加的信息</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateNotSuccessResult(string Message)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result._Message = Message;
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息的失败操作对象,并附加一个附加对象
        /// </summary>
        /// <param name="Message">要附加的信息</param>
        /// <param name="TagA">附加信息</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateNotSuccessResult(string Message, object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result._Message = Message;
            Result._TagA = TagA;
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息的失败操作对象,并附加一个附加对象
        /// </summary>
        /// <param name="Message">要附加的信息</param>
        /// <param name="TagA">附加信息</param>
        /// <param name="TagB">附加信息B</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateNotSuccessResult(string Message, object TagA, object TagB)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result._Message = Message;
            Result._TagA = TagA;
            Result._TagB = TagB;
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息组的失败操作对象
        /// </summary>
        /// <param name="MessageArray">要附加的信息组</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateNotSuccessResult(string[] MessageArray)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result.AppendMessage(MessageArray);
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息组的失败操作对象,并附加一个附加对象
        /// </summary>
        /// <param name="MessageArray">要附加的信息组</param>
        /// <param name="TagA">要附加的对象</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateNotSuccessResult(string[] MessageArray, object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result.AppendMessage(MessageArray);
            Result._TagA = TagA;
            return Result;
        }

        /// <summary>
        /// 返回一个附加信息组的失败操作对象,并附加一个附加对象
        /// </summary>
        /// <param name="MessageArray">要附加的信息组</param>
        /// <param name="TagA">要附加的对象</param>
        /// <param name="TagB">要附加的对象</param>
        /// <returns>返回新的对象</returns>
        public static AjaxResult CreateNotSuccessResult(string[] MessageArray, object TagA, object TagB)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result.AppendMessage(MessageArray);
            Result._TagA = TagA;
            Result._TagB = TagB;
            return Result;
        }


        #endregion


        /// <summary>
        /// 初始化结果对象
        /// </summary>
        public AjaxResult(){}

        /// <summary>
        /// 获取或设置附加信息
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        /// <summary>
        /// 获取附加信息总数
        /// </summary>
        public int MessageCount
        {
            get
            {
                if (_MessageArray == null) return 0;
                return _MessageArray.Count;
            }
        }

        /// <summary>
        /// 获取附加信息A
        /// </summary>
        public object TagA
        {
            get { return _TagA; }
            set { _TagA = value; }
        }
        /// <summary>
        /// 获取附加信息B
        /// </summary>
        public object TagB
        {
            get { return _TagB; }
            set { _TagB = value; }
        }


        /// <summary>
        /// 向信息组中增加一个新的信息
        /// </summary>
        /// <param name="Message">要增加的信息值</param>
        public void AppendMessage(string Message)
        {
            if (_MessageArray == null) _MessageArray = new List<string>();
            _MessageArray.Add(Message);
        }
        /// <summary>
        /// 向信息组中增加一个新的信息数组
        /// </summary>
        /// <param name="Messages">要增加的信息数组</param>
        public void AppendMessage(string[] Messages)
        {
            if (_MessageArray == null) _MessageArray = new List<string>();
            _MessageArray.AddRange(Messages);
        }

        /// <summary>
        /// 向信息数组增加一个新的信息,并按其格式化串进行格式化
        /// </summary>
        /// <param name="MessageFormat">信息格式串</param>
        /// <param name="Ags">与格式化相匹配的信息组</param>
        public void AppendMessage(string MessageFormat, params object[] Ags)
        {
            AppendMessage(string.Format(MessageFormat, Ags));
        }

        /// <summary>
        /// 清除信息组中的信息
        /// </summary>
        public void Clear()
        {
            if (_MessageArray != null)
                _MessageArray = null;
        }

        /// <summary>
        /// 获取信息组
        /// </summary>
        public string[] MessageArray
        {
            get {return _MessageArray.ToArray();}
        }

        /// <summary>
        /// 获取或设置是否成功
        /// </summary>
        public bool Success
        {
            get { return _Success; }
            set { _Success = value; }
        }
    }
}
 