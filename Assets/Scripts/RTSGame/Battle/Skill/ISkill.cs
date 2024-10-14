using System;
using UnityEngine;

namespace RTSGame
{
    public interface ISkill
    {
        string Name { get; }
        SkillType Type { get; }
        float Cooldown { get; }
        float CurrentCooldown { get; }
        bool IsReady { get; }
        void Use(IUnit caster, IUnit target, Vector3 targetPosition);
        void UpdateCooldown(float deltaTime);
    }

    public abstract class SkillBase : ISkill
    {
        public string Name { get; protected set; }
        public SkillType Type { get; protected set; }
        public float Cooldown { get; protected set; }
        public float CurrentCooldown { get; protected set; }
        public bool IsReady => CurrentCooldown <= 0;

        public abstract void Use(IUnit caster, IUnit target, Vector3 targetPosition);

        public virtual void UpdateCooldown(float deltaTime)
        {
            if (CurrentCooldown > 0)
            {
                CurrentCooldown = Math.Max(0, CurrentCooldown - deltaTime);
            }
        }
    }
}