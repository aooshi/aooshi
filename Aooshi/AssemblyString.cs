using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace Aooshi
{
    /// <summary>
    /// �������
    /// </summary>
    public class AssemblyString
    {
        Assembly myAssembly;
        FileVersionInfo fileVersionInfo;

        /// <summary>
        /// ����ǰִ�г��򼯳�ʼ��
        /// </summary>
        public AssemblyString() : this(Assembly.GetExecutingAssembly())
        {
        }

        /// <summary>
        /// ��ָ�����򼯳�ʼ����
        /// </summary>
        /// <param name="assembly">����</param>
        public AssemblyString(Assembly assembly)
        {
            this.myAssembly = assembly;
            this.fileVersionInfo = null;
        }


        /// <summary>
        /// ��ȡ�汾�ļ���Ϣ
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
        /// ��ȡ��Ʒ�汾��
        /// </summary>
        /// <returns></returns>
        public string GetProductVersion()
        {
            return string.Format("{0}.{1}.{2}", this.VersionInfo.ProductMajorPart, this.VersionInfo.ProductMinorPart, this.VersionInfo.ProductBuildPart);
        }

        /// <summary>
        /// ��ȡ�ļ��汾��
        /// </summary>
        /// <returns></returns>
        public string GetFileVersion()
        {
            return string.Format("{0}.{1}.{2}", this.VersionInfo.FileMajorPart, this.VersionInfo.FileMinorPart, this.VersionInfo.FileBuildPart);
        }
    }
}
