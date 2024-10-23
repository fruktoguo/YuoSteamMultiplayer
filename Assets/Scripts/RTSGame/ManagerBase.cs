using System;
using System.Collections.Generic;
using QFramework;

namespace RTSGame
{
    public interface IManager : IBattleTick
    {
        void Init();
    }
    
    public abstract class ManagerBase<T> : SingletonClass<T> ,IManager, IUnRegisterList where T : ManagerBase<T>, new()
    { 
        public List<IUnRegister> UnregisterList { get; } = new();
        public virtual void Init()
        {
            
        }

        public virtual void Tick(float deltaTime)
        {
            
        }
        
        #region 事件部分
        protected void SendEvent<TEvent>() where TEvent : new()
        {
            GameEventManager.SendEvent<TEvent>(); 
        }
 
        protected void SendEvent<TEvent>(TEvent e) where TEvent : IPoolType
        {
            GameEventManager.SendEvent<TEvent>(e);  
        }

        protected virtual void RegisterEvent()
        {
            
        }
        
        protected virtual void UnRegisterEvent()
        {
            this.UnRegisterAll();
        } 
        
        protected void RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            GameEventManager.RegisterEvent(onEvent).AddToUnregisterList(this); 
        }

        /// <summary>
        /// 如果要取消注册，建议用this.UnRegisterAll();
        /// </summary>
        protected void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            GameEventManager.UnRegisterEvent(onEvent); 
        }
        #endregion
        

        public virtual void UnInit()
        {
            this.UnRegisterAll();
        }
        
    }
}
