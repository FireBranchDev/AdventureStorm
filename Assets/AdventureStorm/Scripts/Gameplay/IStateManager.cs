namespace AdventureStorm.Gameplay
{
    public interface IStateManager
    {
        public float Health { get; }
        public bool IsAlive { get; }

        public void SwitchState(BaseState state);

        public void Damage(float damage);
    }
}