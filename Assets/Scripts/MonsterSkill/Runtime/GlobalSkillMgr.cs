using System.Collections.Generic;

namespace MonsterSkill.Runtime
{
    public static class GlobalSkillMgr
    {
        private static readonly Dictionary<int, Skill> _skills = new();

        public static void AddSkill(int id, Skill skill)
        {
            _skills[id] = skill;
        }

        public static void RemoveSkill(int id)
        {
            _skills.Remove(id);
        }

        public static Skill GetSkill(int id)
        {
            return _skills[id];
        }

        public static bool HasSkill(int id)
        {
            return _skills.ContainsKey(id);
        }
    }
}