namespace RTSGame
{
    public interface IBattleInit
    {
        void Init();
    }
    public interface IBattleTick
    {
        void Tick(float deltaTime);
    }
    
    /// <summary>
    /// 销毁
    /// </summary>
    public interface IBattleDispose
    {
        void Dispose();
    }
}