using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.IO.Compression;

namespace Aooshi.Web
{
    /// <summary>
    /// AspNet Compress Module
    /// </summary>
    public class CompressModule : IHttpModule
    {
        private const string GZIP = "gzip";
        private const string DEFLATE = "deflate";
        private const string NO_COMPRESS = "X-NoCompress";

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 重载模块初始
        /// </summary>
        /// <param name="context">Http Context</param>
        public void Init(HttpApplication context)
        {
            context.PostReleaseRequestState += new EventHandler(Context_PostReleaseRequestState);
        }

        private void Context_PostReleaseRequestState(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            Stream filter = app.Response.Filter;
            string acceptEncoding = app.Request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(acceptEncoding) && !IsNoCompress(app.Context))
            {
                acceptEncoding = acceptEncoding.ToLower();
                if (IsGZIP(acceptEncoding))
                {
                    app.Response.Filter = new GZipStream(filter, CompressionMode.Compress);
                    app.Response.AppendHeader("Content-Encoding", GZIP);
                }
                else if (IsDEFLATE(acceptEncoding))
                {
                    app.Response.Filter = new DeflateStream(filter, CompressionMode.Compress);
                    app.Response.AppendHeader("Content-Encoding", DEFLATE);
                }
            }
        }

        private static bool IsGZIP(string acceptEncoding)
        {
            return acceptEncoding.Contains(GZIP) || acceptEncoding.Contains("x-gzip") || acceptEncoding.Contains("*");
        }

        private static bool IsDEFLATE(string acceptEncoding)
        {
            return acceptEncoding.Contains(DEFLATE);
        }

        private static bool IsNoCompress(HttpContext context)
        {
            return true.Equals(context.Items[NO_COMPRESS]);
        }

        /// <summary>
        /// 设置不进行压缩处理
        /// </summary>
        /// <param name="context">Http Context</param>
        public static void NoCompress(HttpContext context)
        {
            context.Items[NO_COMPRESS] = true;
        }
    }
}


 //改进：
 //    1.可以在config文件中加入自定义的ConfigurationSection，使用配置的方式实现对指定的文件扩展名进行压缩，或排除对指定的文件扩展名的压缩。
 //    2.网络上看到有人说要针对微软的AJAX做处理，不能进行压缩，判断的方法：
 //         return (app.Request["HTTP_X_MICROSOFTAJAX"] != null
 //                 || app.Request["Anthem_CallBack"] != null);
 //    3.可针对实现了某个基类或接口的页面进行压缩或不压缩：
 //          ICompressable p = app.Context.Handler as ICompressable;
 //          return (p == null);

//PS:  接口排除法对性能有过多的浪费，现提供 CompressModule.NoCompress(this.Context) 以供设置不进行压缩