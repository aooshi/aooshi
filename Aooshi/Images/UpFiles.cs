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
    /// 上传组件图片水印
    /// </summary>
    public class UpFilesMark : ImageMark
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="imgMark">水印图片</param>
        public UpFilesMark(string imgMark)
            : base(imgMark)
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="txtMark">水印文字</param>
        /// <param name="FontSize">文字大小</param>
        public UpFilesMark(string txtMark, int FontSize)
            : base(txtMark, FontSize)
        {
        }
    }

    #endregion

    #region min img
    /// <summary>
    /// 文件上传缩略图对象
    /// </summary>
    public class UpFilesMin
    {
        int _w, _h;
        string _name, _filename;

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="width">对象宽度</param>
        /// <param name="height">对象高度</param>
        /// <param name="MinPath">缩略图路径,如果不设置则自动在原路径上再进入Min目录层</param>
        public UpFilesMin(int width, int height, string MinPath)
            : this(width, height, MinPath, null)
        {
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="width">对象宽度</param>
        /// <param name="height">对象高度</param>
        /// <param name="MinPath">缩略图路径,如果不设置则自动在原路径上再进入Min目录层</param>
        /// <param name="filename">缩略图名称</param>
        public UpFilesMin(int width, int height, string MinPath, string filename)
        {
            _w = width;
            _h = height;
            _name = MinPath;
            _filename = filename;
        }

        /// <summary>
        /// 获取或设置不带后缀的缩略图名,不设置则与原图一至
        /// </summary>
        public string FileName
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// 获取或设置缩略图路径,如果不设置则自动在原路径上再进入Min目录层
        /// </summary>
        public string MinPath
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 获取或设置图片对象高度
        /// </summary>
        public int Height
        {
            get { return _h; }
            set { _h = value; }
        }

        /// <summary>
        /// 获取或设置图片对象宽度
        /// </summary>
        public int Width
        {
            get { return _w; }
            set { _w = value; }
        }
    }

    #endregion

    /// <summary>
    /// 文件上传组件
    /// </summary>
    [Obsolete("该类已不建议使用.可以使用 DisposeImage 替换该类的图片处理函数")]
    public class UpFiles
    {
        /// <summary>
        /// 初始化新实例
        /// </summary>
        public UpFiles() : this("", null) { }
        /// <summary>
        /// 初始化新实例,并指定文件上传限制类型
        /// </summary>
        /// <param name="FileType">文件上传类型限制</param>
        public UpFiles(string[] FileType)
            : this(null, FileType)
        {
        }
        /// <summary>
        /// 初始化新实例,并指定文件上传类型与目录
        /// </summary>
        /// <param name="BaseDir">上传文件目录(注:如不为绝对路径组件将自动进行虚拟目录转换)</param>
        public UpFiles(string BaseDir)
            : this(BaseDir, null)
        {
        }
        /// <summary>
        /// 初始化新实例,并指定文件上传类型
        /// </summary>
        /// <param name="BaseDir">上传文件目录(注:如不为绝对路径组件将自动进行虚拟目录转换)</param>
        /// <param name="FileType">上传的文件类型限定</param>
        public UpFiles(string BaseDir, string[] FileType)
        {
            this.BaseDir = BaseDir;
            this.FileType = FileType;
        }

        int _len = -1;
        /// <summary>
        /// 设置上传文件的长度
        /// </summary>
        public int Length
        {
            set { _len = value; }
        }

        string _baseDir;
        /// <summary>
        /// 获取或设置文件上传路径
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
        /// 获取或设置文件上传限制类型
        /// </summary>
        public string[] FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        string _Result;
        /// <summary>
        /// 获取本次上传的结果值,如果成功则返回为文件名称,如果失败则返回为失败的原因
        /// </summary>
        public string Result
        {
            get { return _Result; }
            private set { _Result = value; }
        }


        bool _isToJpeg;
        /// <summary>
        /// 获取或设置当前UpImage方法时,是否自动将文件转换为Jpg格式,默认为false;
        /// </summary>
        public bool IsToJpeg
        {
            get { return _isToJpeg; }
            set { _isToJpeg = value; }
        }

        bool _isToGif;
        /// <summary>
        /// 获取或设置当前UpImage方法时,是否自动将文件转换为Gif格式,默认为false,如果设置了IsToJpeg此设置无效;
        /// </summary>
        public bool IsToGif
        {
            get { return _isToGif; }
            set { _isToGif = value; }
        }

        UpFilesMark mark;
        UpFilesMark minmark;
        /// <summary>
        /// 获取或设置图片上传时使用的水印
        /// </summary>
        public UpFilesMark ImgMark
        {
            get { return mark; }
            set { mark = value; }
        }

        /// <summary>
        /// 获取或设置图片上传时略缩图使用的水印
        /// </summary>
        public UpFilesMark MinImgMark
        {
            get { return minmark; }
            set { minmark = value; }
        }

        /// <summary>
        /// 本次上传的本地路径
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
        /// 上传指定的文件,并返回是否上传成功!
        /// </summary>
        /// <param name="PostFile">文件对象</param>
        public bool UpImage(HttpPostedFile PostFile)
        {
            return UpImage(PostFile, null, -1, -1, null);
        }

        /// <summary>
        /// 上传指定的文件,并返回是否上传成功!
        /// </summary>
        /// <param name="PostFile">文件对象</param>
        /// <param name="width">图的宽度限制</param>
        /// <param name="height">图的高度限制,当为-1时则自动计算高</param>
        public bool UpImage(HttpPostedFile PostFile, int width, int height)
        {
            return UpImage(PostFile, null, width, height, null);
        }

        /// <summary>
        /// 上传指定的文件,并返回是否上传成功!
        /// </summary>
        /// <param name="PostFile">文件对象</param>
        /// <param name="width">图的宽度限制</param>
        /// <param name="height">图的高度限制,当为-1时则自动计算高</param>
        /// <param name="min">缩略图路径,如果不设置则自动在原路径上再进入Min目录层</param>
        public bool UpImage(HttpPostedFile PostFile, int width, int height, UpFilesMin min)
        {
            return UpImage(PostFile, null, width, height, min);
        }

        /// <summary>
        /// 上传指定的文件,并返回是否上传成功!
        /// </summary>
        /// <param name="PostFile">文件对象</param>
        /// <param name="min">缩略图路径,如果不设置则自动在原路径上再进入Min目录层</param>
        public bool UpImage(HttpPostedFile PostFile, UpFilesMin min)
        {
            return UpImage(PostFile, null, -1, -1, min);
        }

        /// <summary>
        /// 上传指定的文件,并返回是否上传成功!
        /// </summary>
        /// <param name="PostFile">文件对象</param>
        /// <param name="FileName">文件名称,如果不设置则自动分配一个</param>
        /// <param name="width">图的宽度限制</param>
        /// <param name="height">图的高度限制,当为-1时则自动计算高</param>
        public bool UpImage(HttpPostedFile PostFile, string FileName, int width, int height)
        {
            return UpImage(PostFile, FileName, width, height, null);
        }

        /// <summary>
        /// 上传指定的文件,并返回是否上传成功!
        /// </summary>
        /// <param name="PostFile">文件对象</param>
        /// <param name="FileName">文件名称,如果不设置则自动分配一个</param>
        /// <param name="min">缩略图路径,如果不设置则自动在原路径上再进入Min目录层</param>
        public bool UpImage(HttpPostedFile PostFile, string FileName, UpFilesMin min)
        {
            return UpImage(PostFile, FileName, -1, -1, min);
        }

        /// <summary>
        /// 上传指定的文件,并返回是否上传成功!
        /// </summary>
        /// <param name="PostFile">文件对象</param>
        /// <param name="FileName">文件名称,如果不设置则自动分配一个</param>
        public bool UpImage(HttpPostedFile PostFile, string FileName)
        {
            return UpImage(PostFile, FileName, -1, -1, null);
        }

        /// <summary>
        /// 上传指定的文件,并返回是否上传成功!
        /// </summary>
        /// <param name="PostFile">文件对象</param>
        /// <param name="FileName">文件名称,如果不设置则自动分配一个</param>
        /// <param name="width">上传图片的宽度限制</param>
        /// <param name="height">上传图片的高度限制,当为-1时则自动计算高</param>
        /// <param name="min">缩略图对象,如果不须要请设置为null;</param>
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
                //当不为jpg时将图片转换为jpg
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


                //大图宽高限制
                if (width > 0)
                {
                   new DisposeImage(tmpName).CompressImage(width,height);
                }
                //缩略图
                if (min != null)
                {
                    if (string.IsNullOrEmpty(min.MinPath)) min.MinPath = tmpDir += "\\Min";
                    if (!Directory.Exists(min.MinPath))
                    {
                        Directory.CreateDirectory(min.MinPath);
                        //this.Result = string.Format("缩略图目录{0}不存在!", min.MinPath);
                        //return false;
                    }

                    img = Image.FromFile(tmpName);//打开现在文件
                    imgformat = img.RawFormat;
                    Image imgB = img.GetThumbnailImage(min.Width, min.Height, null, IntPtr.Zero);
                    if (string.IsNullOrEmpty(min.FileName))
                        tmp = string.Format("{0}\\{1}", min.MinPath, Path.GetFileName(tmpName));
                    else
                        tmp = string.Format("{0}\\{1}", min.MinPath, min.FileName + Path.GetExtension(tmpName));
                    img.Dispose();
                    imgB.Save(tmp, imgformat);
                    imgB.Dispose();
                    if (MinImgMark != null) //水印
                    {
                        if (MinImgMark.Create(tmp)) throw new AooshiException(" Create Mark Error : " + MinImgMark.ErrorMessage);
                    }
                }

                //水印
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
        /// 上传指定的文件,并返回是否上传成功!
        /// </summary>
        /// <param name="PostFile">文件对象</param>
        public bool UpFile(HttpPostedFile PostFile)
        {
            return UpFile(PostFile, null);
        }

        /// <summary>
        /// 上传指定的文件,并返回是否上传成功!
        /// </summary>
        /// <param name="PostFile">文件对象</param>
        /// <param name="FileName">文件名称,不含后缀,如果不设置则自动分配一个</param>
        public bool UpFile(HttpPostedFile PostFile, string FileName)
        {
            if (string.IsNullOrEmpty(FileName)) FileName = RandomString.CreateOnlyID();// MyRand.CreateRanID();
            if (PostFile == null || PostFile.FileName.Trim() == "")
            {
                this.Result = "上传数据为空!";
                return false;
            }
            if (!CheckFileType(PostFile.FileName.Trim()))
            {
                this.Result = "上传文件类型不匹配!";
                return false;
            }
            if (_len > -1 && PostFile.ContentLength > _len)
            {
                this.Result = "上传的文件过大!";
                return false;
            }
            tmpName = string.IsNullOrEmpty(this.BaseDir) ? FileName : this.BaseDir + FileName;
            tmpName += Path.GetExtension(PostFile.FileName);
            try
            {
                if (!Verify.IsHard(tmpName))  //当前坠不为盘符时自动转换虚拟路径
                    tmpName = HttpContext.Current.Server.MapPath(tmpName);
                if (!Directory.Exists(tmpName.Replace(Path.GetFileName(tmpName), "")))
                {
                    this.Result = string.Format("指定的上传目录'{0}'不存在!", tmpName.Replace(Path.GetFileName(tmpName), ""));
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
        /// 验证是否为合法文件类型
        /// </summary>
        /// <param name="fileSuff">文件名称</param>
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
