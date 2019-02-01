using System;
using System.Web;

namespace Aooshi.Web
{
    /// <summary>
    /// 一个当前请求线程内缓存描述
    /// </summary>
    /// <remarks>若派发多子线程时请注意线程互斥</remarks>
    /// <typeparam name="TClass">缓存类型</typeparam>
    public abstract class ContextCache<TClass> : ValueManager<TClass> where TClass : class
    {
        /// <summary>
        /// 获取当前实例
        /// </summary>
        /// <param name="cacheName">缓存名</param>
        protected static T GetInstance<T>(string cacheName) where T : ContextCache<TClass> , new()
        {
            cacheName = "_CONTEXT_CACHE_" + cacheName;
            var result = HttpContext.Current.Items[cacheName] as T;
            if (result == null)
            {
                result = new T();
                HttpContext.Current.Items[cacheName] = result;
            }
            return result;
        }

    }
    
    //class demo : ContextCache<Array>
    //{
    //    const string CAHCE_NAME = "DEMO";

    //    public static demo Instance
    //    {
    //        get
    //        {
    //            return ContextCache<Array>.GetInstance<demo>(CAHCE_NAME);
    //        }
    //    }

    //    protected override Array Init()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
