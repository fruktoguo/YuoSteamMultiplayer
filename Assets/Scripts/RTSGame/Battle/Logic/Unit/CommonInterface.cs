using System.Collections.Generic;

namespace RTSGame
{
    public interface ICanDef
    {
        void Def(IAttackData attackData);
    }

    public interface ICanUseSkill
    { 
        void UseSkill(ISkill skillData);
    }
    
    // TODO: 关于攻击，类似这种东西都当做技能来处理

}