using System;

namespace Aooshi.Web.Pagination
{
	/// <summary>
	/// 分页控件更改页序时的事件数据
	/// </summary>
	public class PostEventArgs : EventArgs
	{
		private int _Index;

		/// <summary>
		/// 获取所选定项的页序
		/// </summary>
		/// <value>Int类型值</value>
		public int Index
		{
			get
			{
				return this._Index;
			}
		}

        PostPagination _Pagination;

        /// <summary>
        /// 获取相关联的分页组件
        /// </summary>
        public PostPagination Pagination
        {
            get { return this._Pagination; }
            private set { this._Pagination = value; }
        }

		/// <summary>
		/// 初始化新的实例
		/// </summary>
		/// <param name="Index">新的页序</param>
        /// <param name="pagation">组件</param>
		internal PostEventArgs(int Index,PostPagination pagation)
		{
			this._Index = Index;
            this.Pagination = pagation;
		}
	}
}
