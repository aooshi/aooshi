using System;

namespace Aooshi
{
    /// <summary>
    /// ���ֵ�����
    /// </summary>
    public class NumberCount
    {
        int seed;
        int size;
        /// <summary>
        /// ��ʼ������Ϊ1������,����Ϊ1��ʵ��
        /// </summary>
        public NumberCount()
            : this(1, 1)
        {
        }

        /// <summary>
        /// ��ʼ����ʵ��,��ָ������ֵ,����ֵ����ΪĬ��1
        /// </summary>
        /// <param name="Seed">����ֵ</param>
        public NumberCount(int Seed)
            : this(Seed, 1)
        {
        }

        /// <summary>
        /// ��ʼ����ʵ��
        /// </summary>
        /// <param name="Seed">Ĭ������ֵ</param>
        /// <param name="Size">Ĭ�ϵ���ֵ</param>
        public NumberCount(int Seed, int Size)
        {
            this.Seed = Seed;
            this.Size = Size;
        }

        /// <summary>
        /// ��ȡ�����õ�ǰ����ֵ
        /// </summary>
        public int Seed
        {
            get { return this.seed; }
            set { this.seed = value; }
        }

        /// <summary>
        /// ��ȡ�����õ���ֵ
        /// </summary>
        public int Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        /// <summary>
        /// ������׷��һ��ֵ
        /// </summary>
        public void Add()
        {
            this.Seed += this.size;
        }

        /// <summary>
        /// ��ȡ��ǰֵ��׷��һ��
        /// </summary>
        public int GetAdd
        {
            get
            {
                this.Add();
                return this.Seed;
            }
        }

        /// <summary>
        /// ������,��ȡ�ַ�����ʽ��ǰֵ,�������ӵ���һ��
        /// </summary>
        public override string ToString()
        {
            return this.GetAdd.ToString();
        }
    }
}
