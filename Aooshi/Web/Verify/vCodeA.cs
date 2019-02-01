using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Aooshi.Web.Verify
{
    /// <summary>
    /// ��֤���ʼ��
    /// </summary>
    public class vCodeA : vCodeBase
    {
        /// <summary>
        /// ����ͼ������
        /// </summary>
        /// <param name="code">��֤��</param>
        protected override void CreateImage(string code)
        {

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(base.Width, base.Height);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //�������������
                Random random = new Random();

                //���ͼƬ����ɫ
                g.Clear(Color.White);

                ////��ͼƬ�ı���������
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

                //��ͼƬ��ǰ��������
                for (int i = 0; i < 20; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //��ͼƬ�ı߿���
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
