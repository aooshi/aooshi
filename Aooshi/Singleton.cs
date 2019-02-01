namespace Aooshi
{
    /// <summary>
    /// 单实例管理
    /// </summary>
    /// <typeparam name="T">实例类型</typeparam>
    public static class Singleton<T> where T : new ()
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        public readonly static T Instance;



        static Singleton()
        {
            Singleton<T>.Instance = new T();
        }
    }
}
