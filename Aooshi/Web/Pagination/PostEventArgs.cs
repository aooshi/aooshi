using System;

namespace Aooshi.Web.Pagination
{
	/// <summary>
	/// ��ҳ�ؼ�����ҳ��ʱ���¼�����
	/// </summary>
	public class PostEventArgs : EventArgs
	{
		private int _Index;

		/// <summary>
		/// ��ȡ��ѡ�����ҳ��
		/// </summary>
		/// <value>Int����ֵ</value>
		public int Index
		{
			get
			{
				return this._Index;
			}
		}

        PostPagination _Pagination;

        /// <summary>
        /// ��ȡ������ķ�ҳ���
        /// </summary>
        public PostPagination Pagination
        {
            get { return this._Pagination; }
            private set { this._Pagination = value; }
        }

		/// <summary>
		/// ��ʼ���µ�ʵ��
		/// </summary>
		/// <param name="Index">�µ�ҳ��</param>
        /// <param name="pagation">���</param>
		internal PostEventArgs(int Index,PostPagination pagation)
		{
			this._Index = Index;
            this.Pagination = pagation;
		}
	}
}
