using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Aooshi
{
    /// <summary>
    /// 程序集相关
    /// </summary>
    public class AssemblyString
    {
        Assembly myAssembly;
        FileVersionInfo fileVersionInfo;

        /// <summary>
        /// 按当前执行程序集初始化
        /// </summary>
        public AssemblyString() : this(Assembly.GetExecutingAssembly())
        {
        }

        /// <summary>
        /// 按指定程序集初始数据
        /// </summary>
        /// <param name="assembly">程序集</param>
        public AssemblyString(Assembly assembly)
        {
            this.myAssembly = assembly;
            this.fileVersionInfo = null;
        }


        /// <summary>
        /// 获取版本文件信息
        /// </summary>
        public FileVersionInfo VersionInfo
        {
            get
            {
                if (this.fileVersionInfo == null)
                {
                    this.fileVersionInfo = FileVersionInfo.GetVersionInfo(this.myAssembly.Location);
                }
                return this.fileVersionInfo;
            }
        }

        /// <summary>
        /// 获取产品版本号
        /// </summary>
        /// <returns></returns>
        public string GetProductVersion()
        {
            return string.Format("{0}.{1}.{2}", this.VersionInfo.ProductMajorPart, this.VersionInfo.ProductMinorPart, this.VersionInfo.ProductBuildPart);
        }

        /// <summary>
        /// 获取文件版本号
        /// </summary>
        /// <returns></returns>
        public string GetFileVersion()
        {
            return string.Format("{0}.{1}.{2}", this.VersionInfo.FileMajorPart, this.VersionInfo.FileMinorPart, this.VersionInfo.FileBuildPart);
        }
    }
}
