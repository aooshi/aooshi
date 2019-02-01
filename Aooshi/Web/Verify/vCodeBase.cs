using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Web;
using System.Web.Caching;

namespace Aooshi.Web.Verify
{
    /// <summary>
    /// ��֤�����
    /// </summary>
    public class vCodeBase : System.Web.UI.Page
    {
        /// <summary>
        /// ���س�ʼ
        /// </summary>
        protected override void Construct()
        {
            base.Construct();
            Width = 50;
            Height = 22;
            Size = 4;
            Tim = 5;
        }

        int _width, _height, _size, _tim;
        /// <summary>
        /// ��ȡ������ͼƬ���
        /// </summary>
        protected int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        /// <summary>
        /// ��ȡ������ͼƬ�߶�
        /// </summary>
        protected int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        /// <summary>
        /// ��ȡ������ͼƬλ��
        /// </summary>
        protected int Size
        {
            get { return _size; }
            set { _size = value; }
        }
        /// <summary>
        /// ��ȡ������ʧЧ������
        /// </summary>
        protected int Tim
        {
            get { return _tim; }
            set { _tim = value; }
        }

        /// <summary>
        /// ����д��ʼ������
        /// </summary>
        /// <param name="e">����</param>
        protected override void OnPreRender(EventArgs e)
        {
            CreateImage(AddCache());
            base.OnPreRender(e);
        }

        /// <summary>
        /// ��������ֵ
        /// </summary>
        private string AddCache()
        {
            HttpCookie cook = Request.Cookies[CodeUtils.CacheName];
            //get code
            string code = CreateString() ;
            if (cook == null || cook.Value == "")
            {
                cook = new HttpCookie(CodeUtils.CacheName);
                cook.Value = CodeUtils.Seed;
                Response.AppendCookie(cook);
            }
            if (base.Cache[cook.Value] != null)
            {
                base.Cache[cook.Value] = code;
                return code;
            }
            base.Cache.Add(cook.Value, code, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(Tim), CacheItemPriority.Default, null);
            return code;
        }

        /// <summary>
        /// ����ͼƬ����
        /// </summary>
        /// <param name="code">��֤��</param>
        protected virtual void CreateImage(string code)
        {
            int w = Width, h = Height;
            //gheightΪͼƬ���,�����ַ������Զ�����ͼƬ��� 
            Bitmap img = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(img);
            g.FillRectangle(new SolidBrush(Color.Bisque), new Rectangle(0, 0, w, h));
            Font ft = new Font("Arial", 13, (FontStyle.Bold));
            g.DrawString(code, ft, new SolidBrush(Color.Black), 2, 1);

            //�ھ����ڻ����ִ����ִ������壬������ɫ������x.����y�� 
            Response.ClearContent(); //��Ҫ���ͼ����Ϣ Ҫ�޸�HTTPͷ 
            Response.ContentType = "image/Gif";
            img.Save(Response.OutputStream, ImageFormat.Gif);
            g.Dispose();
            img.Dispose();
            Response.End();
        }

        /// <summary>
        /// ��������ַ���
        /// </summary>
        protected virtual string CreateString()
        {
            return RandomString.CreateNumber(Size);
        }
    }
}
