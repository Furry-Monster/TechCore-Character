using System.Collections.Generic;

namespace MonsterSkill.Runtime
{
    public static class GlobalSkillSystem
    {
        private static readonly Dictionary<int, Skill> Skills = new();

        public static void AddSkill(int id, Skill skill)
        {
            Skills[id] = skill;
        }

        public static void RemoveSkill(int id)
        {
            Skills.Remove(id);
        }

        public static Skill GetSkill(int id)
        {
            return Skills[id];
        }

        public static bool HasSkill(int id)
        {
            return Skills.ContainsKey(id);
        }
    }
}