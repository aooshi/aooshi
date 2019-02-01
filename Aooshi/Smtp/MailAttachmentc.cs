using System;
using System.Collections;

namespace Aooshi.Smtp
{
	
	/// <summary>
	/// 表示多个邮件附件的一个集合
	/// </summary>
	public class MailAttachmentc : IEnumerable
	{
		ArrayList  list;


		/// <summary>
		/// 初始化新实例
		/// </summary>
		public MailAttachmentc()
		{
			this.list = new ArrayList();
		}

		/// <summary>
		/// 增加一个新的附件
		/// </summary>
		/// <param name="mt">要增加的附件</param>
		public void Add(MailAttachment mt)
		{
			//if (string.IsNullOrEmpty(mt.ContentID))  //当未定义ID时，进行定议
			//{
			//	mt.ContentID = "AttID000" + this.Count.ToString();
			//}
			this.list.Add(mt);
		}

		/// <summary>
		/// 清除集合中的所有附件
		/// </summary>
		public void Clear()
		{
			this.list.Clear();
		}

		#region IEnumerable 成员

		/// <summary>
		/// 返回一个可循环的访问
		/// </summary>
		/// <returns>返回一个可循环的访问</returns>
		public IEnumerator GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		#endregion

		/// <summary>
		/// 获取集合中的附件数量
		/// </summary>
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}
	}
}
