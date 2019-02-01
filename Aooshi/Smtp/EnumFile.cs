namespace Aooshi.Smtp
{

	/// <summary>
	/// �ʼ���������
	/// </summary>
	public enum MailType
	{
		/// <summary>
		///  Html��ʽ
		/// </summary>
		Html,
		/// <summary>
		/// ���ı���ʽ
		/// </summary>
		Text
	}

	/// <summary>
	/// �ʼ����ͽ����̶�
	/// </summary>
	public enum MailPriority:int
	{
		/// <summary>
		/// ��
		/// </summary>
		High = 1,
		/// <summary>
		/// ��
		/// </summary>
		Normal =3,
		/// <summary>
		/// ��
		/// </summary>
		Low = 5
	}

	/// <summary>
	/// �ʼ����ı��뷽ʽ
	/// </summary>
	public enum MailEncodeType
	{
		/// <summary>
		/// ��Base64��ʽ�����ʼ�����
		/// </summary>
		BASE64,
		/// <summary>
		/// ��QuotedPrintable��ʽ�����ʼ�����
		/// </summary>
		QP
	}
}
