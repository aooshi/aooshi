using System;
using System.Collections;

namespace Aooshi
{
    /// <summary>
    /// ���׿�������
    /// </summary>
    public class XBitArray
    {
        BitArray Bar;

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="br">Ĭ��ֵ</param>
        private XBitArray(BitArray br)
        {
            Bar = br;
        }

        /// <summary>
        /// ��ȡ��ǰ���ݵ�����ֵ
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
        /// ��ȡ������ָ��λ�ô��Ŀ���ֵ
        /// </summary>
        /// <param name="index">����</param>
        public bool this[int index]
        {
            get { return Bar[index]; }
            set { Bar[index] = value; }
        }

        /// <summary>
        /// ��ȡ���س���
        /// </summary>
        public int Len
        {
            get { return Bar.Length; }
        }

        /// <summary>
        /// ���ý�ȫ��ֵ����Ϊָ����������
        /// </summary>
        /// <param name="value">Ҫ���õ�ֵ</param>
        public void SetAll(bool value)
        {
            Bar.SetAll(value);
        }

        /// <summary>
        /// ����һ�����ض���
        /// </summary>
        /// <param name="len">���س���,��ֵ����Ϊ���������32,���������32λ</param>
        public static XBitArray Create(int len)
        {
            return Create(len, false);
        }
        /// <summary>
        /// ����һ�����ض���
        /// </summary>
        /// <param name="len">���س���,��ֵ����Ϊ���������32,���������32λ</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        public static XBitArray Create(int len, bool defaultValue)
        {
            if (len > 64 || len < 0) throw new ArgumentOutOfRangeException("len","is len > 64 or len < 0 error;");
            return new XBitArray(new BitArray(len, defaultValue));
        }

        /// <summary>
        /// �����������ݴ���һ�����ض���
        /// </summary>
        /// <param name="seed">����</param>
        public static XBitArray CreateSeed(int seed)
        {
            return new XBitArray(new BitArray(new int[] { seed }));
        }
    }



    /*
     
      BitArray myba = new BitArray(8);  //�ֽڳ���,��ͬ�ĳ��Ⱦ���Ҫ������յ��������ͱ仯,
                                          //����: int Ϊ 32  ��������յ�,����Ҫ��֤Ϊ int
                                          //����: byte Ϊ 8 . .........................byte
                                          //.... long Ϊ 64                            long

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

        myba = new BitArray(new int[]{15});  //15����������ת��������

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
