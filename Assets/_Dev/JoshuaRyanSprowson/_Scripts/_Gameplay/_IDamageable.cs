namespace AdventureStorm
{
    public interface _IDamageable
    {
        public float Health { get; }
        public bool IsAlive { get; }
        public void Damage(float damage);
        public void FinishedDyingAnimation();
    }
}
