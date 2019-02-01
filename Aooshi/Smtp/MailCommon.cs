using System;
using System.Text;
namespace Aooshi.Smtp
{
	/// <summary>
	/// �������������ԣ���Ա��Ϊ��̬����
	/// </summary>
	public class MailCommon
	{

        /// <summary>
        /// ����һ���ʼ�����
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
		/// Ĭ�ϵ������˿�
		/// </summary>
		public const int    Port        = 25;

		/// <summary>
		/// ��ָ�����ı���Ϣ��������ָ�����ļ���
		/// </summary>
		/// <param name="FilePath">Ҫ������ļ�·�������ƣ�������ļ������򸲸��ļ�</param>
		/// <param name="Body">�ļ��е�����</param>
		public static void SavaAs(string FilePath,string Body)
		{
			using(System.IO.StreamWriter Sw = new System.IO.StreamWriter(FilePath,false,Encoding.Default))
			{
				Sw.Write(Body);
			}
		}

		/// <summary>
		/// ��Ŀ¼·�������������������Ϊ \ ʱ������� ��б�� \
		/// </summary>
		/// <param name="Dir">Ҫ������Ŀ¼</param>
		/// <returns>���������õ�Ŀ¼</returns>
		public static string TrimDir(string Dir)
		{
			if (string.IsNullOrEmpty(Dir)) return "";

			Dir = Dir.Replace("/","\\");

			if (!Dir.EndsWith("\\")) Dir += "\\";

			return Dir;
		}

		/// <summary>
		/// ���绺������С
		/// </summary>
		internal const int   BufferSize = 1024;
		
		/// <summary>
		/// ��ʾ�µ�һ���ַ���
		/// </summary>
		internal const string NewLine  = "\r\n";
		/// <summary>
		/// MIME ���Ͱ汾
		/// </summary>
		internal const string MimeVersion = "MIME-Version: 1.0";

		internal const string XMailer    = "X-Mailer: http://www.aooshi.org/donet/smtp";

		/// <summary>
		/// ����һ���ʼ�ID
		/// </summary>
		/// <returns>�����ʼ�ID���ַ��������ʽ</returns>
		internal static string MakeMessageID()
		{
			Random  rd = new Random();
			string Rueslt = "Message-ID: <";
			Rueslt += Coding.Base64Encode(DateTime.Now.ToString("yMMddHHmmssffffff") + rd.Next(1,30).ToString(),Encoding.ASCII);//�����ʼ�ID��ʱ������ʽ���Դ˱�ʾID���ظ� 
			Rueslt += string.Format("@{0}>" , System.Net.Dns.GetHostName() );
			return Rueslt;
		}

		/// <summary>
		/// ����һ�����ظ��ָ�
		/// </summary>
		/// <returns>�����ַ��������ʽ</returns>
		internal static string MakeBoundary()
		{
			string Rueslt = "Boundary-=_Next.";
			//���벻�ظ�ʱ��
			Random  rd = new Random();
			Rueslt += Coding.Base64Encode(DateTime.Now.ToString("yMMddHHmmssffff") + rd.Next(1,30).ToString(),Encoding.ASCII);
			return Rueslt;
		}

		/// <summary>
		/// ����һ����ʱ�ļ���
		/// </summary>
		/// <returns>�����ַ��������ʽ</returns>
		internal static string MakeTempFileName()
		{
			string Rueslt = "temp";
			Random  rd = new Random();
			//���벻�ظ�ʱ��
			Rueslt += DateTime.Now.ToString("yMMddHHmmssffffff");
			Rueslt += rd.Next(1,30).ToString();
			return Rueslt;
		}

		/// <summary>
		/// �ж��Ƿ�Ϊ�����ַ�
		/// </summary>
		/// <param name="Src">Ҫ�����жϵ��ַ���</param>
		/// <returns>����Boolֵ��ʾ�Ƿ�Ϊ�����ַ�</returns>
		internal static bool IsAscii(string Src)
		{
			char[] cs = Src.ToCharArray();

			foreach(char c in cs)
				if (c > 126 || c <32) return false;

			return true;
		}
	}
}
