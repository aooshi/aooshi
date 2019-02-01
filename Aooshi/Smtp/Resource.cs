using System;
using System.Resources;
using System.Reflection;

namespace Aooshi.Smtp
{
	/// <summary>
	/// 资料处理类
	/// </summary>
	internal class Resource
	{
		ResourceManager Rm;

		/// <summary>
		/// 初始化
		/// </summary>
		public Resource()
		{
			Rm = new ResourceManager("Aooshi.Smtp.Res.cn",Assembly.GetExecutingAssembly());
		}

		/// <summary>
		/// 获取指定名称的字符串值
		/// </summary>
		/// <param name="Name">名称</param>
		/// <returns>返回资源中的字符串值</returns>
		public string GetString(string Name)
		{
			object r= this.Rm.GetObject(Name);
			if (r == null) return "";
			return r.ToString();
		}

		/// <summary>
		/// 释放资源
		/// </summary>
		public void Close()
		{
			this.Rm.ReleaseAllResources();
		}
	}
}
