using System;
using System.Text;


namespace Aooshi.Smtp
{
	/// <summary>
	/// 表示一个邮件的地址
	/// </summary>
	public class MailAddress
	{
		string address;
		string name;

		/// <summary>
		/// 初始一个默认新实例
		/// </summary>
		public MailAddress():this("","")
		{
		}

		/// <summary>
		/// 初始化实例
		/// </summary>
		/// <param name="Address">要实例的邮件地址</param>
		public MailAddress(string Address):this(Address,"")
		{
		}

		/// <summary>
		/// 初始化实例
		/// </summary>
		/// <param name="Address">要实例的邮件地址</param>
		/// <param name="Name">要实例的邮件所有人</param>
		public MailAddress(string Address,string Name)
		{
			this.address = Address;
			this.name    = Name;
		}

		/// <summary>
		/// 获取或设置邮件地址
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
		/// 获取或设置邮件所有人的姓名
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
		/// 获取该邮件的 Base64 编码方式编码结果，当未设置姓名时，姓名即为邮件地址
		/// </summary>
		/// <param name="enc">要进行编码的编码类型</param>
		/// <param name="Charset">文件编码字符集</param>
		internal protected virtual string Base64Encoding(Encoding enc,string Charset)
		{
			//判断姓名是否为空
			if (string.IsNullOrEmpty(this.Name))
				return string.Format("\"{0}\" <{0}>",this.Address);

			//判断姓名是否为键盘可打印，如果是，则不进行编码，如果不是，则进行编码
			if (MailCommon.IsAscii(this.Name))
				return string.Format("\"{0}\" <{1}>",this.Name,this.Address);
			
			//进行编码输出
			return Coding.EncodBase64Head(this.Name,enc,Charset) + string.Format(" <{0}>",this.Address);
		}
		
		/// <summary>
		/// 获取该邮件的QP编码方式编码结果，当未设置姓名时，姓名即为邮件地址
		/// </summary>
		/// <param name="enc">要进行编码的编码类型</param>
		/// <param name="Charset">文件编码字符集</param>
		internal protected virtual string QPEncoding(Encoding enc,string Charset)
		{
			//判断姓名是否为空
			if (string.IsNullOrEmpty(this.Name))
				return string.Format("\"{0}\" <{0}>",this.Address);

			//判断姓名是否为键盘可打印，如果是，则不进行编码，如果不是，则进行编码
			if (MailCommon.IsAscii(this.Name))
				return string.Format("\"{0}\" <{1}>",this.Name,this.Address);
			
			//进行编码输出
			return  Coding.EncodQPHead(this.Name,enc,Charset) + string.Format(" <{0}>",this.Address);
		}
	}
}
