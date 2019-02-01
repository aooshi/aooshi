using System;
using System.Collections;
namespace Aooshi.Smtp
{
	/// <summary>
	/// ��ʾ����ʼ���һ������
	/// </summary>
	public class MailAddressc : IEnumerable
	{
		ArrayList  list;

		/// <summary>
		/// ��ʼ��һ���µ�ʵ��
		/// </summary>
		public MailAddressc()
		{
			this.list = new ArrayList();
		}

		/// <summary>
		/// ����һ���µ��ʼ���ַ
		/// </summary>
		/// <param name="address">Ҫ���ӵĵ�ַ</param>
		public void Add(MailAddress address)
		{
			this.list.Add(address);
		}

		/// <summary>
		/// ��������е������ʼ���ַ
		/// </summary>
		public void Clear()
		{
			this.list.Clear();
		}

		#region IEnumerable ��Ա

		/// <summary>
		/// ����һ����ѭ���ķ���
		/// </summary>
		/// <returns>����һ����ѭ���ķ���</returns>
		public IEnumerator GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		#endregion

		/// <summary>
		/// ��ȡ�����е��ʼ���ַ��
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
