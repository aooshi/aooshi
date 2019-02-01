using System;
using System.Collections;
namespace Aooshi.Smtp
{
	/// <summary>
	/// 表示多个邮件的一个集合
	/// </summary>
	public class MailAddressc : IEnumerable
	{
		ArrayList  list;

		/// <summary>
		/// 初始化一个新的实例
		/// </summary>
		public MailAddressc()
		{
			this.list = new ArrayList();
		}

		/// <summary>
		/// 增加一个新的邮件地址
		/// </summary>
		/// <param name="address">要增加的地址</param>
		public void Add(MailAddress address)
		{
			this.list.Add(address);
		}

		/// <summary>
		/// 清除集合中的所有邮件地址
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
		/// 获取集合中的邮件地址数
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
