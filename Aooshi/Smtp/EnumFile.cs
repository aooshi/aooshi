namespace Aooshi.Smtp
{

	/// <summary>
	/// 邮件内容类型
	/// </summary>
	public enum MailType
	{
		/// <summary>
		///  Html格式
		/// </summary>
		Html,
		/// <summary>
		/// 纯文本格式
		/// </summary>
		Text
	}

	/// <summary>
	/// 邮件发送紧急程度
	/// </summary>
	public enum MailPriority:int
	{
		/// <summary>
		/// 高
		/// </summary>
		High = 1,
		/// <summary>
		/// 中
		/// </summary>
		Normal =3,
		/// <summary>
		/// 低
		/// </summary>
		Low = 5
	}

	/// <summary>
	/// 邮件正文编码方式
	/// </summary>
	public enum MailEncodeType
	{
		/// <summary>
		/// 以Base64方式编码邮件正文
		/// </summary>
		BASE64,
		/// <summary>
		/// 以QuotedPrintable方式编码邮件正文
		/// </summary>
		QP
	}
}
