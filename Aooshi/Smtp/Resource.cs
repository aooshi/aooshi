using System;
using System.Resources;
using System.Reflection;

namespace Aooshi.Smtp
{
	/// <summary>
	/// ���ϴ�����
	/// </summary>
	internal class Resource
	{
		ResourceManager Rm;

		/// <summary>
		/// ��ʼ��
		/// </summary>
		public Resource()
		{
			Rm = new ResourceManager("Aooshi.Smtp.Res.cn",Assembly.GetExecutingAssembly());
		}

		/// <summary>
		/// ��ȡָ�����Ƶ��ַ���ֵ
		/// </summary>
		/// <param name="Name">����</param>
		/// <returns>������Դ�е��ַ���ֵ</returns>
		public string GetString(string Name)
		{
			object r= this.Rm.GetObject(Name);
			if (r == null) return "";
			return r.ToString();
		}

		/// <summary>
		/// �ͷ���Դ
		/// </summary>
		public void Close()
		{
			this.Rm.ReleaseAllResources();
		}
	}
}
