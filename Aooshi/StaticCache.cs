using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Web;

namespace Aooshi
{
    /// <summary>
    /// 简单文件式数据存储
    /// </summary>
    public class StaticCache
    {
        #region cache
        /// <summary>
        /// 获取一个配置
        /// </summary>
        /// <param name="guid">配置ID</param>
        public static object LoadCaching(string guid)
        {
            return HttpRuntime.Cache[guid];
        }
        /// <summary>
        /// 获取一个配置
        /// </summary>
        /// <param name="guid">配置ID</param>
        public static T LoadCaching<T>(string guid)
        {
            return (T)HttpRuntime.Cache[guid];
        }


        /// <summary>
        /// 添加一个缓存，数据缓存至指定的时间
        /// </summary>
        /// <param name="guid">配置ID</param>
        /// <param name="time">过期时间</param>
        /// <param name="data">数据</param>
        public static void AddCache(string guid, DateTime time, object data)
        {
            HttpRuntime.Cache.Add(guid, data, null, time, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// 添加一个缓存，数据缓存至最后一次获取后的指定分钟数
        /// </summary>
        /// <param name="guid">配置ID</param>
        /// <param name="timespan">最后一次访问后的分钟数</param>
        /// <param name="data">数据</param>
        public static void AddCache(string guid, int timespan, object data)
        {
            HttpRuntime.Cache.Add(guid, data, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(timespan), System.Web.Caching.CacheItemPriority.Normal, null);
        }
        #endregion






        string _path;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="path">数据存储路径</param>
        public StaticCache(string path)
        {
            this._path = path;
            if (!Directory.Exists(path)) throw new IOException("not find " + path);
        }

        #region append

        /// <summary>
        /// 添加一个数据,如果已有数据则覆盖
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="data">数据</param>
        /// <returns>返回所存储文件的路径</returns>
        public string Append(long id, object data)
        {
            string path = GetPath(id);
            if (File.Exists(path)) File.Delete(path);
            else
            {
                string pc = Path.Combine(this._path, NumberToPath(id));
                if (!Directory.Exists(pc)) Directory.CreateDirectory(pc);
            }

            return this.Appended(path, data);
        }

        /// <summary>
        /// 添加一个数据，如果已有数据则覆盖
        /// </summary>
        /// <param name="seed">数据标识</param>
        /// <param name="data">数据</param>
        public string Append(string seed, object data)
        {
            string path = GetPath(seed);
            if (File.Exists(path)) File.Delete(path);
            return this.Appended(path, data);
        }

        /// <summary>
        /// 数据标识
        /// </summary>
        /// <param name="id">数字标识</param>
        /// <param name="seed">区分标识</param>
        /// <param name="data">数据</param>
        public string Append(long id, int seed, object data)
        {
            string path = GetPath2(id, seed);
            if (File.Exists(path)) File.Delete(path);
            else
            {
                string pc = Path.Combine(this._path, NumberToPath(id));
                if (!Directory.Exists(pc)) Directory.CreateDirectory(pc);
            }
            return this.Appended(path, data);
        }


        //添加
        private string Appended(string path, object data)
        {
            IFormatter formatter = new BinaryFormatter();

            lock (this)
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(fs, data);
                    fs.Close();
                }
            }
            return path;
        }

        #endregion

        #region load


        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="output">输出对象</param>
        /// <returns>是否成功获取</returns>
        public bool Read(long id, out object output)
        {
            output = null;
            string path = GetPath(id);
            if (!File.Exists(path)) return false;
            output = this.Readed(path);
            return true;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T">返回的对象</typeparam>
        /// <param name="output">输出对象</param>
        /// <param name="id">数据ID</param>
        public bool Read<T>(long id, out T output)
        {
            object o;
            if (!this.Read(id, out o))
            {
                output = default(T);
                return false;
            }
            output = (T)o;
            return true;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="seed">标识</param>
        /// <param name="output">输出对象</param>
        public bool Read(string seed, out object output)
        {
            output = null;
            string path = GetPath(seed);
            if (!File.Exists(path)) return false;
            output = this.Readed(path);
            return true;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="output">输出对象</param>
        /// <param name="seed">标识</param>
        public bool Read<T>(string seed, out T output)
        {
            object o;
            if (!this.Read(seed, out o))
            {
                output = default(T);
                return false;
            }
            output = (T)o;
            return true;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="id">数据标识</param>
        /// <param name="seed">标识</param>
        /// <param name="output">输出对象</param>
        public bool Read(long id, int seed, out object output)
        {
            output = null;
            string path = GetPath2(id, seed);
            if (!File.Exists(path)) return false;
            output = this.Readed(path);
            return true;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="id">数据标识</param>
        /// <param name="seed">标识</param>
        /// <param name="output">输出对象</param>
        public bool Read<T>(long id, int seed, out T output)
        {
            object o;
            if (!this.Read(id, seed, out o))
            {
                output = default(T);
                return false;
            }
            output = (T)o;
            return true;
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="path">路径</param>
        private object Readed(string path)
        {
            IFormatter formatter = new BinaryFormatter();
            using (FileStream fs_d = File.OpenRead(path))
            {
                return formatter.Deserialize(fs_d);
            }
        }

        #endregion

        #region remove

        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="id">ID</param>
        public void Remove(long id)
        {
            string path = GetPath(id);
            if (File.Exists(path)) File.Delete(path);
        }

        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="seed">数据标识</param>
        public void Remove(string seed)
        {
            string path = GetPath(seed);
            if (File.Exists(path)) File.Delete(path);
        }
        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="seed">数据标识</param>
        public void Remove(long id, int seed)
        {
            string path = GetPath2(id, seed);
            if (File.Exists(path)) File.Delete(path);
        }

        #endregion

        #region common

        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="value">数值</param>
        private string NumberToPath(long value)
        {
            string result = "";
            if (value > 1000)
            {
                do
                {
                    value = value / 1000;
                    result = value + "/" + result;
                }
                while (value > 1000);
            }
            return result;
        }

        //处理路径
        private string GetPath(long id)
        {
            string path = Path.Combine(this._path, NumberToPath(id));
            return Path.Combine(path, "_" + id.ToString());
        }
        //
        private string GetPath2(long id, int seed)
        {
            string path = Path.Combine(this._path, NumberToPath(id));
            return Path.Combine(path, "@" + id.ToString() + "," + seed.ToString());
        }
        //处理路径
        private string GetPath(string seed)
        {
            return Path.Combine(this._path, seed);
        }

        #endregion
    }
}