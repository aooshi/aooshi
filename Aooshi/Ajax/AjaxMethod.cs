using System;
namespace Aooshi.Ajax
{
    /// <summary>
    /// ʵ��Ajax�������õ�����:ע��:��ע��ķ����������public����,�ұ�����Ψһû�����ص�,��Ϊ����ʱ������쳣;
    /// </summary>
    public class AjaxMethod : Attribute
    {
        private AjaxMethodType _type;
        /// <summary>
        /// ��ʼ����ʵ��
        /// </summary>
        public AjaxMethod()
        {
            this._type =  AjaxMethodType.Default;
        }

        /// <summary>
        /// ��ʼ����ʵ��
        /// </summary>
        /// <param name="methodtype">����ִ�в�������</param>
        public AjaxMethod(AjaxMethodType methodtype)
        {
            this._type = methodtype;
        }

        /// <summary>
        /// ��ȡ�����÷�������
        /// </summary>
        public AjaxMethodType MethodType
        {
            get { return _type; }
            //set { _type = value; }
        }
    }
}
