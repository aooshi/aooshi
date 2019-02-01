using System;
using System.Collections.Generic;
using System.Text;

namespace Aooshi
{
    /// <summary>
    /// ͨ�������������
    /// ���з�����Ϊ��̬����
    /// </summary>
    public class RandomString
    {
        /// <summary>
        /// ����һ���������������ݸ��ݴ���ֵ�볤�Ⱦ������
        /// </summary>
        /// <param name="Chars">��������ַ�����</param>
        /// <param name="Size">����ַ�������</param>
        /// <returns>�������ɵ������</returns>
        internal static string Create(char[] Chars, int Size)
        {
            int cl = Chars.Length - 1;  //��һ��ֹ���ݳ���

            string num = "";

            Random rand = new Random(); ;

            for (int i = 0; i < Size; i++)
                num += Chars[rand.Next(0, cl)];

            return num;
        }

        /// <summary>
        /// ����ָ��λ�������0-9�����ַ���
        /// </summary>
        /// <param name="Size">����ַ�������</param>
        /// <returns>�������ɵ��ַ���</returns>
        public static string CreateNumber(int Size)
        {
            return RandomString.Create(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }, Size);
        }

        /// <summary>
        /// ����һ��Сд��ĸ����������ַ���
        /// </summary>
        /// <param name="Size">����ַ�������</param>
        /// <returns>�������ɵ��ַ���</returns>
        public static string CreateNMinLetter(int Size)
        {
            char[] charArray = new char[]{'0','1', '2','3','4','5','6','7','8','9'
										   ,'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q'
										   ,'r','s','t','u','v','w','x','y','z'};
            return RandomString.Create(charArray, Size);
        }

        /// <summary>
        /// ����һ����Сд��ĸ����������ַ���
        /// </summary>
        /// <param name="Size">����ַ�������</param>
        /// <returns>�������ɵ��ַ���</returns>
        public static string CreateNMaxLetter(int Size)
        {
            char[] charArray = new char[]{'0','1', '2','3','4','5','6','7','8','9'
										   ,'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q'
										   ,'r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H'
										   ,'I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};

            return RandomString.Create(charArray, Size);
        }

        /// <summary>
        /// ����һ��Сд��ĸ��ɵ�����ַ���
        /// </summary>
        /// <param name="Size">����ַ�������</param>
        /// <returns>�������ɵ��ַ���</returns>
        public static string CreateLetter(int Size)
        {
            char[] charArray = new char[]{'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q'
										   ,'r','s','t','u','v','w','x','y','z'};

            return RandomString.Create(charArray, Size);
        }

        /// <summary>
        /// ����һ����Сд��ĸ��ɵ�����ַ���
        /// </summary>
        /// <param name="Size">����ַ�������</param>
        /// <returns>�������ɵ��ַ���</returns>
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
        /// ����һ��2000����2700��֮����ʱ��仯�Ҳ����ظ���11λ�����(ע:ÿ�����������������89��,�粻ͬ������ͬʱ����,�ڵ������п��ܳ����ظ���)
        /// </summary>
        public static string CreateOnlyID()
        {
            //���ɷ�ʽ
            // ���:    4
            // �·�:    2
            // ����:    2
            // ʱ��:    4
            // ��:      2

            // ����:    2 

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
                    //ѭ��ֵ,ʹ��ÿ�벻�����ظ�ֵ
                    while (tmp == tmpString)
                    {
                        System.Threading.Thread.Sleep(1); //�ȴ�1����
                        tmp = string.Format("yyyyMMddHHmmss{0}", num);
                        tmp = DateTime.Now.ToString(tmp);
                    }
                    tmpString = tmp;//���û���
                }

            }
            return Hex36( long.Parse( tmp ) );
            //return long.Parse(tmp).ToString("X");
        }

        //�õ���ǰ����ֵ
        private static long _createid = DateTime.Now.Ticks;
        /// <summary>
        /// ����һ����ǰ���̲����ظ�IDֵ;
        /// </summary>
        public static string CreateCourseID()
        {
            return Hex36(_createid++);
        }


        /// <summary>
        /// ����һ����Unixʱ��ر仯�Ķ���ID���������ڱ�֤���Բ��ظ�����ͬ������ͬʱ���п����ظ�,�ظ�ʱ�伶��(��)��
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
            /// ����һ����ID
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
        ///// ����һ�鲻�ظ������(����15λ),��ѭ��С����ʱÿ�����89������;
        ///// </summary>
        ///// <returns>�������ɵ����ֵ</returns>
        //public static string CreateRanID()
        //{
        //    //���ɷ�ʽ
        //    // ���:    3
        //    // �·�:    2
        //    // ����:    2
        //    // ʱ��:    4
        //    // ��:      2
        //    //      ==  13
        //    // ����:    2 

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
        //            //ѭ��ֵ,ʹ��ÿ�벻�����ظ�ֵ
        //            while (tmp == tmpString)
        //            {
        //                System.Threading.Thread.Sleep(1); //�ȴ�1����
        //                tmp = string.Format("yyyyMMddHHmmss{0}", num);
        //                tmp = DateTime.Now.ToString(tmp).Substring(2);
        //            }
        //            tmpString = tmp;//���û���
        //        }

        //    }
        //    return long.Parse(tmp).ToString("X");
        //}

        /// <summary>
        /// ����ʱ�估��ͬ�������ɵĲ��ظ�����
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
            /// ����������
            /// </summary>
            public static decimal CreateNumber()
            {
                return _.NewNumber();
            }
        }


        /// <summary>
        /// ����35λ���ܴ�,����zΪ������
        /// </summary>
        // ���������ظ�11λ�ַ���
        public static string Hex36(long size)
        {
            char[] bs = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F',
                    'G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y'};

            //-zΪ������

            StringBuilder s = new StringBuilder();
            while (size > 0)
            {
                s.Insert(0, bs[size % bs.Length]);
                size = (long)(size / bs.Length);
            }

            return s.ToString();
        }
    }

    //16 ����
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
    /// �������ʱ����
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
        /// ���д������
        /// </summary>
        public void add()
        {
                tmpRand++;
                if (tmpRand > Max)  //ÿ����ظ����ʻ���û��
                    tmpRand = Min;
        }
        /// <summary>
        /// ��ȡ��ǰֵ
        /// </summary>
        public int num
        {
            get { return tmpRand; }
        }
    }
}


