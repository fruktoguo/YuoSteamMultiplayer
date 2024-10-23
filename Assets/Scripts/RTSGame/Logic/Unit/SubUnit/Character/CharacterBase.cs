using System.Collections.Generic;
using UnityEngine;

namespace RTSGame
{ 
    public class CharacterData : UnitData, ICharacterData
    {
        public CharacterType CharacterType { get; protected set; } 
        public float MovementSpeed { get; protected set; }
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
