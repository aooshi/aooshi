using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Web;
using System.Xml;

namespace Aooshi.Ajax
{
    /// <summary>
    /// ����ҳ������
    /// </summary>
    public class CallbackPage : System.Web.UI.Page
    {

        /// <summary>
        /// ����ַ�����ʽ
        /// </summary>
        /// <param name="obj">������ı�����</param>
        protected virtual void WriteString(object obj)
        {
            Response.AppendHeader("Content-Type", "text/html");
            Response.Write(obj.ToString());
            Response.End();
        }
        /// <summary>
        /// ����ַ�����ʽ
        /// </summary>
        /// <param name="obj">�����XML�ĵ�����</param>
        protected virtual void WriteXmlDoc(XmlDocument obj)
        {
            Response.AppendHeader("Content-Type", "text/xml");
            obj.Save(Response.OutputStream);
            Response.End();
        }
        /// <summary>
        /// ����ű��ļ�
        /// </summary>
        /// <param name="obj">�ű��ļ�</param>
        protected virtual void WriteJScript(string obj)
        {
            Response.AppendHeader("Content-Type", "application/x-javascript");
            Response.Write(obj.ToString());
            Response.End();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="e">�¼�</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Request.RequestType.ToLower() != "post") throw new AjaxException("��֧��get��ȡ������get method is not supported");

            try
            {
                string typeName = AjaxUtils.AjaxUnEncode(Request.Form["Ajax_Aooshi_Namespace"].Trim());
                string mtdName = AjaxUtils.AjaxUnEncode(Request.Form["Ajax_Aooshi_MethodName"].Trim());
                string mtdObject = AjaxUtils.AjaxUnEncode(Request.Form["Ajax_Aooshi_MethodObject"].Trim());
                object obj = null;
                string[] pms = new string[Request.Form.Count - 3];

                Type tempType = Type.GetType(typeName, true);
                MethodInfo mi = tempType.GetMethod(mtdName);

                if (mi == null)
                {
                    throw new HttpException(404, "NOT FIND Method Name!");
                }

                if (mi.IsPublic)
                {
                    AjaxMethod[] ajaxmethods = (AjaxMethod[])mi.GetCustomAttributes(typeof(AjaxMethod), true);
                    if (ajaxmethods.Length == 0) throw new AjaxNotFindMethodException("��Ҫ���õĺ�����������ע��ΪAjaxMethod���Եĺ�����");

                    if (ajaxmethods[0].MethodType == AjaxMethodType.Array)//������ʽ
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        for (int i = 3; i < Request.Form.Count; i++)
                            dic.Add(Request.Form.GetKey(i), Request.Form[i]);

                        if (mi.IsStatic)
                            obj = mi.Invoke(null, new object[] { dic });
                        else
                            obj = mi.Invoke(tempType.Module.Assembly.CreateInstance(tempType.FullName), new object[] { dic });
                    }
                    else if (ajaxmethods[0].MethodType == AjaxMethodType.Object) //������ʽ
                    {
                        Type mtdt = Type.GetType(mtdObject, true);
                        object mtdo = mtdt.Module.Assembly.CreateInstance(mtdt.FullName);
                        PropertyInfo pinfo;

                        for (int i = 3; i < Request.Form.Count; i++)
                        {
                            pinfo = mtdt.GetProperty(Request.Form.GetKey(i), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
                            if (pinfo != null)
                                pinfo.SetValue(mtdo, Request.Form[i], null);
                        }

                        if (mi.IsStatic)
                            obj = mi.Invoke(null, new object[] { mtdo });
                        else
                            obj = mi.Invoke(tempType.Module.Assembly.CreateInstance(tempType.FullName), new object[] { mtdo });
                    }
                    else//������ʽ
                    {
                        for (int i = 3; i < Request.Form.Count; i++)
                            pms[i - 3] = AjaxUtils.AjaxUnEncode(Request.Form[i]);

                        if (mi.IsStatic)
                            obj = mi.Invoke(null, pms);
                        else
                            obj = mi.Invoke(tempType.Module.Assembly.CreateInstance(tempType.FullName), pms);
                    }

                    if (obj != null)
                    {
                        Type type = obj.GetType();
                        if (type == typeof(XmlDocument))
                        {
                            WriteXmlDoc((XmlDocument)obj);
                            return;
                        }
                        if (type == typeof(Boolean))
                        {
                            WriteString(Convert.ToInt32(obj).ToString());
                            return;
                        }
                        //����
                        if (type.IsArray)
                        {
                            AjaxMakeObject.RunatArrayObject(obj, type, Response);
                            return;
                        }
                        //������
                        if (type.GetCustomAttributes(typeof(AjaxObject), true).Length > 0)
                        {
                            AjaxMakeObject.RunatClassObject(obj, type, Response);
                            return;
                        }
                        WriteString(obj);
                    }
                    else
                    {
                        WriteString("");
                    }
                }

            }
            catch (System.Exception ex)
            {
                throw new HttpException(500, ex.Message, ex.InnerException);
            }
            throw new HttpException(404, "NOT FIND PAGE!");

        }
    }
}
