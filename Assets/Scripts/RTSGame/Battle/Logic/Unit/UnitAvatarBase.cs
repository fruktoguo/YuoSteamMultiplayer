using System;
using System.Collections.Generic;

namespace RTSGame
{
    public class UnitAvatarBase : IAvatar
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
                
                component.OnInit();
            } 
        }

        public void RemoveComponent<T>() where T : IComponentBase
        {
            if (mComponents.ContainsKey(typeof(T)))
            {
                mComponents[typeof(T)].OnDispose();
                mComponents.Remove(typeof(T));
            } 
        }

        public void Update(float deltaTime)
        {
            foreach (var component in mComponents.Values)
            {
                component.OnUpdate(deltaTime);
            } 
        }
        
        public void Dispose()
        {
            foreach (var component in mComponents.Values)
            {
                component.OnDispose();
            } 
        }
        
    }
}