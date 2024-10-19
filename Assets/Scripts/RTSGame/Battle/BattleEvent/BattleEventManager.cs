using System;
using QFramework;

namespace Game
{  
    public class BattleEventManager : ManagerBase<BattleEventManager>
    {
        private TypeEventSystem mTypeEventSystem = new();
        
        public static void SendEvent<T>() where T : new()
        {
            Instance.mTypeEventSystem.Send<T>();
        }
 
        public static void SendEvent<T>(T e) where T : IPoolType
        {
            Instance.mTypeEventSystem.Send<T>(e); 
            e.Recycle2Cache();
        }
         
        public static IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return Instance.mTypeEventSystem.Register<TEvent>(onEvent);
        }

        public static void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            Instance.mTypeEventSystem.UnRegister<TEvent>(onEvent);
        } 
    }
}
