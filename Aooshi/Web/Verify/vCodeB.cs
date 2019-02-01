using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Aooshi.Web.Verify
{
    /// <summary>
    /// 基本类型
    /// </summary>
    public class vCodeB : vCodeBase
    {
        /// <summary>
        /// 画笔
        /// </summary>
        Brush[] BrushItems = new Brush[] {     Brushes.OliveDrab,
                                                      Brushes.ForestGreen,
                                                      Brushes.DarkCyan,
                                                      Brushes.LightSlateGray,
                                                      Brushes.RoyalBlue,
                                                      Brushes.SlateBlue,
                                                      Brushes.DarkViolet,
                                                      Brushes.MediumVioletRed,
                                                      Brushes.IndianRed,
                                                      Brushes.Firebrick,
                                                      Brushes.Chocolate,
                                                      Brushes.Peru,
                                                      Brushes.Goldenrod
        };

        /// <summary>
        /// 基本类型
        /// </summary>
        /// <param name="code">验证码</param>
        protected override void CreateImage(string code)
        {
            Bitmap objBitmap = null;
            Graphics g = null;
            Random rand = new Random();
            try
            {
                objBitmap = new Bitmap(Width, Height);
                g = Graphics.FromImage(objBitmap);
                g.Clear(Color.White);  //绘画背景颜色
                g.DrawString(code, new Font("Arial", 12, FontStyle.Bold), BrushItems[rand.Next(0, BrushItems.Length)], 3, 1);  //绘画文字
                //// 绘画文字噪音点
                for (int n = 0; n < 10; n++)
                {
                    int x = rand.Next(Width);
                    int y = rand.Next(Height);
                    objBitmap.SetPixel(x, y, Color.Black);
                }
                // 绘画边框
                g.DrawRectangle(Pens.DarkGray, 0, 0, Width - 1, Height - 1);
                objBitmap.Save(Response.OutputStream, ImageFormat.Gif);
                Response.ContentType = "image/gif";
            }
            finally
            {
                if (null != objBitmap)
                    objBitmap.Dispose();
                if (null != g)
                    g.Dispose();
            }
        }
    }
}
