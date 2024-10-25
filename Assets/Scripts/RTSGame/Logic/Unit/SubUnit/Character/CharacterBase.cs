using System.Collections.Generic;
using UnityEngine;

namespace RTSGame
{ 
    public abstract class CharacterData : UnitData, ICharacterData
    { 
        public CharacterType CharacterType { get; protected set; } 
        public float MovementSpeed { get; protected set; }
        public CharacterData(UnitType unitType, CharacterType characterType, float movementSpeed, string name, int maxHp, int curHp, int atkNum, int cost, int buildTime, int hpRecoverySpeed, int hpRecoveryValue) : base(unitType, name, maxHp, curHp, atkNum, cost, buildTime, hpRecoverySpeed, hpRecoveryValue)
        {
            CharacterType = characterType;
            MovementSpeed = movementSpeed;
        } 
    }
    
    public abstract class CharacterBase : UnitBase , ICharacter
    {
        protected CharacterBase(CharacterData data) : base(data)
        {
            
        }

        public void Def(IAttackData attackData)
        {
            
        }

        public void Move(Vector3 destination)
        {
            
        }

        public List<ISkill> Skills { get; }
        public void UseSkill(ISkill skillData)
        { 
        }
    }
}
