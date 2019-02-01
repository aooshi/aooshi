using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
namespace Aooshi.Smtp
{
	/// <summary>
	/// 邮件发送组件类
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
		/// 初始化新实例，指定服务器并使用默认端口25
		/// </summary>
		/// <param name="Server">服务器地址</param>
		public MailSend(string Server):this(Server,MailCommon.Port)
		{
		}
		/// <summary>
		/// 初始化新实例，指定服务器与端口
		/// </summary>
		/// <param name="Server">服务器地址</param>
		/// <param name="Port">服务器端口</param>
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
        /// 获取或设置默认的发送者姓名
        /// </summary>
        public string FromName
        {
            get { return fromname; }
            set { fromname = value; }
        }

        /// <summary>
        /// 获取或设置默认的发送者地址
        /// </summary>
        public string FromAddress
        {
            get { return fromaddress; }
            set { fromaddress = value; }
        }

		#region 基本属性
		
		/// <summary>
		/// 获取或设置邮件服务器有效的帐户名称
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
		/// 获取或设置邮件服务器与帐户名相对应的密码
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
		/// 获取或设置邮件服务器端口，默认值 25
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
		/// 获取或设置邮件服务域，默认为本地计算机
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
		/// 获取或设置当前使用的TCP连接
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
		/// 获取当前正在使用的流
		/// </summary>
		protected NetworkStream NetStream
		{
			get
			{
				return this.NStream;
			}
		}

		/// <summary>
		/// 设置该邮件文本保存至的路径，如果文件已存在，则将其覆盖，如无须保存请将其设置为 null(默认值)
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
		/// 获取或设置是否对当前的操作进行日志记录
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
		/// 获取当前操作的日志，只有当<see cref="IsLog"/>设置为true时，才会记录该日志，默认不记录
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

		#region 常规方法

		/// <summary>
		/// 增加一行日志
		/// </summary>
		/// <param name="text">要增加的串</param>
		protected virtual void AddLog(string text)
		{
			if (this.log == null) return;  //不记录
			this.log.Append(text);
		}

		/// <summary>
		/// 清除当前操作的日志，如果存在的话
		/// </summary>
		protected virtual void ClearLog()
		{
			if (this.log != null)
				this.log = null;
		}

		#region IDisposable 成员

		/// <summary>
		/// 构析函数
		/// </summary>
		~MailSend()
		{
			this.Close();
		}

		/// <summary>
		/// 清除所占用的一切资源
		/// </summary>
		public void Close()
		{
			this.Dispose(true);
		}

		void Dispose(bool disp)
		{
			if (disp && !this.disposed)
			{
				//清除
				
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
		/// 接收SMTP服务器回应
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
		/// 将指定的字符串值，写入流
		/// </summary>
		/// <param name="SendString">要写入的字符串</param>
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
		/// 与服务器交互，发送一条命令并接收回应，并返回网络操作是否正确。
		/// </summary>
		/// <param name="SendString">一个要发送的命令</param>
		/// <param name="successCode">要与服务器端所返回的代码进行验证的代码值</param>
		/// <param name="work">操作说明代码，将写入日志</param>
		void Dialog(string SendString,string successCode,string work)
		{
			this.Write(SendString);
			//验证正确性
			this.CheckForError(successCode,work);
		}

		/// <summary>
		/// 验证服务端所返回的代码值
		/// </summary>
		/// <param name="successCode">要验证的代码值</param>
		/// <param name="work">操作说明代码，将写入日志</param>
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
		/// 进行邮件发送
		/// </summary>
		/// <param name="Message">须要发送的邮件</param>
		/// <returns>返回值，表示发送是否成功</returns>
		/// <exception cref="W3SmtpException">当发送失败时引发</exception>
		public void Send(MailMessage Message)
		{
			if (Message == null) throw new ArgumentNullException("Message");

            //使用默认
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

			this.CheckForError("220","work_001");  //与服务器连接成功

			;Dialog("HELO " + Dns.GetHostName() + MailCommon.NewLine,"250","work_002"); //250 : 操作完成  //尝试登录服务器成功

			//判断是否为登录
			if (!string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.Password))
			{
				this.Dialog("AUTH LOGIN" + MailCommon.NewLine,"334","work_003"); //334响应验证  //响应服务器的帐户认证

				this.Dialog(Convert.ToBase64String(Encoding.ASCII.GetBytes(this.UserName)) + MailCommon.NewLine,"334","work_003");

				this.Dialog(Convert.ToBase64String(Encoding.ASCII.GetBytes(this.Password)) + MailCommon.NewLine,"235","work_004");  //验证成功  //响应服务器帐户认证成功
			}

			//进行邮件写入
			this.Dialog(string.Format("MAIL FROM: <{0}>",Message.From.Address) + MailCommon.NewLine,"250","work_005"); //操作完成   //写入邮件发送人

			//输出 收件人   //work_006
			this.CAddress(Message.To);  //收件人
			this.CAddress(Message.Cc);  //抄送
			this.CAddress(Message.Bcc);  //暗送

			//完成头输入，进行邮件输入
			this.Dialog("DATA"+MailCommon.NewLine,"354","work_007");   //准备邮件输入

			//输入邮件
			this.Write(Message.ReadHead());
			this.Write(Message.ReadText());
			this.Write(Message.ReadHtml());

			//输入邮件附件
			if (Message.Attachmentc != null && Message.Attachmentc.Count > 0)
			{
				IEnumerator ie = Message.Attachmentc.GetEnumerator();
				MailAttachment mt;
				string linebase64;
				while(ie.MoveNext())
				{
					mt = (MailAttachment)ie.Current;
					linebase64 = "";
				
					//输出邮件头
					this.Write(mt.MimeHead(Message.Boundary,Message.Charset,Message.SetCharset));

					mt.ReadStart();

					while(!mt.Read(out linebase64))
					{
						this.Write(linebase64);  //分行写出
					}

					mt.ReadEnd();
					mt.Close();
				}

				this.Write(MailCommon.NewLine + MailCommon.NewLine);
			}



			this.Dialog(Message.ReadEnd(),"250","work_008");  //邮件成功写输入

			Message = null;

			//请求退出服务器成功，完成邮件的所有操作
			this.Dialog("QUIT" + MailCommon.NewLine,"221","work_009");  //服务关闭连接
		}


		/// <summary>
		/// 对收件人集合进行数组联接处理
		/// </summary>
		/// <param name="mc">返回值表示是否出错</param>
		bool CAddress(MailAddressc mc)
		{
			if (mc == null || mc.Count == 0) return true;  //未有时直接退出

			IEnumerator ie = mc.GetEnumerator();
			MailAddress ad;
			
			while(ie.MoveNext())
			{
				ad = (MailAddress)ie.Current;
				//输出一个收件人
				this.Dialog(string.Format("RCPT TO: <{0}>",ad.Address) + MailCommon.NewLine,"250","work_006");  //完成  //成功输出一个收件人
			}

			return true;
		}
	}
}
