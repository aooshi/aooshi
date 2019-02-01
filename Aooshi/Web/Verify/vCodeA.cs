using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Aooshi.Web.Verify
{
    /// <summary>
    /// 验证码初始化
    /// </summary>
    public class vCodeA : vCodeBase
    {
        /// <summary>
        /// 重载图像生成
        /// </summary>
        /// <param name="code">验证码</param>
        protected override void CreateImage(string code)
        {

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(base.Width, base.Height);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.White);

                ////画图片的背景噪音线
                //for (int i = 0; i < 25; i++)
                //{
                //    int x1 = random.Next(image.Width);
                //    int x2 = random.Next(image.Width);
                //    int y1 = random.Next(image.Height);
                //    int y2 = random.Next(image.Height);

                //    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                //}

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(code, font, brush, 2, 2);

                //画图片的前景噪音点
                for (int i = 0; i < 20; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                Response.ClearContent();
                Response.ContentType = "image/Gif";
                Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }
}
