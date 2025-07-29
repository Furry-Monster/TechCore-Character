namespace MonsterSkill.Runtime
{
    public interface ITrigger
    {
        public bool CanTrigger();

        public void Activate();

        public void Deactivate();
    }
}