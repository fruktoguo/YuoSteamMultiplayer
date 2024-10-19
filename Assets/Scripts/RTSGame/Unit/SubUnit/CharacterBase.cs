using System.Collections.Generic;
using UnityEngine;

namespace RTSGame
{ 
    public abstract class CharacterData : UnitData, ICharacterData
    {
        float ICharacterData.MovementSpeed { get; set; }
    }

    
    public abstract class CharacterBase : UnitBase , ICharacter
    {
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
