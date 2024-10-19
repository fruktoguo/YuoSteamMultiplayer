namespace Game
{
    public class SingletonClass<T> where T : class ,new()
    { 
        /// <summary>
        /// 静态实例
        /// </summary>
        private static T mInstance;

        /// <summary>
        /// 标签锁
        /// </summary>
        private static readonly object mLock = new object();

        /// <summary>
        /// 静态属性
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (mLock)
                {
                    if (mInstance == null)
                    {
                        mInstance = new T();
                    }
                }

                return mInstance;
            }
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public static void Dispose()
        {
            mInstance = null;
        }

    }
}