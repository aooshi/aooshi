using System;
using System.Text;
using System.IO;

namespace Aooshi.Smtp
{
	/// <summary>
	/// ������Ҫ����Base64��QP����
	/// </summary>
	internal class Coding
	{
		/// <summary>
		/// ��ָ�����ַ�������Base64����
		/// </summary>
		/// <param name="Src">Ҫ���б�����ַ���</param>
		/// <param name="enc">Ҫ���б�����ַ���������</param>
		/// <returns>�����ַ�����ʽ��Base64�����</returns>
		public static string Base64Encode(string Src,Encoding enc)
		{
			if (string.IsNullOrEmpty(Src)) return "";  //�ձ���

			Src = Convert.ToBase64String(enc.GetBytes(Src));  //�������

			//�����ʼ�Ҫ�󣬽���ÿ����� 76 �ַ��Ĵ���
			if (Src.Length < 77) return Src;

			StringBuilder Result = new StringBuilder();

			long MaxFor = Src.Length - (Src.Length % 76);  //ȡ������

			//��Ҫѭ������
			for(int i=0;i<MaxFor ; i+=76)
			{
				Result.Append(Src.Substring(0,76)); //ȡ��76���ַ�
				Result.Append(MailCommon.NewLine);//���ӻ��з�
				Src = Src.Remove(0,76);
			}

			//�����������
			Result.Append(Src);
			return Result.ToString();
		}

		/// <summary>
		/// ��ָ�����ı�����QP����
		/// </summary>
		/// <param name="Src">Դ�ı�</param>
		/// <param name="enc">Ҫ���б�����ַ���������</param>
		/// <returns>�����ַ��������ʽ������ı�</returns>
		public static string EncodeQuotedPrintable(string Src,Encoding enc)
		{
			//��ȡ������ʽ������
			StringBuilder Sb = new StringBuilder();
			StringBuilder Line = new StringBuilder();

			byte[] Bytes  = enc.GetBytes(Src);  //��ȡ�ֽ�����
			byte   b;
			bool NewLine = false;
			int At;
			string LineString;

			//���б��봦��
			for(int i=0;i<Bytes.Length ;i++)
			{
				//�ж����Ƿ񳬹� ����Ҫ������ ��󳤶� 76 
				// Rule #5: MAXIMUM length 76 characters per line
				//д��ǰ����Ϊ�˷�ֹ �������  for ���������ֵ����
				if (NewLine)
				{
					Sb.Append(Line);
					Sb.Append("\r\n");
					Line = new StringBuilder();
					NewLine = false;
				}
				else
				{
					if (Line.Length > 75)
					{
						LineString = Line.ToString();
						At = LineString.LastIndexOf("=");

						if (At < 70) At = 74;

						Sb.Append(LineString.Substring(0,At));
						Sb.Append("=\r\n");

						Line = new StringBuilder();
						Line.Append(LineString.Substring(At));  //����ȡ������ݼ����µ�һ����
					
						//����ַ���
						LineString = null;
					}
				}

				//�õ���ǰASCII
				b = Bytes[i];

				if (b == 13)  //�س����д���    \r\n
				{
					//�жϼ� һ���Ƿ���   
					if ((i+1) <= Bytes.Length && Bytes[i+1] == 10)  
					{
						//�� i �ٴ� ++ �Դ����������ַ�
						i++;

						//���л��з�����
						NewLine = true;//����  �ı��Ļ���
						continue;
					}
					
					//�����س� 16���� ���� 
					Line.Append(b.ToString("X2")); 
					continue;
				}

				//��������   //����
				if (b == 10)
				{
					//���л��з�����
					NewLine = true;  //����  �ı��Ļ���
					//��������ѭ��
					continue; 
				}


				//�� = �ţ�ת�����
				if (b == 61)
				{
					Line.Append("=3D");
					continue;
				}

				// �� 33 �� 126 �Ŀɴ�ӡ(������) �ַ�����ֱ����� ��  =(61) ����
				//if (b > 32 && b < 127 && b != 61)  //ֱ�����  //�������Ѿ��� = ��ת������˴˴����ٽ��� = ���ж�
				if (b > 31 && b < 127)  //ֱ��  ���˴�ʡ�� �ո�ת��Ϊ =20 �ı任������ֱ�䷽ʽ
				{
					Line.Append((char)b);
					continue;
				}

				//������ֵ����
				Line.AppendFormat("={0:X2}",b);    //תΪ 16����  X2 ��ʾ���뱣֤��2λ����
 
				//Line.Append("=");
				//Line.Append(b.ToString("X"));  //תΪ 16����
			}

			//�������һ��
			Sb.Append(Line);

			return Sb.ToString();
		}

		/// <summary>
		/// ����BASE64ͷ�ı���
		/// </summary>
		/// <param name="Src">Ҫ���б���Ĵ�</param>
		/// <param name="enc">��������</param>
		/// <param name="Charset">�ļ��ַ���</param>
		/// <returns>���ر����Ĵ�</returns>
		internal static string EncodBase64Head(string Src,Encoding enc,string Charset)
		{
			return "=?" + Charset + "?B?" + Coding.Base64Encode(Src,enc) + "?=";
		}
		/// <summary>
		/// ����QPͷ�ı���
		/// </summary>
		/// <param name="Src">Ҫ���б���Ĵ�</param>
		/// <param name="enc">��������</param>
		/// <param name="Charset">�ļ��ַ���</param>
		/// <returns>���ر����Ĵ�</returns>
		internal static string EncodQPHead(string Src,Encoding enc,string Charset)
		{
			return "=?" + Charset + "?Q?" + Coding.EncodeQuotedPrintable(Src,enc) + "?=";
		}
	}
}