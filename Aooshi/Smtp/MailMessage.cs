using System;
using System.Text;
using System.Collections;


namespace Aooshi.Smtp
{
	/// <summary>
	/// ��ʾһ���ʼ�����
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
		/// ��ʼ����ʵ��
		/// </summary>
		public MailMessage():this(null)
		{
		}

		/// <summary>
		/// ��ʼ����ʵ������ָ���ʼ�������
		/// </summary>
		/// <param name="From">�����˵�ַ</param>
		/// <param name="Name">��������������Ϊnull��emptyʱ��ʹ�÷����˵�ַ��Ϊ����</param>
		public MailMessage(string From,string Name):this(new MailAddress(From,Name))
		{
		}

		/// <summary>
		/// ��ʼ����ʵ������ָ���ʼ�������
		/// </summary>
		/// <param name="From">�ʼ�������</param>
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

		#region �ʼ���ַ����

		/// <summary>
		/// ��ȡ�������ʼ�������
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
		/// ��ȡ�������ռ��˵�һ������
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
		/// ��ȡ�����ռ��˵�һ������
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
		/// ��ȡ�����ռ��˵�һ������
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
		/// ��ȡ�������ʼ��ظ���ַ
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
		/// ��ȡ�������Ķ���ִ�ʼ���ַ
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
		/// �����ʼ�������
		/// </summary>
		/// <param name="From">�ʼ������˵�ַ</param>
		/// <param name="FromName">�ʼ���������������ΪNull��Emptyʱ��ʹ���ʼ���ַ��Ϊ����</param>
		public void SetFrom(string From,string FromName)
		{
			if (this.from == null) this.from = new MailAddress();
			this.from.Address = From;
			this.from.Name    = FromName;
		}

		/// <summary>
		/// �����ʼ�������
		/// </summary>
		/// <param name="From">�ʼ������˵�ַ</param>
		public void SetFrom(string From)
		{
			if (this.from == null) this.from = new MailAddress();
			this.from.Address = From;
			this.from.Name    = "";
		}

		/// <summary>
		/// ���ûظ��ʼ���ַ
		/// </summary>
		/// <param name="Replay">�ظ��ʼ���ַ</param>
		/// <param name="ReplayName">�ظ��˵���������ΪNull��Emptyʱ��ʹ�ûظ��ʼ���ַ��Ϊ����</param>
		public void SetReplayTo(string Replay,string ReplayName)
		{
			if (this.replyto == null) this.replyto = new MailAddress();
			this.replyto.Address = Replay;
			this.replyto.Name    = ReplayName;
		}

		/// <summary>
		/// ���ûظ��ʼ���ַ
		/// </summary>
		/// <param name="Replay">�ظ��ʼ���ַ</param>
		public void SetReplayTo(string Replay)
		{
			if (this.replyto == null) this.replyto = new MailAddress();
			this.replyto.Address = Replay;
			this.replyto.Name    = "";
		}

		/// <summary>
		/// ���û�ִ�ʼ���ַ
		/// </summary>
		/// <param name="Notification">��ִ�ʼ���ַ</param>
		/// <param name="NotificationName">��ִʱʹ�õ���������ΪNull��Emptyʱ��ʹ�û�ִ�ʼ���ַ��Ϊ����</param>
		public void SetNotification(string Notification,string NotificationName)
		{
			if (this.notification == null) this.notification = new MailAddress();
			this.notification.Address = Notification;
			this.notification.Name    = NotificationName;
		}

		/// <summary>
		/// ���û�ִ�ʼ���ַ
		/// </summary>
		/// <param name="Notification">��ִ�ʼ���ַ</param>
		public void SetNotification(string Notification)
		{
			if (this.notification == null) this.notification = new MailAddress();
			this.notification.Address = Notification;
			this.notification.Name    = "";
		}

		/// <summary>
		/// ����һ���µ��ռ���
		/// </summary>
		/// <param name="to">�ռ��˵�ַ</param>
		public void AddTo(MailAddress to)
		{
			if (this.to == null)
				this.to = new MailAddressc();

			this.to.Add(to);
		}

		/// <summary>
		/// ����һ���µ��ռ���
		/// </summary>
		/// <param name="address">�ռ��˵�ַ</param>
		public void AddTo(string address)
		{
			this.AddTo(new MailAddress(address,""));
		}

		/// <summary>
		/// ����һ���µ��ռ���
		/// </summary>
		/// <param name="address">�ռ��˵�ַ</param>
		/// <param name="Name">�ռ������������ΪNull��Empty��ʹ���ռ��˵�ַ��Ϊ����</param>
		public void AddTo(string address,string Name)
		{
			this.AddTo(new MailAddress(address,Name));
		}

		/// <summary>
		/// ����һ����Ҫ���͵��ռ���
		/// </summary>
		/// <param name="cc">�ռ��˵�ַ</param>
		public void AddCc(MailAddress cc)
		{
			if (this.cc == null) this.cc = new MailAddressc();

			this.cc.Add(cc);
		}

		/// <summary>
		/// ����һ���µĳ����ռ���
		/// </summary>
		/// <param name="address">�ռ��˵�ַ</param>
		public void AddCc(string address)
		{
			this.AddCc(new MailAddress(address,""));
		}

		/// <summary>
		/// ����һ���µĳ����ռ���
		/// </summary>
		/// <param name="address">�ռ��˵�ַ</param>
		/// <param name="Name">�ռ������������ΪNull��Empty��ʹ���ռ��˵�ַ��Ϊ����</param>
		public void AddCc(string address,string Name)
		{
			this.AddCc(new MailAddress(address,Name));
		}

		/// <summary>
		/// ����һ����Ҫ���͵��ռ���
		/// </summary>
		/// <param name="bcc">�ռ��˵�ַ</param>
		public void AddBcc(MailAddress bcc)
		{
			if (this.bcc == null) this.bcc = new MailAddressc();
			this.bcc.Add(bcc);
		}

		/// <summary>
		/// ����һ���µĳ����ռ���
		/// </summary>
		/// <param name="address">�ռ��˵�ַ</param>
		public void AddBcc(string address)
		{
			this.AddBcc(new MailAddress(address,""));
		}

		/// <summary>
		/// ����һ���µĳ����ռ���
		/// </summary>
		/// <param name="address">�ռ��˵�ַ</param>
		/// <param name="Name">�ռ������������ΪNull��Empty��ʹ���ռ��˵�ַ��Ϊ����</param>
		public void AddBcc(string address,string Name)
		{
			this.AddBcc(new MailAddress(address,Name));
		}

		#endregion

		#region �ʼ�����

		/// <summary>
		/// ��ȡ���ʼ��Ķ���ָ���
		/// </summary>
		internal protected string Boundary
		{
			get
			{
				return this.boundary;
			}
		}

		/// <summary>
		/// ��ȡ�ʼ�ID
		/// </summary>
		internal protected string MessageID
		{
			get
			{
				return this.messageid;
			}
		}

		/// <summary>
		/// ��ȡ�������ʼ�����
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
		/// ��ȡ�������ʼ��ַ���������,���ļ�ͷCharset����ʾ�������ͣ�Ĭ����<see cref="Charset"/>��ͬ��һ������²�Ӧ���ø�����ø����п�����������
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
		/// ��ȡ�����ö��ʼ����б���ʱ���õı������ͣ�Ĭ��ΪGB2312
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
		/// ��ȡ�������ʼ��ı����ݵı��뷽ʽ��Ĭ��ΪBASE64
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
		/// ��ȡ�������ʼ����ͽ����̶ȣ�Ĭ��Ϊ��Normal
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
		/// ��ȡ�������ʼ��еĴ��ı�����
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
		/// ��ȡ�������ʼ��е�Html����
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
		/// ���ʼ��ı����ݺ�׷���µ�����
		/// </summary>
		/// <param name="text">Ҫ׷�ӵ�������</param>
		public void AppedText(string text)
		{
			this.text += text;
		}

		/// <summary>
		/// ���ʼ�html���ݺ�׷���µ�html����
		/// </summary>
		/// <param name="html">Ҫ׷�ӵ�������</param>
		public void AppedHtml(string html)
		{
			this.html += html;
		}
		
		/// <summary>
		/// ��ȡ�������ʼ��ĸ�������
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
		/// ���ʼ��еĸ�����������һ������
		/// </summary>
		/// <param name="Attachment">Ҫ���ӵĸ���</param>
		public void AddAttachmentc(MailAttachment Attachment)
		{
			if (this.ac == null) this.ac = new MailAttachmentc();

			this.ac.Add(Attachment);
		}

		/// <summary>
		/// �����Զ�Mimeͷ
		/// </summary>
		/// <param name="Name">ͷ��</param>
		/// <param name="Value">ֵ</param>
		public void AddHeaders(string Name,string Value)
		{
			if (this.headers == null) this.headers = new ArrayList();
			this.headers.Add(Name + Value);
		}

		#endregion

		#region �ʼ�����

		/// <summary>
		/// �����ʼ�ͷ�Ķ�ȡ
		/// </summary>
		/// <returns>���������ɵ��ʼ�ͷֵ</returns>
		internal protected virtual string ReadHead()
		{
			if (this.From==null || string.IsNullOrEmpty(this.From.Address) || this.To == null || this.To.Count == 0)  throw new InvalidOperationException("Message Not Set From Or To !");
			if (string.IsNullOrEmpty(this.Subject)) throw new InvalidOperationException("Not Set Subject!");

			StringBuilder sb = new StringBuilder();

			//д���ʼ�ID
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

			//�Զ�Headers
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
		/// �����ı����ݵĶ�ȡ
		/// </summary>
		/// <returns>���������ɵ��ı���������</returns>
		internal protected virtual string ReadText()
		{
			if (string.IsNullOrEmpty(this.Text)) return "";

			StringBuilder sb = new StringBuilder();
			//
			sb.Append("--");
			sb.Append(this.Boundary);
			sb.Append(MailCommon.NewLine);  //�ָ����

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
		/// ����Html�ı����ݵĶ�ȡ
		/// </summary>
		/// <returns>���������ɵ�Html�ı���������</returns>
		internal protected virtual string ReadHtml()
		{
			if (string.IsNullOrEmpty(this.Html)) return "";

			StringBuilder sb = new StringBuilder();
			//
			sb.Append("--");
			sb.Append(this.Boundary);
			sb.Append(MailCommon.NewLine);  //�ָ����

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
		/// �ļ���ȡ�����
		/// </summary>
		/// <returns>���ؽ�������</returns>
		internal protected virtual string ReadEnd()
		{
			return "--" + this.Boundary + "--" + MailCommon.NewLine  + "." + MailCommon.NewLine;  //�ӵ��Խ����ʼ�
		}


		/// <summary>
		/// ����ָ���ʼ���ַ���ϵ����飬��Ҫ�ǿ�����Աʹ��
		/// </summary>
		/// <param name="coll">��Ҫ����ļ���</param>
		/// <returns>�����ַ�����ʽ����</returns>
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
		/// �����ʼ��ָ�������
		/// </summary>
		/// <returns>�������ɵ�����ֵ</returns>
		protected virtual string CreateContentType()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("Content-Type: ");

			if (this.Attachmentc != null && this.Attachmentc.Count > 0)
				sb.Append( "multipart/mixed; ");  //�˺�multipart/related ��Ƕ��Դ����������Ƕ��Դ�������ڸ���������
			else
				sb.Append( "multipart/alternative; ");  //���ı�

			sb.Append(MailCommon.NewLine);
			sb.AppendFormat("       boundary=\"{0}\" ",this.Boundary);

			return sb.ToString();
		}

		#endregion


	}
}
