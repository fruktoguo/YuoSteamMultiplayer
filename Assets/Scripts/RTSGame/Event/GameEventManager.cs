using System;
using System.Collections.Generic;
using QFramework;

namespace RTSGame
{  
    public class GameEventManager : ManagerBase<GameEventManager> 
    {
        private TypeEventSystem mTypeEventSystem = new();
        
        public new static void SendEvent<T>() where T : new()
        {
            Instance.mTypeEventSystem.Send<T>();
        }
 
        public new static void SendEvent<T>(T e) where T : IPoolType
        {
            Instance.mTypeEventSystem.Send(e); 
            e.Recycle2Cache();
        }
         
        public new static IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return Instance.mTypeEventSystem.Register(onEvent);
        }

        public new static void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            Instance.mTypeEventSystem.UnRegister<TEvent>(onEvent);
        }

        
    }
}
