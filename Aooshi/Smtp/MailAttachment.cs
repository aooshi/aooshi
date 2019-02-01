using System;
using System.Security.Cryptography;
using System.Globalization;
using System.IO;
using System.Text;

namespace Aooshi.Smtp
{	/// <summary>
	/// ��ʾһ���ʼ�����
	/// </summary>
	public class MailAttachment
	{
		Stream  sm;
		string  name;
		long    size;
		string  mimeType;
		string  tempname;
		string  tempdir;
		FileStream fin;
		string contentid;

		/// <summary>
		/// ��������ʵ��
		/// </summary>
		/// <param name="FilePath">ָ��������·��</param>
		/// <param name="TempDir">���ڴ洢����������ʱ�ļ�Ŀ¼</param>
		public MailAttachment(string FilePath,string TempDir)
		{
			if (string.IsNullOrEmpty(FilePath)) throw new ArgumentNullException("FilePath","FilePath Is Null!");

			FileInfo fi = new FileInfo(FilePath);
			if (!fi.Exists) throw new ArgumentException("Not Find File!","FilePath");
			sm = fi.Open(FileMode.Open,FileAccess.Read,FileShare.Read); //���ļ�

			this.Name = fi.Name;
			this.TempDir = TempDir;

			this.Init();
		}

		/// <summary>
		/// ��ʼ����ʵ��
		/// </summary>
		/// <param name="stream">ָ��������</param>
		/// <param name="TempDir">���ڴ洢����������ʱ�ļ�Ŀ¼</param>
		/// <param name="Name">�ļ�����</param>
		public MailAttachment(Stream stream,string TempDir,string Name)
		{
			this.sm = stream;
			this.Name = Name;
			this.TempDir = TempDir;
			this.Init();
		}

		/// <summary>
		/// ��ɵĳ�ʼ
		/// </summary>
		void Init()
		{
			this.size = this.sm.Length;
			this.mimeType  = "application/octet-stream";
			tempname = MailCommon.MakeTempFileName();
			this.fin = null;
			this.contentid = "";
		}

		/// <summary>
		/// ��ȡ�����ø���ID
		/// </summary>
		public string ContentID
		{
			get
			{
				return this.contentid;
			}
			set
			{
				this.contentid = value;
			}
		}

		/// <summary>
		/// ��ȡ�ø����ĳ���
		/// </summary>
		public long Length
		{
			get
			{
				return this.size;
			}
		}

		/// <summary>
		/// ��ȡ�����ø�������ʱ����ʱ�ļ��洢λ��
		/// </summary>
		public string TempDir
		{
			get
			{
				return this.tempdir;
			}
			set
			{
				if (string.IsNullOrEmpty(value)) throw new NullReferenceException("TempDir is Null Or Empty!");

				this.tempdir = MailCommon.TrimDir(value);
			}
		}

		/// <summary>
		/// ��ȡ�����øø������ļ�����
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
		/// ��ȡ�����ø�����Mime���ͣ�Ĭ��Ϊ application/octet-stream
		/// </summary>
		public string MimeType
		{
			get
			{
				return this.mimeType;
			}
			set
			{
				this.mimeType = value;
			}
		}

		/// <summary>
		/// ��ȡ������Mimeͷ
		/// </summary>
		/// <param name="NextPart">�ʼ��ָ���</param>
		/// <param name="enc">�ַ���������</param>
		/// <param name="Charset">�ļ������ַ���</param>
		public string MimeHead(string NextPart,Encoding enc,string Charset)
		{
			StringBuilder sb=new StringBuilder();

			//����ָ�
			sb.Append("--");
			sb.Append(NextPart + MailCommon.NewLine);

			if (!string.IsNullOrEmpty(this.ContentID))
			{
				sb.AppendFormat("Content-ID: <{0}>",this.ContentID);
				sb.Append(MailCommon.NewLine);
			}

			sb.AppendFormat("Content-Type: {0};",this.MimeType);
			sb.Append(MailCommon.NewLine);

			string temp = "";

			//�ж��ļ������Ƿ���Ҫ����
			if (MailCommon.IsAscii(this.Name))
				temp = string.Format("=\"{0}\"",this.Name);
			else
				temp = string.Format("=\"{0}\"",Coding.EncodBase64Head(this.Name,enc,Charset));

			sb.Append("     name" + temp + MailCommon.NewLine);
			sb.Append("Content-Transfer-Encoding: base64" + MailCommon.NewLine);
			sb.Append("Content-Disposition: attachment;" + MailCommon.NewLine);
			sb.Append("     filename" + temp + MailCommon.NewLine);

			sb.Append(MailCommon.NewLine + MailCommon.NewLine);

			return sb.ToString();
		}

		/// <summary>
		/// ���ļ����б��봦��
		/// </summary>
		protected void Base64Encoding()
		{
			this.fin = new FileStream(this.TempDir + this.tempname, FileMode.OpenOrCreate, FileAccess.Write);
			this.fin.SetLength(0);
			ToBase64Transform transformer = new ToBase64Transform();
			byte[] bin = new byte[this.sm.Length / transformer.InputBlockSize * transformer.OutputBlockSize]; 
			long rdlen = 0;              //This is the total number of bytes written.
			long totlen = this.sm.Length;    //This is the total length of the input file.
			int len;
			CryptoStream encStream = new CryptoStream(this.fin, transformer, CryptoStreamMode.Write);

			while(rdlen < totlen)
			{
				len = this.sm.Read(bin, 0, (int)this.sm.Length);
				encStream.Write(bin, 0, len);
				//inputBlock size(3)
				rdlen = (rdlen + ((len / transformer.InputBlockSize) * transformer.OutputBlockSize));
			}
			encStream.Close();
			this.fin.Close();
		}

		/// <summary>
		/// ���ļ����б��벢׼����ʼ�����Ķ�ȡ
		/// </summary>
		public virtual void ReadStart()
		{
			this.Base64Encoding();
			this.fin = new FileStream(this.TempDir + this.tempname, FileMode.Open, FileAccess.Read);
		}

		/// <summary>
		/// �������ݵ�ѭ����ȡ���õ�������Base64����ÿ�ζ�ȡΪһ�У�����ֵ��ʾ��ȡ�Ƿ����
		/// </summary>
		/// <param name="Base64">�õ��ñ�����ַ���</param>
		/// <returns>����ֵ����ʾ�Ƿ��ȡ���</returns>
		public bool Read(out string Base64)
		{
			byte[] bin;
			Base64 = "";

			if (fin.Position != fin.Length)
			{
				bin = new byte[76];
				int len = fin.Read(bin, 0, 76);
				//sb.Append(System.Text.Encoding.UTF8.GetString(bin , 0, len)+"\r\n");

				Base64 =  Encoding.UTF8.GetString(bin,0,len) + MailCommon.NewLine;
				return false;
			}

			return true;
		}

		/// <summary>
		/// ������ȡ�Ľ���
		/// </summary>
		public virtual void ReadEnd()
		{
			this.fin.Close();
			//ɾ���ļ�
			File.Delete(this.TempDir + this.tempname);
		}

		/// <summary>
		/// ����ļ���վ�õ�������Դ
		/// </summary>
		public void Close()
		{
			this.sm.Close();
			this.fin = null;
		}

		/// <summary>
		/// ����
		/// </summary>
		~MailAttachment()
		{
			if (File.Exists(this.tempdir + this.tempname))
				File.Delete(this.TempDir + this.tempname);
		}
	}
}
