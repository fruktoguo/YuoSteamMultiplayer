using System.Collections.Generic;
using UnityEngine;

namespace RTSGame
{
    // TODO : 这里不对，不该在这里写，他的释放不对
    public class SkillManager
    {
        private Dictionary<SkillType, List<ISkill>> skillsByType = new Dictionary<SkillType, List<ISkill>>();

        public void AddSkill(ISkill skill)
        {
            if (!skillsByType.ContainsKey(skill.Type))
            {
                skillsByType[skill.Type] = new List<ISkill>();
            }
            skillsByType[skill.Type].Add(skill);
        }

        public void UseSkill(SkillType type, int index, IUnit caster, IUnit target, Vector3 targetPosition)
        {
            if (skillsByType.ContainsKey(type) && index >= 0 && index < skillsByType[type].Count)
            {
                skillsByType[type][index].Use(caster, target, targetPosition);
            }
        }

        public void UpdateCooldowns(float deltaTime)
        {
            foreach (var skillList in skillsByType.Values)
            {
                foreach (var skill in skillList)
                {
                    skill.UpdateCooldown(deltaTime);
                }
            }
        }

        public List<ISkill> GetSkillsOfType(SkillType type)
        {
            return skillsByType.ContainsKey(type) ? skillsByType[type] : new List<ISkill>();
        }
    } 
}