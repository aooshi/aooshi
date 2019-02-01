using System;
using System.Web;


namespace Aooshi.Web
{
    /// <summary>
    /// һ����̬ҳ����ʾ��Ϣ��
    /// </summary>
    public class MessageAlert
    {
        /// <summary>
        /// ��ʾ��Ϣ����,��ʾ������һҳ��ť
        /// </summary>
        /// <param name="Msg">Ҫ������ʾ��Ϣ</param>
        public static void Message(string Msg)
        {
            MessageAlert.Message(new string[] { Msg }, "");
        }

        /// <summary>
        /// ��ʾ��Ϣ����,��ʾ�����Ϣ��,��ʾ������һҳ��ť
        /// </summary>
        /// <param name="Msgs">Ҫ������ʾ��Ϣ����</param>
        public static void Message(string[] Msgs)
        {
            MessageAlert.Message(Msgs, "");
        }

        /// <summary>
        /// ������Ϣ��ʾ��ת��ָ����URL��ַ
        /// </summary>
        /// <param name="Msg">Ҫ��ʾ����ʾ��Ϣ</param>
        /// <param name="Url">Ҫת���URL</param>
        public static void Message(string Msg, string Url)
        {
            MessageAlert.Message(new string[] { Msg }, Url);
        }

        /// <summary>
        /// ������Ϣ��ʾ,��ָ�������Ϣ��,��ʾ��ɺ�ת��ָ����URL
        /// </summary>
        /// <param name="Msgs">Ҫ������ʾ����ʾ��Ϣ����</param>
        /// <param name="Url">Ҫת���URL</param>
        public static void Message(string[] Msgs, string Url)
        {
            Message Wm = new Message();

            foreach (string msg in Msgs) Wm.Add(msg);

            if (!string.IsNullOrEmpty(Url))
            {
                Wm.Jump = Url;
                Wm.Back = false;
            }

            HttpResponse Re = HttpContext.Current.Response;
            //��Ϣд��
            Re.Clear();
            Re.Write(Wm.ToString());
            Re.End();
        }


        /// <summary>
        /// ��ʾ��ʾ��Ϣ���ر������
        /// </summary>
        /// <param name="Msg">Ҫ��ʾ����Ϣ</param>
        public static void Close(string Msg)
        {
            MessageAlert.Close(new string[] { Msg });
        }

        /// <summary>
        /// ��ʾ��Ϣ�鲢�ر������
        /// </summary>
        /// <param name="Msgs">Ҫ��ʾ����Ϣ��</param>
        public static void Close(string[] Msgs)
        {
            Message Wm = new Message();

            foreach (string msg in Msgs) Wm.Add(msg);

            Wm.Back = false;
            Wm.Close = true;

            //��Ϣд��
            HttpResponse Re = HttpContext.Current.Response;
            Re.Clear();
            Re.Write(Wm.ToString());
            Re.End();
        }
    }
}
