using Unity.Mathematics;

namespace RTSGame
{
    public abstract class UnitData : IUnitData
    {
        public UnitType UnitType { get; protected set; } 
        public string Name { get; protected set; } 
        public int MaxHp { get; protected set; }
        public int CurHp { get; protected set; }
        public int AtkNum { get; protected set; }
        public int Cost { get; protected set; }
        public int BuildTime { get; protected set; }
        public int HpRecoverySpeed { get; protected set; }
        public int HpRecoveryValue { get; protected set; }

        protected UnitData(UnitType unitType, string name, int maxHp, int curHp, int atkNum, int cost, int buildTime, int hpRecoverySpeed, int hpRecoveryValue)
        {
            UnitType = unitType;
            Name = name;
            MaxHp = maxHp;
            CurHp = curHp;
            AtkNum = atkNum;
            Cost = cost;
            BuildTime = buildTime;
            HpRecoverySpeed = hpRecoverySpeed;
            HpRecoveryValue = hpRecoveryValue;
        } 
    }
    
    
    public abstract class UnitBase : IUnit , IAvatar
    {
        public long Guid { get; private set; } 
        public IUnitBehaviorCtrl BehaviorCtrl { get; private set; }   // 行为控制器，从外部传进来。  想了想还是不放在组件里面了，毕竟是个常用的。 
        public IUnitData UnitData { get; }
        
        // --------
        public float3 Position { get; protected set; }
        
        protected readonly UnitAvatarBase Avatar;   // 用组合的方式进行组件处理
        
        public UnitBase(IUnitData data)
        { 
            UnitData = data; 
            Avatar = new UnitAvatarBase();
        }

        /// <summary>
        /// 这里写基类的必要组件
        /// </summary>
        public void InitComponent()
        { 
            
            OnSubInitComponent();
        }

        /// <summary>
        /// 这里写子类自己的组件
        /// </summary>
        protected virtual void OnSubInitComponent()
        {
            
        } 

        public void TakeDamage(int damage)
        {
            
        }

        public void SetPosition(float3 position)
        {
            Position = position;
            L2VSetUnitPosition.SendEvent(Guid, position);
        }
        
        public void SetGuid(long guid)
        {   
            Guid = guid;
        }
        
        public void SetBehaviorCtrl(IUnitBehaviorCtrl behaviorCtrl)
        {
            BehaviorCtrl = behaviorCtrl;
        }

        public virtual void Init()
        {
        }

        public virtual void Tick(float deltaTime)
        {
            Avatar?.Update(deltaTime);
            BehaviorCtrl?.UpdateState(deltaTime);
        }

        public virtual void Dispose()
        { 
        }
        
        /// <summary>
        /// 注意，该方法不是mono的GetComponent
        /// </summary> 
        public T GetComponent<T>() where T : IComponentBase
        {
            return Avatar.GetComponent<T>();
        }

        public void AddComponent<T>(T component) where T : IComponentBase
        { 
            Avatar.AddComponent(component);
        }

        public void RemoveComponent<T>() where T : IComponentBase
        { 
            Avatar.RemoveComponent<T>();
        }
    }
}
