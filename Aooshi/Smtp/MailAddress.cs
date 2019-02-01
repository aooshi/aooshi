using System;
using System.Text;


namespace Aooshi.Smtp
{
	/// <summary>
	/// ��ʾһ���ʼ��ĵ�ַ
	/// </summary>
	public class MailAddress
	{
		string address;
		string name;

		/// <summary>
		/// ��ʼһ��Ĭ����ʵ��
		/// </summary>
		public MailAddress():this("","")
		{
		}

		/// <summary>
		/// ��ʼ��ʵ��
		/// </summary>
		/// <param name="Address">Ҫʵ�����ʼ���ַ</param>
		public MailAddress(string Address):this(Address,"")
		{
		}

		/// <summary>
		/// ��ʼ��ʵ��
		/// </summary>
		/// <param name="Address">Ҫʵ�����ʼ���ַ</param>
		/// <param name="Name">Ҫʵ�����ʼ�������</param>
		public MailAddress(string Address,string Name)
		{
			this.address = Address;
			this.name    = Name;
		}

		/// <summary>
		/// ��ȡ�������ʼ���ַ
		/// </summary>
		public string Address
		{
			get
			{
				return this.address;
			}
			set
			{
				this.address = value;
			}
		}

		/// <summary>
		/// ��ȡ�������ʼ������˵�����
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>
		/// ��ȡ���ʼ��� Base64 ���뷽ʽ����������δ��������ʱ��������Ϊ�ʼ���ַ
		/// </summary>
		/// <param name="enc">Ҫ���б���ı�������</param>
		/// <param name="Charset">�ļ������ַ���</param>
		internal protected virtual string Base64Encoding(Encoding enc,string Charset)
		{
			//�ж������Ƿ�Ϊ��
			if (string.IsNullOrEmpty(this.Name))
				return string.Format("\"{0}\" <{0}>",this.Address);

			//�ж������Ƿ�Ϊ���̿ɴ�ӡ������ǣ��򲻽��б��룬������ǣ�����б���
			if (MailCommon.IsAscii(this.Name))
				return string.Format("\"{0}\" <{1}>",this.Name,this.Address);
			
			//���б������
			return Coding.EncodBase64Head(this.Name,enc,Charset) + string.Format(" <{0}>",this.Address);
		}
		
		/// <summary>
		/// ��ȡ���ʼ���QP���뷽ʽ����������δ��������ʱ��������Ϊ�ʼ���ַ
		/// </summary>
		/// <param name="enc">Ҫ���б���ı�������</param>
		/// <param name="Charset">�ļ������ַ���</param>
		internal protected virtual string QPEncoding(Encoding enc,string Charset)
		{
			//�ж������Ƿ�Ϊ��
			if (string.IsNullOrEmpty(this.Name))
				return string.Format("\"{0}\" <{0}>",this.Address);

			//�ж������Ƿ�Ϊ���̿ɴ�ӡ������ǣ��򲻽��б��룬������ǣ�����б���
			if (MailCommon.IsAscii(this.Name))
				return string.Format("\"{0}\" <{1}>",this.Name,this.Address);
			
			//���б������
			return  Coding.EncodQPHead(this.Name,enc,Charset) + string.Format(" <{0}>",this.Address);
		}
	}
}
