using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi
{
    /// <summary>
    /// 通用随机数处理类
    /// 所有方法均为静态方法
    /// </summary>
    public class RandomString
    {
        /// <summary>
        /// 创建一个随机数，随机数据根据传入值与长度决定结果
        /// </summary>
        /// <param name="Chars">随机数的字符数组</param>
        /// <param name="Size">随机字符串长度</param>
        /// <returns>返回生成的随机数</returns>
        internal static string Create(char[] Chars, int Size)
        {
            int cl = Chars.Length - 1;  //减一防止数据超限

            string num = "";

            Random rand = new Random(); ;

            for (int i = 0; i < Size; i++)
                num += Chars[rand.Next(0, cl)];

            return num;
        }

        /// <summary>
        /// 生成指定位数的随机0-9数字字符串
        /// </summary>
        /// <param name="Size">随机字符串长度</param>
        /// <returns>返回生成的字符串</returns>
        public static string CreateNumber(int Size)
        {
            return RandomString.Create(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, Size);
        }

        /// <summary>
        /// 生成一个小写字母与数字随机字符串
        /// </summary>
        /// <param name="Size">随机字符串长度</param>
        /// <returns>返回生成的字符串</returns>
        public static string CreateNMinLetter(int Size)
        {
            char[] charArray = new char[]{'0','1', '2','3','4','5','6','7','8','9'
										   ,'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q'
										   ,'r','s','t','u','v','w','x','y','z'};
            return RandomString.Create(charArray, Size);
        }

        /// <summary>
        /// 生成一个大小写字母与数字随机字符串
        /// </summary>
        /// <param name="Size">随机字符串长度</param>
        /// <returns>返回生成的字符串</returns>
        public static string CreateNMaxLetter(int Size)
        {
            char[] charArray = new char[]{'0','1', '2','3','4','5','6','7','8','9'
										   ,'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q'
										   ,'r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H'
										   ,'I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};

            return RandomString.Create(charArray, Size);
        }

        /// <summary>
        /// 生成一个小写字母组成的随机字符串
        /// </summary>
        /// <param name="Size">随机字符串长度</param>
        /// <returns>返回生成的字符串</returns>
        public static string CreateLetter(int Size)
        {
            char[] charArray = new char[]{'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q'
										   ,'r','s','t','u','v','w','x','y','z'};

            return RandomString.Create(charArray, Size);
        }

        /// <summary>
        /// 生成一个大小写字母组成的随机字符串
        /// </summary>
        /// <param name="Size">随机字符串长度</param>
        /// <returns>返回生成的字符串</returns>
        public static string CreateMaxLetter(int Size)
        {
            char[] charArray = new char[]{'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q'
										   ,'r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H'
										   ,'I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};

            return RandomString.Create(charArray, Size);
        }

        static TmpRand tmpRand = new TmpRand();
        static string tmpString = "";

        /// <summary>
        /// 产生一组2000年至2700年之间随时间变化且不会重复的11位随机串(注:每秒产生数单机不超过89个,如不同进程中同时运行,在单秒内有可能出现重复制)
        /// </summary>
        public static string CreateOnlyID()
        {
            //生成方式
            // 年份:    4
            // 月份:    2
            // 日期:    2
            // 时分:    4
            // 秒:      2

            // 叠加:    2 

            string tmp;
            int num;

            lock (tmpRand)
            {
                num = tmpRand.num;
                tmpRand.add();

                tmp = string.Format("yyyyMMddHHmmss{0}", num);
                tmp = DateTime.Now.ToString(tmp);

                if (num == TmpRand.Min)
                {
                    //循环值,使其每秒不出现重复值
                    while (tmp == tmpString)
                    {
                        System.Threading.Thread.Sleep(1); //等待1毫秒
                        tmp = string.Format("yyyyMMddHHmmss{0}", num);
                        tmp = DateTime.Now.ToString(tmp);
                    }
                    tmpString = tmp;//重置基点
                }

            }
            return Hex36( long.Parse( tmp ) );
            //return long.Parse(tmp).ToString("X");
        }

        //得到当前种子值
        private static long _createid = DateTime.Now.Ticks;
        /// <summary>
        /// 生成一个当前进程不可重复ID值;
        /// </summary>
        public static string CreateCourseID()
        {
            return Hex36(_createid++);
        }


        /// <summary>
        /// 创建一个随Unix时间截变化的定单ID，单进程内保证绝对不重复，不同进程相同时间有可能重复,重复时间级别(秒)。
        /// </summary>
        public static int CreateTimeID()
        {
            return _order.NewID();
        }

        static TimeID _order = new TimeID();
        private class TimeID
        {
            int id;

            /// <summary>
            /// 创建一个新ID
            /// </summary>
            public int NewID()
            {
                int n;
                lock (this)
                {
                    n = Aooshi.Common.DateTimeToUnixTimestamp();
                    while (n == id)
                    {
                        System.Threading.Thread.Sleep(100);
                        n = Aooshi.Common.DateTimeToUnixTimestamp();
                    }
                    id = n;
                }
                return n;
            }
        }

        ///// <summary>
        ///// 生成一组不重复随机数(长度15位),当循环小于秒时每秒产生89个数据;
        ///// </summary>
        ///// <returns>返回生成的随机值</returns>
        //public static string CreateRanID()
        //{
        //    //生成方式
        //    // 年份:    3
        //    // 月份:    2
        //    // 日期:    2
        //    // 时分:    4
        //    // 秒:      2
        //    //      ==  13
        //    // 叠加:    2 

        //    string tmp;
        //    int num;

        //    lock (tmpRand)
        //    {
        //        num = tmpRand.num;
        //        tmpRand.add();

        //        tmp = string.Format("yyyyMMddHHmmss{0}", num);
        //        tmp = DateTime.Now.ToString(tmp).Substring(2);

        //        if (num == TmpRand.Min)
        //        {
        //            //循环值,使其每秒不出现重复值
        //            while (tmp == tmpString)
        //            {
        //                System.Threading.Thread.Sleep(1); //等待1毫秒
        //                tmp = string.Format("yyyyMMddHHmmss{0}", num);
        //                tmp = DateTime.Now.ToString(tmp).Substring(2);
        //            }
        //            tmpString = tmp;//重置基点
        //        }

        //    }
        //    return long.Parse(tmp).ToString("X");
        //}

        /// <summary>
        /// 根据时间及不同进程生成的不重复数字
        /// </summary>
        public static decimal CreateTimeNumber()
        {
            return UserNumberBuilder.CreateNumber();
        }

        class UserNumberBuilder
        {
            decimal upcode;
            string hashcode;

            UserNumberBuilder()
            {
                hashcode = this.GetHashCode().ToString();
                while (hashcode.Length < 6) hashcode += "0";
                while (hashcode.Length > 6) hashcode = hashcode.Remove(1, 1);
                //System.Diagnostics.Debug.WriteLine(code);
                //while (code > 9999999) code -= 1000;
                //while (code < 1000000) code += 1000;
                //this.hashcode = code.ToString();
                //this.upcode = code;
                this.upcode = decimal.Parse(this.hashcode);
                //System.Diagnostics.Debug.WriteLine(this.hashcode);
                //HttpContext.Current.Response.Write(this.hashcode + "<br>");
            }

            private decimal NewNumber()
            {
                decimal d = decimal.Parse(DateTime.Now.Ticks + this.hashcode);
                while (d == upcode)
                {
                    System.Threading.Thread.Sleep(1);
                    d = decimal.Parse(DateTime.Now.Ticks + this.hashcode);

                    //System.Diagnostics.Debug.WriteLine(d);
                }
                upcode = d;
                return d;
            }

            static UserNumberBuilder _ = new UserNumberBuilder();
            /// <summary>
            /// 创建新数字
            /// </summary>
            public static decimal CreateNumber()
            {
                return _.NewNumber();
            }
        }


        /// <summary>
        /// 产生35位加密串,其中z为保留字
        /// </summary>
        // 产生不可重复11位字符串
        public static string Hex36(long size)
        {
            char[] bs = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F',
                    'G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y'};

            //-z为保留项

            StringBuilder s = new StringBuilder();
            while (size > 0)
            {
                s.Insert(0, bs[size % bs.Length]);
                size = (long)(size / bs.Length);
            }

            return s.ToString();
        }
    }

    //16 进制
    //private string Hex16(long size)
    //{
    //    char[] bs = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
    //    StringBuilder s = new StringBuilder();
    //    while (size > 0)
    //    {
    //        s.Insert(0,bs[size % 16]);
    //        size = (long)(size / 16);
    //    }
    //    return s.ToString();
    //}

    /// <summary>
    /// 随机数临时变量
    /// </summary>
    internal class TmpRand
    {
        internal const int Min = 10;
        internal const int Max = 99;

        static int tmpRand;
        static TmpRand()
        {
            tmpRand = Min;
        }
        /// <summary>
        /// 进行处理叠加
        /// </summary>
        public void add()
        {
                tmpRand++;
                if (tmpRand > Max)  //每秒的重复机率基本没有
                    tmpRand = Min;
        }
        /// <summary>
        /// 获取当前值
        /// </summary>
        public int num
        {
            get { return tmpRand; }
        }
    }
}


