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
    /// 验证码基类
    /// </summary>
    public class vCodeBase : System.Web.UI.Page
    {
        /// <summary>
        /// 重载初始
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
        /// 获取或设置图片宽度
        /// </summary>
        protected int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        /// <summary>
        /// 获取或设置图片高度
        /// </summary>
        protected int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        /// <summary>
        /// 获取或设置图片位数
        /// </summary>
        protected int Size
        {
            get { return _size; }
            set { _size = value; }
        }
        /// <summary>
        /// 获取或设置失效分钟数
        /// </summary>
        protected int Tim
        {
            get { return _tim; }
            set { _tim = value; }
        }

        /// <summary>
        /// 已重写初始化方法
        /// </summary>
        /// <param name="e">对象</param>
        protected override void OnPreRender(EventArgs e)
        {
            CreateImage(AddCache());
            base.OnPreRender(e);
        }

        /// <summary>
        /// 创建缓存值
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
        /// 创建图片数据
        /// </summary>
        /// <param name="code">验证码</param>
        protected virtual void CreateImage(string code)
        {
            int w = Width, h = Height;
            //gheight为图片宽度,根据字符长度自动更改图片宽度 
            Bitmap img = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(img);
            g.FillRectangle(new SolidBrush(Color.Bisque), new Rectangle(0, 0, w, h));
            Font ft = new Font("Arial", 13, (FontStyle.Bold));
            g.DrawString(code, ft, new SolidBrush(Color.Black), 2, 1);

            //在矩形内绘制字串（字串，字体，画笔颜色，左上x.左上y） 
            Response.ClearContent(); //需要输出图象信息 要修改HTTP头 
            Response.ContentType = "image/Gif";
            img.Save(Response.OutputStream, ImageFormat.Gif);
            g.Dispose();
            img.Dispose();
            Response.End();
        }

        /// <summary>
        /// 创建随机字符串
        /// </summary>
        protected virtual string CreateString()
        {
            return RandomString.CreateNumber(Size);
        }
    }
}
