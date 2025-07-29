using UnityEngine;

namespace MonsterSkill.Runtime
{
    public enum SkillState
    {
        Idle,
        Casting,
        Executing,
        Cooldown,
    }

    public struct Skill
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public SkillState State { get; set; }

        public ITrigger Trigger { get; set; }

        public IDeployer Deployer { get; set; }
    }
}