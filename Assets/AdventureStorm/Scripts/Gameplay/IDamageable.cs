namespace AdventureStorm.Gameplay
{
    public interface IDamageable
    {
        float Health { get; set; }
        bool IsAlive { get; set; }
        void Damage(float damage);
    }
}
