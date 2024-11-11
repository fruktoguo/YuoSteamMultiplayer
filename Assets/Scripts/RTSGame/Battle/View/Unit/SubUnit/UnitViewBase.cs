using UnityEngine;

namespace RTSGame.Battle.View
{
    public class UnitViewBase : MonoBehaviour, IUnitView , IAvatar
    {
        private AvatarBase _avatar;   // 用组合的方式进行组件处理
        protected virtual void Awake()
        {
            _avatar = new AvatarBase();
            InitializeComponents();
        }
        
        protected virtual void InitializeComponents()
        {
            // 在这里初始化并添加所需的组件
            // 例如：
            // _avatar.AddComponent(new HealthComponent(this));
            // _avatar.AddComponent(new NameComponent(this));
        }

        /// <summary>
        /// 注意，该方法不是mono的GetComponent
        /// </summary> 
        public new T GetComponent<T>() where T : IComponentBase
        {
            return _avatar.GetComponent<T>();
        }

        public void AddComponent<T>(T component) where T : IComponentBase
        { 
            _avatar.AddComponent(component);
        }

        public void RemoveComponent<T>() where T : IComponentBase
        { 
            _avatar.RemoveComponent<T>();
        }
    }
}
