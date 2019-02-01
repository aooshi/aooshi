using System;
using System.Collections;

namespace Aooshi.Smtp
{
	
	/// <summary>
	/// ��ʾ����ʼ�������һ������
	/// </summary>
	public class MailAttachmentc : IEnumerable
	{
		ArrayList  list;


		/// <summary>
		/// ��ʼ����ʵ��
		/// </summary>
		public MailAttachmentc()
		{
			this.list = new ArrayList();
		}

		/// <summary>
		/// ����һ���µĸ���
		/// </summary>
		/// <param name="mt">Ҫ���ӵĸ���</param>
		public void Add(MailAttachment mt)
		{
			//if (string.IsNullOrEmpty(mt.ContentID))  //��δ����IDʱ�����ж���
			//{
			//	mt.ContentID = "AttID000" + this.Count.ToString();
			//}
			this.list.Add(mt);
		}

		/// <summary>
		/// ��������е����и���
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
		/// ��ȡ�����еĸ�������
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
