using System;
using System.Collections.Generic;

namespace Aooshi.Ajax
{

    /// <summary>
    /// ͨ��Ajax���׷��ض���
    /// </summary>
    [AjaxObject]
    public class AjaxResult
    {
        bool _Success;
        List<string> _MessageArray;
        object _TagA,_TagB;
        string _Message;


        #region �ɹ��Ĳ�������

        /// <summary>
        /// ����һ�����κθ�����Ϣ�ĳɹ���������
        /// </summary>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateSuccessResult()
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            return Result;
        }

        /// <summary>
        /// ����һ���ĳɹ���������,������һ��������Ϣ
        /// </summary>
        /// <param name="TagA">������Ϣ</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateSuccessResult(object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result._TagA = TagA;
            return Result;
        }


        /// <summary>
        /// ����һ���ĳɹ���������,������һ��������Ϣ
        /// </summary>
        /// <param name="TagA">������Ϣ</param>
        /// <param name="TagB">������ϢB</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateSuccessResult(object TagA,object TagB)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result._TagA = TagA;
            Result._TagB = TagB;
            return Result;
        }

        /// <summary>
        /// ����һ��������Ϣ�ĳɹ���������
        /// </summary>
        /// <param name="Message">Ҫ���ӵ���Ϣ</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateSuccessResult(string Message)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result._Message = Message;
            return Result;
        }

        /// <summary>
        /// ����һ��������Ϣ�ĳɹ���������,������һ�����Ӷ���
        /// </summary>
        /// <param name="Message">Ҫ���ӵ���Ϣ</param>
        /// <param name="TagA">������Ϣ</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateSuccessResult(string Message,object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result._Message = Message;
            Result._TagA = TagA;
            return Result;
        }

        /// <summary>
        /// ����һ��������Ϣ�ĳɹ���������,������һ�����Ӷ���
        /// </summary>
        /// <param name="Message">Ҫ���ӵ���Ϣ</param>
        /// <param name="TagA">������Ϣ</param>
        /// <param name="TagB">������ϢB</param>
        /// <returns>�����µĶ���</returns>
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
        /// ����һ��������Ϣ��ĳɹ���������
        /// </summary>
        /// <param name="MessageArray">Ҫ���ӵ���Ϣ��</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateSuccessResult(string[] MessageArray)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result.AppendMessage(MessageArray);
            return Result;
        }

        /// <summary>
        /// ����һ��������Ϣ��ĳɹ���������,������һ�����Ӷ���
        /// </summary>
        /// <param name="MessageArray">Ҫ���ӵ���Ϣ��</param>
        /// <param name="TagA">Ҫ���ӵĶ���</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateSuccessResult(string[] MessageArray,object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = true;
            Result.AppendMessage(MessageArray);
            Result._TagA = TagA;
            return Result;
        }

        /// <summary>
        /// ����һ��������Ϣ��ĳɹ���������,������һ�����Ӷ���
        /// </summary>
        /// <param name="MessageArray">Ҫ���ӵ���Ϣ��</param>
        /// <param name="TagA">Ҫ���ӵĶ���</param>
        /// <param name="TagB">Ҫ���ӵĶ���</param>
        /// <returns>�����µĶ���</returns>
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


        #region ʧ�ܵĲ�������

        /// <summary>
        /// ����һ�����κθ�����Ϣ��ʧ�ܲ�������
        /// </summary>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateNotSuccessResult()
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            return Result;
        }

        /// <summary>
        /// ����һ����ʧ�ܲ�������,������һ��������Ϣ
        /// </summary>
        /// <param name="TagA">������Ϣ</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateNotSuccessResult(object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result._TagA = TagA;
            return Result;
        }


        /// <summary>
        /// ����һ����ʧ�ܲ�������,������һ��������Ϣ
        /// </summary>
        /// <param name="TagA">������Ϣ</param>
        /// <param name="TagB">������ϢB</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateNotSuccessResult(object TagA, object TagB)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result._TagA = TagA;
            Result._TagB = TagB;
            return Result;
        }

        /// <summary>
        /// ����һ��������Ϣ��ʧ�ܲ�������
        /// </summary>
        /// <param name="Message">Ҫ���ӵ���Ϣ</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateNotSuccessResult(string Message)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result._Message = Message;
            return Result;
        }

        /// <summary>
        /// ����һ��������Ϣ��ʧ�ܲ�������,������һ�����Ӷ���
        /// </summary>
        /// <param name="Message">Ҫ���ӵ���Ϣ</param>
        /// <param name="TagA">������Ϣ</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateNotSuccessResult(string Message, object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result._Message = Message;
            Result._TagA = TagA;
            return Result;
        }

        /// <summary>
        /// ����һ��������Ϣ��ʧ�ܲ�������,������һ�����Ӷ���
        /// </summary>
        /// <param name="Message">Ҫ���ӵ���Ϣ</param>
        /// <param name="TagA">������Ϣ</param>
        /// <param name="TagB">������ϢB</param>
        /// <returns>�����µĶ���</returns>
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
        /// ����һ��������Ϣ���ʧ�ܲ�������
        /// </summary>
        /// <param name="MessageArray">Ҫ���ӵ���Ϣ��</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateNotSuccessResult(string[] MessageArray)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result.AppendMessage(MessageArray);
            return Result;
        }

        /// <summary>
        /// ����һ��������Ϣ���ʧ�ܲ�������,������һ�����Ӷ���
        /// </summary>
        /// <param name="MessageArray">Ҫ���ӵ���Ϣ��</param>
        /// <param name="TagA">Ҫ���ӵĶ���</param>
        /// <returns>�����µĶ���</returns>
        public static AjaxResult CreateNotSuccessResult(string[] MessageArray, object TagA)
        {
            AjaxResult Result = new AjaxResult();
            Result._Success = false;
            Result.AppendMessage(MessageArray);
            Result._TagA = TagA;
            return Result;
        }

        /// <summary>
        /// ����һ��������Ϣ���ʧ�ܲ�������,������һ�����Ӷ���
        /// </summary>
        /// <param name="MessageArray">Ҫ���ӵ���Ϣ��</param>
        /// <param name="TagA">Ҫ���ӵĶ���</param>
        /// <param name="TagB">Ҫ���ӵĶ���</param>
        /// <returns>�����µĶ���</returns>
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
        /// ��ʼ���������
        /// </summary>
        public AjaxResult(){}

        /// <summary>
        /// ��ȡ�����ø�����Ϣ
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        /// <summary>
        /// ��ȡ������Ϣ����
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
        /// ��ȡ������ϢA
        /// </summary>
        public object TagA
        {
            get { return _TagA; }
            set { _TagA = value; }
        }
        /// <summary>
        /// ��ȡ������ϢB
        /// </summary>
        public object TagB
        {
            get { return _TagB; }
            set { _TagB = value; }
        }


        /// <summary>
        /// ����Ϣ��������һ���µ���Ϣ
        /// </summary>
        /// <param name="Message">Ҫ���ӵ���Ϣֵ</param>
        public void AppendMessage(string Message)
        {
            if (_MessageArray == null) _MessageArray = new List<string>();
            _MessageArray.Add(Message);
        }
        /// <summary>
        /// ����Ϣ��������һ���µ���Ϣ����
        /// </summary>
        /// <param name="Messages">Ҫ���ӵ���Ϣ����</param>
        public void AppendMessage(string[] Messages)
        {
            if (_MessageArray == null) _MessageArray = new List<string>();
            _MessageArray.AddRange(Messages);
        }

        /// <summary>
        /// ����Ϣ��������һ���µ���Ϣ,�������ʽ�������и�ʽ��
        /// </summary>
        /// <param name="MessageFormat">��Ϣ��ʽ��</param>
        /// <param name="Ags">���ʽ����ƥ�����Ϣ��</param>
        public void AppendMessage(string MessageFormat, params object[] Ags)
        {
            AppendMessage(string.Format(MessageFormat, Ags));
        }

        /// <summary>
        /// �����Ϣ���е���Ϣ
        /// </summary>
        public void Clear()
        {
            if (_MessageArray != null)
                _MessageArray = null;
        }

        /// <summary>
        /// ��ȡ��Ϣ��
        /// </summary>
        public string[] MessageArray
        {
            get {return _MessageArray.ToArray();}
        }

        /// <summary>
        /// ��ȡ�������Ƿ�ɹ�
        /// </summary>
        public bool Success
        {
            get { return _Success; }
            set { _Success = value; }
        }
    }
}
 