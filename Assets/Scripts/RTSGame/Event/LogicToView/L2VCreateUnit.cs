namespace RTSGame
{
    public class L2VCreateUnit : GameBaseEvent<L2VCreateUnit>
    {
        // 显示层需要的必要信息
        public long Guid;
        public IUnitData UnitData;
    }
}
