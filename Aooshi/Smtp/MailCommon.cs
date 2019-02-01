using System;
using System.Text;
namespace Aooshi.Smtp
{
	/// <summary>
	/// 公共方法及属性，成员均为静态类型
	/// </summary>
	public class MailCommon
	{

        /// <summary>
        /// 创建一个邮件对象
        /// </summary>
        public static MailSend CreateInstance()
        {
            Aooshi.Configuration.SmtpElement smtp = Aooshi.Common.Configuration.Smtp;
            MailSend ms = new MailSend(smtp.Server);
            ms.Port = smtp.Port;
            ms.UserName = smtp.UserName;
            ms.Password = smtp.Password;
            ms.FromAddress = smtp.FromAddress;
            ms.FromName = smtp.FromName;
            return ms;
        }

		/// <summary>
		/// 默认的主机端口
		/// </summary>
		public const int    Port        = 25;

		/// <summary>
		/// 将指定的文本信息保存至，指定的文件上
		/// </summary>
		/// <param name="FilePath">要保存的文件路径及名称，如果该文件存在则覆盖文件</param>
		/// <param name="Body">文件中的内容</param>
		public static void SavaAs(string FilePath,string Body)
		{
			using(System.IO.StreamWriter Sw = new System.IO.StreamWriter(FilePath,false,Encoding.Default))
			{
				Sw.Write(Body);
			}
		}

		/// <summary>
		/// 对目录路径进行修正，并当最后不为 \ 时，则加上 返斜线 \
		/// </summary>
		/// <param name="Dir">要修正的目录</param>
		/// <returns>返回修正好的目录</returns>
		public static string TrimDir(string Dir)
		{
			if (string.IsNullOrEmpty(Dir)) return "";

			Dir = Dir.Replace("/","\\");

			if (!Dir.EndsWith("\\")) Dir += "\\";

			return Dir;
		}

		/// <summary>
		/// 网络缓冲区大小
		/// </summary>
		internal const int   BufferSize = 1024;
		
		/// <summary>
		/// 表示新的一行字符串
		/// </summary>
		internal const string NewLine  = "\r\n";
		/// <summary>
		/// MIME 类型版本
		/// </summary>
		internal const string MimeVersion = "MIME-Version: 1.0";

		internal const string XMailer    = "X-Mailer: http://www.aooshi.org/donet/smtp";

		/// <summary>
		/// 生成一个邮件ID
		/// </summary>
		/// <returns>返回邮件ID的字符串表达形式</returns>
		internal static string MakeMessageID()
		{
			Random  rd = new Random();
			string Rueslt = "Message-ID: <";
			Rueslt += Coding.Base64Encode(DateTime.Now.ToString("yMMddHHmmssffffff") + rd.Next(1,30).ToString(),Encoding.ASCII);//加入邮件ID的时间表达型式，以此表示ID不重复 
			Rueslt += string.Format("@{0}>" , System.Net.Dns.GetHostName() );
			return Rueslt;
		}

		/// <summary>
		/// 生成一个不重复分隔
		/// </summary>
		/// <returns>返回字符串表达形式</returns>
		internal static string MakeBoundary()
		{
			string Rueslt = "Boundary-=_Next.";
			//加入不重复时间
			Random  rd = new Random();
			Rueslt += Coding.Base64Encode(DateTime.Now.ToString("yMMddHHmmssffff") + rd.Next(1,30).ToString(),Encoding.ASCII);
			return Rueslt;
		}

		/// <summary>
		/// 生成一个临时文件名
		/// </summary>
		/// <returns>返回字符串表达形式</returns>
		internal static string MakeTempFileName()
		{
			string Rueslt = "temp";
			Random  rd = new Random();
			//加入不重复时间
			Rueslt += DateTime.Now.ToString("yMMddHHmmssffffff");
			Rueslt += rd.Next(1,30).ToString();
			return Rueslt;
		}

		/// <summary>
		/// 判断是否为键盘字符
		/// </summary>
		/// <param name="Src">要进行判断的字符串</param>
		/// <returns>返回Bool值表示是否为键盘字符</returns>
		internal static bool IsAscii(string Src)
		{
			char[] cs = Src.ToCharArray();

			foreach(char c in cs)
				if (c > 126 || c <32) return false;

			return true;
		}
	}
}
