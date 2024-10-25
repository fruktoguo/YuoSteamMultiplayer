namespace RTSGame
{
    public class WorkerCharacterData : CharacterData
    {
        public WorkerCharacterData(UnitType unitType, CharacterType characterType, float movementSpeed, string name, int maxHp, int curHp, int atkNum, int cost, int buildTime, int hpRecoverySpeed, int hpRecoveryValue) : base(unitType, characterType, movementSpeed, name, maxHp, curHp, atkNum, cost, buildTime, hpRecoverySpeed, hpRecoveryValue)
        {
        }
    }
    
    
    /// <summary>
    /// 工人
    /// </summary>
    public class WorkerUnit : CharacterBase
    {
        public WorkerUnit(CharacterData data) : base(data)
        {
        }
    }
}