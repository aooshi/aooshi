using System;
using System.Security.Cryptography;
using System.Globalization;
using System.IO;
using System.Text;

namespace Aooshi.Smtp
{	/// <summary>
	/// 表示一个邮件附件
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
		/// 初初化新实例
		/// </summary>
		/// <param name="FilePath">指定附件的路径</param>
		/// <param name="TempDir">用于存储所产生的临时文件目录</param>
		public MailAttachment(string FilePath,string TempDir)
		{
			if (string.IsNullOrEmpty(FilePath)) throw new ArgumentNullException("FilePath","FilePath Is Null!");

			FileInfo fi = new FileInfo(FilePath);
			if (!fi.Exists) throw new ArgumentException("Not Find File!","FilePath");
			sm = fi.Open(FileMode.Open,FileAccess.Read,FileShare.Read); //打开文件

			this.Name = fi.Name;
			this.TempDir = TempDir;

			this.Init();
		}

		/// <summary>
		/// 初始化新实例
		/// </summary>
		/// <param name="stream">指定附件流</param>
		/// <param name="TempDir">用于存储所产生的临时文件目录</param>
		/// <param name="Name">文件名称</param>
		public MailAttachment(Stream stream,string TempDir,string Name)
		{
			this.sm = stream;
			this.Name = Name;
			this.TempDir = TempDir;
			this.Init();
		}

		/// <summary>
		/// 完成的初始
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
		/// 获取或设置附件ID
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
		/// 获取该附件的长度
		/// </summary>
		public long Length
		{
			get
			{
				return this.size;
			}
		}

		/// <summary>
		/// 获取或设置附件处理时的临时文件存储位置
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
		/// 获取或设置该附件的文件名称
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
		/// 获取或设置附件的Mime类型，默认为 application/octet-stream
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
		/// 获取附件的Mime头
		/// </summary>
		/// <param name="NextPart">邮件分隔符</param>
		/// <param name="enc">字符编码类型</param>
		/// <param name="Charset">文件编码字符集</param>
		public string MimeHead(string NextPart,Encoding enc,string Charset)
		{
			StringBuilder sb=new StringBuilder();

			//加入分隔
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

			//判断文件名称是否须要编码
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
		/// 将文件进行编码处理
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
		/// 对文件进行编码并准备开始附件的读取
		/// </summary>
		public virtual void ReadStart()
		{
			this.Base64Encoding();
			this.fin = new FileStream(this.TempDir + this.tempname, FileMode.Open, FileAccess.Read);
		}

		/// <summary>
		/// 进行数据的循环读取，得到编码后的Base64串，每次读取为一行，返回值表示读取是否完成
		/// </summary>
		/// <param name="Base64">得到得编码后字符串</param>
		/// <returns>返回值，表示是否读取完成</returns>
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
		/// 附件读取的结束
		/// </summary>
		public virtual void ReadEnd()
		{
			this.fin.Close();
			//删除文件
			File.Delete(this.TempDir + this.tempname);
		}

		/// <summary>
		/// 清除文件所站用的所有资源
		/// </summary>
		public void Close()
		{
			this.sm.Close();
			this.fin = null;
		}

		/// <summary>
		/// 构析
		/// </summary>
		~MailAttachment()
		{
			if (File.Exists(this.tempdir + this.tempname))
				File.Delete(this.TempDir + this.tempname);
		}
	}
}
