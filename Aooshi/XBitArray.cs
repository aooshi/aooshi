using System;
using System.Collections;

namespace Aooshi
{
    /// <summary>
    /// 简易开关数据
    /// </summary>
    public class XBitArray
    {
        BitArray Bar;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="br">默认值</param>
        private XBitArray(BitArray br)
        {
            Bar = br;
        }

        /// <summary>
        /// 获取当前数据的种子值
        /// </summary>
        public int Seed
        {
            get
            {
                int[] i = new int[1];
                Bar.CopyTo(i, 0);
                return i[0];
            }
        }

        /// <summary>
        /// 获取或设置指定位置处的开关值
        /// </summary>
        /// <param name="index">索引</param>
        public bool this[int index]
        {
            get { return Bar[index]; }
            set { Bar[index] = value; }
        }

        /// <summary>
        /// 获取开关长度
        /// </summary>
        public int Len
        {
            get { return Bar.Length; }
        }

        /// <summary>
        /// 设置将全部值设置为指定开关数据
        /// </summary>
        /// <param name="value">要设置的值</param>
        public void SetAll(bool value)
        {
            Bar.SetAll(value);
        }

        /// <summary>
        /// 创建一个开关对象
        /// </summary>
        /// <param name="len">开关长度,此值不得为负数或大于32,即开关最大32位</param>
        public static XBitArray Create(int len)
        {
            return Create(len, false);
        }
        /// <summary>
        /// 创建一个开关对象
        /// </summary>
        /// <param name="len">开关长度,此值不得为负数或大于32,即开关最大32位</param>
        /// <param name="defaultValue">默认值</param>
        public static XBitArray Create(int len, bool defaultValue)
        {
            if (len > 64 || len < 0) throw new ArgumentOutOfRangeException("len","is len > 64 or len < 0 error;");
            return new XBitArray(new BitArray(len, defaultValue));
        }

        /// <summary>
        /// 根据种子数据创建一个开关对象
        /// </summary>
        /// <param name="seed">种子</param>
        public static XBitArray CreateSeed(int seed)
        {
            return new XBitArray(new BitArray(new int[] { seed }));
        }
    }



    /*
     
      BitArray myba = new BitArray(8);  //字节长度,不同的长度就须要下面接收的数组类型变化,
                                          //比如: int 为 32  则下面接收的,至少要保证为 int
                                          //比如: byte 为 8 . .........................byte
                                          //.... long 为 64                            long

        myba[0] = myba[1] = myba[2] = myba[3] = true;

        int[] a = new int[1];
        myba.CopyTo(a, 0);
        int result = a[0];

        Response.Write(result);
        Response.Write("<br>");

        //Response.Write(BitConverter.ToString(a));
        //Response.Write("<br>");


        Response.Write(myba.Length);
        Response.Write("<br>");

        Response.Write(myba.Count);
        Response.Write("<br>");

        for (int i = 0; i < myba.Count; i++)
        {
            Response.Write(myba[i]);
            Response.Write("<br>");
        }

        Response.Write("------------------------------------");
        Response.Write("<br>");

        myba = new BitArray(new int[]{15});  //15是由上面所转换回来的

        Response.Write(myba.Length);
        Response.Write("<br>");

        Response.Write(myba.Count);
        Response.Write("<br>");

        for (int i = 0; i < myba.Count; i++)
        {
            Response.Write(myba[i]);
            Response.Write("<br>");
        }
     
     */
}
