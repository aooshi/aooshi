using System;
using System.Collections.Generic;
using System.Xml;
using System.Timers;

namespace Aooshi
{
    /// <summary>
    /// 快速简单配置
    /// </summary>
    /// <remarks>
    /// <code>
    ///     <root>
    ///         &lt;-- 单一配置 --&gt;
    ///         <set name="onevalue1" value=""></set>
    ///         <set name="onevalue2" value=""></set>
    ///         <set name="onevalue3" value=""></set>
    ///         
    ///         &lt;-- 多配置 --&gt;
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
        /// 根据配置文件初始化类型
        /// </summary>
        /// <param name="path">路径</param>
        public EasyConfigQuick(string path):this(path,false)
        {
        }
        /// <summary>
        /// 根据配置文件初始化类型
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="isautoupdate">当文件变化时是否自动更新</param>
        public EasyConfigQuick(string path,bool isautoupdate)
        {
            this.path = path;
            this.Reload();
            this.IsAutoUpdate = isautoupdate;
        }

        /// <summary>
        /// 根据节点创建配置
        /// </summary>
        /// <param name="element">配置节</param>
        /// <param name="emptylist">是否清空cachelist</param>
        /// <param name="upname">是一级名称</param>
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
        /// 重新载入数据
        /// </summary>
        public void Reload()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            this.CreateCache(doc.DocumentElement, true,"");
        }

        string path;
        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        public string Path
        {
            get { return this.path; }
        }

        /// <summary>
        /// 获取当前配置总量
        /// </summary>
        public int Count
        {
            get { return this._CacheList.Count; }
        }


        /// <summary>
        /// 获取一个配置的值
        /// </summary>
        /// <param name="name">配置名称,子级之间用"/"分隔</param>
        public string this[string name]
        {
            get
            {
                return this.GetValue(name, "");
            }
        }
        

        /// <summary>
        /// 获取一个配置的值
        /// </summary>
        /// <param name="name">配置名称,子级之间用"/"分隔</param>
        /// <param name="def">当未找到时的默认值</param>
        /// <value>未配置则返回Null</value>
        public string GetValue(string name, string def)
        {
            if (!this._CacheList.TryGetValue(name, out name)) return def;
            return name;
        }


        /// <summary>
        /// 判断是否具有指定的配置
        /// </summary>
        /// <param name="name">配置名称,子级之间用"/"分隔</param>
        public bool IsExists(string name)
        {
            return this._CacheList.ContainsKey(name);
        }



        #region auto update


        bool _IsAutoUpdate = false;
        /// <summary>
        /// 获取或设置当文件发生变化时是否自动更新内容
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





