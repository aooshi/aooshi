using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Aooshi.Images
{


    #region Mark
    /// <summary>
    /// �ϴ����ͼƬˮӡ
    /// </summary>
    public class UpFilesMark : ImageMark
    {
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="imgMark">ˮӡͼƬ</param>
        public UpFilesMark(string imgMark)
            : base(imgMark)
        {
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="txtMark">ˮӡ����</param>
        /// <param name="FontSize">���ִ�С</param>
        public UpFilesMark(string txtMark, int FontSize)
            : base(txtMark, FontSize)
        {
        }
    }

    #endregion

    #region min img
    /// <summary>
    /// �ļ��ϴ�����ͼ����
    /// </summary>
    public class UpFilesMin
    {
        int _w, _h;
        string _name, _filename;

        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="width">������</param>
        /// <param name="height">����߶�</param>
        /// <param name="MinPath">����ͼ·��,������������Զ���ԭ·�����ٽ���MinĿ¼��</param>
        public UpFilesMin(int width, int height, string MinPath)
            : this(width, height, MinPath, null)
        {
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="width">������</param>
        /// <param name="height">����߶�</param>
        /// <param name="MinPath">����ͼ·��,������������Զ���ԭ·�����ٽ���MinĿ¼��</param>
        /// <param name="filename">����ͼ����</param>
        public UpFilesMin(int width, int height, string MinPath, string filename)
        {
            _w = width;
            _h = height;
            _name = MinPath;
            _filename = filename;
        }

        /// <summary>
        /// ��ȡ�����ò�����׺������ͼ��,����������ԭͼһ��
        /// </summary>
        public string FileName
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// ��ȡ����������ͼ·��,������������Զ���ԭ·�����ٽ���MinĿ¼��
        /// </summary>
        public string MinPath
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// ��ȡ������ͼƬ����߶�
        /// </summary>
        public int Height
        {
            get { return _h; }
            set { _h = value; }
        }

        /// <summary>
        /// ��ȡ������ͼƬ������
        /// </summary>
        public int Width
        {
            get { return _w; }
            set { _w = value; }
        }
    }

    #endregion

    /// <summary>
    /// �ļ��ϴ����
    /// </summary>
    [Obsolete("�����Ѳ�����ʹ��.����ʹ�� DisposeImage �滻�����ͼƬ������")]
    public class UpFiles
    {
        /// <summary>
        /// ��ʼ����ʵ��
        /// </summary>
        public UpFiles() : this("", null) { }
        /// <summary>
        /// ��ʼ����ʵ��,��ָ���ļ��ϴ���������
        /// </summary>
        /// <param name="FileType">�ļ��ϴ���������</param>
        public UpFiles(string[] FileType)
            : this(null, FileType)
        {
        }
        /// <summary>
        /// ��ʼ����ʵ��,��ָ���ļ��ϴ�������Ŀ¼
        /// </summary>
        /// <param name="BaseDir">�ϴ��ļ�Ŀ¼(ע:�粻Ϊ����·��������Զ���������Ŀ¼ת��)</param>
        public UpFiles(string BaseDir)
            : this(BaseDir, null)
        {
        }
        /// <summary>
        /// ��ʼ����ʵ��,��ָ���ļ��ϴ�����
        /// </summary>
        /// <param name="BaseDir">�ϴ��ļ�Ŀ¼(ע:�粻Ϊ����·��������Զ���������Ŀ¼ת��)</param>
        /// <param name="FileType">�ϴ����ļ������޶�</param>
        public UpFiles(string BaseDir, string[] FileType)
        {
            this.BaseDir = BaseDir;
            this.FileType = FileType;
        }

        int _len = -1;
        /// <summary>
        /// �����ϴ��ļ��ĳ���
        /// </summary>
        public int Length
        {
            set { _len = value; }
        }

        string _baseDir;
        /// <summary>
        /// ��ȡ�������ļ��ϴ�·��
        /// </summary>
        public string BaseDir
        {
            get { return _baseDir; }
            set
            {
                _baseDir = value;
                if (!value.EndsWith("\\") && !value.EndsWith("/")) _baseDir += "\\";
            }
        }

        string[] _fileType;
        /// <summary>
        /// ��ȡ�������ļ��ϴ���������
        /// </summary>
        public string[] FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        string _Result;
        /// <summary>
        /// ��ȡ�����ϴ��Ľ��ֵ,����ɹ��򷵻�Ϊ�ļ�����,���ʧ���򷵻�Ϊʧ�ܵ�ԭ��
        /// </summary>
        public string Result
        {
            get { return _Result; }
            private set { _Result = value; }
        }


        bool _isToJpeg;
        /// <summary>
        /// ��ȡ�����õ�ǰUpImage����ʱ,�Ƿ��Զ����ļ�ת��ΪJpg��ʽ,Ĭ��Ϊfalse;
        /// </summary>
        public bool IsToJpeg
        {
            get { return _isToJpeg; }
            set { _isToJpeg = value; }
        }

        bool _isToGif;
        /// <summary>
        /// ��ȡ�����õ�ǰUpImage����ʱ,�Ƿ��Զ����ļ�ת��ΪGif��ʽ,Ĭ��Ϊfalse,���������IsToJpeg��������Ч;
        /// </summary>
        public bool IsToGif
        {
            get { return _isToGif; }
            set { _isToGif = value; }
        }

        UpFilesMark mark;
        UpFilesMark minmark;
        /// <summary>
        /// ��ȡ������ͼƬ�ϴ�ʱʹ�õ�ˮӡ
        /// </summary>
        public UpFilesMark ImgMark
        {
            get { return mark; }
            set { mark = value; }
        }

        /// <summary>
        /// ��ȡ������ͼƬ�ϴ�ʱ����ͼʹ�õ�ˮӡ
        /// </summary>
        public UpFilesMark MinImgMark
        {
            get { return minmark; }
            set { minmark = value; }
        }

        /// <summary>
        /// �����ϴ��ı���·��
        /// </summary>
        string tmpName;



        //private ColorPalette GetColorPalette()
        //{
        //    Bitmap bitmap = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
        //    ColorPalette palette = bitmap.Palette;
        //    bitmap.Dispose();
        //    return palette;
        //}

        //private Bitmap recolorGif(Image image)
        //{
        //    int nColors;
        //    int width, height;
        //    nColors = 16;
        //    width = image.Width;
        //    height = image.Height;

        //    Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
        //    ColorPalette pal = GetColorPalette();

        //    int i;

        //    for (i = 0; i < nColors; i++)
        //    {
        //        pal.Entries[i] = Color.FromArgb(255, 100, 100, 100);
        //    }

        //    pal.Entries[0] = Color.FromArgb(255, 0, 0, 0);
        //    pal.Entries[1] = Color.FromArgb(255, 255, 0, 0);
        //    pal.Entries[2] = Color.FromArgb(255, 0, 255, 0);
        //    pal.Entries[3] = Color.FromArgb(255, 0, 0, 255);
        //    pal.Entries[4] = Color.FromArgb(255, 204, 204, 204);
        //    pal.Entries[nColors - 1] = Color.FromArgb(0, 255, 255, 255);

        //    bitmap.Palette = pal;

        //    Bitmap bmpcopy = new Bitmap(width, height, PixelFormat.Format32bppArgb);

        //    Graphics g;
        //    g = Graphics.FromImage(bmpcopy);

        //    g.PageUnit = GraphicsUnit.Pixel;

        //    g.DrawImage(image, 0, 0, width, height);
        //    g.Dispose();

        //    Rectangle rect;
        //    rect = new Rectangle(0, 0, width, height);

        //    BitmapData bitmapdata;
        //    bitmapdata = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

        //    IntPtr pixels;
        //    pixels = bitmapdata.Scan0;
        //    Int32 pbits;
        //    if (bitmapdata.Stride > 0)
        //        pbits = pixels.ToInt32();
        //    else
        //        pbits = pixels.ToInt32() + bitmapdata.Stride * (height - 1);

        //    int stride;
        //    stride = Math.Abs(bitmapdata.Stride);

        //    Byte[] bits;
        //    bits = new Byte[height * stride];

        //    Color pixel;
        //    int i8BppPixel;
        //    double colorindex;

        //    int row, col;
        //    for (row = 0; row <= height - 1; row++)
        //        for (col = 0; col <= width - 1; col++)
        //        {
        //            i8BppPixel = row * stride + col;
        //            pixel = bmpcopy.GetPixel(col, row);

        //            if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
        //                colorindex = 0;
        //            else if (pixel.R > 100 && pixel.G == 0 && pixel.B == 0)
        //                colorindex = 1;
        //            else if (pixel.R == 0 && pixel.G > 100 && pixel.B == 0)
        //                colorindex = 2;
        //            else if (pixel.R == 0 && pixel.G == 0 && pixel.B > 100)
        //                colorindex = 3;
        //            else if (pixel.R == 204 && pixel.G == 204 && pixel.B == 204)
        //                colorindex = 4;
        //            else
        //                colorindex = nColors - 1;

        //            bits[i8BppPixel] = Byte.Parse(colorindex.ToString());
        //        }

        //    CopyArrayTo(pbits, bits, height * stride);
        //    bitmap.UnlockBits(bitmapdata);
        //    return bitmap;
        //}


        //[DllImport("KERNEL32.DLL",EntryPoint="RtlMoveMemory",SetLastError=true,CharSet=System.Runtime.InteropServices.CharSet.Auto
        //    ,ExactSpelling=true,CallingConvention=System.Runtime.InteropServices.CallingConvention.StdCall)]
        //static extern void CopyArrayTo([In, MarshalAs(UnmanagedType.I4)] Int32 hpvDest, [In, Out] byte[] hpvSource, int cbCopy);



        /// <summary>
        /// �ϴ�ָ�����ļ�,�������Ƿ��ϴ��ɹ�!
        /// </summary>
        /// <param name="PostFile">�ļ�����</param>
        public bool UpImage(HttpPostedFile PostFile)
        {
            return UpImage(PostFile, null, -1, -1, null);
        }

        /// <summary>
        /// �ϴ�ָ�����ļ�,�������Ƿ��ϴ��ɹ�!
        /// </summary>
        /// <param name="PostFile">�ļ�����</param>
        /// <param name="width">ͼ�Ŀ������</param>
        /// <param name="height">ͼ�ĸ߶�����,��Ϊ-1ʱ���Զ������</param>
        public bool UpImage(HttpPostedFile PostFile, int width, int height)
        {
            return UpImage(PostFile, null, width, height, null);
        }

        /// <summary>
        /// �ϴ�ָ�����ļ�,�������Ƿ��ϴ��ɹ�!
        /// </summary>
        /// <param name="PostFile">�ļ�����</param>
        /// <param name="width">ͼ�Ŀ������</param>
        /// <param name="height">ͼ�ĸ߶�����,��Ϊ-1ʱ���Զ������</param>
        /// <param name="min">����ͼ·��,������������Զ���ԭ·�����ٽ���MinĿ¼��</param>
        public bool UpImage(HttpPostedFile PostFile, int width, int height, UpFilesMin min)
        {
            return UpImage(PostFile, null, width, height, min);
        }

        /// <summary>
        /// �ϴ�ָ�����ļ�,�������Ƿ��ϴ��ɹ�!
        /// </summary>
        /// <param name="PostFile">�ļ�����</param>
        /// <param name="min">����ͼ·��,������������Զ���ԭ·�����ٽ���MinĿ¼��</param>
        public bool UpImage(HttpPostedFile PostFile, UpFilesMin min)
        {
            return UpImage(PostFile, null, -1, -1, min);
        }

        /// <summary>
        /// �ϴ�ָ�����ļ�,�������Ƿ��ϴ��ɹ�!
        /// </summary>
        /// <param name="PostFile">�ļ�����</param>
        /// <param name="FileName">�ļ�����,������������Զ�����һ��</param>
        /// <param name="width">ͼ�Ŀ������</param>
        /// <param name="height">ͼ�ĸ߶�����,��Ϊ-1ʱ���Զ������</param>
        public bool UpImage(HttpPostedFile PostFile, string FileName, int width, int height)
        {
            return UpImage(PostFile, FileName, width, height, null);
        }

        /// <summary>
        /// �ϴ�ָ�����ļ�,�������Ƿ��ϴ��ɹ�!
        /// </summary>
        /// <param name="PostFile">�ļ�����</param>
        /// <param name="FileName">�ļ�����,������������Զ�����һ��</param>
        /// <param name="min">����ͼ·��,������������Զ���ԭ·�����ٽ���MinĿ¼��</param>
        public bool UpImage(HttpPostedFile PostFile, string FileName, UpFilesMin min)
        {
            return UpImage(PostFile, FileName, -1, -1, min);
        }

        /// <summary>
        /// �ϴ�ָ�����ļ�,�������Ƿ��ϴ��ɹ�!
        /// </summary>
        /// <param name="PostFile">�ļ�����</param>
        /// <param name="FileName">�ļ�����,������������Զ�����һ��</param>
        public bool UpImage(HttpPostedFile PostFile, string FileName)
        {
            return UpImage(PostFile, FileName, -1, -1, null);
        }

        /// <summary>
        /// �ϴ�ָ�����ļ�,�������Ƿ��ϴ��ɹ�!
        /// </summary>
        /// <param name="PostFile">�ļ�����</param>
        /// <param name="FileName">�ļ�����,������������Զ�����һ��</param>
        /// <param name="width">�ϴ�ͼƬ�Ŀ������</param>
        /// <param name="height">�ϴ�ͼƬ�ĸ߶�����,��Ϊ-1ʱ���Զ������</param>
        /// <param name="min">����ͼ����,�������Ҫ������Ϊnull;</param>
        public bool UpImage(HttpPostedFile PostFile, string FileName, int width, int height, UpFilesMin min)
        {
            if (!UpFile(PostFile, FileName)) return false;

            //string tmpDir, tmpFile, tmp;
            //tmpFile = Path.GetFileNameWithoutExtension(tmpName);
            string tmpDir, tmp;
            tmpDir = Path.GetDirectoryName(tmpName);
            ImageFormat imgformat;
            
            Image img = null;
            try
            {
                //����Ϊjpgʱ��ͼƬת��Ϊjpg
                if (IsToJpeg && !tmpName.ToLower().EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    tmpName = DisposeImage.ConvertFormat(tmpName, ImageFormat.Jpeg);
                    this.Result = Path.GetFileName(tmpName);
                }
                else if (IsToGif && !tmpName.ToLower().EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    tmpName = DisposeImage.ConvertFormat(tmpName, ImageFormat.Gif);
                    this.Result = Path.GetFileName(tmpName);
                }


                //��ͼ�������
                if (width > 0)
                {
                   new DisposeImage(tmpName).CompressImage(width,height);
                }
                //����ͼ
                if (min != null)
                {
                    if (string.IsNullOrEmpty(min.MinPath)) min.MinPath = tmpDir += "\\Min";
                    if (!Directory.Exists(min.MinPath))
                    {
                        Directory.CreateDirectory(min.MinPath);
                        //this.Result = string.Format("����ͼĿ¼{0}������!", min.MinPath);
                        //return false;
                    }

                    img = Image.FromFile(tmpName);//�������ļ�
                    imgformat = img.RawFormat;
                    Image imgB = img.GetThumbnailImage(min.Width, min.Height, null, IntPtr.Zero);
                    if (string.IsNullOrEmpty(min.FileName))
                        tmp = string.Format("{0}\\{1}", min.MinPath, Path.GetFileName(tmpName));
                    else
                        tmp = string.Format("{0}\\{1}", min.MinPath, min.FileName + Path.GetExtension(tmpName));
                    img.Dispose();
                    imgB.Save(tmp, imgformat);
                    imgB.Dispose();
                    if (MinImgMark != null) //ˮӡ
                    {
                        if (MinImgMark.Create(tmp)) throw new AooshiException(" Create Mark Error : " + MinImgMark.ErrorMessage);
                    }
                }

                //ˮӡ
                if (ImgMark != null)
                {
                    //ImgMark.FileNamePath = tmpName;
                    //if (!ImgMark.Create()) throw new Exception(ImgMark.ErrorMessage);

                    if (ImgMark.Create(tmpName)) throw new AooshiException(" Create Mark Error : " + ImgMark.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                this.Result = ex.Message;
                return false;
            }
            finally
            {
                if (img != null)
                    img.Dispose();
            }

            return true;
        }


        /// <summary>
        /// �ϴ�ָ�����ļ�,�������Ƿ��ϴ��ɹ�!
        /// </summary>
        /// <param name="PostFile">�ļ�����</param>
        public bool UpFile(HttpPostedFile PostFile)
        {
            return UpFile(PostFile, null);
        }

        /// <summary>
        /// �ϴ�ָ�����ļ�,�������Ƿ��ϴ��ɹ�!
        /// </summary>
        /// <param name="PostFile">�ļ�����</param>
        /// <param name="FileName">�ļ�����,������׺,������������Զ�����һ��</param>
        public bool UpFile(HttpPostedFile PostFile, string FileName)
        {
            if (string.IsNullOrEmpty(FileName)) FileName = RandomString.CreateOnlyID();// MyRand.CreateRanID();
            if (PostFile == null || PostFile.FileName.Trim() == "")
            {
                this.Result = "�ϴ�����Ϊ��!";
                return false;
            }
            if (!CheckFileType(PostFile.FileName.Trim()))
            {
                this.Result = "�ϴ��ļ����Ͳ�ƥ��!";
                return false;
            }
            if (_len > -1 && PostFile.ContentLength > _len)
            {
                this.Result = "�ϴ����ļ�����!";
                return false;
            }
            tmpName = string.IsNullOrEmpty(this.BaseDir) ? FileName : this.BaseDir + FileName;
            tmpName += Path.GetExtension(PostFile.FileName);
            try
            {
                if (!Verify.IsHard(tmpName))  //��ǰ׹��Ϊ�̷�ʱ�Զ�ת������·��
                    tmpName = HttpContext.Current.Server.MapPath(tmpName);
                if (!Directory.Exists(tmpName.Replace(Path.GetFileName(tmpName), "")))
                {
                    this.Result = string.Format("ָ�����ϴ�Ŀ¼'{0}'������!", tmpName.Replace(Path.GetFileName(tmpName), ""));
                    return false;
                }

                PostFile.SaveAs(tmpName);
            }
            catch (Exception ex)
            {
                this.Result = ex.Message;
                return false;
            }

            this.Result = Path.GetFileName(tmpName);
            return true;
        }

        /// <summary>
        /// ��֤�Ƿ�Ϊ�Ϸ��ļ�����
        /// </summary>
        /// <param name="fileSuff">�ļ�����</param>
        public bool CheckFileType(string fileSuff)
        {
            if (this.FileType == null || FileType.Length == 0) return true;
            fileSuff = Path.GetExtension(fileSuff.ToLower());
            foreach (string suff in FileType)
            {
                if (suff.ToLower() == fileSuff) return true;
            }
            return false;
        }
    }
}
