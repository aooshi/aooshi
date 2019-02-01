using System;

namespace Aooshi.Smtp
{
	/// <summary>
	/// 异常
	/// </summary>
	public class W3SmtpException:System.Exception
	{
		/// <summary>
		/// 初始化一个默认异常
		/// </summary>
		public W3SmtpException():base()
		{
		}

		/// <summary>
		/// 初始化一个默认异常
		/// </summary>
		public W3SmtpException(string Message):base(Message)
		{
		}
	}
}
