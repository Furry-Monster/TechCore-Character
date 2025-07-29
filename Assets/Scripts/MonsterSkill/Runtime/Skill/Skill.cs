using System;

namespace MonsterSkill.Runtime
{
    public enum SkillState
    {
        Idle,
        Casting,
        Executing,
        Cooldown,
    }

    [Serializable]
    public struct Skill : ISerializable<Skill>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsBuff { get; set; }

        public SkillState State { get; set; }

        public ITrigger Trigger { get; set; }
    }
}