using System;

namespace Aooshi.Smtp
{
	/// <summary>
	/// �쳣
	/// </summary>
	public class W3SmtpException:System.Exception
	{
		/// <summary>
		/// ��ʼ��һ��Ĭ���쳣
		/// </summary>
		public W3SmtpException():base()
		{
		}

		/// <summary>
		/// ��ʼ��һ��Ĭ���쳣
		/// </summary>
		public W3SmtpException(string Message):base(Message)
		{
		}
	}
}
