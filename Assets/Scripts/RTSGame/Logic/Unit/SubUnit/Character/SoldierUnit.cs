namespace RTSGame
{
    public class SoldierCharacterData : CharacterData
    {
        public SoldierCharacterData(UnitType unitType, CharacterType characterType, float movementSpeed, string name, int maxHp, int curHp, int atkNum, int cost, int buildTime, int hpRecoverySpeed, int hpRecoveryValue) : base(unitType, characterType, movementSpeed, name, maxHp, curHp, atkNum, cost, buildTime, hpRecoverySpeed, hpRecoveryValue)
        {
        }
    }
    
    // 战士
    public class SoldierUnit : CharacterBase
    {
        public SoldierUnit(CharacterData data) : base(data)
        {
        }
    }
}
