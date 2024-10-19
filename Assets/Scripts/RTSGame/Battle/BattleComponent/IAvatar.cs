using System;
using System.Collections.Generic;

namespace Game
{
    public interface IAvatar
    {
        T GetComponent<T>() where T : IComponentBase;
        void AddComponent<T>(T component) where T : IComponentBase;
        void RemoveComponent<T>() where T : IComponentBase;
    }
    
    public class AvatarBase : IAvatar
    {
        Dictionary<Type,IComponentBase> mComponents = new();

        public T GetComponent<T>() where T : IComponentBase
        {
            if (mComponents.TryGetValue(typeof(T), out var component))
            {
                return (T) component;
            }
            return default;
        }

        public void AddComponent<T>(T component) where T : IComponentBase
        {
            if (!mComponents.ContainsKey(typeof(T)))
            {
                mComponents.Add(typeof(T), component);
            } 
        }

        public void RemoveComponent<T>() where T : IComponentBase
        {
            if (mComponents.ContainsKey(typeof(T)))
            {
                mComponents.Remove(typeof(T));
            } 
        }
    }


    public interface IComponentBase
    {
        IAvatar Origin { get; set; }
    }
    
    
    
    
}
