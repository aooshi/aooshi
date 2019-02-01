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
    /// ���ļ�ʽ���ݴ洢
    /// </summary>
    public class StaticCache
    {
        #region cache
        /// <summary>
        /// ��ȡһ������
        /// </summary>
        /// <param name="guid">����ID</param>
        public static object LoadCaching(string guid)
        {
            return HttpRuntime.Cache[guid];
        }
        /// <summary>
        /// ��ȡһ������
        /// </summary>
        /// <param name="guid">����ID</param>
        public static T LoadCaching<T>(string guid)
        {
            return (T)HttpRuntime.Cache[guid];
        }


        /// <summary>
        /// ���һ�����棬���ݻ�����ָ����ʱ��
        /// </summary>
        /// <param name="guid">����ID</param>
        /// <param name="time">����ʱ��</param>
        /// <param name="data">����</param>
        public static void AddCache(string guid, DateTime time, object data)
        {
            HttpRuntime.Cache.Add(guid, data, null, time, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// ���һ�����棬���ݻ��������һ�λ�ȡ���ָ��������
        /// </summary>
        /// <param name="guid">����ID</param>
        /// <param name="timespan">���һ�η��ʺ�ķ�����</param>
        /// <param name="data">����</param>
        public static void AddCache(string guid, int timespan, object data)
        {
            HttpRuntime.Cache.Add(guid, data, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(timespan), System.Web.Caching.CacheItemPriority.Normal, null);
        }
        #endregion






        string _path;

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="path">���ݴ洢·��</param>
        public StaticCache(string path)
        {
            this._path = path;
            if (!Directory.Exists(path)) throw new IOException("not find " + path);
        }

        #region append

        /// <summary>
        /// ���һ������,������������򸲸�
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="data">����</param>
        /// <returns>�������洢�ļ���·��</returns>
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
        /// ���һ�����ݣ�������������򸲸�
        /// </summary>
        /// <param name="seed">���ݱ�ʶ</param>
        /// <param name="data">����</param>
        public string Append(string seed, object data)
        {
            string path = GetPath(seed);
            if (File.Exists(path)) File.Delete(path);
            return this.Appended(path, data);
        }

        /// <summary>
        /// ���ݱ�ʶ
        /// </summary>
        /// <param name="id">���ֱ�ʶ</param>
        /// <param name="seed">���ֱ�ʶ</param>
        /// <param name="data">����</param>
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


        //���
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
        /// ��ȡ����
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="output">�������</param>
        /// <returns>�Ƿ�ɹ���ȡ</returns>
        public bool Read(long id, out object output)
        {
            output = null;
            string path = GetPath(id);
            if (!File.Exists(path)) return false;
            output = this.Readed(path);
            return true;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <typeparam name="T">���صĶ���</typeparam>
        /// <param name="output">�������</param>
        /// <param name="id">����ID</param>
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
        /// ��ȡ����
        /// </summary>
        /// <param name="seed">��ʶ</param>
        /// <param name="output">�������</param>
        public bool Read(string seed, out object output)
        {
            output = null;
            string path = GetPath(seed);
            if (!File.Exists(path)) return false;
            output = this.Readed(path);
            return true;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="output">�������</param>
        /// <param name="seed">��ʶ</param>
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
        /// ��ȡ����
        /// </summary>
        /// <param name="id">���ݱ�ʶ</param>
        /// <param name="seed">��ʶ</param>
        /// <param name="output">�������</param>
        public bool Read(long id, int seed, out object output)
        {
            output = null;
            string path = GetPath2(id, seed);
            if (!File.Exists(path)) return false;
            output = this.Readed(path);
            return true;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="id">���ݱ�ʶ</param>
        /// <param name="seed">��ʶ</param>
        /// <param name="output">�������</param>
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
        /// ��ȡ
        /// </summary>
        /// <param name="path">·��</param>
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
        /// ɾ��һ������
        /// </summary>
        /// <param name="id">ID</param>
        public void Remove(long id)
        {
            string path = GetPath(id);
            if (File.Exists(path)) File.Delete(path);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="seed">���ݱ�ʶ</param>
        public void Remove(string seed)
        {
            string path = GetPath(seed);
            if (File.Exists(path)) File.Delete(path);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="seed">���ݱ�ʶ</param>
        public void Remove(long id, int seed)
        {
            string path = GetPath2(id, seed);
            if (File.Exists(path)) File.Delete(path);
        }

        #endregion

        #region common

        /// <summary>
        /// ���ݴ���
        /// </summary>
        /// <param name="value">��ֵ</param>
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

        //����·��
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
        //����·��
        private string GetPath(string seed)
        {
            return Path.Combine(this._path, seed);
        }

        #endregion
    }
}