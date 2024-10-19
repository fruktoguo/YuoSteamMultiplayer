using System;
using QFramework;

namespace Game
{
    public class BattleBaseEvent<T> : IPoolable ,IPoolType where T : BattleBaseEvent<T>, new()
    {
        private static Func<T> FactoryMethod;
        public bool IsRecycled { get; set; }
        public void OnRecycled()
        {
            
        }

        static BattleBaseEvent()
        {
            FactoryMethod = () => new T();
            SafeObjectPool<T>.Instance.SetFactoryMethod(FactoryMethod);
        }

        public virtual void SetFactoryMethod(Func<T> factoryMethod)
        {
            FactoryMethod = factoryMethod;
            SafeObjectPool<T>.Instance.SetFactoryMethod(factoryMethod);
        }

        /// <summary>
        /// 不使用对象池，适用于使用频率不高的事件
        /// </summary> 
        public static T GetNotPool()
        {
            return FactoryMethod?.Invoke();
        }
        
        public static T Allocate()
        {
            return SafeObjectPool<T>.Instance.Allocate();
        }
        
        public static void Recycle(T obj)
        {
            SafeObjectPool<T>.Instance.Recycle(obj);
        }

        public void Recycle2Cache()
        {
            Recycle((T) this);
        }
    } 
}
