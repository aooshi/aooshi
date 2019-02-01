using System;
using System.Text;
using System.Collections;


namespace Aooshi.Smtp
{
	/// <summary>
	/// 表示一个邮件主体
	/// </summary>
	public class MailMessage
	{
		MailAddress   from;
		MailAddressc   to;
		MailAddressc   cc;
		MailAddressc   bcc;
		MailAddress   replyto;
		MailAddress   notification;
		MailAttachmentc  ac;
		ArrayList      headers;
		string         boundary;
		string         messageid;
		string     subject;
		MailEncodeType   enc;
		string       setCharest;
		Encoding    charset;
		MailPriority      priority;

		string          html;
		string          text;

		/// <summary>
		/// 初始化新实例
		/// </summary>
		public MailMessage():this(null)
		{
		}

		/// <summary>
		/// 初始化新实例，并指定邮件发件人
		/// </summary>
		/// <param name="From">发件人地址</param>
		/// <param name="Name">发件人姓名，当为null或empty时则使用发件人地址做为姓名</param>
		public MailMessage(string From,string Name):this(new MailAddress(From,Name))
		{
		}

		/// <summary>
		/// 初始化新实例，并指定邮件发件人
		/// </summary>
		/// <param name="From">邮件发送人</param>
		public MailMessage(MailAddress From)
		{
			this.from = From;
			this.boundary  = MailCommon.MakeBoundary();
			this.messageid = MailCommon.MakeMessageID();
			this.subject = "";

			this.enc  = MailEncodeType.BASE64;

			this.charset  = Encoding.GetEncoding("gb2312");

			this.priority = MailPriority.Normal;
			this.text = "";
			this.html = "";

		}

		#region 邮件地址处理

		/// <summary>
		/// 获取或设置邮件发件人
		/// </summary>
		public MailAddress From
		{
			get
			{
				return this.from;
			}
			set
			{
				this.from = value;
			}
		}

		/// <summary>
		/// 获取或设置收件人的一个集合
		/// </summary>
		public MailAddressc To
		{
			get
			{
				return this.to;
			}
			set
			{
				this.to = value;
			}
		}

		/// <summary>
		/// 获取抄送收件人的一个集合
		/// </summary>
		public MailAddressc Cc
		{
			get
			{
				return this.cc;
			}
			set
			{
				this.cc = value;
			}
		}

		/// <summary>
		/// 获取暗送收件人的一个集合
		/// </summary>
		public MailAddressc Bcc
		{
			get
			{
				return this.bcc;
			}
			set
			{
				this.bcc = value;
			}
		}

		/// <summary>
		/// 获取或设置邮件回复地址
		/// </summary>
		public MailAddress ReplyTo
		{
			get
			{
				return this.replyto;
			}
			set
			{
				this.replyto = value;
			}
		}

		/// <summary>
		/// 获取或设置阅读回执邮件地址
		/// </summary>
		public MailAddress Notification
		{
			get
			{
				return this.notification;
			}
			set
			{
				this.notification = value;
			}
		}

		/// <summary>
		/// 设置邮件发件人
		/// </summary>
		/// <param name="From">邮件发件人地址</param>
		/// <param name="FromName">邮件发件人姓名，当为Null或Empty时则使用邮件地址做为姓名</param>
		public void SetFrom(string From,string FromName)
		{
			if (this.from == null) this.from = new MailAddress();
			this.from.Address = From;
			this.from.Name    = FromName;
		}

		/// <summary>
		/// 设置邮件发件人
		/// </summary>
		/// <param name="From">邮件发件人地址</param>
		public void SetFrom(string From)
		{
			if (this.from == null) this.from = new MailAddress();
			this.from.Address = From;
			this.from.Name    = "";
		}

		/// <summary>
		/// 设置回复邮件地址
		/// </summary>
		/// <param name="Replay">回复邮件地址</param>
		/// <param name="ReplayName">回复人的姓名，当为Null或Empty时，使用回复邮件地址做为姓名</param>
		public void SetReplayTo(string Replay,string ReplayName)
		{
			if (this.replyto == null) this.replyto = new MailAddress();
			this.replyto.Address = Replay;
			this.replyto.Name    = ReplayName;
		}

		/// <summary>
		/// 设置回复邮件地址
		/// </summary>
		/// <param name="Replay">回复邮件地址</param>
		public void SetReplayTo(string Replay)
		{
			if (this.replyto == null) this.replyto = new MailAddress();
			this.replyto.Address = Replay;
			this.replyto.Name    = "";
		}

		/// <summary>
		/// 设置回执邮件地址
		/// </summary>
		/// <param name="Notification">回执邮件地址</param>
		/// <param name="NotificationName">回执时使用的姓名，当为Null或Empty时，使用回执邮件地址做为姓名</param>
		public void SetNotification(string Notification,string NotificationName)
		{
			if (this.notification == null) this.notification = new MailAddress();
			this.notification.Address = Notification;
			this.notification.Name    = NotificationName;
		}

		/// <summary>
		/// 设置回执邮件地址
		/// </summary>
		/// <param name="Notification">回执邮件地址</param>
		public void SetNotification(string Notification)
		{
			if (this.notification == null) this.notification = new MailAddress();
			this.notification.Address = Notification;
			this.notification.Name    = "";
		}

		/// <summary>
		/// 增加一个新的收件人
		/// </summary>
		/// <param name="to">收件人地址</param>
		public void AddTo(MailAddress to)
		{
			if (this.to == null)
				this.to = new MailAddressc();

			this.to.Add(to);
		}

		/// <summary>
		/// 增加一个新的收件人
		/// </summary>
		/// <param name="address">收件人地址</param>
		public void AddTo(string address)
		{
			this.AddTo(new MailAddress(address,""));
		}

		/// <summary>
		/// 增加一个新的收件人
		/// </summary>
		/// <param name="address">收件人地址</param>
		/// <param name="Name">收件人姓名，如果为Null或Empty则使用收件人地址做为姓名</param>
		public void AddTo(string address,string Name)
		{
			this.AddTo(new MailAddress(address,Name));
		}

		/// <summary>
		/// 增加一个须要抄送的收件人
		/// </summary>
		/// <param name="cc">收件人地址</param>
		public void AddCc(MailAddress cc)
		{
			if (this.cc == null) this.cc = new MailAddressc();

			this.cc.Add(cc);
		}

		/// <summary>
		/// 增加一个新的抄送收件人
		/// </summary>
		/// <param name="address">收件人地址</param>
		public void AddCc(string address)
		{
			this.AddCc(new MailAddress(address,""));
		}

		/// <summary>
		/// 增加一个新的抄送收件人
		/// </summary>
		/// <param name="address">收件人地址</param>
		/// <param name="Name">收件人姓名，如果为Null或Empty则使用收件人地址做为姓名</param>
		public void AddCc(string address,string Name)
		{
			this.AddCc(new MailAddress(address,Name));
		}

		/// <summary>
		/// 增加一个须要暗送的收件人
		/// </summary>
		/// <param name="bcc">收件人地址</param>
		public void AddBcc(MailAddress bcc)
		{
			if (this.bcc == null) this.bcc = new MailAddressc();
			this.bcc.Add(bcc);
		}

		/// <summary>
		/// 增加一个新的抄送收件人
		/// </summary>
		/// <param name="address">收件人地址</param>
		public void AddBcc(string address)
		{
			this.AddBcc(new MailAddress(address,""));
		}

		/// <summary>
		/// 增加一个新的抄送收件人
		/// </summary>
		/// <param name="address">收件人地址</param>
		/// <param name="Name">收件人姓名，如果为Null或Empty则使用收件人地址做为姓名</param>
		public void AddBcc(string address,string Name)
		{
			this.AddBcc(new MailAddress(address,Name));
		}

		#endregion

		#region 邮件正文

		/// <summary>
		/// 获取该邮件的段落分隔符
		/// </summary>
		internal protected string Boundary
		{
			get
			{
				return this.boundary;
			}
		}

		/// <summary>
		/// 获取邮件ID
		/// </summary>
		internal protected string MessageID
		{
			get
			{
				return this.messageid;
			}
		}

		/// <summary>
		/// 获取或设置邮件标题
		/// </summary>
		public string Subject
		{
			get
			{
				return this.subject;
			}

			set
			{
				this.subject = value;
			}
		}

		/// <summary>
		/// 获取或设置邮件字符符集名称,即文件头Charset中所示编码类型，默认与<see cref="Charset"/>相同，一般情况下不应设置该项，设置该项有可能引起乱码
		/// </summary>
		public string SetCharset
		{
			get
			{
				if (this.setCharest == null) return this.Charset.HeaderName;
				return this.setCharest;
			}
			set
			{
				this.setCharest= value;
			}
		}

		/// <summary>
		/// 获取或设置对邮件进行编码时采用的编码类型，默认为GB2312
		/// </summary>
		public Encoding Charset
		{
			get
			{
				return this.charset;
			}
			set
			{
				this.charset= value;
			}
		}

		/// <summary>
		/// 获取或设置邮件文本内容的编码方式，默认为BASE64
		/// </summary>
		public MailEncodeType EncodeType
		{
			get
			{
				return this.enc;
			}
			set
			{
				this.enc = value;
			}
		}

		/// <summary>
		/// 获取或设置邮件发送紧急程度，默认为：Normal
		/// </summary>
		public MailPriority Priority
		{
			get
			{
				return this.priority;
			}
			set
			{
				this.priority = value;
			}
		}

		/// <summary>
		/// 获取或设置邮件中的纯文本内容
		/// </summary>
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		/// <summary>
		/// 获取或设置邮件中的Html内容
		/// </summary>
		public string Html
		{
			get
			{
				return this.html;
			}
			set
			{
				this.html = value;
			}
		}

		/// <summary>
		/// 在邮件文本内容后追加新的内容
		/// </summary>
		/// <param name="text">要追加的新内容</param>
		public void AppedText(string text)
		{
			this.text += text;
		}

		/// <summary>
		/// 在邮件html内容后追加新的html内容
		/// </summary>
		/// <param name="html">要追加的新内容</param>
		public void AppedHtml(string html)
		{
			this.html += html;
		}
		
		/// <summary>
		/// 获取或设置邮件的附件集合
		/// </summary>
		public MailAttachmentc Attachmentc
		{
			get
			{
				return this.ac;
			}
			set
			{
				this.ac = value;
			}
		}

		/// <summary>
		/// 向邮件中的附件集合增加一个附件
		/// </summary>
		/// <param name="Attachment">要增加的附件</param>
		public void AddAttachmentc(MailAttachment Attachment)
		{
			if (this.ac == null) this.ac = new MailAttachmentc();

			this.ac.Add(Attachment);
		}

		/// <summary>
		/// 增加自定Mime头
		/// </summary>
		/// <param name="Name">头名</param>
		/// <param name="Value">值</param>
		public void AddHeaders(string Name,string Value)
		{
			if (this.headers == null) this.headers = new ArrayList();
			this.headers.Add(Name + Value);
		}

		#endregion

		#region 邮件处理

		/// <summary>
		/// 进行邮件头的读取
		/// </summary>
		/// <returns>返回所生成的邮件头值</returns>
		internal protected virtual string ReadHead()
		{
			if (this.From==null || string.IsNullOrEmpty(this.From.Address) || this.To == null || this.To.Count == 0)  throw new InvalidOperationException("Message Not Set From Or To !");
			if (string.IsNullOrEmpty(this.Subject)) throw new InvalidOperationException("Not Set Subject!");

			StringBuilder sb = new StringBuilder();

			//写入邮件ID
			sb.Append(this.MessageID + MailCommon.NewLine);
			//from
			sb.Append("From: ");
			sb.Append(this.EncodeType == MailEncodeType.BASE64 ? this.From.Base64Encoding(this.Charset,this.SetCharset) : this.From.QPEncoding(this.Charset,this.SetCharset));
			sb.Append(MailCommon.NewLine);
			//replay
			if (this.replyto != null)
			{
				sb.Append("Replay-To: ");
				sb.Append(this.EncodeType == MailEncodeType.BASE64 ? this.ReplyTo.Base64Encoding(this.Charset,this.SetCharset) : this.ReplyTo.QPEncoding(this.Charset,this.SetCharset));
				sb.Append(MailCommon.NewLine);
			}
			//to
			sb.Append("To: ");
			sb.Append(this.CreateAddressList(this.To));
			sb.Append(MailCommon.NewLine);
			//cc
			if (this.Cc != null && this.Cc.Count > 0)
			{
				sb.Append("CC: ");
				sb.Append(this.CreateAddressList(this.Cc));
				sb.Append(MailCommon.NewLine);
			}
			//Subject
			sb.Append("Subject: ");
			if (MailCommon.IsAscii(this.Subject))
				sb.Append(this.Subject);
			else
				sb.Append(this.EncodeType == MailEncodeType.BASE64 ? Coding.EncodBase64Head(this.Subject,this.Charset,this.SetCharset) : Coding.EncodQPHead(this.Subject,this.Charset,this.SetCharset));

			sb.Append(MailCommon.NewLine);
			//Data
			sb.Append("Date: ");
			sb.Append(DateTime.Now.ToString("R"));
			sb.Append(MailCommon.NewLine);
			//Mime
			sb.Append(MailCommon.MimeVersion);
			sb.Append(MailCommon.NewLine);

			//Notification
			if (this.Notification != null)
			{
				sb.Append("Disposition-Notification-To: ");
				sb.Append(this.EncodeType == MailEncodeType.BASE64 ? this.Notification.Base64Encoding(this.Charset,this.SetCharset) : this.Notification.QPEncoding(this.Charset,this.SetCharset));
				sb.Append(MailCommon.NewLine);
			}
			//priority 
			sb.Append("X-Priority: ");
			sb.Append((int)this.Priority);
			sb.Append(MailCommon.NewLine);

			sb.Append("X-MSMail-Priority: ");
			sb.Append(this.Priority.ToString());
			sb.Append(MailCommon.NewLine);

			//Mailer
			sb.Append(MailCommon.XMailer);
			sb.Append(MailCommon.NewLine);

			//Content-Type
			sb.Append(CreateContentType());
			sb.Append(MailCommon.NewLine);

			//自定Headers
			if (this.headers != null)
			{
				for(int i=0;i<this.headers.Count;i++)
				{
					sb.Append(this.headers[i].ToString());
					sb.Append(MailCommon.NewLine);
				}
			}

			sb.Append(MailCommon.NewLine);

			return sb.ToString();
		}

		/// <summary>
		/// 进行文本内容的读取
		/// </summary>
		/// <returns>返回所生成的文本内容数据</returns>
		internal protected virtual string ReadText()
		{
			if (string.IsNullOrEmpty(this.Text)) return "";

			StringBuilder sb = new StringBuilder();
			//
			sb.Append("--");
			sb.Append(this.Boundary);
			sb.Append(MailCommon.NewLine);  //分隔完成

			sb.Append("Content-Type: text/plain;");
			sb.Append(MailCommon.NewLine);
			sb.AppendFormat("   charset=\"{0}\" ",this.SetCharset);
			sb.Append(MailCommon.NewLine);

			sb.AppendFormat("Content-Transfer-Encoding: ");

			if (this.EncodeType == MailEncodeType.BASE64)
			{
				sb.Append("base64");
				sb.Append(MailCommon.NewLine + MailCommon.NewLine);

				sb.Append(Coding.Base64Encode(this.Text,this.Charset));
				
				sb.Append(MailCommon.NewLine + MailCommon.NewLine);

				return sb.ToString();
			}

			//qp  --  quoted-printable"
			sb.Append("quoted-printable");
			sb.Append(MailCommon.NewLine + MailCommon.NewLine);
			sb.Append(Coding.EncodeQuotedPrintable(this.Text,this.Charset));
			sb.Append(MailCommon.NewLine + MailCommon.NewLine);

			return sb.ToString();

		}

		/// <summary>
		/// 进行Html文本内容的读取
		/// </summary>
		/// <returns>返回所生成的Html文本内容数据</returns>
		internal protected virtual string ReadHtml()
		{
			if (string.IsNullOrEmpty(this.Html)) return "";

			StringBuilder sb = new StringBuilder();
			//
			sb.Append("--");
			sb.Append(this.Boundary);
			sb.Append(MailCommon.NewLine);  //分隔完成

			sb.Append("Content-Type: text/html;");
			sb.Append(MailCommon.NewLine);
			sb.AppendFormat("   charset=\"{0}\" ",this.SetCharset);
			sb.Append(MailCommon.NewLine);

			sb.AppendFormat("Content-Transfer-Encoding: ");

			if (this.EncodeType == MailEncodeType.BASE64)
			{
				sb.Append("base64");
				sb.Append(MailCommon.NewLine + MailCommon.NewLine);

				sb.Append(Coding.Base64Encode(this.Html,this.Charset));
				
				sb.Append(MailCommon.NewLine + MailCommon.NewLine);

				return sb.ToString();
			}

			//qp  --  quoted-printable"
			sb.Append("quoted-printable");
			sb.Append(MailCommon.NewLine + MailCommon.NewLine);
			sb.Append(Coding.EncodeQuotedPrintable(this.Html,this.Charset));
			sb.Append(MailCommon.NewLine + MailCommon.NewLine);

			return sb.ToString();

		}

		/// <summary>
		/// 文件读取的完成
		/// </summary>
		/// <returns>返回结束数据</returns>
		internal protected virtual string ReadEnd()
		{
			return "--" + this.Boundary + "--" + MailCommon.NewLine  + "." + MailCommon.NewLine;  //加点以结束邮件
		}


		/// <summary>
		/// 创建指定邮件地址集合的数组，主要是开发人员使用
		/// </summary>
		/// <param name="coll">须要处理的集合</param>
		/// <returns>返回字符串型式数组</returns>
		protected virtual string CreateAddressList(MailAddressc coll)
		{
			string list = "";
			IEnumerator ie = coll.GetEnumerator();
			MailAddress  ad;

			while(ie.MoveNext())
			{
				ad = (MailAddress)ie.Current;
				list += this.EncodeType == MailEncodeType.BASE64 ? ad.Base64Encoding(this.Charset,this.SetCharset) : ad.QPEncoding(this.Charset,this.SetCharset);
				list += ",";
			}

			if (list.EndsWith(","))
				list = list.Substring(0,list.Length - 1);

			return list;
		}

		/// <summary>
		/// 生成邮件分隔与类型
		/// </summary>
		/// <returns>返回生成的类型值</returns>
		protected virtual string CreateContentType()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("Content-Type: ");

			if (this.Attachmentc != null && this.Attachmentc.Count > 0)
				sb.Append( "multipart/mixed; ");  //此含multipart/related 内嵌资源，因所有内嵌资源均放置在附件集合中
			else
				sb.Append( "multipart/alternative; ");  //纯文本

			sb.Append(MailCommon.NewLine);
			sb.AppendFormat("       boundary=\"{0}\" ",this.Boundary);

			return sb.ToString();
		}

		#endregion


	}
}
