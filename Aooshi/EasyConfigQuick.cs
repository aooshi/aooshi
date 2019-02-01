using System;
using System.Collections.Generic;
using System.Xml;
using System.Timers;

namespace Aooshi
{
    /// <summary>
    /// ���ټ�����
    /// </summary>
    /// <remarks>
    /// <code>
    ///     <root>
    ///         &lt;-- ��һ���� --&gt;
    ///         <set name="onevalue1" value=""></set>
    ///         <set name="onevalue2" value=""></set>
    ///         <set name="onevalue3" value=""></set>
    ///         
    ///         &lt;-- ������ --&gt;
    ///         <set name="more" value="">
    ///             <set name="more1" value=""></set>
    ///             <set name="more2" value=""></set>
    ///         </set>
    ///     </root>
    /// </code>
    /// </remarks>
    public class EasyConfigQuick
    {
        Dictionary<string, string> _CacheList = null;
        /// <summary>
        /// ���������ļ���ʼ������
        /// </summary>
        /// <param name="path">·��</param>
        public EasyConfigQuick(string path):this(path,false)
        {
        }
        /// <summary>
        /// ���������ļ���ʼ������
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="isautoupdate">���ļ��仯ʱ�Ƿ��Զ�����</param>
        public EasyConfigQuick(string path,bool isautoupdate)
        {
            this.path = path;
            this.Reload();
            this.IsAutoUpdate = isautoupdate;
        }

        /// <summary>
        /// ���ݽڵ㴴������
        /// </summary>
        /// <param name="element">���ý�</param>
        /// <param name="emptylist">�Ƿ����cachelist</param>
        /// <param name="upname">��һ������</param>
        private void CreateCache(XmlElement element, bool emptylist,string upname)
        {
            lock (this)
            {
                if (emptylist) this._CacheList = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                string n, v,tn = "";
                if (upname != "") tn = upname + "/";
                foreach (XmlElement xe in element.SelectNodes("set"))
                {
                    n = tn + xe.GetAttribute("name");
                    v = xe.GetAttribute("value");
                    //value
                    if (string.IsNullOrEmpty(n))
                    {
#if DEBUG
                        throw new AooshiException("name is null -- " + xe.OuterXml);
#else
                             throw new AooshiException("name is null");
#endif
                    }
                    if (_CacheList.ContainsKey(n))
                        throw new AooshiException("Contains name=" + n);

                    _CacheList.Add(n, v);

                    this.CreateCache(xe, false,n);
                }
            }
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void Reload()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            this.CreateCache(doc.DocumentElement, true,"");
        }

        string path;
        /// <summary>
        /// ��ȡ�����ļ�·��
        /// </summary>
        public string Path
        {
            get { return this.path; }
        }

        /// <summary>
        /// ��ȡ��ǰ��������
        /// </summary>
        public int Count
        {
            get { return this._CacheList.Count; }
        }


        /// <summary>
        /// ��ȡһ�����õ�ֵ
        /// </summary>
        /// <param name="name">��������,�Ӽ�֮����"/"�ָ�</param>
        public string this[string name]
        {
            get
            {
                return this.GetValue(name, "");
            }
        }
        

        /// <summary>
        /// ��ȡһ�����õ�ֵ
        /// </summary>
        /// <param name="name">��������,�Ӽ�֮����"/"�ָ�</param>
        /// <param name="def">��δ�ҵ�ʱ��Ĭ��ֵ</param>
        /// <value>δ�����򷵻�Null</value>
        public string GetValue(string name, string def)
        {
            if (!this._CacheList.TryGetValue(name, out name)) return def;
            return name;
        }


        /// <summary>
        /// �ж��Ƿ����ָ��������
        /// </summary>
        /// <param name="name">��������,�Ӽ�֮����"/"�ָ�</param>
        public bool IsExists(string name)
        {
            return this._CacheList.ContainsKey(name);
        }



        #region auto update


        bool _IsAutoUpdate = false;
        /// <summary>
        /// ��ȡ�����õ��ļ������仯ʱ�Ƿ��Զ���������
        /// </summary>
        public bool IsAutoUpdate
        {
            get { return _IsAutoUpdate; }
            set
            {
                this._IsAutoUpdate = value;
                this.AutoChange();
            }
        }

        System.IO.FileSystemWatcher fsw = null;
        System.Threading.Thread thread = null;

        private void AutoChange()
        {
            if (this.IsAutoUpdate)
            {
                fsw = new System.IO.FileSystemWatcher(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileName(path));
                fsw.IncludeSubdirectories = false;
                fsw.Changed += new System.IO.FileSystemEventHandler(fsw_Changed);
                fsw.Created += new System.IO.FileSystemEventHandler(fsw_Changed);
                fsw.EnableRaisingEvents = true;
            }
            else
            {
                if (fsw != null)
                {
                    fsw.Dispose();
                    fsw = null;
                }
            }
        }

        private void fsw_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            lock (this)
            {
                if (thread != null)
                {
                    try { thread.Abort(); }
                    catch { }
                }
                thread = new System.Threading.Thread(new System.Threading.ThreadStart(UpFun));
                thread.Start();
            }
        }

        private void UpFun()
        {
            System.Threading.Thread.Sleep(3000);
            this.Reload();
        }

        #endregion

    }
}





