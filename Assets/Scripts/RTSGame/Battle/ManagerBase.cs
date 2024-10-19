namespace Game
{
    public interface IManager : IBattleTick
    {
        void Init();
    }
    
    public abstract class ManagerBase<T> : SingletonClass<T> ,IManager where T : ManagerBase<T>, new()
    {
        public virtual void Init()
        {
            
        }

        public virtual void Tick(float deltaTime)
        {
             
        }
    }
}
