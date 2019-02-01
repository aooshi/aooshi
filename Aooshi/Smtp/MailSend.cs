using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
namespace Aooshi.Smtp
{
	/// <summary>
	/// �ʼ����������
	/// </summary>
	public class MailSend : IDisposable
	{

		string server;
		string username,fromname,fromaddress;
		string password;
		int port;

		TcpClient 	tcpc;
		NetworkStream NStream;
		StringBuilder   log;
		bool disposed;
		Resource Res;
		FileStream   FOut;

		/// <summary>
		/// ��ʼ����ʵ����ָ����������ʹ��Ĭ�϶˿�25
		/// </summary>
		/// <param name="Server">��������ַ</param>
		public MailSend(string Server):this(Server,MailCommon.Port)
		{
		}
		/// <summary>
		/// ��ʼ����ʵ����ָ����������˿�
		/// </summary>
		/// <param name="Server">��������ַ</param>
		/// <param name="Port">�������˿�</param>
		public MailSend(string Server,int Port)
		{
			this.server = Server;
			this.port  = Port;

			this.username = "";
			this.password = "";

			this.tcpc = new TcpClient(Server,Port);


			this.NStream = null;
			this.log = null;
			this.disposed = false;
			this.Res = new Resource();

			this.FOut = null;

		}

        /// <summary>
        /// ��ȡ������Ĭ�ϵķ���������
        /// </summary>
        public string FromName
        {
            get { return fromname; }
            set { fromname = value; }
        }

        /// <summary>
        /// ��ȡ������Ĭ�ϵķ����ߵ�ַ
        /// </summary>
        public string FromAddress
        {
            get { return fromaddress; }
            set { fromaddress = value; }
        }

		#region ��������
		
		/// <summary>
		/// ��ȡ�������ʼ���������Ч���ʻ�����
		/// </summary>
		public string UserName
		{
			get
			{
				return this.username;
			}
			set
			{
				this.username = value;
			}
		}

		/// <summary>
		/// ��ȡ�������ʼ����������ʻ������Ӧ������
		/// </summary>
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}

		/// <summary>
		/// ��ȡ�������ʼ��������˿ڣ�Ĭ��ֵ 25
		/// </summary>
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				this.port = value;
			}
		}

		/// <summary>
		/// ��ȡ�������ʼ�������Ĭ��Ϊ���ؼ����
		/// </summary>
		/// <example>"Smtp.Aooshi.Biz"</example>
		public string Server
		{
			get
			{
				return this.server;
			}
			set
			{
				this.server = value;
			}
		}

		/// <summary>
		/// ��ȡ�����õ�ǰʹ�õ�TCP����
		/// </summary>
		public TcpClient Tcp
		{
			get
			{
				return this.tcpc;
			}
			set
			{
				this.tcpc = value;
			}
		}

		/// <summary>
		/// ��ȡ��ǰ����ʹ�õ���
		/// </summary>
		protected NetworkStream NetStream
		{
			get
			{
				return this.NStream;
			}
		}

		/// <summary>
		/// ���ø��ʼ��ı���������·��������ļ��Ѵ��ڣ����串�ǣ������뱣���뽫������Ϊ null(Ĭ��ֵ)
		/// </summary>
		public string SaveFile
		{
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.FOut = null;
					return;
				}

				this.FOut = new FileStream(value,FileMode.Create,FileAccess.Write,FileShare.Read);
			}
		}

		/// <summary>
		/// ��ȡ�������Ƿ�Ե�ǰ�Ĳ���������־��¼
		/// </summary>
		public bool IsLog
		{
			get
			{
				return this.log != null;
			}
			set
			{
				if (value)
				{
					if (this.log == null)
						this.log = new StringBuilder();
				}
			}
		}

		/// <summary>
		/// ��ȡ��ǰ��������־��ֻ�е�<see cref="IsLog"/>����Ϊtrueʱ���Ż��¼����־��Ĭ�ϲ���¼
		/// </summary>
		public string Log
		{
			get
			{
				if (this.log == null) return "";
				return this.log.ToString();
			}
		}

		#endregion

		#region ���淽��

		/// <summary>
		/// ����һ����־
		/// </summary>
		/// <param name="text">Ҫ���ӵĴ�</param>
		protected virtual void AddLog(string text)
		{
			if (this.log == null) return;  //����¼
			this.log.Append(text);
		}

		/// <summary>
		/// �����ǰ��������־��������ڵĻ�
		/// </summary>
		protected virtual void ClearLog()
		{
			if (this.log != null)
				this.log = null;
		}

		#region IDisposable ��Ա

		/// <summary>
		/// ��������
		/// </summary>
		~MailSend()
		{
			this.Close();
		}

		/// <summary>
		/// �����ռ�õ�һ����Դ
		/// </summary>
		public void Close()
		{
			this.Dispose(true);
		}

		void Dispose(bool disp)
		{
			if (disp && !this.disposed)
			{
				//���
				
				if (this.NStream != null)
					this.NStream.Close();
				if (this.tcpc != null)
					this.tcpc.Close();
				this.Res.Close();

				if (this.FOut != null)
				{
					this.FOut.Flush();
					this.FOut.Close();
				}

				this.disposed = true;
				GC.SuppressFinalize(this);
			}
		}

		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		#endregion

		#endregion


		/// <summary>
		/// ����SMTP��������Ӧ
		/// </summary>
		string RecvResponse()
		{
			int StreamSize;
			byte[]  ReadBuffer = new byte[MailCommon.BufferSize] ;

			StreamSize=this.NStream.Read(ReadBuffer,0,ReadBuffer.Length);

			if (StreamSize==0) return "";

			string re = Encoding.ASCII.GetString(ReadBuffer).Substring(0,StreamSize);

			if (this.FOut != null)
			{
				ReadBuffer = Encoding.ASCII.GetBytes("Server Reply: " + re + MailCommon.NewLine);
				this.FOut.Write(ReadBuffer,0,ReadBuffer.Length);
			}

			return  re;
		}

		/// <summary>
		/// ��ָ�����ַ���ֵ��д����
		/// </summary>
		/// <param name="SendString">Ҫд����ַ���</param>
		void Write(string SendString)
		{
			if (string.IsNullOrEmpty(SendString)) return;

			byte[]  WriteBuffer;

			WriteBuffer = Encoding.ASCII.GetBytes(SendString);

			if (this.FOut != null)
				this.FOut.Write(WriteBuffer,0,WriteBuffer.Length);

			this.NStream.Write(WriteBuffer,0,WriteBuffer.Length);
		}

		/// <summary>
		/// �����������������һ��������ջ�Ӧ����������������Ƿ���ȷ��
		/// </summary>
		/// <param name="SendString">һ��Ҫ���͵�����</param>
		/// <param name="successCode">Ҫ��������������صĴ��������֤�Ĵ���ֵ</param>
		/// <param name="work">����˵�����룬��д����־</param>
		void Dialog(string SendString,string successCode,string work)
		{
			this.Write(SendString);
			//��֤��ȷ��
			this.CheckForError(successCode,work);
		}

		/// <summary>
		/// ��֤����������صĴ���ֵ
		/// </summary>
		/// <param name="successCode">Ҫ��֤�Ĵ���ֵ</param>
		/// <param name="work">����˵�����룬��д����־</param>
		void CheckForError(string successCode,string work)
		{
			string r   = this.RecvResponse();
			string Rec = r.Substring(0,3);

			if (Rec != successCode)
			{
				this.AddLog(this.Res.GetString("NetWork_" + Rec));
				this.AddLog(MailCommon.NewLine + "NetWork Error!");
				throw new W3SmtpException("Stmp Error : " + r + " ex: " + this.Res.GetString("NetWork_" + Rec));
			}

			this.AddLog(this.Res.GetString(work) + MailCommon.NewLine);
		}

		/// <summary>
		/// �����ʼ�����
		/// </summary>
		/// <param name="Message">��Ҫ���͵��ʼ�</param>
		/// <returns>����ֵ����ʾ�����Ƿ�ɹ�</returns>
		/// <exception cref="W3SmtpException">������ʧ��ʱ����</exception>
		public void Send(MailMessage Message)
		{
			if (Message == null) throw new ArgumentNullException("Message");

            //ʹ��Ĭ��
            if (Message.From == null || string.IsNullOrEmpty(Message.From.Address))
            {
                if (string.IsNullOrEmpty(this.FromAddress)) throw new InvalidOperationException("Message Not Set From !");

                string fname, faddress;
                faddress = this.FromAddress;
                fname = this.FromName;
                if (string.IsNullOrEmpty(fname)) fname = faddress;



                if (Message.From == null)
                    Message.From = new MailAddress(faddress,fname);
                else
                {
                    if (string.IsNullOrEmpty(Message.From.Name)) Message.From.Name = fname;
                    if (string.IsNullOrEmpty(Message.From.Address)) Message.From.Address = faddress;
                }
            }

			if (Message.To == null || Message.To.Count == 0)  throw new InvalidOperationException("Message Not Set To !");
			
			this.NStream = this.tcpc.GetStream();

			this.CheckForError("220","work_001");  //����������ӳɹ�

			;Dialog("HELO " + Dns.GetHostName() + MailCommon.NewLine,"250","work_002"); //250 : �������  //���Ե�¼�������ɹ�

			//�ж��Ƿ�Ϊ��¼
			if (!string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.Password))
			{
				this.Dialog("AUTH LOGIN" + MailCommon.NewLine,"334","work_003"); //334��Ӧ��֤  //��Ӧ���������ʻ���֤

				this.Dialog(Convert.ToBase64String(Encoding.ASCII.GetBytes(this.UserName)) + MailCommon.NewLine,"334","work_003");

				this.Dialog(Convert.ToBase64String(Encoding.ASCII.GetBytes(this.Password)) + MailCommon.NewLine,"235","work_004");  //��֤�ɹ�  //��Ӧ�������ʻ���֤�ɹ�
			}

			//�����ʼ�д��
			this.Dialog(string.Format("MAIL FROM: <{0}>",Message.From.Address) + MailCommon.NewLine,"250","work_005"); //�������   //д���ʼ�������

			//��� �ռ���   //work_006
			this.CAddress(Message.To);  //�ռ���
			this.CAddress(Message.Cc);  //����
			this.CAddress(Message.Bcc);  //����

			//���ͷ���룬�����ʼ�����
			this.Dialog("DATA"+MailCommon.NewLine,"354","work_007");   //׼���ʼ�����

			//�����ʼ�
			this.Write(Message.ReadHead());
			this.Write(Message.ReadText());
			this.Write(Message.ReadHtml());

			//�����ʼ�����
			if (Message.Attachmentc != null && Message.Attachmentc.Count > 0)
			{
				IEnumerator ie = Message.Attachmentc.GetEnumerator();
				MailAttachment mt;
				string linebase64;
				while(ie.MoveNext())
				{
					mt = (MailAttachment)ie.Current;
					linebase64 = "";
				
					//����ʼ�ͷ
					this.Write(mt.MimeHead(Message.Boundary,Message.Charset,Message.SetCharset));

					mt.ReadStart();

					while(!mt.Read(out linebase64))
					{
						this.Write(linebase64);  //����д��
					}

					mt.ReadEnd();
					mt.Close();
				}

				this.Write(MailCommon.NewLine + MailCommon.NewLine);
			}



			this.Dialog(Message.ReadEnd(),"250","work_008");  //�ʼ��ɹ�д����

			Message = null;

			//�����˳��������ɹ�������ʼ������в���
			this.Dialog("QUIT" + MailCommon.NewLine,"221","work_009");  //����ر�����
		}


		/// <summary>
		/// ���ռ��˼��Ͻ����������Ӵ���
		/// </summary>
		/// <param name="mc">����ֵ��ʾ�Ƿ����</param>
		bool CAddress(MailAddressc mc)
		{
			if (mc == null || mc.Count == 0) return true;  //δ��ʱֱ���˳�

			IEnumerator ie = mc.GetEnumerator();
			MailAddress ad;
			
			while(ie.MoveNext())
			{
				ad = (MailAddress)ie.Current;
				//���һ���ռ���
				this.Dialog(string.Format("RCPT TO: <{0}>",ad.Address) + MailCommon.NewLine,"250","work_006");  //���  //�ɹ����һ���ռ���
			}

			return true;
		}
	}
}
