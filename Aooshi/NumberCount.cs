using System;

namespace Aooshi
{
    /// <summary>
    /// 数字叠加器
    /// </summary>
    public class NumberCount
    {
        int seed;
        int size;
        /// <summary>
        /// 初始化种子为1的数字,叠加为1的实例
        /// </summary>
        public NumberCount()
            : this(1, 1)
        {
        }

        /// <summary>
        /// 初始化新实例,并指定种子值,叠加值设置为默认1
        /// </summary>
        /// <param name="Seed">种子值</param>
        public NumberCount(int Seed)
            : this(Seed, 1)
        {
        }

        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="Seed">默认种子值</param>
        /// <param name="Size">默认叠加值</param>
        public NumberCount(int Seed, int Size)
        {
            this.Seed = Seed;
            this.Size = Size;
        }

        /// <summary>
        /// 获取或设置当前种子值
        /// </summary>
        public int Seed
        {
            get { return this.seed; }
            set { this.seed = value; }
        }

        /// <summary>
        /// 获取或设置叠加值
        /// </summary>
        public int Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        /// <summary>
        /// 向种子追加一个值
        /// </summary>
        public void Add()
        {
            this.Seed += this.size;
        }

        /// <summary>
        /// 获取当前值并追加一次
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
        /// 已重载,获取字符串型式当前值,并将种子叠加一次
        /// </summary>
        public override string ToString()
        {
            return this.GetAdd.ToString();
        }
    }
}
