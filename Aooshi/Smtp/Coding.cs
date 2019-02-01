using System;
using System.Text;
using System.IO;

namespace Aooshi.Smtp
{
	/// <summary>
	/// 该类主要处理Base64与QP编码
	/// </summary>
	internal class Coding
	{
		/// <summary>
		/// 将指定的字符串进行Base64编码
		/// </summary>
		/// <param name="Src">要进行编码的字符串</param>
		/// <param name="enc">要进行编码的字符编码类型</param>
		/// <returns>返回字符串形式的Base64编码后串</returns>
		public static string Base64Encode(string Src,Encoding enc)
		{
			if (string.IsNullOrEmpty(Src)) return "";  //空编码

			Src = Convert.ToBase64String(enc.GetBytes(Src));  //编码完成

			//满足邮件要求，进行每行最大 76 字符的处理
			if (Src.Length < 77) return Src;

			StringBuilder Result = new StringBuilder();

			long MaxFor = Src.Length - (Src.Length % 76);  //取得余数

			//须要循环的数
			for(int i=0;i<MaxFor ; i+=76)
			{
				Result.Append(Src.Substring(0,76)); //取得76个字符
				Result.Append(MailCommon.NewLine);//增加换行符
				Src = Src.Remove(0,76);
			}

			//增加最后数据
			Result.Append(Src);
			return Result.ToString();
		}

		/// <summary>
		/// 将指定的文本进行QP编码
		/// </summary>
		/// <param name="Src">源文本</param>
		/// <param name="enc">要进行编码的字符编码类型</param>
		/// <returns>返回字符串表达形式编码后文本</returns>
		public static string EncodeQuotedPrintable(string Src,Encoding enc)
		{
			//获取编码型式的数据
			StringBuilder Sb = new StringBuilder();
			StringBuilder Line = new StringBuilder();

			byte[] Bytes  = enc.GetBytes(Src);  //获取字节数组
			byte   b;
			bool NewLine = false;
			int At;
			string LineString;

			//进行编码处理
			for(int i=0;i<Bytes.Length ;i++)
			{
				//判断行是否超过 编码要求的最大 最大长度 76 
				// Rule #5: MAXIMUM length 76 characters per line
				//写在前面是为了防止 多次跳出  for 而引起的数值变异
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
						Line.Append(LineString.Substring(At));  //将截取后的数据加入新的一行中
					
						//清空字符串
						LineString = null;
					}
				}

				//得到当前ASCII
				b = Bytes[i];

				if (b == 13)  //回车换行处理    \r\n
				{
					//判断加 一后是否超限   
					if ((i+1) <= Bytes.Length && Bytes[i+1] == 10)  
					{
						//将 i 再次 ++ 以处理两个数字符
						i++;

						//进行换行符增加
						NewLine = true;//增加  文本的换行
						continue;
					}
					
					//处理单回车 16进制 编码 
					Line.Append(b.ToString("X2")); 
					continue;
				}

				//处理单换行   //换行
				if (b == 10)
				{
					//进行换行符增加
					NewLine = true;  //增加  文本的换行
					//跳出本次循环
					continue; 
				}


				//将 = 号，转换输出
				if (b == 61)
				{
					Line.Append("=3D");
					continue;
				}

				// 将 33 至 126 的可打印(即键盘) 字符进行直接输出 除  =(61) 以外
				//if (b > 32 && b < 127 && b != 61)  //直接输出  //因上面已经将 = 号转换，因此此处不再进行 = 号判断
				if (b > 31 && b < 127)  //直输  ，此处省掉 空格转换为 =20 的变换，采用直输方式
				{
					Line.Append((char)b);
					continue;
				}

				//进行数值编码
				Line.AppendFormat("={0:X2}",b);    //转为 16进制  X2 表示必须保证在2位以内
 
				//Line.Append("=");
				//Line.Append(b.ToString("X"));  //转为 16进制
			}

			//增加最后一行
			Sb.Append(Line);

			return Sb.ToString();
		}

		/// <summary>
		/// 进行BASE64头的编码
		/// </summary>
		/// <param name="Src">要进行编码的串</param>
		/// <param name="enc">编码类型</param>
		/// <param name="Charset">文件字符集</param>
		/// <returns>返回编码后的串</returns>
		internal static string EncodBase64Head(string Src,Encoding enc,string Charset)
		{
			return "=?" + Charset + "?B?" + Coding.Base64Encode(Src,enc) + "?=";
		}
		/// <summary>
		/// 进行QP头的编码
		/// </summary>
		/// <param name="Src">要进行编码的串</param>
		/// <param name="enc">编码类型</param>
		/// <param name="Charset">文件字符集</param>
		/// <returns>返回编码后的串</returns>
		internal static string EncodQPHead(string Src,Encoding enc,string Charset)
		{
			return "=?" + Charset + "?Q?" + Coding.EncodeQuotedPrintable(Src,enc) + "?=";
		}
	}
}