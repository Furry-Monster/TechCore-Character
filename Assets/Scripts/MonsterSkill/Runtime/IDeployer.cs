namespace MonsterSkill.Runtime
{
    public interface IDeployer
    {
        public bool CanDeploy();

        public void Deploy();

        public void Interrupt();
    }
}