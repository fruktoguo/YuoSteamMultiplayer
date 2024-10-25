using Unity.Mathematics;

namespace RTSGame
{
    public class L2VSetUnitPosition : GameBaseEvent<L2VSetUnitPosition>
    {
        // 显示层需要的必要信息
        public long Guid;
        public float3 Position;
        
        public static L2VSetUnitPosition GetEvent(long guid, float3 position)
        {
            var evt = Allocate();
            evt.Guid = guid;
            evt.Position = position;
            return evt;
        }
        
        public static void SendEvent(long guid, float3 position)
        {
            var evt = Allocate();
            evt.Guid = guid;
            evt.Position = position;
            evt.SendEvent();
        }
    }
}
