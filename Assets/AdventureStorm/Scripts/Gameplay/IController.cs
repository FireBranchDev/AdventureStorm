namespace AdventureStorm.Gameplay
{
    public interface IController
    {
        float MaxHealth { get; }
        float Health { get; }
        bool IsAlive { get; }
    }
}