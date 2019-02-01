using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Aooshi.Web
{
    /// <summary>
    /// 直接方法执行映射类
    /// </summary>
    /// <example>
    ///     void Check(){};
    ///     void Check(HttpRequest request,HttpResponse response){}
    /// </example>
    public class MethodHandler : IHttpHandler,IRequiresSessionState
    {
        static Type IMethodHandler = typeof(IMethodHandlerType);

        #region IHttpHandler 成员
        /// <summary>
        /// 是否共享HTTP请求
        /// </summary>
        public bool IsReusable
        {
            get {return false; }
        }

        /// <summary>
        /// 执行处理
        /// </summary>
        /// <param name="context">Http请求上下文</param>
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string @class = System.IO.Path.GetFileNameWithoutExtension(VirtualPathUtility.GetFileName(request.Url.AbsolutePath));

            string action = (request.QueryString["action"] ?? "").Trim();
            if (action == "") throw new HttpException(500, "Not Set Action.");


            Type classtype = Type.GetType(@class, false, true);
            if (classtype == null)
                throw new HttpException(500, "Not Found Page.");

            if (!MethodHandler.IMethodHandler.IsAssignableFrom(classtype))
                throw new HttpException(500,"class not is IMethodHandlerType.");

            System.Reflection.MethodInfo m = classtype.GetMethod(action, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            if (m == null) throw new HttpException(500, "Not Found Method '"+ action +"'.");
            object r,o=null;
            if (!m.IsStatic) o = Activator.CreateInstance(classtype);
            r = (m.GetParameters().Length == 2) ? m.Invoke(o, new object[] { request, response }) : m.Invoke(o, null);
            if (r != null) response.Write(r);
        }

        #endregion
    }
}
