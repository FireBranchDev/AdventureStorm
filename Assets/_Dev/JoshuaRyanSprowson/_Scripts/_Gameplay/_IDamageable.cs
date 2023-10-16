namespace AdventureStorm
{
    public interface _IDamageable
    {
        bool IsAlive { get; }
        float Health { get; }
        void Damage(float damage);
        void FinishedDyingAnimation();
    }
}
